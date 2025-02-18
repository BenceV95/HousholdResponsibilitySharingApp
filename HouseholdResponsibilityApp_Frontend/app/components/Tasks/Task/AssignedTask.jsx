"use client";
import React, { useState } from 'react'
import { apiFetch, apiPut, apiDelete, apiPost } from '../../../../(utils)/api';
import Loading from '../../../../(utils)/Loading';

const AssignedTask = () => {

    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);

    const getData = async () => {
        setLoading(true);
        console.log("downloading");
        const promise = await apiFetch("/scheduleds");
        setData(promise);
        console.log(promise);
        setLoading(false);
    }

    const deleteTask = async (e) => {
        console.log(e.target.id);
        const filtered = data.filter(t => t.scheduledTaskId != e.target.id);
        setData(filtered);

        const deletePromise = await apiDelete(`/scheduled/${e.target.id}`);
        console.log("Deleted ", e.target.id);

    }

    return (
        <div className='viewAssignedTasks'>
            <div className='getDataButton'>
                <button onClick={getData} className='btn btn-primary'>Get Data</button>
            </div>
            <div className='display'>
                {loading ? (<Loading />) :
                    (data.map((dataEntry, i) => (
                        <div key={i} className='taskData'>
                            <span>scheduledTaskId: {dataEntry.scheduledTaskId}</span><br />
                            <span>householdTaskId: {dataEntry.householdTaskId}</span><br />
                            <span>createdByUserId: {dataEntry.createdByUserId}</span><br />
                            <span>createdAt: {dataEntry.createdAt}</span><br />
                            <span>repeat: {dataEntry.repeat}</span><br />
                            <span>eventDate: {dataEntry.eventDate}</span><br />
                            <span>atSpecificTime: {dataEntry.atSpecificTime}</span><br />
                            <span>assignedToUserId: {dataEntry.assignedToUserId}</span><br />
                            <button className='btn btn-danger' id={dataEntry.scheduledTaskId} onClick={(e) => deleteTask(e)}>DELETE</button>
                        </div>
                    )))}
            </div>
        </div>
    )
}

export default AssignedTask