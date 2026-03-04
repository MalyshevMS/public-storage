using Microsoft.Extensions.Options;
using public_storage.Configuration;

namespace public_storage.Services;

public class PasswordService
{
    private readonly string _password;

    public PasswordService(IOptions<PasswordOptions> options)
    {
        _password = options.Value.Password;
    }

    public string GetPassword() => _password;
}