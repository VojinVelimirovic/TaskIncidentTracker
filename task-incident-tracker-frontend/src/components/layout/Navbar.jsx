import { useNavigate } from "react-router-dom";
import { removeToken } from "../../utils/authStorage";
import { isManager } from "../../utils/authUtils";
import "../../styles/Navbar.css";

export default function Navbar() {
  const navigate = useNavigate();

  const handleLogout = () => {
    removeToken();
    navigate("/login");
  };

  const managerAccess = isManager();

  return (
    <nav>
      <div className="nav-content">
        <div className="nav-left">
          <button onClick={() => navigate("/my-tasks")}>My Tasks</button>
          {managerAccess && (
            <button onClick={() => navigate("/all-tasks")}>All Tasks</button>
          )}
          <button onClick={() => navigate("/create-task")}>Create Task</button>
        </div>
        <div className="nav-right">
          <button onClick={handleLogout}>Log out</button>
        </div>
      </div>
    </nav>
  );
}