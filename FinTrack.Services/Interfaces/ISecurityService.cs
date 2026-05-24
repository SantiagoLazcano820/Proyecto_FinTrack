using FinTrack.Core.CustomEntities;
using FinTrack.Core.Entities;

namespace FinTrack.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<Security> GetLoginByCredentials(UserLogin userLogin);
        Task RegisterUser(Security security);
    }
}
