using Simpl.Mobile.Mechanic.Core.Domain;

namespace Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

public interface IUserRepository
{
    Task<bool> IsEmailPasswordValidAsync(string email, string password);
    Task<bool> DoesEmailExistAsync(string email);

    Task<bool> InsertUserAsync(Customer user);
}
