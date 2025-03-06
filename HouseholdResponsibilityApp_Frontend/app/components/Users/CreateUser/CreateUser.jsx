"use client";
import { useForm } from "react-hook-form";
import { useState } from "react";
import { apiPost } from "../../../../(utils)/api";
import "./CreateUser.css";

const CreateUser = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(false); 

  const onSubmit = async (data) => {
    setResponseMessage("");
    setIsError(false);
    setLoading(true);

    const userData = {
      username: data.username,
      email: data.email,
      firstName: data.firstName,
      lastName: data.lastName,
      password: data.password,
      isAdmin: false, 
    };

    try {
      const response = await apiPost("/user", userData);
      setResponseMessage(response?.Message || "User created successfully!"); 
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="user-form">
      <label htmlFor="username">Username:</label>
      <input
        type="text"
        id="username"
        {...register("username", { required: "Username is required" })}
      />
      {errors.username && <span>{errors.username.message}</span>}

      <label htmlFor="email">Email:</label>
      <input
        type="email"
        id="email"
        {...register("email", { required: "Email is required" })}
      />
      {errors.email && <span>{errors.email.message}</span>}

      <label htmlFor="firstName">First Name:</label>
      <input
        type="text"
        id="firstName"
        {...register("firstName", { required: "First Name is required" })}
      />
      {errors.firstName && <span>{errors.firstName.message}</span>}

      <label htmlFor="lastName">Last Name:</label>
      <input
        type="text"
        id="lastName"
        {...register("lastName", { required: "Last Name is required" })}
      />
      {errors.lastName && <span>{errors.lastName.message}</span>}

      <label htmlFor="password">Password:</label>
      <input
        type="password"
        id="password"
        {...register("password", { required: "Password is required" })}
        autoComplete="new-password"
      />
      {errors.password && <span>{errors.password.message}</span>}

      <button type="submit" className="btn btn-success" disabled={loading}>
        {loading ? "Creating User..." : "Submit"}
      </button>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}
    </form>
  );
};

export default CreateUser;
