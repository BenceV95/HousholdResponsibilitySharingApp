"use client"

import Link from "next/link";
import './root.css'
import { useAuth } from "./components/AuthContext/AuthProvider";
import TaskView from "./components/TaskView/TaskView";

export default function Home() {
  const { user } = useAuth();

  return (
    <div className="home">
      <h1>Welcome to Household Responsibility Sharing App !</h1>
      <p>With our App you can set up a household, invite your household members and delegate, assign responsibilities so that living together will be better.</p>
      {
        true && <TaskView></TaskView>
      }
    </div>
  );
}
