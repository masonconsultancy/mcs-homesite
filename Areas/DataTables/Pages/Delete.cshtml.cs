using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserDto = mcs_homesite.Areas.Models.Users.UserDto;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly mcs_homesiteContext _context;

        public DeleteModel(mcs_homesiteContext context)
        {
            _context = context;
        }

        [BindProperty]
      public UserDto UserDto { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.UserDto == null)
            {
                return NotFound();
            }

            var userdto = await _context.UserDto.FirstOrDefaultAsync(m => m.Id == id);

            if (userdto == null)
            {
                return NotFound();
            }
            else 
            {
                UserDto = userdto;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.UserDto == null)
            {
                return NotFound();
            }
            var userdto = await _context.UserDto.FindAsync(id);

            if (userdto != null)
            {
                UserDto = userdto;
                _context.UserDto.Remove(UserDto);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
