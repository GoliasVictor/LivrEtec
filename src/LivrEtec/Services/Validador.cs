using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LivrEtec.Services;
public static class Validador
{
    public static bool EhValido<T>([NotNullWhen(true)] T obj)
    {
        if (obj is null)
        {
            return false;
        }

        var contexto = new ValidationContext(obj, null, null);
        return Validator.TryValidateObject(obj, contexto, null, true);
    }
    public static void ErroSeInvalido<T>([NotNull] T obj)
    {
        if (obj is null)
        {
            throw new ValidationException($"Objeto nulo");
        }

        var contexto = new ValidationContext(obj, null, null);
        Validator.ValidateObject(obj, contexto, true);
    }
}