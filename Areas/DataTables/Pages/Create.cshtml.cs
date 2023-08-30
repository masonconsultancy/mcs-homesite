using AutoMapper;
using mcs_homesite.Areas.Models.Users;
using mcs_homesite.Areas.DataTables.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mcs_homesite.Areas.DataTables.Pages
{
    [AutoValidateAntiforgeryToken]
    public class CreateModel : PageModel
    {
        private readonly mcs_homesiteContext _context;
        private readonly IMapper _mapper;

        public CreateModel(mcs_homesiteContext context, IMapper mapper)
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
