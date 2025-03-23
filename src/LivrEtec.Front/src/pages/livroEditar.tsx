import { useEffect, useState } from 'react'
import '../App.css'
import React from 'react'
import type { components } from "./../lib/api/v1"; 
import { useApi } from './../clientApi';
import { useNavigate, useParams } from 'react-router';
import { useModal } from '../hooks/useModal';
import EditarModal from '../modals/editarModal';
import ConfirmarModal from '../components/confirmarModal';
type Livro = components["schemas"]["Livro"];


function LivrosPage() {

  const [livro, setLivro] = useState<Livro | null>()
  const { id } = useParams();
  const client = useApi()
 
 
  
  
  return (
	  <>
		  Livro Editar {id}
	</>
  )
}

export default LivrosPage
