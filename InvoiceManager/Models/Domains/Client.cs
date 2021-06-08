using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceManager.Models.Domains
{
    public class Client
    {
        //Konstruktor
        public Client()
        {
            //Inicjalizujemy liste
            Invoices = new Collection<Invoice>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public int AddressId { get; set; }  //Pole jest intem czyli typem nie nulowalnym dlatego zawsze bedzie wymagane

        [Required]
        public string Email { get; set; }

        [Required]
        [ForeignKey("User")]  //Klucz obcy w tabeli User
        public string UserId { get; set; }

        public Address Address { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
        public ApplicationUser User { get; set; }
    }

   
}