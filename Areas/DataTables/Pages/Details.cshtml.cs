using mcs_contracts.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly mcs_homesiteContext _context;

        public DetailsModel(mcs_homesiteContext context)
        {
            _context = context;
        }

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
    }
}
