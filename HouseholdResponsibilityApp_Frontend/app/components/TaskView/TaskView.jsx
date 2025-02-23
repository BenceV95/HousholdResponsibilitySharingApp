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


const locales = { "en-US": require("date-fns/locale/en-US") };
const localizer = dateFnsLocalizer({ format, parse, startOfWeek, getDay, locales });



//TODO : configure the backend to give back info acccording to user's token!

export default function WeeklyCalendar() {
    //we need tasks and scheduled tasks as well, since the task stores the title, description and the scheduled task contains the info when, and who must do it!
    const [scheduledTasks, setScheduledTasks] = useState([]);
    const [taskBlueprints, setTaskBlueprints] = useState([]);
    const [events, setEvents] = useState([]);


    useEffect(() => {
        try {
            async function getScheduledTasks() {
                const tasks = await apiFetch("/tasks/my-household");
                const scheduleds = await apiFetch("/scheduleds/my-household")
                console.log("scheduleds", scheduleds)
                console.log("tasks", tasks)
                setScheduledTasks(scheduleds)
                setTaskBlueprints(tasks);
            }
            getScheduledTasks();
            console.log("scheduledTasks: ", scheduledTasks)

        } catch (e) {
            console.log(e.message)
        }

    }, [])




    useEffect(() => {
        const converted = convertScheduledTasksToEvents()
        console.log("converted", converted)
        setEvents(converted)
    }, [scheduledTasks])





    // we have to pair up the tasks and scheduled tasks
    const convertScheduledTasksToEvents = () => {
        return scheduledTasks.map(scheduledTask => {

            const taskBlueprint = taskBlueprints.find(task => task.taskId === scheduledTask.householdTaskId);

            if (taskBlueprint) {
                return {
                    allDay: !scheduledTask.atSpecificTime,
                    title: taskBlueprint.title,
                    description: taskBlueprint.description,
                    start: new Date(scheduledTask.eventDate),
                    end: addHours(new Date(scheduledTask.eventDate), 1),
                    assignedTo: scheduledTask.assignedToUserId,
                };
            }

            return null;
        }).filter(event => event !== null);
    };




    //maybe we can make a color picker, and store the preferred one in the db. idk
    //for now, lets just use hardcoded values
    const getEventStyle = (event) => {
        const colors = {
            "0086ad72-5f23-497f-b183-5bc00158628c": "#FF5733",   // Red
            "195731ee-d2f9-430a-9792-06f573cd754d": "#33FF57",     // Green
            "666ae7c3-582f-4707-9938-27580f0cde18": "#3357FF"  // Blue
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
                // eventPropGetter={getEventStyle}
                localizer={localizer}
                onSelectEvent={(e) => { alert(`Description:${e.description} \n Task name: ${e.title}`) }}
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