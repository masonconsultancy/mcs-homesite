using AutoMapper;
using MCS.HomeSite.Areas.DataTables.Data;
using MCS.HomeSite.Areas.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Areas.DataTables.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly McsHomeSiteContext _context;
        private readonly IMapper _mapper;

        public DeleteModel(McsHomeSiteContext context, IMapper mapper)
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
