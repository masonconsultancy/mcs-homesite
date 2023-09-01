using AutoMapper;
using MCS.HomeSite.Areas.DataTables.Data;
using MCS.HomeSite.Areas.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MCS.HomeSite.Areas.DataTables.Pages
{
    [AutoValidateAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly McsHomeSiteContext _context;
        private readonly IMapper _mapper;

        public CreateModel(McsHomeSiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User UserModel { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.UserDto.Add(_mapper.Map<UserDto>(UserModel));
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index", new { area = "DataTables" });
        }
    }
}
