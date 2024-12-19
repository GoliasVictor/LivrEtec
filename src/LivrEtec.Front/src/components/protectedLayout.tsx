import { Outlet, Link } from "react-router";
import { useAuth } from "../hooks/useAuth";

export const ProtectedLayout = () => {
  const { user } = useAuth()!;

  
  if (!user) {
    return <Link to="/login">
      Não autenticado volte para o login
    </Link>
  }

  return (<>
    login: {user.user_login}<br/>
    token: {user.jwtToken}
    <Outlet />
  </>)
};