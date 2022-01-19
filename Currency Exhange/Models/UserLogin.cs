using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace UserLogin.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Please enter User ID")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        public string Password { get; set; }
    }
}