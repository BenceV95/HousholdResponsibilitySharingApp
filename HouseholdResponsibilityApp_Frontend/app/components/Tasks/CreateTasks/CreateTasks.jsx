"use client";
import { useForm } from "react-hook-form";
import { useEffect, useState } from "react";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import "./CreateTasks.css";

const CreateTasks = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const [message, setMessage] = useState("");



  const [groups, setGroups] = useState([]);

  useEffect(() => {
    async function fetchData() {
      try {
        const groupList = await apiFetch("/groups/my-household");

        setGroups(groupList)

      } catch (err) {
        console.error("Error:", err);
        setMessage(err.message);
      }
    }

    fetchData();

  }, []);




  const onSubmit = async (formData) => {
    const taskData = {
      title: formData.title,
      description: formData.description,
      groupId: Number(formData.groupId),
      priority: formData.priority || false,
    };

    try {
      const posted = await apiPost("/task", taskData);
      setMessage(`Successfully posted task! New Task ID: ${posted}`);
    } catch (error) {
      console.error(error);
      setMessage(error.message);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)} className="task-form">
      <label htmlFor="title">Title:</label>
      <input
        type="text"
        id="title"
        {...register("title", { required: "Title is required" })}
      />
      {errors.title && <span>{errors.title.message}</span>}

      <label htmlFor="description">Description:</label>
      <textarea id="description" {...register("description")} />

      <label htmlFor="groupId">Group:</label>
      <select
        id="groupId"
        {...register("groupId", { required: "Group ID is required" })}
        defaultValue=""
      >
        <option value="" disabled>
          Select group
        </option>
        {groups.map((g) => (
          <option key={g.groupResponseDtoId} value={g.groupResponseDtoId}>
            {g.name}
          </option>
        ))}
      </select>
      {errors.groupId && <span>{errors.groupId.message}</span>}

      <label htmlFor="priority">Priority:</label>
      <input type="checkbox" id="priority" {...register("priority")} />

      <button type="submit" className="btn btn-success">
        Submit
      </button>
      {message && <p>{message}</p>}
    </form>
  );
};

export default CreateTasks;
