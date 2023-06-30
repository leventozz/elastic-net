using Elastic.WEB.Models;
using Elastic.WEB.Repositories;
using Elastic.WEB.ViewModels;

namespace Elastic.WEB.Services
{
    public class BlogService
    {
        private readonly BlogRepository _blogRepository;

        public BlogService(BlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task<bool> SaveAsync(BlogCreateViewModel blogCreateViewModel)
        {
            Blog newBlog = new();
            newBlog.Title = blogCreateViewModel.Title;
            newBlog.Content = blogCreateViewModel.Content;
            newBlog.UserId = Guid.NewGuid();
            newBlog.Tags = blogCreateViewModel.Tags.Split(',');

            var created = await _blogRepository.SaveAsync(newBlog);
            return created != null;
        }

        public async Task<List<BlogViewModel>> SearchAsync(string searchText)
        {
            var blogList =  await _blogRepository.SearchAsync(searchText);
            return blogList.Select(b => new BlogViewModel()
            {
                Id = b.Id,
                Content = b.Content,
                Title = b.Title,
                Created = b.Created.ToShortDateString(),
                Tags = string.Join(",",b.Tags),
                UserId = b.UserId.ToString()
            }).ToList();
        }
    }
}
