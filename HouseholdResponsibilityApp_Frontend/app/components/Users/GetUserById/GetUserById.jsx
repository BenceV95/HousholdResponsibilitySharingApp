"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiFetch } from '../../../(utils)/api';
import './GetUserById.css';

const GetUserById = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [user, setUser] = useState(null);
    const [error, setError] = useState(null);

    const onSubmit = async (data) => {
        try {
            const response = await apiFetch(`/user/${data.userId}`);
            setUser(response);
            setError(null);
        } catch (err) {
            setError("User not found");
            setUser(null);
        }
    };

    return (
        <div className='get-user-by-id'>
            <h1>Find User by ID</h1>
            <form onSubmit={handleSubmit(onSubmit)}>
                <label htmlFor="userId">User ID:</label>
                <input type="number" id="userId" {...register('userId', { required: 'User ID is required' })} />
                {errors.userId && <span>{errors.userId.message}</span>}

                <button type="submit" className='btn btn-primary'>Search</button>
            </form>

            {error && <p className='error'>{error}</p>}
            {user && (
                <div className="user-info">
                    <p><strong>Username:</strong> {user.username}</p>
                    <p><strong>Email:</strong> {user.email}</p>
                    <p><strong>Name:</strong> {user.firstName} {user.lastName}</p>
                </div>
            )}
        </div>
    );
};

export default GetUserById;
