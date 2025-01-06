using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Booktex.Infrastructure.Configuration;
public class GitHubConfiguration
{
    public string AppId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string PrivateKeyFile { get; set; }
    public string BaseUrl { get; set; }
    public string FineGrainedToken { get; set; }

    private RsaSecurityKey? _privateKey;
    public RsaSecurityKey PrivateKey => _privateKey ??= InstantiatePrivateKey();

    private RsaSecurityKey InstantiatePrivateKey()
    {
        var rsa = RSA.Create();
        var privateKeyContent = File.ReadAllText(PrivateKeyFile);
        rsa.ImportFromPem(privateKeyContent);
        var privateKey = new RsaSecurityKey(rsa);
        return privateKey;
    }

    private JsonWebTokenHandler _jwtHandler = new JsonWebTokenHandler() { SetDefaultTimesOnTokenCreation = false };
    private SigningCredentials? _signingCredentials;

    private string? _authToken;
    private SecurityTokenDescriptor? _authTokenDescriptor;

    public string AuthenticationToken()
    {
        if (_authToken != null && _authTokenDescriptor != null && _authTokenDescriptor.Expires > DateTime.UtcNow.AddSeconds(10))
            return _authToken;
        var creds = _signingCredentials ??= new SigningCredentials(PrivateKey, SecurityAlgorithms.RsaSha256);
        var now = DateTime.UtcNow.AddSeconds(-10);
        _authTokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = AppId,
            IssuedAt = now,
            NotBefore = null,
            Expires = now.AddMinutes(10),
            SigningCredentials = creds
        };
        _authToken = _jwtHandler.CreateToken(_authTokenDescriptor);
        return _authToken;
    }


}
