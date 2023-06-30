using Elastic.Clients.Elasticsearch;
using Elastic.WEB.Models;

namespace Elastic.WEB.Repositories
{
    public class BlogRepository
    {
        private readonly ElasticsearchClient _elasticClient;
        private const string indexName = "blog";

        public BlogRepository(ElasticsearchClient client)
        {
            _elasticClient = client;
        }

        public async Task<Blog?> SaveAsync(Blog newBlog)
        {
            newBlog.Created = DateTime.Now;

            var response = await _elasticClient.IndexAsync(newBlog, x => x.Index(indexName));

            if (!response.IsValidResponse) return null;

            newBlog.Id = response.Id;
            return newBlog;
        }
    }
}
