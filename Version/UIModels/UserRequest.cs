using System.ComponentModel.DataAnnotations;

namespace Version.UIModels
{
    public class UserRequest
    {
        [Required]
        public string? UserName { get; set; } = string.Empty;
        public string? Password { get; set; } = string.Empty;
    }
}
