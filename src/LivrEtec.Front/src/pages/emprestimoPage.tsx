import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { useParams } from 'react-router';
type Emprestimo = components["schemas"]["Emprestimo"];


function EmprestimoPage() {
  const [emprestimo, setEmprestimo] = useState<Emprestimo | null>()
  const { id } = useParams();
  const client = useApi()

  useEffect(() => {
    
    client.GET("/emprestimos").then(res => {
      
      if (res.data != null) {
        let emp = res.data.find((e) => e.id == id)
        setEmprestimo(emp);
      }

    });

  }, [])
  return (
    <>
      {JSON.stringify(emprestimo, null, 4)} 
      <br/>
      <button>Emprestar</button>
      <button>Editar</button>
      <button>Excluir</button>
    </>
  )
}

export default EmprestimoPage
