"use client";
import { useState, useEffect } from "react";
import { apiFetch } from "../../../../(utils)/api";
import "./GetUsers.css";

const GetUsers = () => {
  const [users, setUsers] = useState([]);
  const [responseMessage, setResponseMessage] = useState(""); 
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(true); 

  useEffect(() => {
    const fetchUsers = async () => {
      setResponseMessage("");
      setIsError(false);

      try {
        const response = await apiFetch("/users");
        setUsers(response);
        setResponseMessage(response.Message || "Users loaded successfully!"); 
      } catch (err) {
        setIsError(true);
        setResponseMessage(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchUsers();
  }, []);

  return (
    <div className="user-list">
      <h1>Users</h1>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}

      {loading && <p>Loading...</p>}

      {!loading && users.length > 0 ? (
        <ul>
          {users.map((user, index) => (
            <li key={user.UserResponseDtoId || `user-${index}`}>
              {user.username} - {user.email} - {user.firstName} {user.lastName}
            </li>
          ))}
        </ul>
      ) : (
        !loading && !isError && <p>No users found.</p> 
      )}
    </div>
  );
};

export default GetUsers;
