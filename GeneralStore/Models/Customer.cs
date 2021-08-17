using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GeneralStore.Models
{
    // Customer - Entity and a Model
    // Entity - as the object that will be stored in the database
    // Models - a way to pass data around an application
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string FullName => $"{this.FirstName} {this.LastName}";
    }
}