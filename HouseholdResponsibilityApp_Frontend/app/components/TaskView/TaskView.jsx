"use client"
import "./TaskView.css"
import React from "react";
import { Calendar, Views, dateFnsLocalizer } from "react-big-calendar";
import format from "date-fns/format";
import parse from "date-fns/parse";
import startOfWeek from "date-fns/startOfWeek";
import getDay from "date-fns/getDay";
import "react-big-calendar/lib/css/react-big-calendar.css";
import { useEffect, useState } from "react";
import { apiFetch } from "../../../(utils)/api";
import { addHours } from "date-fns";
import { useSelectedLayoutSegment } from "next/navigation";
import { useAuth } from "../AuthContext/AuthProvider";

const locales = { "en-US": require("date-fns/locale/en-US") };
const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales });



//TODO : configure the backend to give back info acccording to user's token!

export default function WeeklyCalendar() {
    const { user } = useAuth();
    const [scheduledTasks, setScheduledTasks] = useState([]);
    const [tasks, setTasks] = useState([]);
    const [tasksToDisplay, setTasksToDisplay] = useState([]);
    const [events, setEvents] = useState([]);


    useEffect(() => {
        console.log("tasks", tasks)
        console.log("scheduledTasks", scheduledTasks)
        console.log("tasksToDisplay", tasksToDisplay)
        console.log("user", user)
    }, [tasks, scheduledTasks, tasksToDisplay, user])



    useEffect(() => {
        try {
            async function getHouseholdTasks() {
                if (user) {
                    const data = await apiFetch(`/tasks/filtered/${user.householdId}`) // and this should return only the tasks in the given household final version should be -> user.householdId
                    setTasks(data)
                }
            }


            async function getScheduledTasks() {
                //todo : make backend enpoint to filter scheduled tasks on the task "blueprint"
                //for now it'll filter on the frontend
                const data = await apiFetch("/scheduleds");
                setScheduledTasks(data)
            }
            getHouseholdTasks()
            getScheduledTasks()
        } catch (e) {
            console.log(e)
        }

    }, [user])

    useEffect(() => {
        setTasksToDisplay(fetchTasks());
    }, [tasks, scheduledTasks]);


    const fetchTasks = () => {
        return scheduledTasks.map(scheduledTask => {
            const template = tasks.find(task => task.taskId === scheduledTask.householdTaskId);    // great naming convention
            return template
                ? {
                    ...scheduledTask,
                    allDay: !scheduledTask.atSpecificTime,
                    title: template.title,
                    description: template.description,
                    start: new Date(scheduledTask.eventDate),
                    end: addHours(new Date(scheduledTask.eventDate), 1),
                    assignedTo: "user4"
                }
                : null;
        }).filter(task => task !== null);
    };

    useEffect(() => {
        setEvents(tasksToDisplay)
    }, [tasksToDisplay]);


    // const events = tasksToDisplay.map(task => ({
    //     title: task.title,
    //     start: new Date(task.eventDate),
    //     end: new Date(task.eventDate),
    //     allDay: !task.atSpecificTime, // Ensures it spans the full day
    // }));

    // const events = [
    //     { title: "🧹 Clean Kitchen", start: new Date("2025-02-18"), end: new Date("2025-02-18"), allDay: true, assignedTo: "Alice" },
    //     { title: "🛒 Buy Groceries", start: new Date("2025-02-19"), end: new Date("2025-02-19"), allDay: false, assignedTo: "Bob" },
    //     { title: "📦 Take Out Trash", start: new Date("2025-02-20"), end: new Date("2025-02-20"), allDay: false, assignedTo: "Chajrlie" },
    //     { title: "📦 Take Out Trash", start: new Date("2025-02-20"), end: new Date("2025-02-20"), allDay: false, assignedTo: "Chajrlie" },
    //     { title: "📦 Take Out Trash", start: new Date("2025-02-01"), end: new Date("2025-02-01"), allDay: false, assignedTo: "Chajrlie" },
    //     { title: "📦 Take Out Trash", start: new Date("2025-02-01"), end: new Date("2025-02-01"), allDay: false, assignedTo: "Chajrlie" },
    //     { title: "📦 Take Out Trash", start: new Date("2025-02-01"), end: new Date("2025-02-01"), allDay: false, assignedTo: "Chajrlie" },
    // ];

    //maybe we can make a color picker, and store the preferred one in the db. idk
    const getEventStyle = (event) => {
        const colors = {
            test4: "#FF5733",   // Red
            Bob: "#33FF57",     // Green
            Charlie: "#3357FF"  // Blue
        };

        return {
            style: {
                backgroundColor: colors[event.assignedTo] || "#999999", // Default gray if no match
                color: "white",
                borderRadius: "5px",
                padding: "5px",
                border: "none"
            }
        };
    };




    return (
        <div style={{ height: "40rem", width: "1000px", padding: "20px" }}>
            <Calendar
                selectable
                eventPropGetter={getEventStyle}
                localizer={localizer}
                onSelectEvent={(e) => { alert(`Task start:${e.start} \n Task name: ${e.title}`) }}
                events={events}
                startAccessor="start"
                endAccessor="end"
                defaultView={Views.WEEK}
                views={["day", "week", "month", "agenda"]}
                style={{ background: "#fff", borderRadius: "8px", boxShadow: "0 4px 10px rgba(0,0,0,0.1)" }}
            />
        </div>
    );
};