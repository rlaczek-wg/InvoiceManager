using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.Domains
{
    public class Invoice
    {
        public Invoice()
        {
            InvoicePositions = new Collection<InvoicePosition>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage ="Pole Tytul jest wymagane")]
        [Display(Name = "Tytul")]
        public string Title { get; set; }

        [Display(Name = "Wartosc")]
        [Required(ErrorMessage = "Pole Wartosc jest wymagane")]
        public decimal Value { get; set; }

        [ForeignKey("MethodOfPayment")]
        [Display(Name = "Sposob platnosci")]
        [Required(ErrorMessage = "Pole Sposob platnosci jest wymagane")]
        public int MethodOfPayementId { get; set; }  

        [Display(Name = "Termin platnosci")]
        [Required(ErrorMessage = "Pole Termin platnosci jest wymagane")]
        public DateTime PayementDate { get; set; }

        [Display(Name = "Data utworzenia")]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Uwagi")]
        public string Comments { get; set; }

        [Display(Name = "Klient")]
        [Required(ErrorMessage = "Pole Klient jest wymagane")]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey("User")]  //Klucz obcy w tabeli User
        public string UserId { get; set; }  //Musi tu byc string poniewaz w tabeli z szablonu juz jest string-klasa ApplicationUser

        public MethodOfPayment MethodOfPayment { get; set; }
        public Client Client { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<InvoicePosition> InvoicePositions { get; set; }
    }
}