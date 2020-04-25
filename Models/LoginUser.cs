using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccountCoDo1.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="You must enter an email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        public string Email {get;set;}
        [Required(ErrorMessage="A password is required.")]
        [MinLength(8, ErrorMessage="Password must be at least 8 characters.")]
        public string Password {get;set;}
    }
}