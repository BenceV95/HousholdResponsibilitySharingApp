"use client";
import { useState } from "react"
import AllHouseholds from "../components/Households/AllHouseholds/AllHouseholds"
import CreateHousehold from "../components/Households/CreateHousehold/CreateHouseholdModal";
import ChangeHouseholdName from "../components/Households/ChangeHouseholdNameModal/ChangeHouseholdNameModal";
import JoinHousehold from "../components/Households/JoinHousehold/JoinHouseholdModal";

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
                            action === "sendInvite" ? <JoinHousehold /> : null
            }

        </>
    )
}