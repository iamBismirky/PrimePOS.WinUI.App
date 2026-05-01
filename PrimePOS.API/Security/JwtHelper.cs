using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PrimePOS.API.Security;

public class JwtHelper
{
    private readonly IConfiguration _config;

    public JwtHelper(IConfiguration config)
    {
        _config = config;
    }

    public string GenerarToken(int usuarioId, string username, string nombre, string rol)
    {
        var jwt = _config.GetSection("Jwt");

        var keyString = jwt["Key"] ?? throw new Exception("Jwt:Key no configurado");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuarioId.ToString()),
            new Claim(ClaimTypes.Name, nombre),
            new Claim(ClaimTypes.Role, rol),

            new Claim("username", username)
        };

        var expireMinutesString = jwt["ExpireMinutes"] ?? throw new Exception("Jwt:ExpireMinutes no configurado");

        if (!int.TryParse(expireMinutesString, out int expireMinutes)) throw new Exception("Jwt:ExpireMinutes inválido");

        var token = new JwtSecurityToken(
            issuer: jwt["Issuer"],
            audience: jwt["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(expireMinutes),
            signingCredentials: creds
            );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}