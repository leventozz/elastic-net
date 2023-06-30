using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
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

        public async Task<List<Blog>> SearchAsync(string searchText)
        {
            List<Action<QueryDescriptor<Blog>>> listQuery = new();

            Action<QueryDescriptor<Blog>> matchAll = (q) => q.MatchAll();

            Action<QueryDescriptor<Blog>> matchContent = (q) => q.Match
                (m => m
                .Field(f => f.Content)
                .Query(searchText));

			Action<QueryDescriptor<Blog>> titleMatchBoolPrefix= (q) => q.MatchBoolPrefix
				(m => m
				.Field(f => f.Title)
				.Query(searchText));

            if (string.IsNullOrEmpty(searchText))
                listQuery.Add(matchAll);
            else
            { 
                listQuery.Add(matchContent); 
                listQuery.Add(titleMatchBoolPrefix); 
            }


            var result = await _elasticClient.SearchAsync<Blog>(s => s.Index(indexName)
                .Size(1000).Query(q => q.Bool(b => b
                    .Should(s => s.Match(m => m.Field(f => f.Content)
                        .Query(searchText))
                            ,s=>s.MatchBoolPrefix(p=>p
                            .Field(f=>f.Title)
                            .Query(searchText))))));
                
            foreach (var hit in result.Hits)
            {
                hit.Source.Id = hit.Id;
            }
            return result.Documents.ToList();
        }
    }
}
