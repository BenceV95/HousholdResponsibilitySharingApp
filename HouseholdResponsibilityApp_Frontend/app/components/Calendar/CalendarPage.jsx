"use client";
import React, { useState, useEffect } from "react";
import { Calendar, Views, dateFnsLocalizer } from "react-big-calendar";
import format from "date-fns/format";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import getDay from "date-fns/getDay";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { apiFetch, apiPatch, apiPost } from "../../../(utils)/api";
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
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState(null);

  const [currentDate, setCurrentDate] = useState(new Date());
  const [currentView, setCurrentView] = useState(Views.WEEK);
  const [reFetchEvents, setReFetchEvents] = useState(false);
  const [errorMsg, setErrorMsg] = useState(null);
  const TODAY = new Date();

  useEffect(() => {
    async function fetchHouseholdEvents() {
      if (user?.householdId) {
        const scheduledTasks = await apiFetch("/scheduleds");
        const householdTasks = await apiFetch(`/tasks/my-household`); //módosítás

        setScheduledTasks(scheduledTasks);
        console.log("scheduleds", scheduledTasks)
        setTasks(householdTasks);
      }
    }
    fetchHouseholdEvents();
  }, [user, reFetchEvents]);

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


  function normalizeDate(dateString) {
    const date = new Date(dateString);
    return new Date(date.getFullYear(), date.getMonth(), date.getDate());
  }
  function closeModal() {
    setErrorMsg(null);
    setIsModalOpen(false);
  }

  function openTaskModal(e) {
    setSelectedTask({ ...e })
    setIsModalOpen(true);
  }

  async function completeTask() {
    try {
      await apiPatch(`/scheduled/${selectedTask.scheduledTaskId
        }/complete`)
      setReFetchEvents((prev) => !prev)
      setIsModalOpen(false);
    } catch (e) {
      setErrorMsg(e);
    }
  }
  return (
    <div style={{ height: "40rem", width: "100%", padding: "20px" }}>
      <Calendar
        selectable
        eventPropGetter={(event) => {
          const backgroundColor = event.isCompleted ? '#28a745' : normalizeDate(event.eventDate) < TODAY ? '#dc3545' : "#808080";
          return {
            style: {
              backgroundColor,
              color: 'white',
              borderRadius: '4px',
              border: 'none',
            },
          };
        }}
        localizer={localizer}
        onSelectEvent={(e) =>
          openTaskModal(e)
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

      {isModalOpen && (
        <div className="modal-overlay">
          <div className="modal">
            {!errorMsg ?
              <>
                <h2>{selectedTask.title}</h2>
                <p>Description: <br />{selectedTask.description ? selectedTask.description : "no description"}</p>
                <div className="modal-buttons">
                  <button onClick={() => completeTask()} className="btn btn-success">
                    Complete
                  </button>
                  <button onClick={closeModal} className="btn btn-secondary">
                    Close
                  </button>
                </div>
              </> :
              <>
                <h2>Error!</h2>
                <p>Description: <br />{errorMsg}</p>
                <div className="modal-buttons">
                  <button onClick={closeModal} className="btn btn-secondary">
                    Close
                  </button>
                </div>
              </>
            }

          </div>
        </div>
      )}
    </div>
  );
}
