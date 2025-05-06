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
        <button className={`btn btn-warning ${taskAction === 'create' ? 'selected' : ''}`} onClick={viewAction} name='create'>
          Create Tasks
        </button>
        <button className={`btn btn-warning ${taskAction === 'get' ? 'selected' : ''}`} onClick={viewAction} name='get'>
          Get Tasks
        </button>
        <button className={`btn btn-warning ${taskAction === 'assign' ? 'selected' : ''}`} onClick={viewAction} name='assign'>
          Assign Tasks
        </button>
        <button className={`btn btn-warning ${taskAction === 'view_assigned' ? 'selected' : ''}`} onClick={viewAction} name='view_assigned'>
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
        <ol className='mainList'>
          <li>Create a Task</li>
          <ol className='subList'>
            <li>Add a title</li>
            <li>Add a description</li>
            <li>Add a Group</li>
            <li>Priority means that task should be done ASAP</li>
          </ol>
          <li>Assign it to someone within your household</li>
          <ol className='subList'>
            <li>Select the appropiate task</li>
            <li>Assign it to someone within your household</li>
            <li>Choose the repeat frequency (WIP)</li>
            <li>Choose the date and time</li>
          </ol>
          <li>Once you have created a task and assigned it to someone you can check under calendar and manage it.</li>
        </ol>
        </div>
        )}
      </div>
    </div>
  );
};

export default TaskForm;
