"use client";
import React, { useState } from "react";
import "./ChangeHouseholdNameModal.css";
import { apiPut, apiFetch } from "../../../../(utils)/api";

export default function ChangeHouseholdNameModal({
  isOpen,
  onClose,
  householdId,
  userId,
  onHouseholdNameChanged,
}) {
  if (!isOpen) return null; 

  const [newName, setNewName] = useState("");
  const [message, setMessage] = useState("");

  async function handleChange(e) {
    e.preventDefault();

    if (!newName.trim()) {
      setMessage("Household name cannot be empty.");
      return;
    }

    try {
      await apiPut(`/household/${householdId}`, {
        Name: newName,
        UserId: userId,
      });

      const updatedHousehold = await apiFetch(`/household/${householdId}`);
      onHouseholdNameChanged(updatedHousehold);

      setMessage("Household name changed successfully!");
      setNewName("");

      onClose();
    } catch (error) {
      setMessage(error.message);
    }
  }

  return (
    <div className="modal-overlay">
      <div className="modal-content">
        <h2>Change Household Name</h2>

        <form onSubmit={handleChange} className="modal-form">
          <input
            type="text"
            placeholder="New Household Name"
            value={newName}
            onChange={(e) => setNewName(e.target.value)}
          />

          <div className="modal-buttons">
            <button type="submit" className="btn btn-primary">
              Change
            </button>
            <button type="button" className="btn btn-secondary" onClick={onClose}>
              Cancel
            </button>
          </div>
        </form>

        {message && <p className="modal-message">{message}</p>}
      </div>
    </div>
  );
}
