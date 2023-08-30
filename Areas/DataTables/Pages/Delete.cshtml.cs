using AutoMapper;
using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly mcs_homesiteContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(mcs_homesiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var userDto = await _context.UserDto.FindAsync(id);

            if (userDto == null) return RedirectToPage("./Index");

            _context.UserDto.Remove(userDto);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
