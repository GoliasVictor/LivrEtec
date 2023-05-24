use datetime::Instant;
pub type Id = i32;
pub type Senha = String;
pub enum Recurso<T> {
	NaoCarregado(Id),
	Carregado(T)
}
pub struct Pessoa {
	pub id: Id,
	pub nome : String, 
	pub telefone: String, 
	pub rm:  Option<String>
}
pub struct Autor {
	pub id: Id, 
	pub nome: String,
}

pub struct AutorLivro {
	pub id_autor : Id,
	pub id_livro : Id
}
pub struct Livro {
	pub id : Id,
    pub nome : String,
    pub descricao : Option<String>,
    pub arquivado : bool
}

pub struct TagLivro {
	pub id_tag: Id,
	pub id_livro : Id
}

pub struct Tag {
	pub id : Id,
	pub nome: String
}
pub struct Cargo {
	pub id : Id,
	pub nome: String,
	pub permissoes : Vec<Permissao>
}
pub struct  Permissao {
	pub id : Id,
	pub nome: String,
	pub descricao : String,
	pub dependencias : Vec<Permissao>
}
pub struct  Usuario {
	pub id : Id,
	pub senha : Senha,
	pub login : String, 
	pub cargo : Recurso<Cargo>
}

pub struct Emprestimo {
	pub id:Id,
	pub livro : Recurso<Livro>,
	pub pessoa : Recurso<Pessoa>,
	pub usuario_criador : Recurso<Usuario>,
	pub data_emprestimo: Instant,
	pub fim_data_emprestimo: Instant,
	pub estado : EstadosEmprestimo
}
pub enum EstadosEmprestimo {
	Aberto,
	Fechado {
		data_fechamento : Instant,
		usuario_fechador : Recurso<Usuario>,
		devolvido : bool,
		estado_devolucao :EstadoDevolucao,
	}
}
pub enum EstadoDevolucao {
	Devolvido  {
		estado_atraso : EstadoAtraso
	},
	NaoDevolvido
}
pub enum EstadoAtraso {
	Justificado(String),
	NaoJustificado
}
