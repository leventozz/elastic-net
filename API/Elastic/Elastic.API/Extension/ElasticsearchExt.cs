using Elastic.Clients.Elasticsearch;
using Elasticsearch.Net;
using Nest;

namespace Elastic.API.Extension
{
    public static class ElasticsearchExt
    {
        public static void AddElastic(this IServiceCollection services, IConfiguration configuration)
        {
            var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            var settings = new ConnectionSettings(pool);
            var client = new ElasticClient(settings);

            services.AddSingleton(client);
        }

        public static void AddElasticNew(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = new ElasticsearchClientSettings(new Uri(configuration.GetSection("Elastic")["Url"]!));

            var client = new ElasticsearchClient(settings);

            //var pool = new SingleNodeConnectionPool(new Uri(configuration.GetSection("Elastic")["Url"]!));
            //var settings = new ConnectionSettings(pool);
            //var client = new ElasticClient(settings);

            services.AddSingleton(client);
        }
    }
}
