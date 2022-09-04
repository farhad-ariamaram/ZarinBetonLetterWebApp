using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages.SinglePages
{
    public class SloganModel : PageModel
    {
        AppDbContext _context;

        public SloganModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        public List<Slogan> _slogans { get; set; }

        public async Task OnGet()
        {
            _slogans = await _context.Slogans.ToListAsync();
            if(_slogans==null || _slogans.Count() < 1)
            {
                _slogans = new List<Slogan>();
            }
        }
    }
}
