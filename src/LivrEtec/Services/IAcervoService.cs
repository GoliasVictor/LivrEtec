using Microsoft.Extensions.Logging;

namespace LivrEtec;

public interface IAcervoService
{
	IRepLivros Livros { get; init; }

}
