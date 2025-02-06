"use client";
import React, { useState } from 'react'
import { apiDelete, apiFetch, apiPut, apiPost } from '../../../(utils)/api';
import './GetTasks.css'
import Loading from '../../../(utils)/Loading';
import Task from '../Task/Task';

const GetTasks = () => {

    const [data, setData] = useState({});
    const [loading, setLoading] = useState(false);

    const getData = async () => {
        setLoading(true);
        console.log("downloading");
        const promise = await apiFetch("/tasks");
        setData(promise);
        console.log(promise);
        setLoading(false);
    }

    const deleteTask = async (e) => {
        console.log(e.target.id);
        const filtered = data.filter(t => t.taskId != e.target.id);
        setData(filtered);

        const deletePromise = await apiDelete(`/task/${e.target.id}`);
        console.log("Deleted ",e.target.id);
        
    }

    return (
        <div className='getTasks'>
            <div className='getDataButton'>
                <button onClick={getData} className='btn btn-primary'>Get Data</button>
            </div>
            <div className='display'>
                {loading ? (<Loading />) :
                    (
                        Object.entries(data).map(([dataID, dataEntry]) => (
                            <div key={dataID} className='taskData'>
                                <Task data={dataEntry} deleteTask={deleteTask}/>
                            </div>
                        ))

                    )}

            </div>
        </div>
    )
}

export default GetTasks