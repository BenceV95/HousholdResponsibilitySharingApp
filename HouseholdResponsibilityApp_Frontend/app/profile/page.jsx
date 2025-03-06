"use client";
import React, { useState, useEffect } from "react";
import { useAuth } from "../components/AuthContext/AuthProvider";
import { apiFetch, apiDelete, apiPut } from "../../(utils)/api";
import CreateHouseholdModal from "../components/Households/CreateHousehold/CreateHouseholdModal";
import JoinHouseholdModal from "../components/Households/JoinHousehold/JoinHouseholdModal";
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
  const [succesFullyCreated, setSuccesFullyCreated] = useState(false);
  const [succesFullyJoined, setSuccesFullyJoined] = useState(false);

  const [isEditModalOpen, setIsEditModalOpen] = useState(false);
  const [isHouseholdModalOpen, setIsHouseholdModalOpen] = useState(false);
  const [isCreateModalOpen, setIsCreateModalOpen] = useState(false);
  const [isJoinModalOpen, setIsJoinModalOpen] = useState(false);

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
  }, [user, succesFullyCreated, succesFullyJoined]);

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
      const response = await apiDelete(`/user/delete-self`);
      const message = response?.Message || "Account deleted successfully!";
      alert(message);
      logout();
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message || "Error deleting account.");
    }
  };

  const deleteHousehold = async () => {
    if (!window.confirm("Are you sure you want to delete your household?")) {
      return;
    }
    try {
      await apiDelete(`/household/${user.householdId}`);
      setUser({ ...user, householdId: null });
      setHousehold(null);
      alert("Household deleted successfully!");
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message || "Error deleting household.");
    }
  };

  const leaveHousehold = async () => {
    if (!window.confirm("Are you sure you want to leave this household?")) {
      return;
    }
    try {
      console.log("user:", user );
      
      await apiPut(`/user/leave-household`, {});
      setUser({ ...user, householdId: null });
      setHousehold(null);
      alert("You have left the household successfully!");
    } catch (error) {
      setIsError(true);
      setResponseMessage(error.message || "Error leaving household.");
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

                {household.createdByUsername === user?.userName ? (
                  <>
                    <button className="btn btn-danger" onClick={deleteHousehold}>
                      Delete Household
                    </button>
                    <button
                      className="btn btn-warning"
                      onClick={() => setIsHouseholdModalOpen(true)}
                    >
                      Change Household Name
                    </button>
                  </>
                ) : (
                  <button className="btn btn-danger" onClick={leaveHousehold}>
                    Leave Household
                  </button>
                )}
              </>
            ) : (
              loading && <Loading />
            )
          ) : (
            <>
              <h3>You are not part of any household.</h3>
              <button
                className="btn btn-success"
                onClick={() => setIsCreateModalOpen(true)}
              >
                Create one
              </button>
              <button
                className="btn btn-primary"
                onClick={() => setIsJoinModalOpen(true)}
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

      <CreateHouseholdModal
        isOpen={isCreateModalOpen}
        onClose={() => setIsCreateModalOpen(false)}
        setSuccesFullyCreated={setSuccesFullyCreated}
      />

      <JoinHouseholdModal
        isOpen={isJoinModalOpen}
        onClose={() => setIsJoinModalOpen(false)}
        setSuccesFullyJoined={setSuccesFullyJoined}
      />

      <div className="notes">
        <ul>
          <li>
            Add an option here that once the user has a household and if the admin then edit household.
          </li>
          <li>
            User deletion is broken ATM both for users with or without household. <b>Must investigate</b>
          </li>
          <li>
            Refresh site once user has joined or created household else broken
          </li>
          <li>
            We need to use PATCH for the user to edit their settings so only the necessary info needs to be sent/updated
          </li>
        </ul>
      </div>
    </div>
  );
}
