const API_URL = "http://localhost:5000/api/auth";
import { getToken } from "../utils/authStorage";

export async function login(username, password) {
  const res = await fetch(`${API_URL}/login`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password })
  });

  if (!res.ok) {
    throw new Error("Invalid credentials");
  }

  return res.json();
}

export async function register(username, password) {
  const res = await fetch(`${API_URL}/register`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ username, password })
  });

  if (!res.ok) {
    throw new Error("Registration failed");
  }

  return res.json();
}

export const getAllUsers = async () => {
  const token = getToken();

  const response = await fetch(`${API_URL}`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    const text = await response.text();
    throw new Error(text || "Failed to fetch users");
  }

  return response.json();
};
