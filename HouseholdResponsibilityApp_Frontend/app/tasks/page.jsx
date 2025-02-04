"use client";
import { useState } from 'react';
import { apiFetch, apiPut } from '../(utils)/api';
import './tasks.css';
import CreateTasks from '../components/Tasks/CreateTasks/CreateTasks';
import GetTasks from '../components/Tasks/GetTasks/GetTasks';
import AssignTasks from '../components/Tasks/AssignTasks/AssignTasks';
import AssignedTask from '../components/Tasks/Task/AssignedTask';

const TaskForm = () => {

  const [taskActionVisible, setTaskActionVisible] = useState(false);
  const [taskAction, setTaskAction] = useState("");

  const viewAction = (e) => {
    let action = e.target.name;
    if (taskActionVisible && taskAction == action) {
      setTaskActionVisible(false);
      setTaskAction("");
    } else {

      setTaskAction(action);
      setTaskActionVisible(true);
    }

  }

  /* 
    Once the proper backend is in place,
    massive rewrite is required,
    this is just for Sprint 2,
    so that we can demo our progress.
  */
  return (
    <div className='tasks'>
      <div className='taskButtons'>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='create'>Create Tasks</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='get'>Get Tasks</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='assign'>Assign Tasks</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='view_assigned'>Get Assigned Tasks</button>
      </div>
      <div className='taskAction'>
        {taskActionVisible && (
          taskAction == "create" ? (
            <>
              <h1>Create Tasks</h1>
              <CreateTasks />
            </>) :
            taskAction == "get" ? (
              <>
                <h1>Get Tasks</h1>
                <GetTasks />
              </>
            ) :
            taskAction == "assign" ? (
              <>
              <h1>Assign Tasks</h1>
              <AssignTasks />
              </>
            ) :
            (taskAction == "view_assigned" &&
              <>
                <h1>View Assigned Tasks</h1>
                <AssignedTask />
              </>
            )
        )}
      </div>
    </div>
  );
};

export default TaskForm;