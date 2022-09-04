using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ZarinBetonLetterWebApp.Models;

namespace ZarinBetonLetterWebApp.Pages
{
    public class SettingsModel : PageModel
    {

        AppDbContext _context;

        public SettingsModel()
        {
            _context = new AppDbContext();
            _context.Database.EnsureCreated();
        }

        [BindProperty]
        public Default _default { get; set; }

        [BindProperty]
        public Slogan _slogan { get; set; }

        public async Task OnGet()
        {
            _default = await _context.Defaults.FirstOrDefaultAsync();
            if (_default == null)
            {
                _default = new Default();
                await _context.Defaults.AddAsync(_default);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IActionResult> OnPost()
        {
            _context.Defaults.Update(_default);
            await _context.SaveChangesAsync();

            var sloganOfSelectedYear = await _context.Slogans.Where(a => a.Year == _slogan.Year).FirstOrDefaultAsync();

            if(_slogan.Year!=0 || !string.IsNullOrEmpty(_slogan.Slogan1))
            {
                if (sloganOfSelectedYear == null)
                {
                    sloganOfSelectedYear = new Slogan();
                    await _context.Slogans.AddAsync(_slogan);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    sloganOfSelectedYear.Year = _slogan.Year;
                    sloganOfSelectedYear.Slogan1 = _slogan.Slogan1;
                    _context.Slogans.Update(sloganOfSelectedYear);
                    await _context.SaveChangesAsync();
                }
            }

            return Page();
        }
    }
}
