using System.ComponentModel.DataAnnotations;

namespace Zadanie_rekrutacyjne.Models
{
    public class ToDo
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime Expiry { get; set; }

        [Range(0, 100, ErrorMessage = "Wartość musi być między 0 a 100.")]
        public int PercentComplete { get; set; }
    }
}
