"use client"

import { useState } from "react"
import { apiPut } from "../../../(utils)/api"
import AllHouseholds from "../AllHouseholds/AllHouseholds";

export default function ChangeHouseholdName() {
    const [newName, setNewName] = useState(null);

    //in the future this will be extracted from the url
    const [householdId, setHouseholdId] = useState(null);

    async function changeName() {
        //also, here this catches an exception, beacuse we try to parse the response, which has no content
        try {
            await apiPut(`/household/${householdId}`, { name: newName })
        } catch (e) {
            alert(e)
        }
    }



    return (
        <div className="change-household-name">
            <form onSubmit={changeName} action="">
                <input placeholder="New name" onChange={(e) => setNewName(e.target.value)} type="newName" />
                <input placeholder="Household Id" onChange={(e) => setHouseholdId(parseInt(e.target.value))} type="number" name="householdId" id="" />
                <button type="submit" className="btn btn-primary" >Change</button>
            </form>
        </div>
    )
}