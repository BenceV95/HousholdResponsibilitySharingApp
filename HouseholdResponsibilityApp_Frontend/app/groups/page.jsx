"use client";
import { useState } from 'react';
import CreateGroup from '../components/Groups/CreateGroup/CreateGroup';

export default function GroupsPage() {
  const [groupActionVisible, setGroupActionVisible] = useState(false);
  const [groupAction, setGroupAction] = useState("");

  const viewAction = (e) => {
    const action = e.target.name;
    if (groupActionVisible && groupAction === action) {
      setGroupActionVisible(false);
      setGroupAction("");
    } else {
      setGroupAction(action);
      setGroupActionVisible(true);
    }
  };

  return (
    <div className="groups">
      <div className="groupButtons">
        <button
          className="btn btn-warning"
          onClick={viewAction}
          name="create"
        >
          Create Group
        </button>
       
      </div>

      <div className="groupAction">
        {groupActionVisible && (
          groupAction === "create" ? (
            <>
              <h1>Create Group</h1>
              <CreateGroup />
            </>
          ) : groupAction === "get" ? (
            <>
              <h1>Get Groups</h1>
            </>
          ) : null
        )}
      </div>
    </div>
  );
}
