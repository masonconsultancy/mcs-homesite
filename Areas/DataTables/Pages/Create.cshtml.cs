using mcs_contracts.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class CreateModel : PageModel
    {
        private readonly mcs_homesiteContext _context;

        public CreateModel(mcs_homesiteContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public UserDto UserDto { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.UserDto == null || UserDto == null)
            {
                return Page();
            }

            _context.UserDto.Add(UserDto);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index", new { area = "DataTables" });
        }
    }
}
