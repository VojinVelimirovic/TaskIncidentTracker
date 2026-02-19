import { API_BASE_URL } from "../config/api";
import { getToken } from "../utils/authStorage";

const API_URL = `${API_BASE_URL}/auth`;

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
    const data = await res.json();
    const firstError = data.errors?.[0]?.message ?? data.message ?? "Registration failed";
    throw new Error(firstError);
  }

  return res.json();
}