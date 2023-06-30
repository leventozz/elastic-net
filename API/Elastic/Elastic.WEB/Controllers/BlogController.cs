using Elastic.WEB.Services;
using Elastic.WEB.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Elastic.WEB.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogService _blogService;

        public BlogController(BlogService blogService)
        {
            _blogService = blogService;
        }

        public async Task<IActionResult> Search()
        {
			return View(await _blogService.SearchAsync(string.Empty));
		}
		[HttpPost]
		public async Task<IActionResult> Search(string searchText)
		{
            var blogList = await _blogService.SearchAsync(searchText);
			return View(blogList);
		}

		public IActionResult Save()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save(BlogCreateViewModel model)
        {
            var isSuccess = await _blogService.SaveAsync(model);

            if (!isSuccess) 
            {
                TempData["result"] = "save is unsuccessful";
                return RedirectToAction(nameof(BlogController.Save));
            }

            TempData["result"] = "save is success";
            return RedirectToAction(nameof(BlogController.Save));
        }
    }
}
