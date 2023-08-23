using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mcs_homesite.Pages
{
    public class IndexModel : PageModel
    {
        public readonly string ChatMessageText = "Hello I found your site";
        public readonly string ChatMessageNumber = "+447847337097";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}