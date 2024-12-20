import { Outlet, Link, NavLink } from "react-router";
import { useAuth } from "../hooks/useAuth";
import "../App.css";

export const AppLayout = () => {
  const { user } = useAuth()!;

  const a = ({ isActive }: {isActive : boolean}) =>
          isActive ? "text-red-500" : ""

  return (<>
    <ul>
      <li>login: {user?.user_login}</li>
      <li><NavLink to="/livros" className={a}>Livros</NavLink></li>
      <li><NavLink to="/emprestimos" className={a}>Emprestimos</NavLink></li>      
    </ul> 
    <Outlet />
  </>)
};