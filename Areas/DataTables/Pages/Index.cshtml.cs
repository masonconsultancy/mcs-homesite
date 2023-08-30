using AutoMapper;
using mcs_homesite.Areas.DataTables.Data;
using mcs_homesite.Areas.Models.Users;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace mcs_homesite.Areas.DataTables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly mcs_homesiteContext _context;
        private readonly IMapper _mapper;

        public IndexModel(mcs_homesiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<User> UserModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            UserModel = _mapper.Map<IList<User>>(await _context.UserDto.ToListAsync());
        }
    }
}
