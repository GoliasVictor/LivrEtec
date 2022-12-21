using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.Testes.Doubles
{
    class RelogioStub : IRelogio
    {
        public RelogioStub(DateTime agora)
        {
            Agora = agora;
        }
        
        public DateTime Agora { get; set; }

    }
}
