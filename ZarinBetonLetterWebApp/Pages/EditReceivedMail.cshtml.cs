using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class EditReceivedMailModel : PageModel
    {
        AppDbContext _context;

        public EditReceivedMailModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public ReceivedMail _receivedMail { get; set; }

        [BindProperty]
        public IFormFile Upload { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            _receivedMail = await _context.ReceivedMails.FindAsync(id);
            if (_receivedMail == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task OnPost()
        {
            if (Upload != null)
            {
                using (var reader = new BinaryReader(Upload.OpenReadStream()))
                {
                    byte[] imageBytes = reader.ReadBytes((int)Upload.Length);
                    string base64String = Convert.ToBase64String(imageBytes);
                    _receivedMail.LetterImage = base64String;
                }
            }
            _context.ReceivedMails.Update(_receivedMail);
            await _context.SaveChangesAsync();
        }
    }
}
