// User.cs

using System.ComponentModel.DataAnnotations;

namespace SystemObslugiBiblioteki.Model
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public bool IsAdmin { get; set; }
    }
}
