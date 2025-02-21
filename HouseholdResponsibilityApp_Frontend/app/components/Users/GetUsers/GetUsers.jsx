"use client";
import { useState, useEffect } from 'react';
import { apiFetch } from '../../../../(utils)/api';
import './GetUsers.css';

const GetUsers = () => {
    const [users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchUsers = async () => {
            try {
                const response = await apiFetch("/users");
                setUsers(response);
            } catch (err) {
                setError("Failed to fetch users");
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, []);

    return (
        <div className='user-list'>
            <h1>Users</h1>
            {loading && <p>Loading...</p>}
            {error && <p className='error'>{error}</p>}
            <ul>
                {users.map((user, index) => (
                    <li key={user.UserResponseDtoId || `user-${index}`}>  
                        {user.username} - {user.email} - {user.firstName} {user.lastName}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default GetUsers;
