"use client";
import React, { createContext, useContext, useState, useEffect, useCallback, useReducer } from "react";
import { apiPost } from "../../../(utils)/api";
import { useRouter } from "next/navigation";


const AuthContext = createContext();

export const useAuth = () => {
    return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
    const router = useRouter();

    const [user, setUser] = useState(null);


    // set the user every page render/refresh
    useEffect(() => {
        const storedUser = localStorage.getItem("userData");
        setUser(storedUser)
    }, [])

    function storeUserInLocalStorage(userData){
        localStorage.setItem("userData", JSON.stringify(userData));
    }



    async function login(email, password) {
        try {
            // set cookies from the backend
            const responseFromLogin = await fetch("/api/Auth/Login", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ email, password })
            });

            // for error checking purposes i had to modify so we can check response here
            const jsonResponse = await responseFromLogin.json();
            // console.log(jsonResponse);
            
            if (!responseFromLogin.ok) {

                let errorString = "";
                Object.entries(jsonResponse).forEach(([key, value]) => {
                    errorString += (`${key}:`, value)+"\n";
                });

                throw new Error(errorString);
            }

            console.log(jsonResponse)

            //this endpoint gives back an object with user meta data (username, email, householdId etc...)
            storeUserInLocalStorage(jsonResponse)
            setUser(jsonResponse)
            return jsonResponse;

        } catch (error) {
            setUser(null);
            throw (error);
        }
    };


    async function logout() {
        try {
            await apiPost("/Auth/Logout", {});
            setUser(null);
            router.push("/");
        } catch (err) {
            console.error("Logout error:", err);
        }
    };

    return (
        <AuthContext.Provider value={{ login, logout, user, setUser }}>
            {children}
        </AuthContext.Provider>
    );



};



