using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Docs.SocialCards{
    public class SocialCardModel : PageModel
    {
        [BindProperty(Name = "title", SupportsGet = true)]
        public string Title { get; set; }

        [BindProperty(Name = "desc", SupportsGet = true)]
        public string Description { get; set; }

        [BindProperty(Name = "highlights", SupportsGet = true)]
        public string Highlights { get; set; }

        [BindProperty(Name = "footer", SupportsGet = true)]
        public string Footer { get; set; }

        public void OnGet()
        {
        }
    }
}