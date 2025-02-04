"use client";
import React, { useState } from 'react'
import { apiFetch, apiPut } from '../../../(utils)/api';
import Loading from '../../../(utils)/Loading';

const AssignedTask = () => {

    const [data, setData] = useState({});
    const [loading, setLoading] = useState(false);

    const getData = async () => {
        setLoading(true);
        console.log("downloading");
        const promise = await apiFetch("/assigned_tasks");
        setData(promise);
        console.log(promise);
        setLoading(false);
    }

    return (
        <div className='viewAssignedTasks'>
            <div className='getDataButton'>
                <button onClick={getData} className='btn btn-primary'>Get Data</button>
            </div>
            <div className='display'>
                {loading ? (<Loading />) :
                    (Object.entries(data).map(([dataID, dataEntry]) => (
                        <div key={dataID} className='taskData'>
                            <h3>task_id: {dataEntry.task_id}</h3>
                            <h3>assigned_to: {dataEntry.assigned_to}</h3>
                            <h3>created_by: {dataEntry.created_by}</h3>
                            <h3>repeat: {dataEntry.repeat}</h3>
                            <h3>specific_time: {dataEntry.specific_time}</h3>
                        </div>
                    )))}
            </div>
        </div>
    )
}

export default AssignedTask