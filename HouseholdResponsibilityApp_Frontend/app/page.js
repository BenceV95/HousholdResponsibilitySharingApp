"use client";
import "./root.css";
import { useAuth } from "./components/AuthContext/AuthProvider";
import PersonalAgenda from "./components/Calendar/PersonalAgenda";

export default function Home() {
  const { user } = useAuth();

  return (
    <main className="home">
      <h1>🏡 Household Responsibility Sharing App</h1>
      <p>
        Set up a household, invite members, and assign responsibilities for a more organized and better living experience.
      </p>
      
      {user && user.householdId && (
        <div className="calendar-container">
          <PersonalAgenda />
        </div>
      )}
    </main>
  );
}
