"use client";
import { useEffect, useState } from 'react';
import "./groups.css";
import { apiDelete, apiFetch } from '../../(utils)/api';
import Loading from '../../(utils)/Loading';

export default function GroupsPage() {

  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchGroups = async () => {
      try {
        const data = await apiFetch("/groups/my-household");
        setGroups(data);
      }
      catch (e) {
        console.error(e);
      }
    };

    setLoading(true);
    fetchGroups();
    setLoading(false);

  }, []);

  const deleteGroup = async (id) => {
    try {
      await apiDelete(`/group/${id}`);
      setGroups(groups.filter(group => group.groupResponseDtoId !== id));
    } catch (error) {
      console.error('Error deleting group:', error);
      alert('Failed to delete group');
    }
  }

  return (
    <div className='groupsPage'>
      <h1>
        Here you can manage your household's groups.
      </h1>
      <div className='groupList'>
        {loading ? <Loading /> : ( groups.length < 1 ? <h1>No groups yet</h1> :
          groups.map(g => {
            return (
              <div key={g.groupResponseDtoId} className='groupName'>
                <h1>{g.name}</h1>
                <button 
                  className='btn btn-danger' 
                  onClick={() => deleteGroup(g.groupResponseDtoId)}
                >
                  Delete
                </button>
              </div>)
          })
        )}
      </div>
    </div>
  );
}
