using System.Text.Json.Serialization;

namespace Elastic.WEB.ViewModels
{
    public class BlogCreateViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public List<string> Tags { get; set; } = new();
    }
}
