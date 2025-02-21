"use client";
import React, { useState } from "react";
import { apiDelete, apiFetch } from "../../../../(utils)/api";
import "./GetTasks.css";
import Loading from "../../../../(utils)/Loading";
import Task from "../Task/Task";

const GetTasks = () => {
  const [tasks, setTasks] = useState([]);
  const [users, setUsers] = useState([]);
  const [groups, setGroups] = useState([]); 
  const [loading, setLoading] = useState(false);

  const getData = async () => {
    setLoading(true);
    try {
      const [tasksResp, usersResp, groupsResp] = await Promise.all([
        apiFetch("/tasks"),
        apiFetch("/users"),
        apiFetch("/groups"),  
      ]);
      setTasks(tasksResp);
      setUsers(usersResp);
      setGroups(groupsResp);
    } catch (err) {
      console.error("Error:", err);
    }
    setLoading(false);
  };

  const deleteTask = async (e) => {
    const idToDelete = e.target.id;
    const filtered = tasks.filter((t) => t.taskId != idToDelete);
    setTasks(filtered);
    await apiDelete(`/task/${idToDelete}`);
    console.log("Deleted", idToDelete);
  };

  function findUsernameById(userId) {
    const user = users.find((u) => u.userResponseDtoId === userId);
    return user ? user.username : userId;
  }

  function findGroupNameById(groupId) {
    const group = groups.find((g) => g.groupResponseDtoId === groupId);
    return group ? group.name : `Group #${groupId}`;
  }

  return (
    <div className="getTasks">
      <div className="getDataButton">
        <button onClick={getData} className="btn btn-primary">
          Get Data
        </button>
      </div>
      <div className="display">
        {loading ? (
          <Loading />
        ) : (
          tasks.map((task) => {
            const userName = findUsernameById(task.userId);
            const groupName = findGroupNameById(task.groupId); 
            return (
              <div key={task.taskId} className="taskData">
                <Task
                  data={task}
                  deleteTask={deleteTask}
                  userName={userName}
                  groupName={groupName}
                />
              </div>
            );
          })
        )}
      </div>
    </div>
  );
};

export default GetTasks;
