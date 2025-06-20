# Projeto.Renda.Variavel

Este projeto tem como objetivo centralizar e gerenciar informações relacionadas a operações de renda variável. A arquitetura é composta por dois serviços principais: uma Web API e um Worker Service.

## Arquitetura do Projeto

### Web API (.NET)
Responsável por expor os endpoints para consulta dos seguintes dados:

* Total investido por ativo
* Posição do investidor por ativo
* Posição global de um investidor
* Total de corretagem pago por investidor
* Preço médio pago pelo investidor por ativo
* Preço médio global de um ativo
* Última cotação de um ativo
* Valor financeiro ganho pela corretora com as corretagens
* Top 10 clientes com maiores posições
* Top 10 clientes que mais pagaram corretagem

Foram implementadas regras de fail fast em todas as rotas com input do cliente, executando a validação do payload no início da camada de aplicação para evitar desperdício de recursos com requisições inválidas.

### Worker Service (.NET)

Serviço responsável por inserir os novos valores de cotações no banco de dados continuamente por meio do consumo de um tópico Kafka. Para um cenário de indisponibilidade, foram implementadas as seguintes estratégias de resilência:

* Retry: em caso de falha no processamento de uma mensagem, o worker tem uma política de retry - no momento configurada para 5 retentativas com intervalo de 1 segundo.
* Circuit breaker: se, após as 5 retentativas, uma mensagem continuar tendo falhas no processamento, a política de circuit breaker interrompe o fluxo de requisições do worker para o banco de dados por 30 segundos conforme configurado. Após esse intervalo, o circuit breaker testa uma requisição e em caso de sucesso fecha o circuito novamente. O limite de erros permitidos antes de interromper o fluxo está configurado para 5.
* Fallback: caso a mensagem não seja processada com sucesso após todas as retentativas e possível ação do circuit breaker, foi implementado fallback usando um tópico Dead-Letter-Queue (DLQ). Nesse cenário, o worker publicaa mensagem original em um tópico destinado a armazenar mensagens com erro de processamento. DLQs são úteis para evitar gargalos de processamento nos tópicos da aplicação e permitem reprocessamento das mensagens caso faça sentido.

O worker também tem validação de idempotência para garantir que uma mesma mensagem não seja processada mais de uma vez e assegurar a consistência dos dados.

O projeto aplica técnicas da Clean Architecture e Domain Driven Design (DDD), além de Clean Code, em sua organização interna para garantir uma estrutura clara, de fácil entendimento e manutenção.

## Tecnologias Utilizadas

* .NET 8 / .NET Core
* ASP.NET Core Web API
* Worker Service
* Entity Framework Core
* MySQL
* Kafka
* Prometheus
* Serilog
* Seq
* Swagger
* Docker
* XUnit e Moq
* FluentValidation

---

## Instruções para rodar a aplicação

1. Clone o repositório
   ```bash
   git clone https://github.com/Gust4vo-Santana/Projeto.Renda.Variavel.git
   cd Projeto.Renda.Variavel
   ```
2. Inicie o container da aplicação e suas dependências
   ```bash
   docker compose up --build
   ```
3. Para testar as rotas da API, acesse ``http://localhost:8080/swagger/index.html``
4. A documentação no modelo OpenAPI 3.0 em json encontra-se em [doc-open-api-3-0.json](doc-open-api-3-0.json)

## Banco de dados

O script de criação do schema se encontra em [init.sql](init.sql), incluindo alguns comandos de ``INSERT`` para proporcionar testes mais realistas.

* Para a Primary Key de todas as tabelas (menos ``Quotes``), o tipo de dado escolhido foi ``INT`` com auto-incremento na inserção de uma nova linha. Dessa forma, não é necessário implementar a lógica para gerar IDs únicos e, no contexto desse projeto, não há necessidade de tipos mais complexos de ID.
* Especificamente na tabela ``Quotes``, o tipo escolhido para a chave primária foi o ``CHAR(36)``, para armazenamento de GUID, porque essa tabela está inserida no contexto do Worker; logo é interessante que o Id seja igual à Key da mensagem recebida do tópico Kafka (usualmente do tipo GUID) para realizar a validação de idempotência de forma mais próxima da realidade.
* Para colunas referentes a valores numéricos - como ``price``, ``average_price``, ``quantity``, ``p_and_l`` - foi usado o tipo ``DECIMAL``, visto que essas colunas devem comportar valores de números racionais, com ponto flutuante.
* Para colunas de data e hora, o tipo de dado escolhido foi o ``TIMESTAMP``, que inclui data e hora com grande precisão.
* Para as demais colunas com valores nominais, como nome e email do usuário, ou nome e código de um ativo, foi escolhido o tipo ``VARCHAR``, que permite armazenar strings de tamanho variável dentro do limite definido para cada coluna.

