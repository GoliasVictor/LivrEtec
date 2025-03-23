import { Outlet, Link, NavLink, useParams } from "react-router";
import "../App.css";
import { PropsWithChildren } from "react";
import { components } from "../lib/api/v1";
import { useApi } from "../clientApi";
import { useModal } from "../hooks/useModal";
type Livro = components["schemas"]["Livro"];
type Pessoa = components["schemas"]["Livro"];

interface EditarModalProps extends PropsWithChildren {
	livro: Livro
}
export default function EmprestarModal({ livro, children }: EditarModalProps) {
	let api = useApi()
	let { closeModal } = useModal()!;
	const handleCancel = () => {
		closeModal();
	}
	let  pessoas : Pessoa[] = [
		{
			id: 1,
			nome: "carlos",

		},
		{
			id: 2,
			nome: "carlitos",

		}
	]
	const handleConfirm = async (e: React.FormEvent) => {
		e.preventDefault();
		const form = e.target as HTMLFormElement;
		const formData = new FormData(form);

		const formJson = Object.fromEntries(formData.entries()) as {
			livro: string,
			pessoa: string,
		};
		console.log(formJson);
		
		let idPessoa = parseInt(formJson.pessoa);
		let idLivro = parseInt(formJson.livro);

		if (isNaN(idPessoa) || isNaN(idLivro)) {
			return;
		}
		console.log("passou");
		await api.POST("/emprestimos", {
		  body: {
				idLivro: idLivro,
				idPessoa: idPessoa
			}
		})
		closeModal();

	}
	return (<>
		{children}
		<form className="flex flex-col border-2" onSubmit={handleConfirm}>
			<label>
				Livros:
				<select disabled={false} className="m-2" name="livro" id="" defaultValue={livro?.id}>
					<option value={livro?.id}> {livro?.nome}</option>
				</select>
			</label>

			<label>
				Pessoa:
				<select className="m-2" name="pessoa" id="">
					{
						pessoas.map(p => (
							<option key={p.id} value={p.id}> {p.nome}</option>
						))
					}
				</select>
			</label>


			<div className="flex flex-row">
				<button className="m-2" type="button" onClick={handleCancel}> Cancelar </button>
				<button className="m-2"> Confirmar </button>
			</div>
		</form>
	</>)
};