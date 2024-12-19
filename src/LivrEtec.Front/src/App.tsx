import { Route, Routes } from "react-router";
import { LoginPage } from './pages/loginPage';
import LivrosPage from './pages/livrosPage';
import { ProtectedLayout } from "./components/protectedLayout";


function App() {

  return (
    <Routes>
      <Route element={<ProtectedLayout />}>
        <Route path="/livros" element={<LivrosPage />}/>
      </Route>
      <Route path="/login" element={<LoginPage />}/>
    </Routes>

  )
}

export default App
