﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LivrEtec.Models;

public sealed class Usuario
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key, Required, InteiroPositivo(nameof(Id))]
    public int Id { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string Senha { get; set; } = null!;

    [Required]
    public string Login { get; set; } = null!;
    [Required]
    public string Nome { get; set; } = null!;
    [Required]
    public Cargo Cargo { get; set; } = null!;

    public Usuario()
    {
    }

    public Usuario(int id, string senha, string login, string nome, Cargo cargo)
    {
        Id = id;
        Senha = senha;
        Login = login;
        Nome = nome;
        Cargo = cargo;
    }
}
