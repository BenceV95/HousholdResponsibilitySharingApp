"use client";
import React, { useState, useEffect } from "react";
import { Calendar, Views, dateFnsLocalizer } from "react-big-calendar";
import format from "date-fns/format";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import getDay from "date-fns/getDay";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { apiFetch } from "../../../(utils)/api";
import { useAuth } from "../AuthContext/AuthProvider";
import "./personalAgenda.css";

const locales = { "en-US": require("date-fns/locale/en-US") };
const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales });

export default function PersonalAgenda() {
  const { user } = useAuth();
  const [tasks, setTasks] = useState([]);

  useEffect(() => {
    async function fetchUserTasks() {
      if (user?.userId) {
        const scheduledTasks = await apiFetch(`/scheduleds`);
        const filteredTasks = scheduledTasks.filter(task => task.assignedToUserId === user.userId);
        
        const householdTasks = await apiFetch(`/tasks/filtered/${user.householdId}`);
        const events = filteredTasks.map(scheduledTask => {
          const template = householdTasks.find(task => task.taskId === scheduledTask.householdTaskId);
          return template ? {
            title: template.title,
            start: new Date(scheduledTask.eventDate),
            end: new Date(scheduledTask.eventDate),
            allDay: !scheduledTask.atSpecificTime,
            assignedTo: scheduledTask.assignedToUserId, 
          } : null;
        }).filter(event => event !== null);

        setTasks(events);
      }
    }

    fetchUserTasks();
  }, [user]);

  const getEventStyle = (event) => {
    const colors = {
      "0086ad72-5f23-497f-b183-5bc00158628c": "#FF5733", 
      "195731ee-d2f9-430a-9792-06f573cd754d": "#33FF57", 
      "666ae7c3-582f-4707-9938-27580f0cde18": "#3357FF", 
    };

    return {
      style: {
        backgroundColor: colors[event.assignedTo] || "#999999", 
        color: "white",
        borderRadius: "5px",
        padding: "5px",
        border: "none",
      },
    };
  };

  return (
    <div style={{ height: "40rem", width: "100%", padding: "20px" }}>
      <Calendar
        localizer={localizer}
        events={tasks}
        eventPropGetter={getEventStyle} 
        startAccessor="start"
        endAccessor="end"
        defaultView={Views.AGENDA}
        views={["agenda"]}
        toolbar={false} 
        style={{
          background: "#fff",
          borderRadius: "8px",
          boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
        }}
      />
    </div>
  );
}
