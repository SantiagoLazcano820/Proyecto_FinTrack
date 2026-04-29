using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infrastructure.Queries;

namespace FinTrack.Infraestructure.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly IDapperContext _dapper;
        public CategoryRepository(FinTrackContext context, IDapperContext dapper) : base(context) 
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<Category>> GetCategoriesByUserIdDapperAsync(int userId)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.CategoriesByUserIdMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Category>(sql, new { UserId = userId });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en GetCategoriesByUserIdDapper: {ex.Message}");
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesDapperAsync()
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.AllCategoriesMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryAsync<Category>(sql);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> GetCategoryByIdDapperAsync(int id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.CategoryByIdMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<Category>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
