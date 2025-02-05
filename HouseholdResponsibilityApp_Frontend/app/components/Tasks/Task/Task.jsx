import React from 'react'

const Task = ({ data }) => {

    return (
        <>
            <h1>{data.title}</h1>
            <span>{data.description ? (data.description) : (<i>No description provided.</i>)}</span><br />
            <span>Priority: {data.priority ? (<>ASAP</>) : (<>No</>)}</span><br />
            <span>Group: {data.group_id}</span>
        </>
    )
}

export default Task