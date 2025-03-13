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

✅ Logowanie i rejestracja użytkowników  
✅ Zarządzanie salami (dodawanie, edycja, usuwanie)  
✅ Tworzenie i edytowanie rezerwacji  
✅ Wyświetlanie mapy dojazdu do sali  
✅ Generowanie statystyk i wykresów

---

## 🖥 Uruchamianie aplikacji WPF

1. **Otwórz projekt w Visual Studio**
2. **Skonfiguruj adres API w kodzie (`ApiService.cs`)**
   ```csharp
   private readonly string _baseUrl = "http://adres-twojego-serwera:5001/api";
   ```
3. **Uruchom aplikację (`F5`)**

---

## 📌 Autorzy

- **Emilia Kowalczyk i Julia Rojek**

## 📜 Licencja
