import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { useParams } from 'react-router';
type Livro = components["schemas"]["Livro"];


function LivrosPage() {
  const [livro, setLivro] = useState<Livro | null>()
  const { id } = useParams();
  const client = useApi()

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
  return (
    <>
      {JSON.stringify(livro, null, 4)} 
      <br/>
      <button>Emprestar</button>
      <button>Editar</button>
      <button>Excluir</button>
    </>
  )
}

export default LivrosPage
