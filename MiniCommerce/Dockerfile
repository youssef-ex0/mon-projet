# Étape 1 : Build
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Copier le projet
COPY . .

# Restaurer les dépendances
RUN dotnet restore

# Compiler et publier
RUN dotnet publish -c Release -o /app/publish

# Étape 2 : Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app

# Copier les fichiers publiés
COPY --from=build /app/publish .

# Exposer le port
EXPOSE 8080

# Lancer l'application
ENTRYPOINT ["dotnet", "MiniCommerce.dll"]
