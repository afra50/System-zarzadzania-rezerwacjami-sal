# 🏢 Rezerwacje Sal - System Zarządzania

## 📌 Opis

Aplikacja **WPF** umożliwiająca zarządzanie rezerwacjami sal.  
Komunikuje się z istniejącym backendem API oraz korzysta z bazy danych **MariaDB/MySQL**.

## 🛠 Technologie

- **C# (WPF)** – aplikacja desktopowa
- **REST API (Node.js)** – komunikacja z backendem
- **MariaDB/MySQL** – baza danych
- **Google Maps API** – integracja z mapami

## 🎯 Funkcjonalności

Administratorzy:  
✅ Zarządzanie salami - edycja  
✅ Przeglądanie rezerwacji  
✅ Generowanie statystyk i wykresów  

Klienci:  
✅ Logowanie i rejestracja użytkowników  
✅ Tworzenie i anulowanie rezerwacji  
✅ Wyświetlanie mapy dojazdu do sali  

---

## 🖥 Uruchamianie aplikacji WPF

1. **Otwórz projekt w Visual Studio**
2. **Skonfiguruj adres API w kodzie - plik App.config**
   ```csharp
   <?xml version="1.0" encoding="utf-8" ?>
   <configuration>
	   <add key="ApiBaseUrl" value="http://95.215.232.175:5001/api" />
	   <add key="GoogleMapsApiKey" value="API_KEY" />
   </configuration>
   ```
3. **Uruchom aplikację (`F5`)**

---

## 📌 Autorzy

- **Emilia Kowalczyk i Julia Rojek**

## 📜 Licencja
