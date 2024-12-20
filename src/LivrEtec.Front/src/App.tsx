import { Route, Routes } from "react-router";
import { LoginPage } from './pages/loginPage';
import LivrosPage from './pages/livrosPage';
import { ProtectedLayout } from "./components/protectedLayout";
import { AppLayout } from "./components/appLayout";
import LivroPage from "./pages/livroPage";
import EmprestimosPage from "./pages/emprestimosPage";
import EmprestimoPage from "./pages/emprestimoPage";


function App() {

  return (
    <Routes>
      <Route path="/" element={<AppLayout/>}>
        <Route path="*" element={<>not found</>} />
        <Route element={<ProtectedLayout />}>
        
          <Route path="livros">
            <Route index element={<LivrosPage />}/>
            <Route path=":id" element={<LivroPage />}/>
          </Route>
          <Route path="emprestimos">
            <Route index element={<EmprestimosPage />}/>
            <Route path=":id" element={<EmprestimoPage />}/>
          </Route>
        </Route>
        <Route path="login" element={<LoginPage />} />
      </Route>
      
    </Routes>

  )
}

export default App
