FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY Quotes.Consumer.csproj ./
RUN dotnet restore Quotes.Consumer.csproj

COPY . ./
RUN dotnet publish Quotes.Consumer.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "Quotes.Consumer.dll"]
