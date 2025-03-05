"use client";
import React, { useState, useEffect } from "react";
import { useAuth } from "../components/AuthContext/AuthProvider";
import { apiFetch, apiDelete } from "../../(utils)/api";
import CreateHousehold from "../components/Households/CreateHousehold/CreateHousehold";
import JoinHousehold from "../components/Households/JoinHousehold/page";
import Household from "../components/Households/Household/Household";
import Loading from "../../(utils)/Loading";
import "./Profile.css";

import EditProfileModal from "../components/Profile/EditProfileModal";
import ChangeHouseholdNameModal from "../components/Households/ChangeHouseholdNameModal/ChangeHouseholdNameModal";

export default function Profile() {
  const { user, logout, setUser } = useAuth();

  const [household, setHousehold] = useState(null);
  const [loading, setLoading] = useState(false);
  const [responseMessage, setResponseMessage] = useState("");
  const [isError, setIsError] = useState(false);

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isHouseholdModalOpen, setIsHouseholdModalOpen] = useState(false);

  const [action, setAction] = useState(null);

  useEffect(() => {
    async function getHousehold() {
      setLoading(true);
      setResponseMessage("");
      setIsError(false);
      try {
        const householdData = await apiFetch(`/household/${user.householdId}`);
        setHousehold(householdData);
      } catch (error) {
        setIsError(true);
        setResponseMessage(error.message || "Failed to fetch household.");
      } finally {
        setLoading(false);
      }
    }
    if (user?.householdId) {
      getHousehold();
    }
  }, [user]);

  function handleProfileUpdated(updatedUser) {
    setResponseMessage("Profile updated successfully!");
  }
  function handleHouseholdNameChanged(updatedHousehold) {
    setHousehold(updatedHousehold);
    setResponseMessage("Household name changed successfully!");
  }
  const deleteAccount = async () => {
    if (!window.confirm("Are you sure you want to delete your account?")) {
      return;
    }
    try {
      const response = await apiDelete(`/users/${user.userId}`);
      setResponseMessage(response.Message || "Account deleted. Goodbye!");
      logout();
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message || "Error deleting account.");
    }
  };

  return (
    <div className="profile-page">
      <h1>Hello, {user?.userName?.toUpperCase()}!</h1>

      {responseMessage && (
        <p className={isError ? "error" : "success"}>{responseMessage}</p>
      )}

      <div className="user-actions">
        <div className="household-info">
          {user?.householdId ? (
            household ? (
              <>
                <Household dataEntry={household} />
                <button
                  className="btn btn-danger"
                  onClick={() => alert("Does not work yet")}
                >
                  Delete/Leave Household (WIP)
                </button>
                <button
                  className="btn btn-warning"
                  onClick={() => setIsHouseholdModalOpen(true)}
                >
                  Change Household Name
                </button>
              </>
            ) : (
              loading && <Loading />
            )
          ) : (
            <>
              <h3>You are not part of any household.</h3>
              <button
                className="btn btn-success"
                onClick={() => setAction("create")}
              >
                Create one
              </button>
              <button
                className="btn btn-primary"
                onClick={() => setAction("join")}
              >
                Join a household
              </button>
            </>
          )}
        </div>

        <div className="Account-Actions">
          <h3>Account Actions</h3>
          <button
            className="btn btn-warning"
            onClick={() => setIsEditModalOpen(true)}
          >
            Edit Profile
          </button>
          <button className="btn btn-danger" onClick={deleteAccount}>
            Delete Account
          </button>
        </div>
      </div>

      <EditProfileModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        onProfileUpdated={handleProfileUpdated}
      />

      <ChangeHouseholdNameModal
        isOpen={isHouseholdModalOpen}
        onClose={() => setIsHouseholdModalOpen(false)}
        householdId={household?.householdResponseDtoId}
        userId={user?.userId}
        onHouseholdNameChanged={handleHouseholdNameChanged}
      />

      {action === "create" && <CreateHousehold />}
      {action === "join" && <JoinHousehold />}

      <div className="notes">
        <ul>
          <li>Add an option here that once the user has a household and if the admin then edit household.</li>
          <li>User deletion is broken ATM both for users with or without household. <b>Must investigate</b></li>
          <li>Refresh site once user has joined or created household else broken</li>
          <li>We need to use PATCH for the user to edit their settings so only the necessary info needs to be sent/updated</li>
        </ul>
      </div>
    </div>
  );
}
