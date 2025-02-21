"use client";
import { useState } from "react";
import { useAuth } from "../../AuthContext/AuthProvider";
import "./CreateHousehold.css";
import { apiFetch, apiPost } from "../../../../(utils)/api";

export default function CreateHousehold() {
  const [householdName, setHouseholdName] = useState("");
  const { user, setUser } = useAuth();

  async function sendHouseholdCreateRequest(e) {
    e.preventDefault();
    if (householdName && user) {
      try {
        const payload = {
          name: householdName,
          userId: user.userId,
        };
        await apiPost("/household", payload);

        //refresh the user token, to store the newly created households id

        // so this works, it assigns a new token but its not refreshing the user global state... just if i hit reload page
       const refreshResult =  await apiFetch("/Auth/refresh")

       console.log("refreshResult", refreshResult)
      } catch (e) {
        alert(e);
      }
    }
  }

  return (
    <div className="create-household">
      <form onSubmit={sendHouseholdCreateRequest}>
        <input
          onChange={(e) => setHouseholdName(e.target.value)}
          type="text"
          name="householdName"
          id="householdName"
          placeholder="Household name"
        />
        <button type="submit" className="btn btn-primary">
          Create
        </button>
      </form>
    </div>
  );
}
