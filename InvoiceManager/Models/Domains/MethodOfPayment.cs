
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace InvoiceManager.Models.Domains
{
    public class MethodOfPayment
    {
        //Konstruktor
        public MethodOfPayment()
        {
            //Inicjalizujemy liste
            Invoices = new Collection<Invoice>(); 
        }


        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        //Lista faktur
        public ICollection<Invoice> Invoices { get; set; }

    }
}