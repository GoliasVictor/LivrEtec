import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { Link, useNavigate, useParams } from 'react-router';
import { useModal } from '../hooks/useModal';
import EditarModal from '../modals/editarModal';
import ConfirmarModal from '../components/confirmarModal';
import EmprestarModal from '../modals/emprestarModal';
type Livro = components["schemas"]["Livro"];


function LivrosPage() {

  const [livro, setLivro] = useState<Livro | null>()
  const { id } = useParams();
  const client = useApi()
  const navigate = useNavigate();
  const { setModal } = useModal()!;
  const handleConfirmDeletar = ()=> {
    client.DELETE("/livros/{id}",
      {
        params: {
          path: {
            id: parseInt(id!)
          }
        }
      }
    )
    navigate("/livros")
  }
  useEffect(() => {
    
    client.GET("/livros/{id}", {
      params: {
        path: {
         id: parseInt(id!)
        }
     }
  }).then(res => {
      if(res.data != null)
        setLivro(res.data);

    });

  }, [])
  console.log(JSON.stringify(livro, null, 4))
  
  
  return (
    <>
      <p>{JSON.stringify(livro, null, 4)}</p> 
      <br/>
      <button onClick={() => {
        setModal(<EmprestarModal livro={livro!}/>)
      }}>
        Emprestar
      </button>
      <Link to={`/livros/${id}/editar`}>
        <button>Editar</button>
      </Link>
      <button onClick={() => {
        setModal(<ConfirmarModal onConfirm={handleConfirmDeletar} >
          "Vocẽ tem certeza que deseja deletar o livro?"
        </ConfirmarModal>
        )
      }}>
        Excluir</button>
    </>
  )
}

export default LivrosPage
