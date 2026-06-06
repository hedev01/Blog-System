FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy solution
COPY . .

# restore فقط روی solution
RUN dotnet restore BlogSystem.Api/BlogSystem.Api.csproj

# build
RUN dotnet publish BlogSystem.Api/BlogSystem.Api.csproj -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "BlogSystem.Api.dll"]