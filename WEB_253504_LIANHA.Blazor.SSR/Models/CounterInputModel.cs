using System.ComponentModel.DataAnnotations;

namespace WEB_253504_LIANHA.Blazor.SSR.Models
{
    public class CounterInputModel
    {
        [Required(ErrorMessage = "Count should be between 1 and 10")]
        [Range(1, 10)]
        public int Count { get; set; }
    }
}
