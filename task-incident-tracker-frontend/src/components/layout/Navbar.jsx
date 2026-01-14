import { useNavigate } from "react-router-dom";
import { removeToken } from "../../utils/authStorage";

export default function Navbar() {
  const navigate = useNavigate();

  const handleLogout = () => {
    removeToken();
    navigate("/login");
  };

  return (
    <nav>
      <button onClick={() => navigate("/create-task")}>Create Task</button>
      <button onClick={handleLogout}>Log out</button>
    </nav>
  );
}