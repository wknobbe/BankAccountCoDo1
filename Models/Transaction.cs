using System;
using System.ComponentModel.DataAnnotations;

namespace BankAccountCoDo1.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId {get;set;}
        [Required]
        public decimal Amount {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        [Required]
        public int UserId {get;set;}
        public User User {get;set;}
    }
}