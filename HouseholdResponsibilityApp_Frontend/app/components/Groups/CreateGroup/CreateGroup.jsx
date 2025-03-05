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

  useEffect(() => {
    register("userId", { required: "User ID is required" });
    register("householdId", { required: "Household ID is required" });
  }, [register]);

  useEffect(() => {
    if (user) {
      setValue("userId", user.userId);
      setValue("householdId", user.householdId || 0);
    }
  }, [user, setValue]);

  const onSubmit = async (formData) => {
    setResponseMessage("");
    setIsError(false);

    const groupData = {
      name: formData.name,
      userId: formData.userId,
      householdId: Number(formData.householdId),
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
            type="text"
            id="name"
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
