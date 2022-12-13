# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY . .
RUN dotnet restore

RUN dotnet build -c Release -o out
FROM build AS publish
RUN dotnet publish -c Release -o out

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build . .
# Padrão de container ASP.NET
# ENTRYPOINT ["dotnet", "RegisterCredentials.dll"]
# Opção utilizada pelo Heroku
CMD ASPNETCORE_URLS=http://*:$PORT dotnet RegisterCredentials.dll