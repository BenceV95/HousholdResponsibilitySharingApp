import React, { useState } from "react";
import { apiFetch, apiPost } from "../../../../(utils)/api";
import Loading from "../../../../(utils)/Loading";
import "./JoinHousehold.css";

const JoinHousehold = () => {
  const [responseMessage, setResponseMessage] = useState(""); 
  const [isError, setIsError] = useState(false); 
  const [loading, setLoading] = useState(false);
  const [joinSuccess, setJoinSuccess] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setResponseMessage("");
    setIsError(false);
    setLoading(true);

    const formData = new FormData(e.target);
    const householdId = `/household/join?id=${formData.get("householdId")}`;

    try {
      const response = await apiPost(householdId);
      
      setJoinSuccess(true);
      setResponseMessage(response?.Message || "Successfully joined household!"); 

      await apiFetch("/Auth/refresh");

    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="join-household">
      <h2>Join Household</h2>
      <form onSubmit={handleSubmit}>
        <input
          type="number"
          placeholder="Household ID"
          name="householdId"
          disabled={joinSuccess} 
          min={0}
        /><br />

        <button type="submit" className="btn btn-success" disabled={loading || joinSuccess}>
          {loading ? "Joining..." : "Join"}
        </button>
      </form>

      <div className="response">
        {loading ? <Loading /> : responseMessage && (
          <h3 style={{ color: isError ? "red" : "green" }}>{responseMessage}</h3>
        )}
      </div>
    </div>
  );
};

export default JoinHousehold;
