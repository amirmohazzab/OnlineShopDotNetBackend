

namespace Infrastructure.Models;

public class Configs
{
    public string TokenKey { get; set; }

    public int TokenTimeout { get; set; }

    public int RefreshTokenTimeout { get; set; }
}