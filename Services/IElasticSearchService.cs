using Nest;

namespace ElasticAPI.Services
{
    public interface IElasticSearchService<T> where T : class
    {
        public Task<IndexResponse> IndexDocument(T document);
        public Task<T> GetDocument(string id);
        public Task<IEnumerable<T>?> SearchDocument(string query);
        public Task<IEnumerable<T>> SearchAllWithPagination(int page, int pageSize);
        public Task<UpdateResponse<T>> Update(string index, T model, string id);
    }
}
