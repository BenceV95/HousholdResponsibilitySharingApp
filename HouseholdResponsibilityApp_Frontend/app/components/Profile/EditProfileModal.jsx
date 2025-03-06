"use client";
import React, { useState, useEffect } from "react";
import { useForm } from "react-hook-form";
import { apiPut, apiFetch } from "../../../(utils)/api";
import { useAuth } from "../AuthContext/AuthProvider";
import "./EditProfileModal.css";

export default function EditProfileModal({ isOpen, onClose, onProfileUpdated }) {
  const { user, setUser } = useAuth();
  const { register, handleSubmit, setValue } = useForm();
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (user) {
      setValue("username", user.userName);
      setValue("email", user.email);
      setValue("firstName", user.firstName);
      setValue("lastName", user.lastName);
      setValue("password", "");
    }
  }, [user, setValue]);

  if (!isOpen) return null; 

  const onSubmit = async (data) => {
    try {
      const updatedUserData = {
        username: data.username,
        email: data.email,
        firstName: data.firstName,
        lastName: data.lastName,
        password: data.password,
      };
  
      await apiPut(`/user/${user.userId}`, updatedUserData);
  
      await apiFetch("/auth/refresh");
  
      const updatedUser = await apiFetch("/auth/user");
      setUser(updatedUser);
      onProfileUpdated(updatedUser);
      
      setMessage("Profile updated successfully!");
      onClose();
    } catch (error) {
      setMessage(error.message);
    }
  };
  

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Edit Profile</h2>
        <form onSubmit={handleSubmit(onSubmit)} className="modal-form">
          <input
            type="text"
            placeholder="Username"
            {...register("username", { required: "Username is required" })}
          />
          <input
            type="email"
            placeholder="Email"
            {...register("email", { required: "Email is required" })}
          />
          <input
            type="text"
            placeholder="First Name"
            {...register("firstName", { required: "First name is required" })}
          />
          <input
            type="text"
            placeholder="Last Name"
            {...register("lastName", { required: "Last name is required" })}
          />
          <input
            type="password"
            placeholder="Password"
            {...register("password", { required: "Password is required" })}
          />

          <div className="modal-buttons">
            <button type="submit" className="btn btn-primary">
              Save
            </button>
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
        {message && <p className="modal-message">{message}</p>}
      </div>
    </div>
  );
}
