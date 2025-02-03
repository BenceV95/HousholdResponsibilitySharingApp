"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiFetch, apiPut } from '../(utils)/api';
import './tasks.css';
import CreateTasks from '../components/Tasks/CreateTasks/CreateTasks';
import GetTasks from '../components/Tasks/GetTasks/GetTasks';

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

  return (
    <div className='tasks'>
      <div className='taskButtons'>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='create'>Create Tasks</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='get'>Get Tasks</button>
      </div>
      {taskActionVisible && (
        taskAction == "create" ? (
          <>
            <h1>Create Tasks</h1>
            <CreateTasks />
          </>) :
          taskAction == "get" && (
            <>
              <GetTasks />
            </>
          )
      )}

    </div>
  );
};

export default TaskForm;