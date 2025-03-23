import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { Link } from 'react-router';
type Livro = components["schemas"]["Livro"];


function LivrosPage() {
  const [todos, setTodos] = useState<Livro[]>([])
  const client = useApi()

  useEffect(() => {
    
    client.GET("/livros").then(res => {
      console.log(res.data);
      if(res.data != null)
        setTodos(res.data);

    });

  }, [])
  return (
    <>
      <Link to="/livros/criar">
        <button>Adicionar Livro</button>
      </Link>
        {todos.map((t) =>
          <React.Fragment key={t.id}>
            <Link to={"/livros/" + t.id}>
              <p>{ t.nome }</p>
            </Link>
          </React.Fragment>
        )} 
    </>
  )
}

export default LivrosPage
