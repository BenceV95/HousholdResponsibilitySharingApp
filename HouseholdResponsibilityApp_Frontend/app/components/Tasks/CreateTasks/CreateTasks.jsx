"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiFetch, apiPut, apiPost, apiDelete } from '../../../../(utils)/api';
import './CreateTasks.css';
import uuidv4 from '../../../../(utils)/uuidv4';

const CreateTasks = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [message, setMessage] = useState('');



    const onSubmit = async (data) => {

        const taskData = {
            title: data.title,
            description: data.description,
            createdbyid: data.createdBy,
            groupid: data.groupId,
            priority: data.priority || false,
            householdid: data.householdId
        };

        console.log(taskData);

        try {
            let postedForm = await apiPost("/task", taskData);
            console.log(postedForm);
            setMessage('Successfully posted form data !');

        } catch (error) {
            console.log(error);
            setMessage('An error occurred while posting the task');
        }
    };

    return (
        <form onSubmit={handleSubmit(onSubmit)} className="task-form">
            <label htmlFor="title">Title:</label>
            <input
                type="text"
                id="title"
                {...register('title', { required: 'Title is required' })}
            />
            {errors.title && <span>{errors.title.message}</span>}

            <label htmlFor="description">Description:</label>
            <textarea
                id="description"
                {...register('description')}
            />

            <label htmlFor="createdBy">Created By (User ID):</label>
            <input
                type="number"
                id="createdBy"
                {...register('createdBy', { required: 'Created By is required' })}
            />
            {errors.created_by && <span>{errors.created_by.message}</span>}

            <label htmlFor="groupId">Group ID:</label>
            <input
                type="number"
                id="groupId"
                {...register('groupId', { required: 'Group ID is required' })}
            />
            {errors.groupId && <span>{errors.groupId.message}</span>}

            <label htmlFor="householdId">Household ID:</label>
            <input
                type="number"
                id="householdId"
                {...register('householdId', { required: 'Household ID is required' })}
            />
            {errors.householdId && <span>{errors.householdId.message}</span>}

            <label htmlFor="priority">Priority:</label>
            <input
                type="checkbox"
                id="priority"
                {...register('priority')}
            />

            <button type="submit" className='btn btn-success'>Submit</button>
            {message && <p>{message}</p>}
        </form>
    );
};

export default CreateTasks;