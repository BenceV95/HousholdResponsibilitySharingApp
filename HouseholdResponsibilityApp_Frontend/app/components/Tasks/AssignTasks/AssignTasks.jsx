import { useForm } from 'react-hook-form';
import React, { useState } from 'react';
import { apiDelete, apiFetch, apiPut, apiPost } from '../../../../(utils)/api';
import './AssignTasks.css';
import uuidv4 from '../../../../(utils)/uuidv4';

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

        data.eventDate = new Date(data.eventDate).toISOString();
        data.repeat = parseInt(data.repeat,10);
        const modifiedData = {
            ...data,
            atSpecificTime: specificTimeSelected
        }

        console.log(modifiedData);
        
        clearErrors("event_date");

        try {
            const promise = await apiPost("/scheduled", modifiedData);
            console.log(promise);

            setSuccess("Task scheduled successfully!");
        } catch (error) {
            setSuccess("Failed to schedule task.");
        }
    };

    return (
        <div className="assignTasks">
            <form onSubmit={handleSubmit(onSubmit)}>

                <input type="number" {...register("householdTaskId", { required: true, setValueAs: value => value === "" ? undefined : parseInt(value, 10) })} placeholder='Task ID' />
                {errors.householdTaskId && <p className="error">Task ID is required</p>}

                <input type="number" {...register("createdByUserId", { required: true, setValueAs: value => value === "" ? undefined : parseInt(value, 10) })} placeholder='Created By (UserId)' />
                {errors.createdByUserId && <p className="error">Created By is required</p>}

                <input type="number" {...register("assignedToUserId", { required: true, setValueAs: value => value === "" ? undefined : parseInt(value, 10) })} placeholder='Assigned To (UserId)' />
                {errors.assignedToUserId && <p className="error">Assigned To is required</p>}

                <label>Repeat Frequency:</label>
                <div className='repeatRadios'>
                    <label>
                        <input type="radio" value="0" {...register("repeat") } /> Daily
                    </label>
                    <label>
                        <input type="radio" value="1" {...register("repeat")} /> Weekly
                    </label>
                    <label>
                        <input type="radio" value="2" {...register("repeat")} /> Monthly
                    </label>
                    <label>
                        <input type="radio" value="3" {...register("repeat")} /> No Repeat
                    </label>                    
                </div>
                {errors.repeat && <p className="error">Repeat Frequency is required</p>}

                <label>Specific Time:</label>
                <label className="switch">
                    <input type="checkbox" onChange={SelectSpecificTime} />
                        <span className="slider round"></span>
                </label>
                
                <input type={specificTimeSelected ? ("datetime-local") : ("date")} {...register("eventDate", { required: true })} />
                {errors.eventDate && <p className="error">Date is required</p>}
                <br />
                <br />
                <button type="submit" className='btn btn-success'>Submit</button>
            </form>
            {success && <p className="success">{success}</p>}
        </div>
    );
}

export default AssignTasks