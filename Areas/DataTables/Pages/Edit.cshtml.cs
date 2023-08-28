using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserDto = mcs_homesite.Areas.Models.Users.UserDto;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class EditModel : PageModel
    {
        private readonly mcs_homesiteContext _context;

        public EditModel(mcs_homesiteContext context)
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

            var userDto =  await _context.UserDto.FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }
            UserDto = userDto;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDtoExists(UserDto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserDtoExists(long? id)
        {
          return (id != null) && (_context.UserDto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
