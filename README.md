# Zadanie_rekrutacyjne_API

## 📋 Opis

To repozytorium zawiera rozwiązanie zadania rekrutacyjnego – prostego API do zarządzania zadaniami typu ToDo.

## 🛠️ Instalacja bazy danych

Aby uruchomić aplikację, należy najpierw utworzyć strukturę bazy danych PostgreSQL.

Wykonaj poniższe polecenie w swojej bazie danych:

```sql
CREATE TABLE Todos (
    Id SERIAL PRIMARY KEY,
    Title TEXT NOT NULL,
    Description TEXT NOT NULL,
    Expiry TIMESTAMP NOT NULL,
    Percent_complete INTEGER NOT NULL
);
