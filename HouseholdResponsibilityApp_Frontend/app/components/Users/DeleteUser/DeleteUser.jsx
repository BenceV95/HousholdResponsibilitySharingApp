"use client";
import { useForm } from "react-hook-form";
import { useState } from "react";
import { apiDelete } from "../../../../(utils)/api";
import "./DeleteUser.css";

const DeleteUser = () => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [responseMessage, setResponseMessage] = useState(""); 
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(false); 

  const onSubmit = async (data) => {
    setResponseMessage("");
    setIsError(false);
    setLoading(true);

    try {
      const response = await apiDelete(`/user/${data.userId}`);
      setResponseMessage(response?.Message || "User deleted successfully!"); 
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="user-form">
      <label htmlFor="userId">User ID:</label>
      <input
        type="text"
        id="userId"
        {...register("userId", { required: "User ID is required" })}
      />
      {errors.userId && <span>{errors.userId.message}</span>}

      <button type="submit" className="btn btn-danger" disabled={loading}>
        {loading ? "Deleting..." : "Delete"}
      </button>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}
    </form>
  );
};

export default DeleteUser;
