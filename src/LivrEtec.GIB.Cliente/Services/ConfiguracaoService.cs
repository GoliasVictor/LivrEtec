using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
namespace LivrEtec.GIB.Cliente.Services;
internal class ConfiguracaoService : IConfiguracaoService
{
    public string this[string key]
    {
        get => Get(key, null);
        set => Set(key, value);
    }
    public string Get(string key, string defaultValue)
    {
        return Preferences.Get(key, defaultValue);
    }
    public void Set(string key, string value)
    {
        Preferences.Set(key, value);
    }
}
