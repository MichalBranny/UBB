using System;
using System.ComponentModel.DataAnnotations;
using SystemObslugiBiblioteki.Model;
using SystemObslugiBiblioteki.Model.Dto;

namespace SystemObslugiBiblioteki.Models
{
    public class CheckOuts
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int BookId { get; set; }
        public Books Book { get; set; }
        public UserDto User { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
