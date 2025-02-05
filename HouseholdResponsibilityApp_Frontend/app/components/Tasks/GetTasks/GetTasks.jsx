"use client";
import React, { useState } from 'react'
import { apiFetch, apiPut } from '../../../(utils)/api';
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
                                <Task data={dataEntry} />
                            </div>
                        ))

                    )}

            </div>
        </div>
    )
}

export default GetTasks