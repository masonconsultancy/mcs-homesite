using AutoMapper;
using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly mcs_homesiteContext _context;
        private readonly IMapper _mapper;

        public DetailsModel(mcs_homesiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

      public User UserModel { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDto = await _context.UserDto.FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }
            else 
            {
                UserModel = _mapper.Map<User>(userDto);
            }
            return Page();
        }
    }
}
