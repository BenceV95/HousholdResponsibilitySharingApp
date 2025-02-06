"use client"
import Loading from "../../../(utils)/Loading";
import { useState } from "react";
import Household from "../Household/Household";
import { apiFetch } from "../../../(utils)/api";








export default function AllHouseholds() {
    const [data, setData] = useState([]);
    const [loading, setLoading] = useState(false);


    console.log("data:", data)
    const getData = async () => {
        setLoading(true);
        console.log("downloading");
        const promise = await apiFetch("/households");
        setData([...promise]);
        console.log(promise);
        setLoading(false);
    }

    return (
        <div >
            <div>
                <button onClick={getData} className='btn btn-primary'>Get Data</button>
            </div>
            <div className='display'>
                {loading ? (<Loading />) :
                    data?.map(household => 
                      <Household key={household.householdResponseDtoId} dataEntry={household} />
                    )
                }

            </div>
        </div>
    )
}