using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Simpl.Mobile.Mechanic.Core.Configuration;
using Simpl.Mobile.Mechanic.Core.Interfaces.Providers;

namespace Simpl.Mobile.Mechanic.Utilities;

public class JwtProvider : IJwtProvider
{
    private readonly JwtConfiguration _jwtConfig;

    public JwtProvider(IOptions<JwtConfiguration> jwtConfig)
    {
        _jwtConfig = jwtConfig.Value;
    }

    public Task<string> GenerateJwtToken(Claim[] claims, TimeSpan validFor)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.Add(validFor),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return Task.FromResult(jwt);
    }
}
