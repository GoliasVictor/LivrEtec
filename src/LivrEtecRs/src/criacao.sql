drop schema if exists LivrEtecRs;
create schema if not exists LivrEtecRs;
use LivrEtecRs;
create table pessoa (
	id int not null auto_increment,
	nome varchar(255) not null, 
	telefone varchar(255) not null, 
	rm varchar(255),
	primary key (id) 
);

create table autor (
	id int not null auto_increment,
	nome varchar(255) not null,
	primary key(id)
);

create  table tag (
	id int not null auto_increment,
	nome varchar(255) not null,
	primary key (id)
);

create table livro(
	id int not null auto_increment, 
	nome varchar(255) not null,
	arquivado boolean not null,
	descricao text,
    primary key (id)
);
create table tag_livro (
	id_livro int not null, 
	id_tag int not null, 
	primary key (id_livro, id_tag),
	foreign key (id_livro) references livro(id),
	foreign key (id_tag) references tag(id)
);
create table autor_livro (
	id_livro int not null, 
	id_autor int not null, 
	primary key (id_livro, id_autor),
	foreign key (id_livro) references livro(id),
	foreign key (id_autor) references autor(id)
);

create table permissao (
	id int not null auto_increment,
	nome varchar(255) not null,
	descricao text not null,
	primary key (id)
);

create table permissao_dependente (
	id_permissao int not null,
	id_dependencia int not null,
	primary key (id_permissao, id_dependencia),
	foreign key (id_permissao) references permissao(id),
	foreign key (id_dependencia) references permissao(id)
);

create table cargo (
	id int not null auto_increment, 
	nome varchar(255) not null,
	primary key (id)
);

create table permissao_cargo (
	id_permissao int not null,
	id_cargo int not null,
	foreign key (id_permissao) references permissao(id),
	foreign key (id_cargo) references cargo(id)
);

create table usuario (
	id int not null auto_increment, 
	nome varchar(255) not null,
	senha varchar(64) not null, 
	login varchar(255) not null, 
	id_cargo int not null,
	foreign key (id_cargo) references cargo(id),
	primary key (id)
);

create table emprestimo (
	id int not null auto_increment,
	id_livro int not null,
	id_pessoa int not null, 
	dt_emprestimo datetime not null,
	data_fim_emprestimo datetime not null,
	id_usuario_criador int not null, 
	fechado boolean not null,
	data_fechamento  datetime,
	atraso_justificado boolean,
	comentario text,
	id_usuario_fechador  int,
	devolvido boolean,
	justificativa_atraso text,
	primary key (id),
	foreign key (id_livro) references livro(id),
	foreign key (id_pessoa) references pessoa(id),
	foreign key (id_usuario_criador) references usuario(id),
	foreign key (id_usuario_fechador) references usuario(id)
);