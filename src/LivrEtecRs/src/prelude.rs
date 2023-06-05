use tonic::{Status};

pub trait ToStatus<T>  {
    fn to_status_res(self) -> Result<T, Status>;
}

impl<T> ToStatus<T> for Result<T,mysql::Error>
    where 
{
    fn to_status_res(self) -> Result<T, Status> {
        match  self {
            Ok(o) => return Ok(o),  
            Err(erro) => return Err(Status::internal(""))  
        } 
    }
}