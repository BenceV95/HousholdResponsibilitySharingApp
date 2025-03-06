"use client";
import { useState } from "react";
import { useAuth } from "../../AuthContext/AuthProvider";
import "./CreateHouseholdModal.css";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";

export default function CreateHouseholdModal({ isOpen, onClose, setSuccesFullyCreated }) {
  if (!isOpen) return null; 

  const [householdName, setHouseholdName] = useState("");
  const [response, setResponse] = useState("");
  const [loading, setLoading] = useState(false);
  const { user, setUser } = useAuth();

  async function sendHouseholdCreateRequest(e) {
    e.preventDefault();
    setSuccesFullyCreated(false);
    setLoading(true);

    if (householdName && user) {
      try {
        const payload = {
          name: householdName,
          userId: user.userId,
        };

        const householdId = await apiPost("/household", payload);
        if (householdId != null) {
          setResponse(`Household ${householdName} created!`);
        }

        const refreshResult = await apiFetch("/Auth/refresh");
        setUser({ ...user, householdId: householdId });
        console.log("refreshResult", refreshResult);

        setSuccesFullyCreated(true);
        setHouseholdName(""); 
        onClose(); 

      } catch (e) {
        setResponse("Error: " + e);
      } finally {
        setLoading(false);
      }
    }
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Create a Household</h2>
        
        <form onSubmit={sendHouseholdCreateRequest} className="modal-form">
          <input
            type="text"
            name="householdName"
            placeholder="Enter household name"
            minLength={5}
            maxLength={20}
            value={householdName}
            onChange={(e) => setHouseholdName(e.target.value)}
          />

          <div className="modal-buttons">
            <button type="submit" className="btn btn-success" disabled={loading}>
              {loading ? <Loading /> : "Create"}
            </button>
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>

        {response && <p className="modal-message">{response}</p>}
      </div>
    </div>
  );
}
