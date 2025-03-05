"use client";
import { useForm } from "react-hook-form";
import React, { useEffect, useState } from "react";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import "./AssignTasks.css";
import { useAuth } from "../../AuthContext/AuthProvider";

export default function AssignTasks() {
  const {
    register,
    handleSubmit,
    setValue,
    setError,
    clearErrors,
    formState: { errors },
  } = useForm();

  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const [specificTimeSelected, setSpecificTimeSelected] = useState(false);
  const [tasks, setTasks] = useState([]);
  const [users, setUsers] = useState([]);
  const { user } = useAuth();

  useEffect(() => {
    async function fetchData() {
      try {
        const taskList = await apiFetch("/tasks");
        const userList = await apiFetch("/users");

        if (user?.householdId) {
          const filteredTasks = taskList.filter(
            (task) => Number(task.householdId) === Number(user.householdId)
          );
          const filteredUsers = userList.filter(
            (u) => Number(u.householdId) === Number(user.householdId)
          );

          setTasks(filteredTasks);
          setUsers(filteredUsers);
        }
      } catch (err) {
        setIsError(true);
        setResponseMessage("Failed to load data.");
      }
    }
    if (user) {
      fetchData();
    }
  }, [user]);

  useEffect(() => {
    register("createdByUserId", { required: "Created By is required" });
  }, [register]);

  useEffect(() => {
    if (user) {
      setValue("createdByUserId", user.userId);
    }
  }, [user, setValue]);

  const SelectSpecificTime = () => {
    setSpecificTimeSelected(!specificTimeSelected);
  };

  const onSubmit = async (data) => {
    setResponseMessage("");
    setIsError(false);

    if (!data.repeat) {
      setError("repeat", {
        type: "manual",
        message: "No Repeat option is selected.",
      });
      return;
    }

    data.eventDate = new Date(data.eventDate).toISOString();
    data.repeat = parseInt(data.repeat, 10);

    const modifiedData = {
      ...data,
      atSpecificTime: specificTimeSelected,
    };

    clearErrors("event_date");

    try {
      const response = await apiPost("/scheduled", modifiedData);
      setResponseMessage(response?.Message || "Task scheduled successfully!");
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    }
  };

  return (
    <div className="assign-tasks-container">
      <form onSubmit={handleSubmit(onSubmit)} className="assign-tasks-form">
        
        <div className="form-group">
          <label>Select Task:</label>
          <select
            {...register("householdTaskId", {
              required: true,
              setValueAs: (value) =>
                value === "" ? undefined : parseInt(value, 10),
            })}
            defaultValue=""
          >
            <option value="">-- Select a Task --</option>
            {tasks.map((t) => (
              <option key={t.taskId} value={t.taskId}>
                {t.title}
              </option>
            ))}
          </select>
          {errors.householdTaskId && <p className="error">Task is required</p>}
        </div>

        <div className="form-group">
          <label>Assigned To (User):</label>
          <select
            {...register("assignedToUserId", { required: true })}
            defaultValue=""
          >
            <option value="">-- Select a User --</option>
            {users.map((u) => (
              <option key={u.userResponseDtoId} value={u.userResponseDtoId}>
                {u.username === user.userName ? "Me" : u.username}
              </option>
            ))}
          </select>
          {errors.assignedToUserId && <p className="error">Assigned To is required</p>}
        </div>

        <div className="form-group repeat-frequency">
          <label>Repeat Frequency:</label>
          <div className="repeatRadios">
            <label>
              <input type="radio" value="0" {...register("repeat")} /> Daily
            </label>
            <label>
              <input type="radio" value="1" {...register("repeat")} /> Weekly
            </label>
            <label>
              <input type="radio" value="2" {...register("repeat")} /> Monthly
            </label>
            <label>
              <input type="radio" value="3" {...register("repeat")} /> No Repeat
            </label>
          </div>
          {errors.repeat && <p className="error">{errors.repeat.message}</p>}
        </div>

        <div className="form-group specific-time">
          <label>Specific Time:</label>
          <div className="switch-wrapper">
            <label className="switch">
              <input type="checkbox" onChange={SelectSpecificTime} />
              <span className="slider round"></span>
            </label>
          </div>
          <input
            type={specificTimeSelected ? "datetime-local" : "date"}
            {...register("eventDate", { required: true })}
          />
          {errors.eventDate && <p className="error">Date is required</p>}
        </div>

        <div className="form-group submit-group">
          <button type="submit" className="btn btn-success">
            Submit
          </button>
        </div>
      </form>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}
    </div>
  );
}
