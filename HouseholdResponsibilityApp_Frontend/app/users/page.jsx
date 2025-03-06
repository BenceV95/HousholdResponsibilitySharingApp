"use client";
import { useState } from 'react';
import { apiPost, apiFetch, apiPut, apiDelete } from '../../(utils)/api';
import './users.css';
import CreateUser from '../components/Users/CreateUser/CreateUser';
import GetUsers from '../components/Users/GetUsers/GetUsers';
import GetUserById from '../components/Users/GetUserById/GetUserById';
import UpdateUser from '../components/Users/UpdateUser/UpdateUser';
import DeleteUser from '../components/Users/DeleteUser/DeleteUser';

const UsersPage = () => {
  const [userActionVisible, setUserActionVisible] = useState(false);
  const [userAction, setUserAction] = useState("");

  const viewAction = (e) => {
    let action = e.target.name;
    if (userActionVisible && userAction === action) {
      setUserActionVisible(false);
      setUserAction("");
    } else {
      setUserAction(action);
      setUserActionVisible(true);
    }
  };

  return (
    <div className='users'>
      <div className='userButtons'>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='create'>Create User</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='get'>Get All Users</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='getById'>Get User by ID</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='update'>Update User</button>
        <button className='btn btn-warning' onClick={(e) => viewAction(e)} name='delete'>Delete User</button>
      </div>
      <div className='userAction'>
        {userActionVisible && (
          userAction === "create" ? <CreateUser /> :
          userAction === "get" ? <GetUsers /> :
          userAction === "getById" ? <GetUserById /> :
          userAction === "update" ? <UpdateUser /> :
          userAction === "delete" && <DeleteUser />
        )}
      </div>
    </div>
  );
};

export default UsersPage;
