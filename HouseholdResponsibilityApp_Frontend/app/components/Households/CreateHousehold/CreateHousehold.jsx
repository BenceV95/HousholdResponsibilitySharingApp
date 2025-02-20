"use client";
import { useState } from "react";
import { useAuth } from "../../AuthContext/AuthProvider"; 
import "./CreateHousehold.css";
import { apiPost } from "../../../../(utils)/api";

export default function CreateHousehold() {
  const [householdName, setHouseholdName] = useState("");
  const { user } = useAuth();

  async function sendHouseholdCreateRequest(e) {
    e.preventDefault(); 
    if (householdName && user) {
      try {
        const payload = {
          name: householdName,
          userId: user.userId, 
        };
        await apiPost("/household", payload);
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
