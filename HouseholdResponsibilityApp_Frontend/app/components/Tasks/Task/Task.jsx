import React from 'react'

const Task = ({ data, deleteTask }) => {

    return (
        <>
            <h1>{data.title}</h1>
            <span>{data.description ? (data.description) : (<i>No description provided.</i>)}</span><br />
            <span>Priority: {data.priority ? (<>ASAP</>) : (<>No</>)}</span><br />
            <span>Group: {data.groupId}</span><br />
            <span>Created By: {data.userId}</span><br />
            <span>Task ID: {data.taskId}</span><br />
            <button className='btn btn-danger' id={data.taskId} onClick={(e) => deleteTask(e)}>DELETE</button>
        </>
    )
}

export default Task