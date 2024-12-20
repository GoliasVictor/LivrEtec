import { Outlet, Link, NavLink, useParams } from "react-router";
import "../App.css";
import { PropsWithChildren } from "react";
import { components } from "../lib/api/v1";
import { useApi } from "../clientApi";
type Livro = components["schemas"]["Livro"];

interface EditarModalProps extends PropsWithChildren {
  onClose: () => void
  livro: Livro
}
export default function EditarModal({ livro, children, onClose}: EditarModalProps) {
  let api = useApi()
  const handleCancel = () => {
    onClose();
  }
  const handleConfirm = (e : React.SyntheticEvent) => {
    e.preventDefault();
    const form = e.target;
    const formData = new FormData(form);
    const formJson = Object.fromEntries(formData.entries()) as {
      titulo: string,
      quantidade: string, 
      descricao: string
    };
    console.log(formJson);
    api.PUT("/livros", {
      body: {
        
        nome: formJson.titulo,
        quantidade: parseInt(formJson.quantidade),
        descricao: formJson.descricao,
        arquivado: livro.arquivado,
        id: livro.id,
        autores: livro.autores,
        tags: livro.autores
      }
    })
    onClose();
  }
  return (<> 
    {children}
    <form className="flex flex-col border-2" onSubmit={handleConfirm}>
      <label>
        Titulo:
        <input className="m-2" type="text" name="titulo" id="" defaultValue={livro.nome} />
      </label>
      <label>
        Quantidade:
        <input className="m-2" type="number" name="quantidade" id="" defaultValue={livro.quantidade}/>
      </label>
      <label>
        Descricao:
        <input className="m-2" type="text" name="descricao" id="" defaultValue={livro.descricao}/>
      </label>
      <div className="flex flex-row">
        <button className="m-2" type="button" onClick={handleCancel}> Cancelar </button>
        <button className="m-2"> Confirmar </button>
      </div>
    </form>
  </>)
};