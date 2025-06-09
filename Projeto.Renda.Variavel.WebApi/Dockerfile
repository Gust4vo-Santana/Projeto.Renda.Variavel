FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

WORKDIR /src/Projeto.Renda.Variavel.WebApi
RUN dotnet restore
RUN dotnet publish Projeto.Renda.Variavel.WebApi.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENTRYPOINT ["dotnet", "Projeto.Renda.Variavel.WebApi.dll"]
