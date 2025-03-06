"use client";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { apiPost } from "../../../../(utils)/api";
import { useAuth } from "../../AuthContext/AuthProvider";
import "./CreateGroup.css"; 

export default function CreateGroup() {
  const { register, handleSubmit, setValue, formState: { errors } } = useForm();
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const { user } = useAuth();


  const onSubmit = async (formData) => {
    console.log("asd");
    
    setResponseMessage("");
    setIsError(false);
    
    const groupData = {
      GroupName: formData.name,
      
    };

    try {
      await apiPost("/group", groupData);
      setResponseMessage(`Successfully created group: ${formData.name}`);
    } catch (error) {
      console.error(error);
      setIsError(true);
      setResponseMessage(error.message);
    }
  };

  

  return (
    <div className="create-group-container">
      <form onSubmit={handleSubmit(onSubmit)} className="create-group-form">
        <div className="form-group">
          <label htmlFor="name">Group Name:</label>
          <input
            placeholder="Enter group name..."
            {...register("name", { required: "Group name is required" })}
          />
          {errors.name && <span className="error">{errors.name.message}</span>}
        </div>

        <button type="submit" className="btn btn-success">
          Create Group
        </button>

        {responseMessage && (
          <p className={isError ? "error" : "success"}>{responseMessage}</p>
        )}
      </form>
    </div>
  );
}
