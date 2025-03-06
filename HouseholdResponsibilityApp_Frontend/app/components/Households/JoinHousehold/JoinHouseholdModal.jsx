"use client";
import React, { useState } from "react";
import { useAuth } from "../../AuthContext/AuthProvider";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";
import "./JoinHouseholdModal.css";

const JoinHouseholdModal = ({ isOpen, onClose, setSuccesFullyJoined }) => {
  if (!isOpen) return null; 

  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);
  const [loading, setLoading] = useState(false);
  const [joinSuccess, setJoinSuccess] = useState(false);
  const { user, setUser } = useAuth();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setResponseMessage("");
    setIsError(false);
    setLoading(true);

    const formData = new FormData(e.target);
    const householdId = formData.get("householdId");

    try {
      const response = await apiPost(`/household/join?id=${householdId}`);

      if (response) {
        setJoinSuccess(true);
        setResponseMessage(response?.Message || "Successfully joined household!");

        const refreshResult = await apiFetch("/Auth/refresh");
        setUser({ ...user, householdId: householdId });

        console.log("refreshResult", refreshResult);

        setSuccesFullyJoined(true);
        onClose(); 
      }
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Join Household</h2>
        <form onSubmit={handleSubmit} className="modal-form">
          <input
            type="number"
            placeholder="Household ID"
            name="householdId"
            disabled={joinSuccess}
            min={1}
            required
          />
          <div className="modal-buttons">
            <button type="submit" className="btn btn-success" disabled={loading || joinSuccess}>
              {loading ? <Loading /> : "Join"}
            </button>
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>
        {responseMessage && (
          <h3 className="modal-message" style={{ color: isError ? "red" : "green" }}>{responseMessage}</h3>
        )}
      </div>
    </div>
  );
};

export default JoinHouseholdModal;
