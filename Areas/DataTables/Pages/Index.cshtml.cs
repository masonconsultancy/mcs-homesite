﻿using AutoMapper;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Areas.DataTables.Pages
{
    public class IndexModel : PageModel
    {
        private readonly McsHomeSiteContext _context;
        private readonly IMapper _mapper;

        public IndexModel(McsHomeSiteContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IList<User> UserModel { get; set; } = default!;

        public async Task OnGetAsync()
        {
            UserModel = _mapper.Map<IList<User>>(await _context.Users.ToListAsync());
        }

        public PartialViewResult OnGetCrudMenuPartial(long? id)
        {
            return Partial("_CrudMenuPartial", id);
        }
    }
}
