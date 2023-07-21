using System.ComponentModel.DataAnnotations;

namespace Version.EntityModels
{
    public class KerwaEmployee
    {

        public long  Id { get; set; }
        [Required]
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? Email { get; set; }

    }
}
