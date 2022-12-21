using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Servidor
{
    internal class RepPessoas : Repositorio, IRepPessoas
    {
        public RepPessoas(AcervoService acervoService) : base(acervoService)
        {

        }
        public async Task<Pessoa?> ObterAsync(int id)
        {
            using var BD = BDFactory.CreateDbContext();
            return await BD.Pessoas.FindAsync(id);
        }
    }
}
