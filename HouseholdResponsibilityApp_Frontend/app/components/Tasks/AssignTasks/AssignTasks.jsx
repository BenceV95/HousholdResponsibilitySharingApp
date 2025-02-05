import { useForm } from 'react-hook-form';
import React, { useState } from 'react';
import { apiFetch, apiPut } from '../../../(utils)/api';
import './AssignTasks.css';
import uuidv4 from '../../../(utils)/uuidv4';

const AssignTasks = () => {
    const { register, handleSubmit, watch, setError, clearErrors, formState: { errors } } = useForm();
    const [success, setSuccess] = useState(null);
    const [specificTimeSelected, setSpecificTimeSelected] = useState(false);


    const SelectSpecificTime = () => {
        setSpecificTimeSelected(!specificTimeSelected)
    }


    const onSubmit = async (data) => {
        if (!data.repeat) {
            setError("repeat", { type: "manual", message: "No Repeat option is selected." });
            return;
        }

        let idAssignedData = {
            [uuidv4()]:{...data}
        }

        clearErrors("event_date");

        // API call simulation
        try {
            const promise = await apiPut("/assigned_tasks", idAssignedData);
            console.log(promise);

            setSuccess("Task scheduled successfully!");
        } catch (error) {
            setSuccess("Failed to schedule task.");
        }
    };

    return (
        <div className="assignTasks">
            <form onSubmit={handleSubmit(onSubmit)}>

                <input type="number" {...register("task_id", { required: true })} placeholder='Task ID' />
                {errors.task_id && <p className="error">Task ID is required</p>}

                <input type="number" {...register("created_by", { required: true })} placeholder='Created By (UserId)' />
                {errors.created_by && <p className="error">Created By is required</p>}

                <input type="number" {...register("assigned_to", { required: true })} placeholder='Assigned To (UserId)' />
                {errors.assigned_to && <p className="error">Assigned To is required</p>}

                <label>Repeat Frequency:</label>
                <div className='repeatRadios'>
                    <label>
                        <input type="radio" value="daily" {...register("repeat")} /> Daily
                    </label>
                    <label>
                        <input type="radio" value="weekly" {...register("repeat")} /> Weekly
                    </label>
                    <label>
                        <input type="radio" value="monthly" {...register("repeat")} /> Monthly
                    </label>
                    <label>
                        <input type="radio" value="no_repeat" {...register("repeat")} /> No Repeat
                    </label>                    
                </div>
                {errors.repeat && <p className="error">Repeat Frequency is required</p>}
                <label>Specific Time:</label>
                <label className="switch">
                    <input type="checkbox" onChange={SelectSpecificTime}/>
                        <span className="slider round"></span>
                </label>
                
                {/* toggle switch here. for switching between time and or date */}
                <input type={specificTimeSelected ? ("datetime-local") : ("date")} {...register("specific_time", { required: true })} />
                {errors.specific_time && <p className="error">Date is required</p>}
                <br />
                <br />
                <button type="submit" className='btn btn-success'>Submit</button>
            </form>
            {success && <p className="success">{success}</p>}
        </div>
    );
}

export default AssignTasks