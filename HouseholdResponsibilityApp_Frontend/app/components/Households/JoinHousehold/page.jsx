import React, { useState } from 'react'
import { apiPost } from '../../../../(utils)/api';
import Loading from '../../../../(utils)/Loading';
import './JoinHousehold.css';

const JoinHousehold = () => {

    const [response, setResponse] = useState("");
    const [loading, setLoading] = useState(false);
    const [joinSuccess, setJoinSuccess] = useState(false);

    const handleSubmit = async (e) => {

        e.preventDefault();
        setLoading(true);
        const formData = new FormData(e.target);
        const householdId = `/household/join?id=${formData.get('householdId')}`;

        try {
            const response = await apiPost(householdId);
            setJoinSuccess(true);
            setResponse(response.message);
            const refreshResult =  await apiFetch("/Auth/refresh");
        } catch (error) {
            setResponse('Failed to join household');
            console.log(error);
        }
        finally {
            setLoading(false);
        }
    }

    return (
        <div className='join-household'>
            <h2>Join Household</h2>
            <form onSubmit={handleSubmit}>
                <input
                    type="number"
                    placeholder="Household ID"
                    name='householdId'
                    disabled={joinSuccess}
                    min={0}
                /><br />
                <button type="submit" className='btn btn-success' disabled={loading || joinSuccess}>Join</button>
            </form>
            <div className='response'>
                {loading && <Loading />}
                <h3 style={{ color: joinSuccess ? '#28a745' : '#dc3545' }}>
                    {response}
                </h3>
            </div>
        </div>
    )
}

export default JoinHousehold