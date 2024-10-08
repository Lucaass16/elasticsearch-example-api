using Nest;

namespace ElasticAPI.Services
{
    public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
    {

        private readonly IElasticClient _client;

        public ElasticSearchService(Uri elasticUri) 
        {
            var settings = new ConnectionSettings(elasticUri)
                                .DefaultIndex("user");

            _client = new ElasticClient(settings);
        }

        public async Task<IndexResponse> IndexDocument(T document)
        {
            var response = await _client.IndexDocumentAsync(document);

            return response;
        }                       

        public async Task<T> GetDocument(string id)
        {
            var response = await _client.GetAsync<T> (id);

            if (!response.IsValid || response.Source == null) { throw new Exception($"Erro ao buscar o doumento com id {id}"); }

            return response.Source;
        }

        public async Task<IEnumerable<T>?> SearchDocument(string query)
        {
            var searchResponse = await _client.SearchAsync<T>(s => s
                        .Query(q => q
                            .Match(m => m
                                .Field("_all")
                                .Query(query)
                            )
                        )
                    );

            if (!searchResponse.IsValid)
            {
                return null;
            }

            return searchResponse.Documents;
        }

        public async Task<UpdateResponse<T>> Update(string index, T model, string id)
        {
            var response = await _client.UpdateAsync<T, T>(id, u => u.Index(index).Doc(model));

            return response;
        }

        public async Task<IEnumerable<T>> SearchAllWithPagination(int page, int pageSize)
        {
            var response = await _client.SearchAsync<T, T>(s => s
                .Query(q => q
                .MatchAll())
                .From((page - 1) * pageSize)
                .Size(pageSize));

            if (!response.IsValid) { throw new Exception("Ocorreu algum erro ao buscar"); }

            return response.Documents;
        }
    }
}
