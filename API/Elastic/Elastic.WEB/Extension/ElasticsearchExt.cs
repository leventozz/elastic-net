using Elastic.Clients.Elasticsearch;
namespace Elastic.WEB.Extension
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!));
            var client = new ElasticsearchClient(settings);
            services.AddSingleton(client);
        }
    }
}
