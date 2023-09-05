using AutoMapper;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Areas.DataTables.Pages
{
    public class EditModel : PageModel
    {
        private readonly McsHomeSiteContext _context;
        private readonly IMapper _mapper;

        public EditModel(McsHomeSiteContext context, IMapper mapper)
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

            var userDto =  await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (userDto == null)
            {
                return NotFound();
            }
            UserModel = _mapper.Map<User>(userDto);
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

            _context.Attach(_mapper.Map<UserDto>(UserModel)).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesWithAuditAsync(default,true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserDtoExists(UserModel.Id))
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
          return (id != null) && (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
