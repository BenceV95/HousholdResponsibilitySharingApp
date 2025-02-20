import "./Household.css";

export default function Household({ dataEntry }) {
  const createdAtDate = new Date(dataEntry.createdAt);

  return (
    <div className="householdCard">
      Household ID: {dataEntry.householdResponseDtoId} <br />
      Household name : {dataEntry.name} <br />
      Created by : {dataEntry.createdByUsername} <br />
      Created at: {createdAtDate.toLocaleString()} <br />
    </div>
  );
}
