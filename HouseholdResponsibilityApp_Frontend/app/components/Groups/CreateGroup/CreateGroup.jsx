"use client";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { apiPost } from "../../../../(utils)/api";
import { useAuth } from "../../AuthContext/AuthProvider";

const CreateGroup = () => {
  const {
    register,
    handleSubmit,
    // setValue,
    formState: { errors },
  } = useForm();

  const [message, setMessage] = useState("");


  const onSubmit = async (formData) => {
    console.log("submit stuff")
    const groupData = {
      groupName: formData.name,
    };

    try {
      await apiPost("/group", groupData);
      setMessage(`Successfully created group: ${formData.name}`);
    } catch (error) {
      // we shouldnt just drop a random msg saying error while creating a grup. we should give an exact reason why it failed
      setMessage(error.message);
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
