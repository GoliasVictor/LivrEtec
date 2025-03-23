import { Route, Routes } from "react-router";
import { LoginPage } from './pages/loginPage';
import LivrosPage from './pages/livrosPage';
import { ProtectedLayout } from "./layouts/protectedLayout";
import { AppLayout } from "./layouts/appLayout";
import LivroPage from "./pages/livroPage";
import EmprestimosPage from "./pages/emprestimosPage";
import EmprestimoPage from "./pages/emprestimoPage";
import LivroCriarPage from "./pages/livroCriar";
import LivroEditarPage from "./pages/livroEditar";


function App() {

  return (
    <Routes>
      <Route path="/" element={<AppLayout />}>
          <Route path="*" element={<>not found</>} />
          <Route element={<ProtectedLayout />}>

            <Route path="livros">
              <Route index element={<LivrosPage />} />
              <Route path="criar" element={<LivroCriarPage />} />
            
              <Route path=":id" element={<LivroPage />} />
              <Route path=":id/editar" element={<LivroEditarPage />} />
            </Route>
            <Route path="emprestimos">
              <Route index element={<EmprestimosPage />} />
              <Route path=":id" element={<EmprestimoPage />} />
            </Route>
          </Route>
          <Route path="login" element={<LoginPage />} />
      </Route>
    </Routes>
  )
}

export default App
