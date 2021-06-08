using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManager.Models.Domains

{
    public class Address
    {
        public Address()
        {
            //Inicjalizujemy liste
            Clients = new Collection<Client>();
            Users = new Collection<ApplicationUser>();

        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "Numer")]
        public string Numer { get; set; }

        [Required]
        [Display(Name = "Miejscowosc")]
        public string City { get; set; }

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        //Listy
        public ICollection<Client> Clients { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }

    }
}
