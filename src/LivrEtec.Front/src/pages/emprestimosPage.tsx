import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { Link, useParams } from 'react-router';
type Emprestimo = components["schemas"]["Emprestimo"];


export default function EmprestimosPage() {
  const [emprestimos, setEmprestimos] = useState<Emprestimo[]>([])
  const client = useApi()

  useEffect(() => {
    
    client.GET("/emprestimos").then(res => {
      
      if (res.data != null) {
        setEmprestimos(res.data);
      }

    });

  }, [])
  return (
    <>
        {emprestimos.map((t) =>
          <React.Fragment key={t.id}>
            <Link to={"/livros/" + t.id}>
              <p>{ t.id } - { t.pessoa?.id }- { t.livro?.nome }</p>
            </Link>
          </React.Fragment>
        )} 
    </>
  )
}

