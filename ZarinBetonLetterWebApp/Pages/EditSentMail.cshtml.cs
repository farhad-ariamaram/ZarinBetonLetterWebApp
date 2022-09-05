using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class EditSentMailModel : PageModel
    {
        AppDbContext _context;

        public EditSentMailModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public SentMail _sentMail { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            _sentMail = await _context.SentMails.FindAsync(id);
            if (_sentMail == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task OnPost()
        {
            _context.SentMails.Update(_sentMail);
            await _context.SaveChangesAsync();
        }
    }
}
