"use client";

import React, { use, useState, useEffect } from 'react';
import { useAuth } from '../components/AuthContext/AuthProvider';
import { apiFetch, apiDelete } from '../../(utils)/api';
import CreateHousehold from '../components/Households/CreateHousehold/CreateHousehold';
import JoinHousehold from '../components/Households/JoinHousehold/page';
import UpdateUser from '../components/Users/UpdateUser/UpdateUser';
import Household from '../components/Households/Household/Household';
import Loading from '../../(utils)/Loading';
import './Profile.css';

const Profile = () => {

  const { user, logout } = useAuth();
  const [action, setAction] = useState(null);
  const [household, setHousehold] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const getHousehold = async () => {
      setLoading(true);
      try {
        const household = await apiFetch(`/household/${user.householdId}`);
        setHousehold(household);
        console.log(household);

      }
      catch (error) {
        console.log("Error getting household: ", error);
      }
      finally {
        setLoading(false);
      }
    }
    getHousehold();

  }, []);

  // add some sort of check like entering a password
  // to make sure that the user actually intends to delete their account,
  // maybe send an email
  const deleteAccount = async () => {
    try {
      await apiDelete(`/users/${user.userId}`);
      alert('Account deleted. Goodbye!');
      logout();
    }
    catch (error) {
      alert(error);
    }
  }



  return (
    <div className='profile-page'>

      <h1>Hello, {user?.userName.toUpperCase()}!</h1>

      <div className='user-actions'>

        <div className='household-info'>
          {user &&
            (user.householdId ?
              (household ?
                <>
                  <Household dataEntry={household} />
                  <button className='btn btn-danger' onClick={() => alert("Does not work yet")}>Delete/Leave Household (WIP)</button>
                </> : loading && <Loading />)
              :
              <>
                <h3>You are not part of any household.</h3>
                <button className='btn btn-success' value="create" onClick={(e) => setAction(e.target.value)}>Create one</button>
                <button className='btn btn-primary' value="join" onClick={(e) => setAction(e.target.value)}>Join a household</button>
              </>)}
        </div>

        <div className='Account-Actions'>
          <h3>Account Actions</h3>
          <button className='btn btn-warning' value="profile" onClick={(e) => setAction(e.target.value)}>Edit Profile</button>
          <button className='btn btn-danger' onClick={deleteAccount}>Delete Account</button>
        </div>

      </div>
      {action && (
        <div className='action-menu'>
          {action === "create" && <CreateHousehold />}
          {action === "join" && <JoinHousehold />}
          {action === "profile" && <UpdateUser />}
        </div>
      )}
      <div className='notes'>
        <ul>
          <li>Add an option here that once the user has a household and if the admin then edit household.</li>
          <li>User deletion is broken ATM both for users with or without household. <b>Must investigate</b></li>
          <li>Refresh site once user has joined or created household else broken</li>
          <li>We need to use PATCH for the user to edit their settings so only the necessary info need to be sent/updated</li>
        </ul>
      </div>
    </div>
  )
}

export default Profile