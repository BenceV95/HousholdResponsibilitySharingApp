import React from "react";

function Task({ data, deleteTask, userName, groupName }) {
  return (
    <div>
      <h3>{data.title}</h3>
      <p>
        {data.description
          ? data.description
          : <i>No description provided.</i>}
      </p>
      <p>Priority: {data.priority ? "ASAP" : "No"}</p>
      <p>Group: {groupName}</p>
      <p>Created By: {userName}</p>
      <p>Task ID: {data.taskId}</p>

      <button className="btn btn-danger" id={data.taskId} onClick={deleteTask}>
        DELETE
      </button>
    </div>
  );
}


export default Task;
