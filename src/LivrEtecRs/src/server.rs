use mysql::{ Pool};
use tonic::{transport::Server};


use mysql::*;
use crate::{servicos::livros_service::LivrosService, livretec_rpc::livros_server};


#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let addr = "[::1]:50051".parse()?;
    println!("insira a string de conexão para teste:");
    println!("Templaet: mysql://Livr:password@localhost:3307/db_name");
    let mut url = String::new();
    std::io::stdin().read_line(&mut url)?;
    let pool = Pool::new(&*url)?;
    let gerenciamento_sessao = LivrosService { pool };

    Server::builder()
        .add_service(livros_server::LivrosServer::new(gerenciamento_sessao))
        .serve(addr)
        .await?;
    Ok(())
}

