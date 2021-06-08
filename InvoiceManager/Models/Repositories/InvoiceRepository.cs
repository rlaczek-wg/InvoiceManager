using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;


namespace InvoiceManager.Models.Repositories
{
    public class InvoiceRepository
    {
      public List<Invoice> GetInvoices(string userId)
        {
            //Pobieramy wszytkie faktury wraz z informacjami o kliencie dla konkretnego uzytkownika
            using (var context = new ApplicationDbContext())
            {
                return context.Invoices.Include(x => x.Client).Where(x => x.UserId == userId).ToList();
            }
        }

        public Invoice GetInvoice(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                //Pobieramy faktury
                return context.Invoices
                    .Include(x => x.InvoicePositions) //informacje o pozycjach 
                    .Include(x => x.InvoicePositions.Select(y => y.Product)) //informacje o produktach na pozycjach
                    .Include(x => x.MethodOfPayment)
                    .Include(x => x.User)
                    .Include(x => x.User.Address) //Adresy uzytkownikow
                    .Include(x => x.Client)
                    .Include(x => x.Client.Address)
                    .Single(x => x.Id == id && x.UserId == userId);/*Interesuje nas pojedynczy rekord gdzie Id faktury bedzie rowne ID i Id uzytkownika tej faktury 
                     bedzie rowne userID przekazane do tej metody*/
       
            }
        }

        public List<MethodOfPayment> GetMethodOfPayment()
        {
            using (var context = new ApplicationDbContext())
            {
                return context.MethodOfPayments.ToList();  //Zwracamy wszytkie metody z BD.
            }
        }

        public InvoicePosition GetInvoicePosition(int invoicePositionId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                return context.InvoicePositions
                    .Include(x => x.Invoice)
                    .Single(x => x.Id == invoicePositionId && x.Invoice.UserId == userId);
                    /* Sungle - Sprawdzamy czy mamy odpowiednie ID oraz sprawdzamy czy faktura tej pozycji 
                    jest przypisana do tego uzytkownika ktory zostal przekazany w kontrolerze*/
            }
        }

        public void Add(Invoice invoice)
        {
            using (var context = new ApplicationDbContext())
            {
                //Dodajemy nowa fakture
                invoice.CreateDate = DateTime.Now;
                context.Invoices.Add(invoice);
                context.SaveChanges();
            }
        }

        public void Update(Invoice invoice)
        {
            using (var context = new ApplicationDbContext())
            {
                var invoiceToUpdate = context.Invoices
                      .Single(x => x.Id == invoice.Id && x.UserId == invoice.UserId);  //Faktura ktora jest obecnie w bazie danych

                //To co moglo zostac zmienione
                invoiceToUpdate.ClientId = invoice.ClientId;
                invoiceToUpdate.Comments = invoice.Comments;
                invoiceToUpdate.MethodOfPayementId = invoice.MethodOfPayementId;
                invoiceToUpdate.PayementDate = invoice.PayementDate;
                invoiceToUpdate.Title = invoice.Title;

                context.SaveChanges();
                Console.WriteLine("Update of invoice is done");
               
            }
        }

        public void AddPosition(InvoicePosition invoicePosition, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                //Dodajemy nowa pozycje do faktury
                /* Ponizej- Sprawdzamy czy na podstawie tego jaki zostal przekazany uzytkownik i pozycja , 
                 sprawdzamy czy w tabeli faktur istnieje taki record gdzie id faktury (x.Id) jest rowne id faktury z pozycji
                 i czy ona przynalezy do tego konkretnego uzytkownika. Jesli nie ma takiego recordu to dostaniemy wyjatek 
                 a jesli jest taki record to mozna go zaktualizowac.*/
                var invoice = context.Invoices
                    .Single(x => x.Id == invoicePosition.InvoiceId && x.UserId == userId);

                context.InvoicePositions.Add(invoicePosition);
                context.SaveChanges();
            }
        }

        public void UpdatePosition(InvoicePosition invoicePosition, string userId)
        {

            using (var context = new ApplicationDbContext())
            {
                //Pobieramy pozycje z BD ktora chcemy zaktualizowac
                var positionToUpdate = context.InvoicePositions
                    .Include(x => x.Product)
                    .Include(x => x.Invoice)
                    .Single(x => x.Id == invoicePosition.Id && x.Invoice.UserId == userId); //Single - Pobieramy pierwszy jedyny taki record  (x.Id to Id pozycji)

                positionToUpdate.Lp = invoicePosition.Lp;
                positionToUpdate.ProductId = invoicePosition.ProductId;
                positionToUpdate.Quantity = invoicePosition.Quantity;
                positionToUpdate.Value = positionToUpdate.Product.Value * invoicePosition.Value;

                context.SaveChanges();
            }

        }

        public decimal UpdateInvoiceValue(int invoiceId, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                //Pobieramy fakture
                var invoice = context.Invoices
                    .Include(x => x.InvoicePositions)
                    .Single(x => x.Id == invoiceId && x.UserId == userId);

                //Sumujemy pozycje po wartosci
                invoice.Value = invoice.InvoicePositions.Sum(x => x.Value);
                context.SaveChanges();
                return invoice.Value;
            }
        }

        public void Delete(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                //Usuwamy fakture
                var invoiceToDelete = context.Invoices
                     .Single(x => x.Id ==id && x.UserId == userId);

                Console.WriteLine("invoiceToDelete before deleting is " + invoiceToDelete);
                context.Invoices.Remove(invoiceToDelete);
                Console.WriteLine("invoiceToDelete after deleting is " + invoiceToDelete);
                context.SaveChanges();
                Console.WriteLine("invoiceToDelete after saving to database is " + invoiceToDelete);

            }
        }

        public void DeletePosition(int id, string userId)
        {
            using (var context = new ApplicationDbContext())
            {
                //Usuwamy pozycje 
                //Szukamy pozycji do usuniecia
                var positionToDelete = context.InvoicePositions
                    .Include(x => x.Invoice)  //Zalaczamy fakture dzieki czemu bedziemy mogli zidentyfikowac uzytkownika
                    .Single(x => x.Id == id && x.Invoice.UserId == userId);
                context.InvoicePositions.Remove(positionToDelete);
                context.SaveChanges();
               
            }
        }
    }
}