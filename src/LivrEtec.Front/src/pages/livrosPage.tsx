import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
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
        {todos.map((t) =>
          <React.Fragment key={t.id}>
            <p>{ t.nome }</p>
          </React.Fragment>
        )} 
    </>
  )
}

export default LivrosPage
