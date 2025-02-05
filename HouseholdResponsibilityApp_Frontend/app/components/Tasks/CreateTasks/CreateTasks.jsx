"use client";
import { useForm } from 'react-hook-form';
import { useState } from 'react';
import { apiFetch,apiPut } from '../../../(utils)/api';
import './CreateTasks.css';
import uuidv4 from '../../../(utils)/uuidv4';

const CreateTasks = () => {
    const { register, handleSubmit, formState: { errors } } = useForm();
    const [message, setMessage] = useState('');

    

    const onSubmit = async (data) => {

        const taskData = {
            [uuidv4()]: {
                title: data.title,
                description: data.description,
                created_by: data.created_by,
                group_id: data.group_id,
                priority: data.priority || false,
            }
        };

        try {
            let postedForm = await apiPut("/tasks", taskData);
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
                {...register('created_by', { required: 'Created By is required' })}
            />
            {errors.created_by && <span>{errors.created_by.message}</span>}

            <label htmlFor="groupId">Group ID:</label>
            <input
                type="number"
                id="groupId"
                {...register('group_id')}
            />

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