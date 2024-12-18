import { useEffect, useState } from 'react'
import './App.css'
import React from 'react'
import createClient, { Middleware } from "openapi-fetch";
import type { components, paths } from "./lib/api/v1"; 
type Livro = components["schemas"]["Livro"];

const accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIyIiwibmJmIjoxNzM0NTA1MDk1LCJleHAiOjE3MzQ1MjY2OTUsImlhdCI6MTczNDUwNTA5NX0.TQKUyTeCW2VKvguIVO53dUs3zGFLzIb8yfiIxCOX6Lg"

const authMiddleware: Middleware = {
  async onRequest({ request }) {
    request.headers.set("Authorization", `Bearer ${accessToken}`);
    return request;
  },
};

const client = createClient<paths>({ baseUrl: "http://localhost:5259" })
client.use(authMiddleware);

function App() {
  const [todos, setTodos] = useState<Livro[]>([])
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

export default App
