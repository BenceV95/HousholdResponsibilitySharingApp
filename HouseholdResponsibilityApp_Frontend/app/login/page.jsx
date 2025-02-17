"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import { apiPost } from "../(utils)/api.js";

export default function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState(null);
  const router = useRouter();

  const handleLogin = async (e) => {
    e.preventDefault();
    setError(null);
    try {
      await apiPost("/Auth/Login", { Email: email, Password: password });
      // Dispatch-oljuk a userUpdated eseményt a Navbar frissítéséhez
      window.dispatchEvent(new Event("userUpdated"));
      router.push("/solar-watch");
    } catch (err) {
      setError("Incorrect email or password");
      console.error(err);
    }
  };

  return (
    <div className="login-container">
      <h2>Login</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleLogin}>
        <input type="email" placeholder="Email" onChange={(e) => setEmail(e.target.value)} required />
        <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)} required />
        <button type="submit">Login</button>
      </form>
    </div>
  );
}
