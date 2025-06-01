# ----------- Build Stage -----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution and restore as distinct layers
COPY *.sln .
COPY BookBazaar.API/*.csproj ./BookBazaar.API/
COPY BookBazaar.Application/*.csproj ./BookBazaar.Application/
COPY BookBazaar.Infrastructure/*.csproj ./BookBazaar.Infrastructure/
COPY BookBazaar.Domain/*.csproj ./BookBazaar.Domain/

RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/BookBazaar.API
RUN dotnet publish -c Release -o /out

# ----------- Runtime Stage -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /out .

# Expose ports (optional: adjust as needed)
EXPOSE 80

ENTRYPOINT ["dotnet", "BookBazaar.API.dll"]
