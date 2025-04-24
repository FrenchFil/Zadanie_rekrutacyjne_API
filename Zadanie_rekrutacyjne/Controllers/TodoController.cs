using Microsoft.AspNetCore.Mvc;
using Zadanie_rekrutacyjne.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _service; // Pole przechowuj¹ce referencjê do serwisu TODO

    public TodoController(TodoService service) => _service = service; // Konstruktor wstrzykuj¹cy zale¿noœæ serwisu

    [HttpGet] // Zwrócenie wszystkich zadañ
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAll()); // Zwrócenie wyniku jako HTTP 200 OK z list¹ zadañ

    [HttpGet("{id}")] // Zwrócenie pojedyñczego zadania na podstawie ID
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await _service.GetById(id); // Pobranie zadania z serwisu
        return todo is null ? NotFound() : Ok(todo); // Jeœli nie znaleziono, zwracany jest kod 404 Not Found, w przeciwnym razie 200 OK
    }

    [HttpGet("incoming")]  // Zwrócenie tylko nadchodz¹cych zadañ
    public async Task<IActionResult> GetIncoming() =>
        Ok(await _service.GetIncoming()); // Zwrócenie listy nadchodz¹cych zadañ jako HTTP 200 OK

    [HttpPost]  // Tworzenie nowego zadanie
    public async Task<IActionResult> Create(ToDo todo) 
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); // Jeœli model danych jest nieprawid³owy, zwracany jest HTTP 400 Bad Request
        var created = await _service.Create(todo); // Tworzenie nowego zadania w bazie danych
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); // Zwrócenie HTTP 201 Created oraz nag³ówek Location z adresem nowego zasobu
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ToDo todo)  // Aktualizacja istniej¹cego zadanie
    {
        var result = await _service.Update(id, todo); // Próba aktualizacji zadania
        return result ? NoContent() : NotFound(); // Jeœli uda³o siê, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpPatch("{id}/percent")] // Ustawienie procentu wykonania zadania
    public async Task<IActionResult> SetPercent(int id, [FromQuery] int percent)
    {
        if (percent > 100 || percent < 0) return BadRequest(ModelState); // Jeœli procent wykonania zadania jest nieprawid³owy, zwracany jest HTTP 400 Bad Request
        var result = await _service.SetPercent(id, percent); // Próba aktualizacji procentu wykonania zadania
        return result ? NoContent() : NotFound(); // Jeœli uda³o siê, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpPatch("{id}/done")]    // Oznaczenie zadania jako zakoñczone
    public async Task<IActionResult> MarkDone(int id)
    {
        var result = await _service.MarkDone(id); // Próba oznaczenia zadania jako zakoñczone
        return result ? NoContent() : NotFound(); // Jeœli uda³o siê, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpDelete("{id}")] // Usuniêcie zadania na podstawie ID
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id); // Próba usuniêcia zadania
        return result ? NoContent() : NotFound(); // Jeœli uda³o siê, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }
}
