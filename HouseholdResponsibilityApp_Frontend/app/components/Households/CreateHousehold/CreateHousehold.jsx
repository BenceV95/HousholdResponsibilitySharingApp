"use client"
import { useState } from "react"
import "./CreateHousehold.css"
import { apiPost } from "../../../(utils)/api";


//this component also needs the user in the future
export default function CreateHousehold() {
    const [householdName, setHouseholdName] = useState(null);


    async function sendHouseholdCreateRequest() {
        if (householdName) {
            try {
                //this catches an error, cause its not fixed what the server should send back!
                await apiPost("/household", { name: householdName })

            } catch (e) {
                alert(e)
            }
        }
    }

    return (
        <div className="create-household">
            <form onSubmit={sendHouseholdCreateRequest} action="">
                <input onChange={(e) => setHouseholdName(e.target.value)} type="text" name="householdName" id="householdName" placeholder="Household name" />
                <button type="submit">Create</button>
            </form>
        </div>
    )
}