using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _context.ReceivedMails.Update(_receivedMail);
            await _context.SaveChangesAsync();
        }
    }
}
