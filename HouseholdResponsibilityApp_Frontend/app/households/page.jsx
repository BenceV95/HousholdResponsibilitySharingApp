"use client"

import AllHouseholds from "../components/Households/AllHouseholds/AllHouseholds"



export default function Households() {
    return (
        <>
            <button className='btn btn-warning'>Create Household</button>
            <button className='btn btn-warning' >Get Households</button>
            <button className='btn btn-warning'>Change Household Name</button>
            <button className='btn btn-warning' >Send Household Invite</button>
            <AllHouseholds/>
        </>
    )
}