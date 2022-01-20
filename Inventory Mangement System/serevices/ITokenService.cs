using System.Collections.Generic;
using System.Security.Claims;

namespace Inventory_Mangement_System.serevices
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}