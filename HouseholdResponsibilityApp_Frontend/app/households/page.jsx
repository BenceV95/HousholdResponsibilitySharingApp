"use client"

import { useState } from "react"
import AllHouseholds from "../components/Households/AllHouseholds/AllHouseholds"
import CreateHousehold from "../components/Households/CreateHousehold/CreateHousehold";
import ChangeHouseholdName from "../components/Households/ChangeHouseholdName/ChangeHouseholdName";



function selectAction() {

}



export default function Households() {
    const [action, setAction] = useState(null);



    return (
        <>
            <button value={"create"} onClick={(e) => setAction(e.target.value)} className='btn btn-warning'>Create Household</button>
            <button value={"listAll"} onClick={(e) => setAction(e.target.value)} className='btn btn-warning' >Get Households</button>
            <button value={"changeName"} onClick={(e) => setAction(e.target.value)} className='btn btn-warning'>Change Household Name</button>
            <button value={"sendInvite"} onClick={(e) => setAction(e.target.value)} className='btn btn-warning' >Send Household Invite</button>
            {
                action === "create" ? <CreateHousehold /> :
                    action === "listAll" ? <AllHouseholds /> :
                        action === "changeName" ? <ChangeHouseholdName /> :
                            action === "sendInvite" ? <>sendInvite</> : null
            }

        </>
    )
}