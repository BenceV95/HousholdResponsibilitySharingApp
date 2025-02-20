"use client";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { apiPost } from "../../../../(utils)/api";
import { useAuth } from "../../AuthContext/AuthProvider";

const CreateGroup = () => {
  const {
    register,
    handleSubmit,
    setValue,
    formState: { errors },
  } = useForm();

  const [message, setMessage] = useState("");
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
    const groupData = {
      name: formData.name,
      userId: formData.userId,
      householdId: Number(formData.householdId),
    };

    try {
      await apiPost("/group", groupData);
      setMessage(`Successfully created group: ${formData.name}`);
    } catch (error) {
      console.error(error);
      setMessage("An error occurred while creating the group");
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="group-form">
      <label htmlFor="name">Group Name:</label>
      <input
        type="text"
        id="name"
        {...register("name", { required: "Group name is required" })}
      />
      {errors.name && <span>{errors.name.message}</span>}

      <button type="submit" className="btn btn-success">
        Create Group
      </button>

      {message && <p>{message}</p>}
    </form>
  );
};

export default CreateGroup;
