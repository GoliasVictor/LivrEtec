﻿// <auto-generated />
using System;
using LivrEtec;
using LivrEtec.Servidor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LivrEtec.Servidor.Migrations
{
    [DbContext(typeof(PacaContext))]
    partial class PacaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AutorLivro", b =>
                {
                    b.Property<int>("Autorescd")
                        .HasColumnType("int");

                    b.Property<int>("Livroscd")
                        .HasColumnType("int");

                    b.HasKey("Autorescd", "Livroscd");

                    b.HasIndex("Livroscd");

                    b.ToTable("AutorLivro");
                });

            modelBuilder.Entity("LivrEtec.Aluno", b =>
                {
                    b.Property<int>("Cd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RM")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Telefone")
                        .HasMaxLength(14)
                        .HasColumnType("varchar(14)");

                    b.HasKey("Cd");

                    b.ToTable("Alunos");
                });

            modelBuilder.Entity("LivrEtec.Autor", b =>
                {
                    b.Property<int>("cd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("cd");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("LivrEtec.Emprestimo", b =>
                {
                    b.Property<int>("Cd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AlunoCd")
                        .HasColumnType("int");

                    b.Property<string>("Comentario")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("DataDevolucao")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DataEmprestimo")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("FimDataEmprestimo")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Livrocd")
                        .HasColumnType("int");

                    b.HasKey("Cd");

                    b.HasIndex("AlunoCd");

                    b.HasIndex("Livrocd");

                    b.ToTable("Emprestimos");
                });

            modelBuilder.Entity("LivrEtec.Livro", b =>
                {
                    b.Property<int>("cd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<bool>("Arquivado")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Descricao")
                        .HasColumnType("longtext");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("cd");

                    b.ToTable("Livros");
                });

            modelBuilder.Entity("LivrEtec.Tag", b =>
                {
                    b.Property<int>("Cd")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Cd");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("LivroTag", b =>
                {
                    b.Property<int>("Livroscd")
                        .HasColumnType("int");

                    b.Property<int>("TagsCd")
                        .HasColumnType("int");

                    b.HasKey("Livroscd", "TagsCd");

                    b.HasIndex("TagsCd");

                    b.ToTable("LivroTag");
                });

            modelBuilder.Entity("AutorLivro", b =>
                {
                    b.HasOne("LivrEtec.Autor", null)
                        .WithMany()
                        .HasForeignKey("Autorescd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivrEtec.Livro", null)
                        .WithMany()
                        .HasForeignKey("Livroscd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LivrEtec.Emprestimo", b =>
                {
                    b.HasOne("LivrEtec.Aluno", "Aluno")
                        .WithMany()
                        .HasForeignKey("AlunoCd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivrEtec.Livro", "Livro")
                        .WithMany()
                        .HasForeignKey("Livrocd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aluno");

                    b.Navigation("Livro");
                });

            modelBuilder.Entity("LivroTag", b =>
                {
                    b.HasOne("LivrEtec.Livro", null)
                        .WithMany()
                        .HasForeignKey("Livroscd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LivrEtec.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsCd")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
