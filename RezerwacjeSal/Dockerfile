# Wybieramy obraz SDK .NET, aby zbudować aplikację
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Ustawiamy katalog roboczy w kontenerze
WORKDIR /app

# Kopiujemy plik csproj i przywracamy zależności
COPY *.csproj ./
RUN dotnet restore

# Kopiujemy resztę plików do kontenera
COPY . ./

# Budujemy aplikację
RUN dotnet publish -c Release -o /app/publish

# Teraz wybieramy obraz bazowy z tylko środowiskiem uruchomieniowym .NET (mniejszy rozmiar)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

# Ustawiamy katalog roboczy w kontenerze
WORKDIR /app

# Kopiujemy zbudowaną aplikację z obrazu 'build'
COPY --from=build /app/publish .

# Udostępniamy port 80
EXPOSE 8080

# Uruchamiamy aplikację
ENTRYPOINT ["dotnet", "RezerwacjeSal.dll"]
