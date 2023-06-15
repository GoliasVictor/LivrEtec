namespace LivrEtec.GIB.Cliente.Services;

internal interface IConfiguracaoService
{
    string this[string key] { get; set; }

    string Get(string key, string defaultValue);
    void Set(string key, string value);
}