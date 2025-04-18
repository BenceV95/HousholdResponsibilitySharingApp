"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { useAuth } from "../components/AuthContext/AuthProvider.js";
import Link from "next/link";
import "./Login.css";

export default function Login() {
  const { login } = useAuth();
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const router = useRouter();

  const handleLogin = async (e) => {
    e.preventDefault();
    setResponseMessage("");
    setIsError(false);

    const formData = new FormData(e.currentTarget);
    const email = formData.get("email");
    const password = formData.get("password");

    try {
      const response = await login(email, password);
      setResponseMessage(response.Message ||"Login successful! Redirecting...");
      setTimeout(() => router.push("/"));
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <div className="form-group">
          <label>Email</label>
          <input type="email" placeholder="Email" name="email" required />
        </div>
        <div className="form-group">
          <label>Password</label>
          <input type="password" placeholder="Password" name="password" required />
        </div>

        <button type="submit" className="btn btn-success">Login</button>
      </form>

      {responseMessage && (
        <div className="response-message">
          <p style={{ color: isError ? "red" : "green" }}>{responseMessage}</p>
        </div>
      )}

      <div className="register">
        <p>Don't have an account yet?</p>
        <Link href={"/register"} className="btn btn-primary">Register</Link>
      </div>
    </div>
  );
}
