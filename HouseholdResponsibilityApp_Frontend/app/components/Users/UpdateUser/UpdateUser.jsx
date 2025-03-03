"use client";
import { useForm } from 'react-hook-form';
import React, { useState, useEffect } from 'react';
import { apiPut, apiFetch } from '../../../../(utils)/api';
import Loading from '../../../../(utils)/Loading';
import './UpdateUser.css';
import { useAuth } from '../../AuthContext/AuthProvider';

const UpdateUser = () => {

    const { user } = useAuth();

    const { register, handleSubmit, formState: { errors } } = useForm();
    const [message, setMessage] = useState('');
    const [loading, setLoading] = useState(true);
    const [userData, setUserData] = useState(null);

    useEffect(() => {

        const getUser = async () => {
            try {
                const fetchedUserData = await apiFetch(`/user/${user.userId}`);
                setUserData(fetchedUserData);                
            } catch (error) {
                console.log("Error getting user: ", error);
            } finally {
                setLoading(false);
            }
        };

        getUser();

    }, []);

    const onSubmit = async (data) => {
        const userData = {
            username: data.username,
            email: data.email,
            firstName: data.firstName,
            lastName: data.lastName,
            password: data.password,
        };

        try {
            let response = await apiPut(`/user/${user.userId}`, userData);
            setMessage("User updated successfully!");
        } catch (error) {
            console.log(error);
            setMessage("An error occurred while updating the user");
        }
    };


    return (
        <>
            {loading ? <Loading /> : (

                <form onSubmit={handleSubmit(onSubmit)} className="userDataUpdate-form">

                    <label htmlFor="username">Username:</label>
                    <input type="text" defaultValue={userData.username} id="username" {...register('username', { required: 'Username is required' })} />
                    {errors.username && <span>{errors.username.message}</span>}

                    <label htmlFor="email">Email:</label>
                    <input type="email" id="email" defaultValue={userData.email} {...register('email', { required: 'Email is required' })} />
                    {errors.email && <span>{errors.email.message}</span>}

                    <label htmlFor="firstName">First Name:</label>
                    <input type="text" id="firstName" defaultValue={userData.firstName} {...register('firstName', { required: 'First Name is required' })} />
                    {errors.firstName && <span>{errors.firstName.message}</span>}

                    <label htmlFor="lastName">Last Name:</label>
                    <input type="text" id="lastName" defaultValue={userData.lastName}  {...register('lastName', { required: 'Last Name is required' })} />
                    {errors.lastName && <span>{errors.lastName.message}</span>}

                    <label htmlFor="password">Password:</label>
                    <input type="password" id="password" {...register('password', { required: 'Password is required' })} autoComplete="new-password" />
                    {errors.password && <span>{errors.password.message}</span>}

                    <button type="submit" className='btn btn-success'>Update</button>
                    {message && <p>{message}</p>}
                </form>
            )}
        </>
    );
};

export default UpdateUser;
