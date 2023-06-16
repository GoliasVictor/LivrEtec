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

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity = null;
        if (IdentidadeService.Usuario is null)
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
            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes. Sid, IdentidadeService.Usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, IdentidadeService.Usuario.Login) 
            }, "LivrEtecAuth");
            foreach ( var permissao in IdentidadeService.Usuario.Cargo.Permissoes )
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, permissao.Nome));
            }

        }
        return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }


    public void Login()
    {
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());

    }
    public Task Logout()
    {
        IdentidadeService.EstaAutenticado = false;
        IdentidadeService.Usuario = null;
        var task = this.GetAuthenticationStateAsync();
        this.NotifyAuthenticationStateChanged(this.GetAuthenticationStateAsync());
        return task;
    }
}