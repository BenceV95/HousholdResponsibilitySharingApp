"use client";
import { useForm } from "react-hook-form";
import React, { useEffect, useState } from "react";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import "./CreateTasks.css";
import { useAuth } from "../../AuthContext/AuthProvider"; //módosítás


export default function CreateTasks() {
  const { register, handleSubmit, setValue, formState: { errors } } = useForm();
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const [groups, setGroups] = useState([]);
  const { user } = useAuth();

  useEffect(() => {
    async function fetchData() {
      try {
        const groupList = await apiFetch("/groups/my-household");

        setGroups(groupList)

      } catch (err) {
        setIsError(true);
        setResponseMessage("Failed to load groups.");
      }
    }
    if (user) fetchData();
  }, [user]);

  /*
  useEffect(() => {
    register("createdBy", { required: "Created By is required" });
    register("householdId", { required: "Household ID is required" });
  }, [register]);

  useEffect(() => {
    if (user) {
      setValue("createdBy", user.userId);
      setValue("householdId", user.householdId || 0);
    }
  }, [user, setValue]);
  */

  const onSubmit = async (formData) => {
    setResponseMessage("");
    setIsError(false);

    const taskData = {
      title: formData.title,
      description: formData.description,
      groupId: Number(formData.groupId),
      priority: formData.priority || false,
    };

    try {
      const response = await apiPost("/task", taskData);
      setResponseMessage(response?.Message || "Successfully created task!");
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    }
  };

  return (
    <div className="create-task-container">

      <form onSubmit={handleSubmit(onSubmit)} className="create-task-form">
        
        <div className="form-group">
          <label htmlFor="title">Task Title</label>
          <input
            type="text"
            id="title"
            placeholder="Enter a task title..."
            {...register("title", { required: "Title is required" })}
          />
          {errors.title && <span className="error">{errors.title.message}</span>}
        </div>

        <div className="form-group">
          <label htmlFor="description">Description</label>
          <textarea
            id="description"
            placeholder="Enter task details..."
            {...register("description")}
          />
        </div>

        <div className="form-group">
          <label htmlFor="groupId">Group</label>
          <select
            id="groupId"
            defaultValue=""
            {...register("groupId", { required: "Group ID is required" })}
          >
            <option value="" disabled>Select group</option>
            {groups.map((g) => (
              <option key={g.groupResponseDtoId} value={g.groupResponseDtoId}>
                {g.name}
              </option>
            ))}
          </select>
          {errors.groupId && <span className="error">{errors.groupId.message}</span>}
        </div>

        <div className="form-group checkbox-group">
          <label className="checkbox-label" htmlFor="priority">
            <input type="checkbox" id="priority" {...register("priority")} />
            Priority
          </label>
        </div>

        <div className="form-group submit-group">
          <button type="submit" className="btn btn-success">Submit</button>
        </div>

        {responseMessage && (
          <p className={isError ? "error" : "success"}>{responseMessage}</p>
        )}
      </form>
    </div>
  );
}
