using SparkybitTest.Domain.Models;

namespace SparkybitTest.Domain.Services;

public interface IUserService
{
    Task CreateAsync(string name, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<UserModel>> GetUsersAsync(CancellationToken cancellationToken = default);
}