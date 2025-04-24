using Microsoft.AspNetCore.Mvc;
using Zadanie_rekrutacyjne.Models;
using TodoApi.Services;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _service; // Pole przechowuj�ce referencj� do serwisu TODO

    public TodoController(TodoService service) => _service = service; // Konstruktor wstrzykuj�cy zale�no�� serwisu

    [HttpGet] // Zwr�cenie wszystkich zada�
    public async Task<IActionResult> GetAll() =>
        Ok(await _service.GetAll()); // Zwr�cenie wyniku jako HTTP 200 OK z list� zada�

    [HttpGet("{id}")] // Zwr�cenie pojedy�czego zadania na podstawie ID
    public async Task<IActionResult> GetById(int id)
    {
        var todo = await _service.GetById(id); // Pobranie zadania z serwisu
        return todo is null ? NotFound() : Ok(todo); // Je�li nie znaleziono, zwracany jest kod 404 Not Found, w przeciwnym razie 200 OK
    }

    [HttpGet("incoming")]  // Zwr�cenie tylko nadchodz�cych zada�
    public async Task<IActionResult> GetIncoming() =>
        Ok(await _service.GetIncoming()); // Zwr�cenie listy nadchodz�cych zada� jako HTTP 200 OK

    [HttpPost]  // Tworzenie nowego zadanie
    public async Task<IActionResult> Create(ToDo todo) 
    {
        if (!ModelState.IsValid) return BadRequest(ModelState); // Je�li model danych jest nieprawid�owy, zwracany jest HTTP 400 Bad Request
        var created = await _service.Create(todo); // Tworzenie nowego zadania w bazie danych
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created); // Zwr�cenie HTTP 201 Created oraz nag��wek Location z adresem nowego zasobu
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, ToDo todo)  // Aktualizacja istniej�cego zadanie
    {
        var result = await _service.Update(id, todo); // Pr�ba aktualizacji zadania
        return result ? NoContent() : NotFound(); // Je�li uda�o si�, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpPatch("{id}/percent")] // Ustawienie procentu wykonania zadania
    public async Task<IActionResult> SetPercent(int id, [FromQuery] int percent)
    {
        if (percent > 100 || percent < 0) return BadRequest(ModelState); // Je�li procent wykonania zadania jest nieprawid�owy, zwracany jest HTTP 400 Bad Request
        var result = await _service.SetPercent(id, percent); // Pr�ba aktualizacji procentu wykonania zadania
        return result ? NoContent() : NotFound(); // Je�li uda�o si�, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpPatch("{id}/done")]    // Oznaczenie zadania jako zako�czone
    public async Task<IActionResult> MarkDone(int id)
    {
        var result = await _service.MarkDone(id); // Pr�ba oznaczenia zadania jako zako�czone
        return result ? NoContent() : NotFound(); // Je�li uda�o si�, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }

    [HttpDelete("{id}")] // Usuni�cie zadania na podstawie ID
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.Delete(id); // Pr�ba usuni�cia zadania
        return result ? NoContent() : NotFound(); // Je�li uda�o si�, zwraca 204 No Content, w przeciwnym razie 404 Not Found
    }
}
