import { jwtDecode } from "jwt-decode";
import { getToken } from "./authStorage";

export const getUserRole = () => {
  const token = getToken();
  if (!token) return null;

  try {
    const decoded = jwtDecode(token);
    return decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
  } catch (error) {
    return null;
  }
};

export const isManager = () => {
  const role = getUserRole();
  return role === "Manager" || role === "Admin";
};

export const isAdmin = () => {
  const role = getUserRole();
  return role === "Admin";
};