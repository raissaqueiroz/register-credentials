FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /app

COPY *.sln .
COPY . .

RUN dotnet restore "RegisterCredentials.Api/RegisterCredentials.Api.csproj"

RUN dotnet test

RUN dotnet build "RegisterCredentials.Api/RegisterCredentials.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegisterCredentials.Api/RegisterCredentials.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS http://*:7000

EXPOSE 7000

ENTRYPOINT ["dotnet", "RegisterCredentials.Api.dll"]
