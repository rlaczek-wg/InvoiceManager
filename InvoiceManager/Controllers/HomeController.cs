using InvoiceManager.Models.Domains;
using InvoiceManager.Models.Repositories;
using InvoiceManager.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{
    public class HomeController : Controller
    {

        private InvoiceRepository _invoiceRepository = new InvoiceRepository();
        private ClientRepository _clientRepository = new ClientRepository();
        private ProductRepository _productRepository = new ProductRepository();

        


        [Authorize]
        public ActionResult Index()
        {
            //Wyswietlamy faktury z bazy danych.
            var userId = User.Identity.GetUserId();  //Pobieramy Id zalogowanego uzytkownika
            //Zwracamy faktury dla zalogowanego uzytkownika
            var invoices = _invoiceRepository.GetInvoices(userId);

            /*
            //Tworzymy liste faktur na sztywno
            var invoices = new List<Invoice>
            {
                new Invoice
                {
                    Id= 1,
                    Title = "Fa/01/2020",
                    CreateDate = DateTime.Now,
                    PayementDate = DateTime.Now,
                    Value = 999,
                    Client = new Client {Name = "Klient 1" }
                },

                new Invoice
                {
                    Id= 2,
                    Title = "Fa/02/2020",
                    CreateDate = DateTime.Now,
                    PayementDate = DateTime.Now,
                    Value = 999,
                    Client = new Client {Name = "Klient 2" }
                }
            };
            */
            return View(invoices);
        }

        public ActionResult Invoice(int id=0)
        {
            var userId = User.Identity.GetUserId();  //Pobieramy Id zalogowanego uzytkownika

            //Jesli zostanie przekazane id 0 to stworzymy nowa fakture a w pzreciwnym wypadku pobierzemy fakture z repozytorium
            var invoice = id == 0 ?
                GetNewInvoice(userId) :
                _invoiceRepository.GetInvoice(id, userId);
            var vm = PrepareInvoiceVM(invoice, userId);

            /*
            EditInvoiceViewModel vm = null;

            if (id == 0)
            {
                //Wyswietlamy nowa fakture
                //Tworzymy nowy ViewModel i pozniej przekazujemy go do Widoku
                vm = new EditInvoiceViewModel
                {
                    Clients = new List<Client>
                    {
                    new Client { Id = 1, Name = "Klient 1" }
                    },

                    MethodOfPayment = new List<MethodOfPayment>
                    {
                    new MethodOfPayment { Id = 1, Name = "Przelew" },
                    },
                    Heading = "Edycja faktury",
                    Invoice = new Invoice()
                };
            }
            else
            {
                vm = new EditInvoiceViewModel
                {
                    Clients = new List<Client>
                    {
                    new Client { Id = 1, Name = "Klient 1" }
                    },

                    MethodOfPayment = new List<MethodOfPayment>
                    {
                    new MethodOfPayment { Id = 1, Name = "Przelew" },
                    },

                    Heading = "Edycja faktury",
                    Invoice = new Invoice
                    { 
                        ClientId = 1,
                        Comments ="Test comment",
                        CreateDate = DateTime.Now,
                        PayementDate = DateTime.Now,
                        MethodOfPayementId = 1,
                        Id =1,
                        Value = 100,
                        Title = "FA/10/2020",
                        InvoicePosition = new List<InvoicePosition>
                        { 
                            new InvoicePosition
                            {
                                Id = 1,
                                Lp = 1,
                                Product = new Product {Name = "Produkt1" },
                                Quantity = 2,
                                Value = 50
                            },

                              new InvoicePosition
                            {
                                Lp = 2,
                                Product = new Product {Name = "Produkt2" },
                                Quantity = 3,
                                Value = 501
                            }

                        }
                    }
                };

            } */

            return View(vm);
        }

        private EditInvoiceViewModel PrepareInvoiceVM(Invoice invoice, string userId)
        {
            return new EditInvoiceViewModel
            {
                Invoice = invoice,
                //Jesli invoiceid rowne 0 to naglowek "Dodawanie nowej faktury" a jesli rozne od 0 to naglowek "Faktura"
                Heading = invoice.Id == 0 ? "Dodawanie nowej faktury" : "Faktura",
                Clients = _clientRepository.GetClients(userId),
                MethodOfPayment = _invoiceRepository.GetMethodOfPayment()
            };
        }

        private Invoice GetNewInvoice(string userId)
        {
            return new Invoice
            {
                UserId = userId,
                CreateDate = DateTime.Now,
                PayementDate = DateTime.Now.AddDays(7)

            };
        }

        public ActionResult InvoicePosition(int invoiceId, int invoicePositionId = 0)  //invoicePositionId domyslnie Zero
        {
            var userId = User.Identity.GetUserId();  //Pobieramy Id zalogowanego uzytkownika
            var invoicePosition = invoicePositionId == 0 ?
                GetNewPosition(invoiceId, invoicePositionId) :
                _invoiceRepository.GetInvoicePosition(invoicePositionId, userId);

            var vm = PrepareInvoicePositionVM(invoicePosition);

            /*
            EditInvoicePositionViewModel vm = null;

            //Jesli 0 to bedzie dodawanie nowej pozycji do faktury a jesli nie 0 to bedzie edycja
            if (invoicePositionId == 0)
            {
                vm = new EditInvoicePositionViewModel
                {
                    InvoicePosition = new InvoicePosition(),
                    Heading = "Dodawanie nowej pozycji",
                    Products = new List<Product> 
                    { 
                        new Product {Id = 1, Name = "Product1" } 
                    }
                };
            }
            else
            {
                vm = new EditInvoicePositionViewModel
                {
                    InvoicePosition = new InvoicePosition
                    {
                        Lp = 1, Value = 100, Quantity = 2, ProductId = 1
                    },

                    Heading = "Dodawanie nowej pozycji",
                    Products = new List<Product>
                    {
                        new Product {Id = 1, Name = "Product1" }
                    }
                };
            }
            */
            return View(vm);
        }

        private EditInvoicePositionViewModel PrepareInvoicePositionVM(InvoicePosition invoicePosition)
        {
            return new EditInvoicePositionViewModel
            {
                InvoicePosition = invoicePosition,
                Heading = invoicePosition.Id == 0 ? "Dodawanie nowej pozycji" : "Pozycja",
                Products = _productRepository.GetProducts()

            };
        }

        private InvoicePosition GetNewPosition(int invoiceId, int invoicePositionId)
        {
            return new InvoicePosition
            {
                InvoiceId = invoiceId,
                Id = invoicePositionId
            };
        }

        //Oczekujemy ze z formularza zostanie przekazany objekt typu Invoice
        [HttpPost] //Dzieki temu jest to akcja Post/metoda Post
        [ValidateAntiForgeryToken]
        public ActionResult Invoice(Invoice invoice)
        {
            var userId = User.Identity.GetUserId();  //Pobieramy Id zalogowanego uzytkownika
            invoice.UserId = userId;

            if (!ModelState.IsValid)
            {
                var vm = PrepareInvoiceVM(invoice, userId);
                return View("Invoice", vm);
            }

            if (invoice.Id == 0) 
            {
                _invoiceRepository.Add(invoice);
            }
            else
            {
                _invoiceRepository.Update(invoice);
            }
            //Przekierowanie spowrotem do listy wszytkich faktur
            return RedirectToAction("Index"); 
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InvoicePosition(InvoicePosition invoicePosition)
        {
            var userId = User.Identity.GetUserId();  //Pobieramy Id zalogowanego uzytkownika takze ze wzgledow bezpieczenstwa pobieramy w tym miejscu
            var product = _productRepository.GetProduct(invoicePosition.ProductId);

            if (!ModelState.IsValid)
            {
                var vm = PrepareInvoicePositionVM(invoicePosition);
                return View("InvoicePosition", vm);
            }


            invoicePosition.Value = invoicePosition.Quantity * product.Value;

            if (invoicePosition.Id == 0)
            {
                _invoiceRepository.AddPosition(invoicePosition, userId);
            }
            else
            {
                _invoiceRepository.UpdatePosition(invoicePosition, userId);
            }

            _invoiceRepository.UpdateInvoiceValue(invoicePosition.InvoiceId, userId);


            return RedirectToAction("Invoice", new {id = invoicePosition.InvoiceId });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                _invoiceRepository.Delete(id, userId); //Usuwamy fakture
            }
            catch (Exception exception)
            {
                //W tym miejscu powinno byc Logowanie do pliku ale my tego nie bedziemy robic w tym przykladzie.
                return Json(new { Success = false, Message = exception.Message});
            }
          
            //Zwracamy nowy objekt Json z wlasciwosca Success ustawiona na True
            return Json(new {Success = true }); //Zwracamy Jason poniewaz akcja bedzie wywolywana za pomoca Ajaxa
        }

        [HttpPost]
        public ActionResult DeletePosition(int id, int invoiceId)
        {
            var invoiceValue = 0m; //m znaczy cecimal

            try
            {
                var userId = User.Identity.GetUserId();
                _invoiceRepository.DeletePosition(id, userId); //Usuwamy pozycje
                invoiceValue = _invoiceRepository.UpdateInvoiceValue(invoiceId, userId);

            }
            catch (Exception exception)
            {

                return Json(new { Success = false, Message = exception.Message });
            }

            return Json(new { Success = true, InvoiceValue = invoiceValue }); 
        }



        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
         
            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}