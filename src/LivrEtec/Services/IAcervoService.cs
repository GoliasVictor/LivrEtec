using Microsoft.Extensions.Logging;

namespace LivrEtec;

public interface IAcervoService
{
	ILivroService Livros { get; init; }
	
}
