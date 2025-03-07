"use client";
import "./root.css";
import { useAuth } from "./components/AuthContext/AuthProvider";
import PersonalAgenda from "./components/Calendar/PersonalAgenda";

export default function Home() {
  const { user } = useAuth();

  return (
    <div className="home">

      <div className="Intro">
        <h1>🏡 Household Responsibility Sharing App</h1>
        <p>
          Set up a household, invite members, and assign responsibilities for a more organized and better living experience.
        </p>
      </div>

      {user && user.householdId && (
        <div className="calendar-container">
          <PersonalAgenda />
        </div>
      )}

      <div className="Info">
        <h1 style={{ color: "yellow" }}>🚧👷‍♂️Please keep in mind that the site is under developement !👷‍♂️🚧<br />❌📱And is unoptimised for mobile.📱❌</h1>

      </div>
    </div>
  );
}
