import { Route, Routes } from "react-router";
import { LoginPage } from './pages/loginPage';
import LivrosPage from './pages/livrosPage';
import { ProtectedLayout } from "./components/protectedLayout";
import { AppLayout } from "./components/appLayout";


function App() {

  return (
    <Routes>
      <Route element={<AppLayout/>}>
        <Route path="*" element={<>not found</>}/>
        <Route element={<ProtectedLayout />}>
          <Route path="/livros" element={<LivrosPage />}/>
        </Route>
        <Route path="/login" element={<LoginPage />} />
      </Route>
      
    </Routes>

  )
}

export default App
