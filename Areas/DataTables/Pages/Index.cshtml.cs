using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using UserDto = mcs_homesite.Areas.Models.Users.UserDto;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly mcs_homesiteContext _context;

        public IndexModel(mcs_homesiteContext context)
        {
            _context = context;
        }

        public IList<UserDto> UserDto { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.UserDto != null)
            {
                UserDto = await _context.UserDto.ToListAsync();
            }
        }
    }
}
