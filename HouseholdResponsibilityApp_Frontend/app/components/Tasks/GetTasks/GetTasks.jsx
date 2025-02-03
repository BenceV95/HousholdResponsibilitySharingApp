"use client";
import React, { useState } from 'react'
import { apiFetch, apiPut } from '../../../(utils)/api';
import './GetTasks.css'

const GetTasks = () => {

    const [data, setData] = useState({});

    const getData = async () => {
        console.log("downloading");
        const promise = await apiFetch("/tasks");
        setData(promise);
        console.log(promise);

    }

    return (
        <div className='dataDisplay'>
            <div className='getData'>
                <button onClick={getData} className='btn btn-primary'>Get Data</button>
            </div>
            <div className='display'>
                {Object.entries(data).map(([dataID, dataEntry]) => (
                    <div key={dataID}>
                        <h3>{dataEntry.title}</h3>
                    </div>
                ))}
            </div>
        </div>
    )
}

export default GetTasks