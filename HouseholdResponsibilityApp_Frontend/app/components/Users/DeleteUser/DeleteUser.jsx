"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiDelete } from '../../../(utils)/api';
import './DeleteUser.css';

const DeleteUser = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [message, setMessage] = useState('');

    const onSubmit = async (data) => {
        try {
            await apiDelete(`/user/${data.userId}`);
            setMessage('User deleted successfully!');
        } catch (error) {
            setMessage('An error occurred while deleting the user');
        }
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="user-form">
            <label htmlFor="userId">User ID:</label>
            <input type="number" id="userId" {...register('userId', { required: 'User ID is required' })} />
            {errors.userId && <span>{errors.userId.message}</span>}

            <button type="submit" className='btn btn-danger'>Delete</button>
            {message && <p>{message}</p>}
        </form>
    );
};

export default DeleteUser;
