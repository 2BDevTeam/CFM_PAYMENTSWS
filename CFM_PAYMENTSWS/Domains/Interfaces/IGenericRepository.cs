using CFM_PAYMENTSWS.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CFM_PAYMENTSWS.Domains.Interface
{
    public interface IGenericRepository<TContext> where TContext : DbContext
    {

        public void UpsertEntity<T>(T entity,List<string> keysToExclude, List<KeyValuePair<string, object>> conditions, bool saveChanges) where T : class;
        public void Add<T>(T entity) where T : class;
        public void Update<T>(T entity) where T : class;

        public void BulkDelete<T>(IEnumerable<T> entityList) where T : class;
        public void Delete<T>(T entity) where T : class;
        public void BulkAdd<T>(IEnumerable<T> entityList) where T : class;

        public void BulkUpdate<T>(IEnumerable<T> entityList) where T : class;
        public void BulkOverWrite<T>(List<List<T>> entityLists) where T : class;
        public void BulkUpsertEntity<T>(List<T> entities, List<string> keyToExclude, bool saveChanges) where T : class;
        void SaveChanges();
        public void SaveChangesAsync();
        public Task<ResponseDTO> SaveChangesRespDTO();

    }
}
