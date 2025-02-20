import React from "react";

const Task = ({ data, deleteTask, userName, groupName }) => {
  return (
    <>
      <h1>{data.title}</h1>
      <span>
        {data.description ? data.description : <i>No description provided.</i>}
      </span>
      <br />
      <span>Priority: {data.priority ? "ASAP" : "No"}</span>
      <br />
      <span>Group: {groupName}</span>
      <br />
      <span>Created By: {userName}</span>
      <br />
      <span>Task ID: {data.taskId}</span>
      <br />
      <button className="btn btn-danger" id={data.taskId} onClick={deleteTask}>
        DELETE
      </button>
    </>
  );
};

export default Task;
