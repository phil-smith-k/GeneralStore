using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStore.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ItemCount { get; set; }

        [Required]
        public DateTime DateOfTransaction { get; set; }

        // Is the primary key of another table
                         // name of the navigation property
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        [ForeignKey(nameof(Product))]
        public string Sku { get; set; }
        public virtual Product Product { get; set; }


        //SQL - we can't have tables inside of tables
        // In our entities (POCOs that get mapped to SQL tables) anytime we are referencing another class we want to use virtual

        // when we use that virtual keyword with Entity Framework it helps us to save memory when we are fetching data from the Database
    }
}