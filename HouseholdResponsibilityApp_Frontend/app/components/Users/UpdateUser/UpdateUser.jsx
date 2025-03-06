"use client";
import { useForm } from "react-hook-form";
import React, { useState, useEffect } from "react";
import { apiPut, apiFetch } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";
import "./UpdateUser.css";
import { useAuth } from "../../AuthContext/AuthProvider";

const UpdateUser = () => {
  const { user } = useAuth();
  const { register, handleSubmit, formState: { errors } } = useForm();
  
  const [responseMessage, setResponseMessage] = useState(""); 
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(true);
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    if (!user || !user.userId) {
      setLoading(false);
      return;
    }

    const getUser = async () => {
      try {
        setResponseMessage("");
        setIsError(false);

        const fetchedUserData = await apiFetch(`/user/${user.userId}`);
        setUserData(fetchedUserData);
      } catch (error) {
        setIsError(true);
        setResponseMessage(error.message || "Error retrieving user data.");
      } finally {
        setLoading(false);
      }
    };

    getUser();
  }, [user?.userId]);

  const onSubmit = async (data) => {
    if (!user || !user.userId) {
      setResponseMessage("User ID is missing. Please log in again.");
      setIsError(true);
      return;
    }

    setResponseMessage("");
    setIsError(false);

    const updatedUserData = {
      username: data.username,
      email: data.email,
      firstName: data.firstName,
      lastName: data.lastName,
      password: data.password,
    };

    try {
      const response = await apiPut(`/user/${user.userId}`, updatedUserData);
      setResponseMessage(response?.Message || "User updated successfully!");
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message || "An error occurred while updating the user.");
    }
  };

  return (
    <>
      {loading ? (
        <Loading />
      ) : (
        <form onSubmit={handleSubmit(onSubmit)} className="userDataUpdate-form">
          <label htmlFor="username">Username:</label>
          <input
            type="text"
            defaultValue={userData?.username || ""}
            id="username"
            {...register("username", { required: "Username is required" })}
          />
          {errors.username && <span>{errors.username.message}</span>}

          <label htmlFor="email">Email:</label>
          <input
            type="email"
            id="email"
            defaultValue={userData?.email || ""}
            {...register("email", { required: "Email is required" })}
          />
          {errors.email && <span>{errors.email.message}</span>}

          <label htmlFor="firstName">First Name:</label>
          <input
            type="text"
            id="firstName"
            defaultValue={userData?.firstName || ""}
            {...register("firstName", { required: "First Name is required" })}
          />
          {errors.firstName && <span>{errors.firstName.message}</span>}

          <label htmlFor="lastName">Last Name:</label>
          <input
            type="text"
            id="lastName"
            defaultValue={userData?.lastName || ""}
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

          <button type="submit" className="btn btn-success">Update</button>

          {responseMessage && (
            <p className={isError ? "error" : "success"}>{responseMessage}</p>
          )}
        </form>
      )}
    </>
  );
};

export default UpdateUser;
