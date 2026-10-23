FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
# Adjust this path to wherever your .csproj sits relative to the repo root
RUN dotnet publish TodoAppWithLogin/TodoAppWithLogin.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "TodoAppWithLogin.dll"]
