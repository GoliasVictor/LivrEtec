﻿namespace LivrEtec
{
    public interface ILivro
    {
        bool Arquivado { get; set; }
        List<Autor> Autores { get; set; }
        int Id { get; set; }
        string Nome { get; set; }
        List<Tag> Tags { get; set; }
    }
}