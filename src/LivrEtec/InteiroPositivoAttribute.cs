using System.ComponentModel.DataAnnotations;

namespace LivrEtec
{
	public sealed class InteiroPositivoAttribute : RangeAttribute
	{
		public InteiroPositivoAttribute(string nome) : base(0, int.MaxValue)
		{
			ErrorMessage = $"O valor deve de {nome} deve estar entre 0 e " + int.MaxValue;
		}
	}
}