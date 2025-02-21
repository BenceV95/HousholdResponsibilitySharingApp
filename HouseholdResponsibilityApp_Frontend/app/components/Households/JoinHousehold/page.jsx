import React from 'react'
import { apiPost } from '../../../../(utils)/api';

const JoinHousehold = () => {

    const handleSubmit = async (e) => {

        e.preventDefault();
        const formData = new FormData(e.target);
        const householdId = `/household/join?id=${formData.get('householdId')}`;
        try {
            const response = await apiPost(householdId);
            alert(response.message); 
        } catch (error) {
            alert('Failed to join household');
            console.log(error);
            
        }
    }
    
    return (
        <div>
            <h2>Join Household</h2>
            <form onSubmit={handleSubmit}>
                <input type="number" placeholder="Household ID" name='householdId' />
                <button type="submit" className='btn btn-success'>Join</button>
            </form>
        </div>
    )
}

export default JoinHousehold