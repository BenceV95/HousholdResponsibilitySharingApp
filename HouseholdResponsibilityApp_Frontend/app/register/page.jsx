"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import { useForm } from "react-hook-form";
import { apiPost } from "../../(utils)/api.js";
import "./Register.css";

export default function Register() {

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const [loading, setLoading] = useState(false);
  const [responseMessage, setResponseMessage] = useState("");
  const [error, setError] = useState(false);

  const router = useRouter();

  const onSubmit = async (data) => {
    setLoading(true);
    console.log(data);

    try {
      const response = await apiPost("/Auth/Register", data);
      setResponseMessage("Registration successful. Please log in.");
      //router.push("/login");
    } catch (error) {
      console.error(error);
      setError(true);
      setResponseMessage("An error occurred. Please try again later. "+error);
    } finally {
      setLoading(false);
      
    }
  };

  return (
    <div className="register-container">
      <h2>Register</h2>
      <form onSubmit={handleSubmit(onSubmit)}>

        <div className="form-group">
          <label>First Name</label>
          <input {...register("firstName", { required: "First Name is required" })} />
          {errors.firstName && <p className="error-message">{errors.firstName.message}</p>}
        </div>

        <div className="form-group">
          <label>Last Name</label>
          <input {...register("lastName", { required: "Last Name is required" })} />
          {errors.lastName && <p className="error-message">{errors.lastName.message}</p>}
        </div>

        <div className="form-group">
          <label>Username</label>
          <input {...register("username", { required: "Username is required" })} />
          {errors.username && <p className="error-message">{errors.username.message}</p>}
        </div>

        <div className="form-group">
          <label>Email</label>
          <input type="email" {...register("email", { required: "Email is required" })} />
          {errors.email && <p className="error-message">{errors.email.message}</p>}
        </div>

        <div className="form-group">
          <label>Password</label>
          <input type="password" {...register("password", { required: "Password is required", minLength: 6 })} />
          {errors.password && <p className="error-message">{errors.password.message}</p>}
        </div>

        <button type="submit" disabled={loading || responseMessage} className="btn btn-primary">
          {loading ? "Registering..." : "Register"}
        </button>

      </form>

      {responseMessage && (<div className="response-message">
        <p style={{color: error ? "red" : "#28a745"}}> {responseMessage}</p>
      </div>)}

      <div className="login">
        <p>Already have an account?</p>
        <Link href={"/login"} className="btn btn-success">Log in</Link>
      </div>
    </div>
  );
}
