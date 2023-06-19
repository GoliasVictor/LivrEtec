using LivrEtec.GIB.RPC;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LivrEtec.GIB.Cliente.Services;

public class LEAuthenticationStateProvider : AuthenticationStateProvider
{
    IIdentidadeService IdentidadeService { get; set; }
    public LEAuthenticationStateProvider(IIdentidadeService identidadeService) {
        IdentidadeService = identidadeService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity? identity = null;
        if (IdentidadeService.IdUsuario is null || !IdentidadeService.EstaAutenticado)
        {
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Sid, "0"),
                new Claim(ClaimTypes.Name, "Anonymous"),
                new Claim(ClaimTypes.Role, "Anonymous")
            }, null);
        }
        else
        {
            var usuario = await IdentidadeService.ObterUsuario();
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes. Sid, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Login) 
            }, "LivrEtecAuth");
            foreach ( var permissao in usuario.Cargo.Permissoes )
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, permissao.Nome));
            }

        }
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }


    public void Login()
    {
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());

    }
    public Task Logout()
    {
        IdentidadeService.EstaAutenticado = false;
        IdentidadeService.IdUsuario = null;
        var task = this.GetAuthenticationStateAsync();
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        return task;
    }
}