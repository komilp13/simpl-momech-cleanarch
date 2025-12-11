using System.Security.Claims;

namespace Simpl.Mobile.Mechanic.Core.Interfaces.Providers;

public interface IJwtProvider
{
    Task<string> GenerateJwtToken(Claim[] claims, TimeSpan validFor);
}
