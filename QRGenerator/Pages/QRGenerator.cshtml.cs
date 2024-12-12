using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QRCoder;
using System;
using System.IO;

namespace QRGenerator.Pages
{
    public class QRGeneratorModel : PageModel
    {
        [BindProperty]
        public string? Text { get; set; }

        public string? QRCodeImageData { get; set; }

        public void OnGet()
        {
            QRCodeImageData = null;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(Text))
            {
                ModelState.AddModelError("Text", "Please enter some text.");
                return Page();
            }

            using (var qrGenerator = new QRCodeGenerator())
            {
                var qrCodeData = qrGenerator.CreateQrCode(Text, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new PngByteQRCode(qrCodeData);
                var qrCodeBytes = qrCode.GetGraphic(20);

                QRCodeImageData = Convert.ToBase64String(qrCodeBytes);
            }

            return Page();
        }
    }
}
