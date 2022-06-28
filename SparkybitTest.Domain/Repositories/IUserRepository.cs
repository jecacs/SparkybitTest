using SparkybitTest.Domain.Models;

namespace SparkybitTest.Domain.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(string name, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default);
}