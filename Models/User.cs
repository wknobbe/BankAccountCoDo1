using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccountCoDo1.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Your first name is required.")]
        [MinLength(2, ErrorMessage="Your first name must be at least 2 characters")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="Your last name is required.")]
        [MinLength(2, ErrorMessage="Your last name must be at least 2 characters")]
        public string LastName {get;set;}
        [Required(ErrorMessage="You must enter an email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        public string Email {get;set;}
        [Required(ErrorMessage="A password is required.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Your password must be at least 8 characters in length.")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get;set;}
        public List<Transaction> MyTransactions {get;set;}
    }
}