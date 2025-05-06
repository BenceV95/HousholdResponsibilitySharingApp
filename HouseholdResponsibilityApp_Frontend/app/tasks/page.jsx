"use client";
import { useState } from 'react';
import { apiFetch, apiPut } from '../../(utils)/api';
import './tasks.css';
import CreateTasks from '../components/Tasks/CreateTasks/CreateTasks';
import GetTasks from '../components/Tasks/GetTasks/GetTasks';
import AssignTasks from '../components/Tasks/AssignTasks/AssignTasks';
import AssignedTask from '../components/Tasks/Task/AssignedTask';
import CreateGroup from '../components/Groups/CreateGroup/CreateGroup';

const TaskForm = () => {
  const [taskActionVisible, setTaskActionVisible] = useState(false);
  const [taskAction, setTaskAction] = useState("");

  const viewAction = (e) => {
    let action = e.target.name;
    if (taskActionVisible && taskAction === action) {
      setTaskActionVisible(false);
      setTaskAction("");
    } else {
      setTaskAction(action);
      setTaskActionVisible(true);
    }
  };

  return (
    <div className='tasks'>

      <div className='taskButtons'>
        <button className='btn btn-warning' onClick={viewAction} name='create'>
          Create Tasks
        </button>
        <button className='btn btn-warning' onClick={viewAction} name='get'>
          Get Tasks
        </button>
        <button className='btn btn-warning' onClick={viewAction} name='assign'>
          Assign Tasks
        </button>
        <button className='btn btn-warning' onClick={viewAction} name='view_assigned'>
          Get Assigned Tasks
        </button>
      </div>

      <div className='taskAction'>
        {taskActionVisible ? (
          taskAction === "create" ? (
            <>
              <h1>Create Tasks</h1>
              <CreateTasks />
            </>
          ) : taskAction === "get" ? (
            <>
              <h1>Get Tasks</h1>
              <GetTasks />
            </>
          ) : taskAction === "assign" ? (
            <>
              <h1>Assign Tasks</h1>
              <AssignTasks />
            </>
          ) : taskAction === "view_assigned" && (
            <>
              <h1>View Assigned Tasks</h1>
              <AssignedTask />
            </>
          )
        ) : (
          <div className='instructions'>          
        <h1>
          Create a Task and Assign it here
        </h1>
        <br />
        <ol>
          <li>Create a Task</li>
          <li>Assign it to someone within your household</li>
        </ol>
        </div>
        )}
      </div>
    </div>
  );
};

export default TaskForm;
