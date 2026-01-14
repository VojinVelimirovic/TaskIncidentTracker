import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import Login from "../pages/Login";
import Register from "../pages/Register";
import CreateTask from "../pages/CreateTask";
import ProtectedRoute from "./ProtectedRoute";

export default function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Navigate to="/login" />} />

        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />

        <Route 
          path="/create-task" 
          element={
            <ProtectedRoute>
              <CreateTask />
            </ProtectedRoute>
          } 
        />
      </Routes>
    </BrowserRouter>
  );
}
