use mysql::{params, Pool};
use tonic::{Request, Response, Status};
use crate::livretec_rpc::*;
use crate::prelude::*;
use mysql::*;
use mysql::prelude::*;

pub struct LivrosService {
    pub pool : Pool
}


#[tonic::async_trait]
impl livros_server::Livros for LivrosService {
    async fn registrar(&self, request: Request<Livro>) -> Result<Response<Empty>, Status>{
        let mut conn = self.pool.get_conn().to_status_res()?;
        let livro = request.into_inner();
        for tag in livro.tags{
            let opexists : Option<bool> = conn.exec_first(
                "SELECT EXISTS(SELECT 1 FROM tag WHERE id == :id)",
                params!{ "id" => tag.id }
            ).to_status_res()?;
            if let Some(exists) = opexists {
                 if !exists {
                    return Err(Status::failed_precondition(""))  
                 }
            }   
        }
        conn.exec_drop(
            r"insert id, nome, descricao, arquivado FROM livros WHERE id = :id, :nome, :descricao, :arquivado",
            params! {
                "id" => livro.id,
                "nome" => livro.nome,
                "descricao" => livro.descricao,
                "arquivado" => livro.arquivado
            }
        ).to_status_res()?;
        Ok(Response::new(Empty {  }))

    }
    async fn obter(&self, request: Request<IdLivro>) -> Result<Response<Livro>, Status>{
        let mut conn = self.pool.get_conn().to_status_res()?;
        let id = request.into_inner().id;         
        let livro = conn.exec_map(
            r"SELECT id, nome, descricao, arquivado FROM livros WHERE id = :id",
            params! {
                "id" => id,
            },
            |(id, nome, descricao, arquivado)| Livro {
                id,
                nome,
                descricao,
                arquivado,
                ..Default::default()
            },
        ).to_status_res()?.into_iter().next();

        match livro {
            Some(l) => return Ok(Response::new(l)),
            None => return Err(Status::not_found("livro não existe")) 
        };
    }
    async fn buscar(&self, request: Request<ParamBusca>) -> Result<Response<ListaLivros>, Status>{
        todo!();
    }
    async fn editar(&self, request: Request<Livro>) -> Result<Response<Empty>, Status>{
        todo!();
    }
    async fn remover(&self, request: Request<IdLivro>) -> Result<Response<Empty>, Status>{
        todo!();
    }
}