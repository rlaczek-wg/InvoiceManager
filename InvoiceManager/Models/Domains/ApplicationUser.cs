using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InvoiceManager.Models.Domains;
using System.ComponentModel.DataAnnotations;

namespace InvoiceManager.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            //Inicjalizujemy listy
            Invoices = new Collection<Invoice>();
            Clients = new Collection<Client>();
        }

        [Required]
        public string Name { get; set; }
        public int AddressId { get; set; }



        public Address Address { get; set; }

        //Listy
        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Client> Clients { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}