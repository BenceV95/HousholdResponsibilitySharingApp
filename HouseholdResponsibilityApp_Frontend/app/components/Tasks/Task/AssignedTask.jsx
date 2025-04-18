"use client";
import React, { useState, useEffect } from "react";
import { apiFetch, apiDelete } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";
import "./AssignedTask.css"; 
import { useAuth } from "../../../components/AuthContext/AuthProvider";

export default function AssignedTask() {
  const [scheduledTasks, setScheduledTasks] = useState([]);
  const [tasks, setTasks] = useState([]);
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(false);

  const { user } = useAuth();

  const getData = async () => {
    setLoading(true);
    try {
      const [schedResp, taskResp, userResp] = await Promise.all([
        apiFetch("/scheduleds"),
        apiFetch("/tasks"),
        apiFetch("/users"),
      ]);
      setScheduledTasks(schedResp);
      setTasks(taskResp);
      setUsers(userResp);
      console.log(schedResp);
    } catch (err) {
      console.error("Error:", err);
    }
    setLoading(false);
    
    
  };

  const deleteTask = async (id) => {
    const filtered = scheduledTasks.filter(
      (st) => st.scheduledTaskId !== id
    );
    setScheduledTasks(filtered);
    await apiDelete(`/scheduled/${id}`);
  };

  function findTaskTitle(taskId) {
    const t = tasks.find((x) => x.taskId === taskId);
    return t ? t.title : `Task #${taskId}`;
  }

  function findUserName(userId) {
    const u = users.find((x) => x.userResponseDtoId === userId);
    return u ? u.username : userId;
  }

  function repeatToString(repeatNum) {
    switch (repeatNum) {
      case 0:
        return "Daily";
      case 1:
        return "Weekly";
      case 2:
        return "Monthly";
      default:
        return "No Repeat";
    }
  }

  return (
    <div className="assignedTasks-container">
      <div className="getDataButton">
        <button onClick={getData} className="btn btn-primary">
          Get Data
        </button>
      </div>

      {loading ? (
        <Loading />
      ) : (
        <div className="assignedTaskCards">
          {scheduledTasks
            .filter((dataEntry) => {
              const relatedTask = tasks.find(
                (t) => t.taskId === dataEntry.householdTaskId
              );
              return (
                relatedTask &&
                Number(relatedTask.householdId) === Number(user?.householdId)
              );
            })
            .map((dataEntry, i) => {
              const title = findTaskTitle(dataEntry.householdTaskId);
              const createdByName = findUserName(dataEntry.createdByUserId);
              const assignedName = findUserName(dataEntry.assignedToUserId);
              const repeatStr = repeatToString(dataEntry.repeat);
              const eventDate = new Date(dataEntry.eventDate).toLocaleString();

              return (
                <div key={i} className="assignedTaskCard">
                  <h3>{title}</h3>
                  <p>Created By: {createdByName}</p>
                  <p>Assigned To: {assignedName}</p>
                  <p>Repeat: {repeatStr}</p>
                  <p>Event Date: {eventDate}</p>
                  <p>
                    Specific Time:{" "}
                    {dataEntry.atSpecificTime ? "Yes" : "No"}
                  </p>
                  <button
                    className="btn btn-danger"
                    onClick={() =>
                      deleteTask(dataEntry.scheduledTaskId)
                    }
                  >
                    DELETE
                  </button>
                </div>
              );
            })}
        </div>
      )}
    </div>
  );
}
