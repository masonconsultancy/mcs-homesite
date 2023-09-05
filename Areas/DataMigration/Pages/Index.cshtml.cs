using AutoMapper;
using MCS.HomeSite.Data;
using MCS.HomeSite.Data.Models.Migration;
using MCS.HomeSite.Data.Models.Users;
using MCS.HomeSite.Handlers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MCS.HomeSite.Areas.DataMigration.Pages
{
    public class IndexModel : PageModel
    {
        private readonly McsHomeSiteDestContext _context;
        private readonly IMapper _mapper;
        private readonly IMigrationHandler _migrateHandler;

        public IndexModel(McsHomeSiteDestContext context, IMapper mapper, IMigrationHandler migrateHandler)
        {
            _context = context;
            _mapper = mapper;
            _migrateHandler = migrateHandler;
        }

        public IList<MigrationLogDto> MigrationLogs { get; set; } = default!;
        public IList<UserDto> Users { get; set; } = default!;

        public async Task OnGetAsync()
        {
            MigrationLogs = await _context.MigrationLogs.ToListAsync();
            Users = await _context.Users.ToListAsync();
        }

        public async Task<PartialViewResult> OnGetMigrationLogsPartial()
        {
            var cts = new CancellationTokenSource();
            await _migrateHandler.Process(cts).ConfigureAwait(false);
            var result = await _migrateHandler.Result().ConfigureAwait(false);
            return Partial("_MigrationLogsPartial", result);
        }
    }
}
