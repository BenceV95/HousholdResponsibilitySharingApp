"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiPut } from '../../../../(utils)/api';
import './UpdateUser.css';

const UpdateUser = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [message, setMessage] = useState('');

    const onSubmit = async (data) => {
        const userData = {
            username: data.username,
            email: data.email,
            firstName: data.firstName,
            lastName: data.lastName,
            password: data.password, 
        };
    
        try {
            let response = await apiPut(`/user/${data.id}`, userData);
            console.log(response);
            setMessage("User updated successfully!");
        } catch (error) {
            console.log(error);
            setMessage("An error occurred while updating the user");
        }
    };
    

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="user-form">
            <label htmlFor="id">User ID:</label>
            <input type="text" id="id" {...register('id', { required: 'User ID is required' })} />
            {errors.id && <span>{errors.id.message}</span>}

            <label htmlFor="username">Username:</label>
            <input type="text" id="username" {...register('username', { required: 'Username is required' })} />
            {errors.username && <span>{errors.username.message}</span>}

            <label htmlFor="email">Email:</label>
            <input type="email" id="email" {...register('email', { required: 'Email is required' })} />
            {errors.email && <span>{errors.email.message}</span>}

            <label htmlFor="firstName">First Name:</label>
            <input type="text" id="firstName" {...register('firstName', { required: 'First Name is required' })} />
            {errors.firstName && <span>{errors.firstName.message}</span>}

            <label htmlFor="lastName">Last Name:</label>
            <input type="text" id="lastName" {...register('lastName', { required: 'Last Name is required' })} />
            {errors.lastName && <span>{errors.lastName.message}</span>}

            <label htmlFor="password">Password:</label>
            <input type="password" id="password" {...register('password', { required: 'Password is required' })} autoComplete="new-password" />
            {errors.password && <span>{errors.password.message}</span>}

            <button type="submit" className='btn btn-success'>Update</button>
            {message && <p>{message}</p>}
        </form>
    );
};

export default UpdateUser;
