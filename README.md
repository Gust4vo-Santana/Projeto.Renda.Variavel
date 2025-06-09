# Projeto.Renda.Variavel

Este projeto tem como objetivo centralizar e organizar as informações relacionadas a operações de renda variável. A arquitetura é composta por dois serviços principais: uma Web API e um Worker Service.

## Arquitetura do Projeto

Web API (.NET):
Responsável por expor os endpoints para consulta de:

* Total investido por ativo
* Posição por ativo de um investidor 
* Posição global de um investidor
* Total de corretagem pago por investidor
* Preço médio de um ativo pago por investidor
* Preço médio global de um ativo
* Última cotação de um ativo
* Valor financeiro ganho pela corretora com as corretagens
* Top 10 clientes com maiores posições
* Top 10 clientes que mais pagaram corretagem

Worker Service (.NET):
Serviço em segundo plano responsável por inserir os novos valores de cotações continuamente por meio do consumo de um tópico Kafka.

O projeto aplica técnicas da Clean Architecture e Domain Driven Design (DDD), além de Clean Code, em sua organização interna para garantir uma estrutura clara, de fácil entendimento e manutenção.

## Tecnologias Utilizadas

* .NET 8 / .NET Core
* ASP.NET Core Web API
* Worker Service
* Entity Framework Core
* MySQL
* Kafka
* Swagger (documentação da API - OpenAPI 3.0)
* Docker
* XUnit e Moq

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
3. Para testar as rotas da API e acessar a documentação no formato OpenAPI 3.0, acesse ``http://localhost:8080/swagger/index.html``

## Banco de dados

O script de criação do schema se encontra em [init.sql](init.sql), incluindo alguns comandos de ``INSERT`` para garantir testes mais realistas.

* Para a Primary Key de todas as tabelas, o tipo de dado escolhido foi ``INT`` com auto-incremento na inserção de uma nova linha. Dessa forma, não é necessário implementar a lógica para gerar IDs únicos e, no contexto desse projeto, não há necessidade de tipos mais complexos como GUID.
* Para colunas referentes a valores numéricos - como ``price``, ``average_price``, ``quantity``, ``p_and_l`` - foi usado o tipo ``DECIMAL``, visto que essas colunas devem comportar valores de números racionais, com ponto flutuante.
* Para colunas de data e hora, o tipo de dado escolhido foi o ``TIMESTAMP``, que inclui data e hora com grande precisão.
* Para as demais colunas com valores nominais, como nome e email do usuário, ou nome e código de um ativo, foi escolhido o tipo ``VARCHAR``, que permite armazenar strings de tamanho variável dentro do limite definido para cada coluna.

Dada a necessidade de consultar rapidamente todas as operações de um usuário em determinado ativo nos últimos 30 dias, é vantajoso criar 2 índices na tabela ``Operations``

1. Índice na coluna ``user_id``
2. Índice na coluna ``asset_id``

Esses índices são interessantes porque as colunas utilizadas na operação de busca da query em questão são ``user_id`` e ``asset_id``. Índices nessas colunas tornarão a consulta mais rápida pois a busca sobre colunas indexadas é mais eficiente.

## Testes automatizados

### Testes unitários

Para os testes unitários, as tecnologias utilizadas foram o XUnit e o Moq. para rodar os testes unitários, execute o comando
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

2. Por latência: o balanceamento por latência tem como principal característica justamente cobrir o maior ponto fraco do round-robin, pois pois monitora constantemente o tempo de resposta das instâncias e envia cada nova requisição para a mais rápida naquele momento. Dessa forma, servidores lentos ou mais ocupados não sâo sobrecarregados e podem se recuperar de problemas de desempenho.
