using System.ComponentModel.DataAnnotations;

namespace Version.EntityModels
{
    public class KerwaUsers
    {
        [Required]
        public string? UserName { get; set; } = string.Empty;
        public string? Token { get; set; }
    }
}
