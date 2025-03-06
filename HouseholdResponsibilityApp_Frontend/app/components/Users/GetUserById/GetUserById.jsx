"use client";
import { useForm } from "react-hook-form";
import { useState } from "react";
import { apiFetch } from "../../../../(utils)/api";
import "./GetUserById.css";

const GetUserById = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [user, setUser] = useState(null);
  const [responseMessage, setResponseMessage] = useState(""); 
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(false); 

  const onSubmit = async (data) => {
    setResponseMessage("");
    setIsError(false);
    setUser(null);
    setLoading(true);

    try {
      const response = await apiFetch(`/user/${data.userId}`);
      setUser(response);
      setResponseMessage(response.Message || "User found successfully!"); 
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="get-user-by-id">
      <h1>Find User by ID</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <label htmlFor="userId">User ID:</label>
        <input
          type="text"
          id="userId"
          {...register("userId", { required: "User ID is required" })}
        />
        {errors.userId && <span>{errors.userId.message}</span>}

        <button type="submit" className="btn btn-primary" disabled={loading}>
          {loading ? "Searching..." : "Search"}
        </button>
      </form>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}

      {user && (
        <div className="user-info">
          <p><strong>Username:</strong> {user.username}</p>
          <p><strong>Email:</strong> {user.email}</p>
          <p><strong>Name:</strong> {user.firstName} {user.lastName}</p>
        </div>
      )}
    </div>
  );
};

export default GetUserById;
