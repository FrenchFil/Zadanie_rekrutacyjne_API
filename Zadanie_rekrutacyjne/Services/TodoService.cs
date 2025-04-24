
using Microsoft.EntityFrameworkCore;
using Zadanie_rekrutacyjne.Data;
using Zadanie_rekrutacyjne.Models;


namespace TodoApi.Services;

public class TodoService
{
    private readonly TodoDbContext _db; // Inicjalizacja serwisu TODO z wstrzykniętym kontekstem bazy danych.
    public TodoService(TodoDbContext db) => _db = db; // _db umożliwia dostęp do danych zapisanych w bazie za pomocą Entity Framework.

    public async Task<List<ToDo>> GetAll() => await _db.todos.ToListAsync(); // Pobieranie listy wszystkich zadań

    public async Task<ToDo?> GetById(int id) => await _db.todos.FindAsync(id); // Wyszukiwanie poszczególnych zadań poprzez ID

    public async Task<List<ToDo>> GetIncoming() // Pobieranie listy zadań z datą wygaśnięcia do tygodnia w przód
    {
        var now = DateTime.UtcNow.AddDays(1); // Ustawienie dolnej granicy dla daty
        var nextWeek = now.AddDays(7); // Ustawienie górnej granicy dla daty

        return await _db.todos
            .Where(t => t.Expiry.Date >= now && t.Expiry.Date <= nextWeek)
        .ToListAsync(); // Zwrócenie listy zadań, z datami pomiędzy dniem następnym a siedmioma dniami
    }

    public async Task<ToDo> Create(ToDo todo)
    {
        _db.todos.Add(todo); // Dodanie obiektu zadania
        await _db.SaveChangesAsync(); // Zapisanie asynchronicznie zmian do bazy danych 
        return todo; // Zwrócenie obiektu zadania
    }

    public async Task<bool> Update(int id, ToDo update)
    {
        var todo = await _db.todos.FindAsync(id); // Wyszukanie obiektu zadania o podanym przez użytkownika ID i przypisanie go do zmiennej
        if (todo == null) return false; // Sprawdzenie czy obiekt nie jest nullem, jeśli jest to zwracany jest fałsz

        todo.Title = update.Title; // Przypisanie tytułu obiektu zaktualizowanego do obiektu pobranego
        todo.Description = update.Description; // Przypisanie opisu obiektu zaktualizowanego do obiektu pobranego
        todo.Expiry = update.Expiry; // Przypisanie daty wygaśnięcia zaktualizowanego obiektu do obiektu pobranego
        todo.PercentComplete = update.PercentComplete; // Przypisanie procentu ukończenia zadania zaktualizowanego obiektu do obiektu pobranego

        await _db.SaveChangesAsync(); // Zapisanie asynchronicznie zmian do bazy danych 
        return true; // Zwrócenie true, czyli powodzenie w wykonaniu aktualizacji
    }

    public async Task<bool> SetPercent(int id, int percent)
    {
        var todo = await _db.todos.FindAsync(id); // Wyszukanie obiektu zadania o podanym przez użytkownika ID i przypisanie go do zmiennej
        if (todo == null) return false; // Sprawdzenie czy obiekt nie jest nullem, jeśli jest to zwracany jest fałsz
        todo.PercentComplete = percent; // Przypisanie procentu ukończenia zadania zaktualizowanego obiektu do obiektu pobranego
        await _db.SaveChangesAsync(); // Zapisanie asynchronicznie zmian do bazy danych 
        return true; // Zwrócenie true, czyli powodzenie w wykonaniu aktualizacji
    }

    public async Task<bool> MarkDone(int id)
    {
        return await SetPercent(id, 100); // Wywołanie metody SetPercent, która ustawia procent wykonania zadania na 100
    }

    public async Task<bool> Delete(int id)
    {
        var todo = await _db.todos.FindAsync(id); // Wyszukanie obiektu zadania o podanym przez użytkownika ID i przypisanie go do zmiennej
        if (todo == null) return false; // Sprawdzenie czy obiekt nie jest nullem, jeśli jest to zwracany jest fałsz

        _db.todos.Remove(todo); // Usunięcie obiektu z bazy danych
        await _db.SaveChangesAsync(); // Zapisanie asynchronicznie zmian do bazy danych  
        return true; // Zwrócenie true, czyli powodzenie w wykonaniu operacji usuwania
    }
}
