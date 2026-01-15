import { Navigate } from "react-router-dom";
import { getToken } from "../utils/authStorage";
import { isManager } from "../utils/authUtils";

export default function ManagerRoute({ children }) {
  const token = getToken();

  if (!token) {
    return <Navigate to="/login" replace />;
  }

  if (!isManager()) {
    return <Navigate to="/my-tasks" replace />;
  }

  return children;
}