---

Dada a necessidade de consultar rapidamente todas as operações de um usuário em determinado ativo nos últimos 30 dias, é vantajoso criar um índice composto na tabela ``Operations`` sobre as colunas ``user_id, asset_id, date_time`` pois essas são as colunas envolvidas na cláusula ``WHERE``, logo o índice sobre elas torna a operação de busca muito mais eficiente. Além disso, o índice envolvendo a coluna ``date_time`` acelera o ``ORDER BY DESC`` pois faz com que os dados estejam ordenados previamente. A criação dos índices também está no arquivo [init.sql](init.sql).

A query que recupera as operações de um usuário em determinado ativo nos últimos 30 dias encontra-se em [select-operations.sql](select-operations.sql).

A estrutura para atualização automática do P&L da posição dos clientes para determinado ativo quando uma nova cotação é inserida encontra-se em [update-position.sql](update-position.sql). Para isso, um trigger é disparado após todo ``INSERT`` na tabela ``Quotes`` executando o cálculo para atualizar a relação Lucro vs Perda levando em conta o novo preço do ativo.
O preço médio pago pelo cliente é atualizado no momento em que uma nova operação de compra é criada, portanto a atualização da cotação não altera esse valor imediatamente.

## Testes automatizados

### Testes unitários

Os testes unitários cobrem todos os use cases do projeto (33 testes no total) para assegurar que a lógica de negócio está implementada corretamente. Para rodar os testes unitários, execute:
```bash
dotnet test
```

### Testes mutantes

Testes mutantes são um tipo de teste de software que tem o objetivo de garantir a qualidade de um conjunto de testes. Essa técnica consiste em aplicar mutaçõs no código para verificar se os testes existentes de fato são capazes de detectar bugs e erros de implementação.

Para o caso de uso da média ponderada de preço de um ativo, uma possível mutação é alterar o cálculo, por exemplo trocando a parte final do método por ``totalQuantity / weightedSum``. Dessa forma, o retorno do método será diferente do esperado e, portanto, os testes unitários existentes devem ser capazes de detectar esse erro inserido pelo teste de mutação. Caso o teste unitário não falhasse com a versão mutada do código, o teste mutante registraria essa métrica.

## Escalabilidade e performance

### Auto-scaling horizontal

Para a WebAPI, o auto-scaling horizontal consiste em, de forma automática, aumentar o número de instâncias do componente baseado em métricas como consumo de CPU, memória ou outras regras personalizadas, como time-based-scale. Nesse contexto, os principais provedores de nuvem, como a Azure por exemplo, fornecem ferramentas que executam esse escalonamento de forma fácil e automatizada por meio de configurações simples como arquivos ``azure-pipelines.yml``. Além disso, serviços de Kubernetes, como o AKS e o EKS, também são soluções muito robustas para implementar essa orquestração.

### Balanceamento de carga

Outra técnica importante para otimizar o consumo de recursos é o balanceamento de carga, que conta com diversas estratégias. Duas das principais são:

1. Round-robin: consiste em distribuir a carga entre as instâncias de forma uniforme, circular e sequencial. Ou seja, cada nova requisição é enviada à próxima instância da fila. Esse algoritmo tem como principal vantagem sua simplicidade de implementação e é mais indicado para cenários em que o desempenho dos servidores é similar, pois não leva em conta a latência de resposta de cada instância, e com isso pode sobrecarregar servidores mais lentos ou ocupados.

2. Por latência: o balanceamento por latência tem como principal característica justamente cobrir o maior ponto fraco do round-robin, pois monitora constantemente o tempo de resposta das instâncias e envia cada nova requisição para a instância mais rápida naquele momento. Dessa forma, servidores lentos ou mais ocupados não sâo sobrecarregados e podem se recuperar de problemas de desempenho.

## Observabilidade

Neste projeto, a observabilidade foi implementada de 2 formas: logs estruturados e métricas.

1. Logs estruturados: a aplicação implementa logs estruturados utilizando Serilog, facilitando a visualização e análise dos logs ao longo do fluxo de toda requisição no sistema. Esses logs são acessíveis via console ou acessando ``http://localhost:5341`` para a WebApi e ``http://localhost:5342`` para o Worker quando a aplicação está rodando. Esse formato torna os logs mais consistentes, rastreáveis e úteis para identificar problemas.
2. Métricas: o componente WebApi tem métricas por meio da integração com o Prometheus. Tratam-se de métricas básicas sobre as requisições http, mas que são capazes de fazer um primeiro diagnóstico de problemas com a aplicação, por exemplo monitorando a duração das requisições. As métricas são acessíveis em ``http://localhost:8080/metrics`` quando a aplicação está rodando.
