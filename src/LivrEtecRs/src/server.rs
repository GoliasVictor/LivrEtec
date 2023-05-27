use mysql::{params, Pool};
use tonic::{transport::Server, Request, Response, Status};
use livretec_rpc::*;
use mysql::*;
use mysql::prelude::*;

pub mod livretec_rpc {
    tonic::include_proto!("livr_etec_rpc"); // The string specified here must match the proto package name
}


trait ToStatus<T>  {
    fn to_status_res(self) -> Result<T, Status>;
}





impl<T> ToStatus<T> for Result<T,Error>
    where {
    fn to_status_res(self) -> Result<T, Status> {
        match  self {
            Ok(o) => return Ok(o),
            Err(erro) => return Err(Status::internal(""))  
        } 
    }
}

pub struct Livros {
    pool : Pool
}


#[tonic::async_trait]
impl livros_server::Livros for Livros {
    async fn registrar(&self, request: Request<Livro>) -> Result<Response<Empty>, Status>{
        todo!();
    }
    async fn obter(&self, request: Request<IdLivro>) -> Result<Response<Livro>, Status>{
        let mut conn = self.pool.get_conn().to_status_res()?;
        let id = request.into_inner().id;         
        let livro = conn.exec_map(
            r"SELECT id, nome, descricao, arquivado FROM livros WHERE id = id",
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

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let addr = "[::1]:50051".parse()?;
    println!("insira a string de conexão para teste:");
    println!("Templaet: mysql://Livr:password@localhost:3307/db_name");
    let mut url = String::new();
    std::io::stdin().read_line(&mut url)?;
    let pool = Pool::new(&*url)?;
    let gerenciamento_sessao = Livros{ pool };

    Server::builder()
        .add_service(livros_server::LivrosServer::new(gerenciamento_sessao))
        .serve(addr)
        .await?;
    Ok(())
}

