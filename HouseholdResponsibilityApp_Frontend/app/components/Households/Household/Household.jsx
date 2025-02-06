import "./Household.css"

export default function Household({ dataEntry }) {
    return (
        <div className="householdCard">
            Household ID: {dataEntry.householdResponseDtoId} <br />
            Household name : {dataEntry.name} <br />
            Created by : {dataEntry.createdByUsername} <br />
            Created at : {dataEntry.createdAt} <br />
        </div>
    )
}