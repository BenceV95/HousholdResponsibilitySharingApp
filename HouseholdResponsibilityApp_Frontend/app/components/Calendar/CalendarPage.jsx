"use client";
import React, { useState, useEffect } from "react";
import { Calendar, Views, dateFnsLocalizer } from "react-big-calendar";
import format from "date-fns/format";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import getDay from "date-fns/getDay";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { apiFetch } from "../../../(utils)/api";
import { addHours } from "date-fns";
import { useAuth } from "../AuthContext/AuthProvider";
import "./calendar.css";

const locales = { "en-US": require("date-fns/locale/en-US") };
const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales });

export default function CalendarPage() {
  const { user } = useAuth();
  const [scheduledTasks, setScheduledTasks] = useState([]);
  const [tasks, setTasks] = useState([]);
  const [tasksToDisplay, setTasksToDisplay] = useState([]);
  const [events, setEvents] = useState([]);

  const [currentDate, setCurrentDate] = useState(new Date());
  const [currentView, setCurrentView] = useState(Views.WEEK);

  useEffect(() => {
    async function fetchHouseholdEvents() {
      if (user?.householdId) {
        const scheduledTasks = await apiFetch("/scheduleds");
        const householdTasks = await apiFetch(`/tasks/filtered/${user.householdId}`);

        setScheduledTasks(scheduledTasks);
        setTasks(householdTasks);
      }
    }
    fetchHouseholdEvents();
  }, [user]);

  useEffect(() => {
    setTasksToDisplay(fetchTasks());
  }, [tasks, scheduledTasks]);

  useEffect(() => {
    setEvents(tasksToDisplay);
  }, [tasksToDisplay]);

  const fetchTasks = () => {
    return scheduledTasks
      .map((scheduledTask) => {
        const template = tasks.find(
          (task) => task.taskId === scheduledTask.householdTaskId
        );
        return template
          ? {
              ...scheduledTask,
              allDay: !scheduledTask.atSpecificTime,
              title: template.title,
              description: template.description,
              start: new Date(scheduledTask.eventDate),
              end: addHours(new Date(scheduledTask.eventDate), 1),
              assignedTo: scheduledTask.assignedToUserId,
            }
          : null;
      })
      .filter((task) => task !== null);
  };

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
        selectable
        eventPropGetter={getEventStyle}
        localizer={localizer}
        onSelectEvent={(e) =>
          alert(`Description: ${e.description}\nTask name: ${e.title}`)
        }
        events={events}
        startAccessor="start"
        endAccessor="end"
        defaultView={Views.WEEK}
        views={["day", "week", "month", "agenda"]}
        date={currentDate}
        view={currentView}
        onNavigate={(newDate, view, action) => {
          setCurrentDate(newDate);
          console.log("Navigation =>", { newDate, view, action });
        }}
        onView={(newView) => {
          setCurrentView(newView);
          console.log("View change =>", newView);
        }}
        style={{
          background: "#fff",
          borderRadius: "8px",
          boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
        }}
      />
    </div>
  );
}
