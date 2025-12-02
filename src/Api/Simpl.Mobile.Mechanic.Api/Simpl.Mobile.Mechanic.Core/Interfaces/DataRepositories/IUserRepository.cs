namespace Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

public interface IUserRepository
{
  Task<bool> IsEmailPasswordValidAsync(string email, string password);
}
