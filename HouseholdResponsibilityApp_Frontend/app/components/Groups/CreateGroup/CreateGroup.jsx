"use client";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { apiPost } from "../../../../(utils)/api";
import { useAuth } from "../../AuthContext/AuthProvider";
import "./CreateGroup.css";

export default function CreateGroup({ isOpen, onClose }) {
  if (!isOpen) {
    return null;
  }

  const { register, handleSubmit, setValue, formState: { errors } } = useForm();
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const [loading, setLoading] = useState(false);
  const { user } = useAuth();


  const onSubmit = async (formData) => {
    setLoading(true);
    setResponseMessage("");
    setIsError(false);

    const groupData = {
      GroupName: formData.name
    };

    try {
      const group = await apiPost("/group", groupData);
      setResponseMessage(group.message);
    } catch (error) {
      setIsError(true);
      setResponseMessage(error);
    }
    finally {
      setLoading(false);
    }
  };



  return (
    <div className="modal-overlay">
        <form onSubmit={handleSubmit(onSubmit)} className="create-group-form">
        
          <label htmlFor="name">Group Name:</label>
          <input
            placeholder="Enter group name..."
            {...register("name", { required: "Group name is required" })}
            disabled={loading}
            minLength={1}
            maxLength={20}
            id="name"
          />          
          {errors.name && <span className="error">{errors.name.message}</span>}

          {responseMessage && (
            <p className={isError ? "error" : "success"}>{responseMessage}</p>
          )}

          <button type="submit" className="btn btn-success" disabled={loading}>
            {loading ? "Creating Group..." : "Create Group"}
          </button>

          <button onClick={() => onClose()} className="btn btn-danger" disabled={loading}>Close</button>
        </form>
      </div>
  );
}
