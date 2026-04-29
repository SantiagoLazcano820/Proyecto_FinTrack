using FinTrack.Core.Entities;
using FinTrack.Core.Enum;
using FinTrack.Core.Interfaces;
using FinTrack.Infraestructure.Data;
using FinTrack.Infrastructure.Queries;

namespace FinTrack.Infraestructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IDapperContext _dapper;

        public UserRepository(FinTrackContext context, IDapperContext dapper) : base(context)
        {
            _dapper = dapper;
        }

        public async Task<User> GetUserByEmailDapperAsync(string email)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.UserByEmailMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };
                var users = await _dapper.QueryAsync<User, Role, User>(
                    sql,
                    (user, role) =>
                    {
                        user.Role = role;
                        return user;
                    },
                    new { Email = email },
                    splitOn: "Id"
                );

                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersDapperAsync()
        {
            var sql = _dapper.Provider switch
            {
                DataBaseProvider.MySql => Primero.AllUsersMySql,
                _ => throw new NotSupportedException("Provider no soportado")
            };
            return await _dapper.QueryAsync<User>(sql);
        }

        public async Task<User> GetUserByIdDapperAsync(int id)
        {
            try
            {
                var sql = _dapper.Provider switch
                {
                    DataBaseProvider.MySql => Primero.UserByIdMySql,
                    _ => throw new NotSupportedException("Provider no soportado")
                };

                return await _dapper.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener usuario por ID con Dapper: {ex.Message}");
            }
        }
    }
}
