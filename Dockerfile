# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar arquivo .csproj para restaurar depend�ncias
COPY Projeto.Renda.Variavel.WebApi/Projeto.Renda.Variavel.WebApi.csproj ./Projeto.Renda.Variavel.WebApi/
RUN dotnet restore ./Projeto.Renda.Variavel.WebApi/Projeto.Renda.Variavel.WebApi.csproj

# Copiar o restante do c�digo
COPY . .

# Publicar a aplica��o
WORKDIR /src/Projeto.Renda.Variavel.WebApi
RUN dotnet publish Projeto.Renda.Variavel.WebApi.csproj -c Release -o /app/out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Projeto.Renda.Variavel.WebApi.dll"]
