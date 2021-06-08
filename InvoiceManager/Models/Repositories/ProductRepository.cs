using InvoiceManager.Models.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InvoiceManager.Models.Repositories
{
    public class ProductRepository
    {
        public List<Product> GetProducts()
        {
            //Zwracamy wszytkie produkty bez podzialu na uzytkownikow poniewaz tego podzialu nie mamy zaimplementowanego w naszym przykladzie
            using (var context = new ApplicationDbContext())
            {
                return context.Products.ToList();
            }
        }

        public Product GetProduct(int productId)
        {
            //Zwracamy konkretny produkt
            using (var context = new ApplicationDbContext())
            {
                //Single - jesli nie znajdzie produktu w BD lub bedzie wiecej produktow o takim ID to zwraca wyjatek.
                return context.Products.Single(x => x.Id == productId);
            }
        }
    }
}