"use client";
import { useState } from "react";
import { useAuth } from "../../AuthContext/AuthProvider";
import "./CreateHousehold.css";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";

export default function CreateHousehold() {
  const [householdName, setHouseholdName] = useState("");
  const [response, setResponse] = useState("");
  const [loading, setLoading] = useState(false);
  const { user, setUser } = useAuth();

  async function sendHouseholdCreateRequest(e) {

    e.preventDefault();
    setLoading(true);
    if (householdName && user) {
      try {
        const payload = {
          householdName: householdName,
        };

        const householdId = await apiPost("/household", payload);
        if (householdId != null) {
          setResponse(`Household ${householdName} created!`);
        }
        //refresh the user token, to store the newly created households id

        // so this works, it assigns a new token but its not refreshing the user global state... just if i hit reload page
       const refreshResult =  await apiFetch("/Auth/update-token")

       console.log("refreshResult", refreshResult)
       
       
      } catch (e) {
        setResponse("Error: "+e);
      }
      finally {
        setLoading(false);
      }
    }
  }

  return (
    <div className="create-household">
      <h3>What should your hosehould be called ?</h3>
      <form onSubmit={sendHouseholdCreateRequest}>
        <input
          onChange={(e) => setHouseholdName(e.target.value)}
          type="text"
          name="householdName"
          id="householdName"
          placeholder="Household name"
          minLength={5}
          maxLength={20}
        />
        <br />
        <button type="submit" disabled={loading} className="btn btn-success">
          Create
        </button>
      </form>
      <div className="response">
      {loading ? <Loading /> :
        <p>{response && response}</p>
      }
      </div>
    </div>
  );
}
