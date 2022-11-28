using Microsoft.Extensions.Logging;

namespace LivrEtec;

public interface IAcervoService
{
	IRepLivro Livros { get; init; }

}
