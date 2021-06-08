
using InvoiceManager.Models.Domains;
using InvoiceManager.Models.Repositories;
using Microsoft.AspNet.Identity;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceManager.Controllers
{
    [Authorize]  //Dostepny tylko dla osob zalogowanych
    public class PrintController : Controller
    {
        private InvoiceRepository _invoiceRepository = new InvoiceRepository();
       
        /* Kod testowy
        public ActionResult InvoiceTemplate(int id)
        {
            var userId = User.Identity.GetUserId();
            var invoice = _invoiceRepository.GetInvoice(id, userId);  //Podajemy id faktury i Id zalogowanego uzytkownika
            return View(invoice);  //Przekazyjemy nasz model do widoku InvoiceTemplate
        }
        */

        //Wygenerowanie PDFa
        public ActionResult InvoiceToPdf(int id)
        {
            var handle = Guid.NewGuid().ToString();
            var userId = User.Identity.GetUserId();
            var invoice = _invoiceRepository.GetInvoice(id, userId);

            //Generuje sie PDF do tablicy bajtow. Tablice bajtow przechowywujemy po stronie serwera. Jest to zapis tymczasowy po strronie serwera
            TempData[handle] = GetPdfContent(invoice);  

            return Json( new
                { 
                FileGuid = handle,
                FileName = $@"Faktura_{invoice.Id}.pdf"
                    });
        }

        //Metoda zwraca tablice bajtow
        private byte[] GetPdfContent(Invoice invoice)
        {
            var pdfResult = new ViewAsPdf(@"InvoiceTemplate", invoice)
            {
                PageSize = Size.A4,
                PageOrientation = Orientation.Portrait
            };
            return pdfResult.BuildFile(ControllerContext);
        }

        public ActionResult DownloadInvoicePdf(string fileGuid, string fileName)
        {
            //Sprawdzamy czy po stronie serwera jest zapis tymczasowy
            if (TempData[fileGuid] == null)
                throw new Exception("Blad przy probie eksportu faktury do PDF");

            var data = TempData[fileGuid] as byte[]; //rzutujemy na tablice bajtow
            return File(data, "application/pdf", fileName);  //zwracamy plik
        }


    }
}