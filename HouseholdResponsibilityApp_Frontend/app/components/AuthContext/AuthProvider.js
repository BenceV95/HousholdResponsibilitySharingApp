"use client";
import React, { createContext, useContext, useState, useEffect, useCallback } from "react";
import { apiPost } from "../../../(utils)/api";
import { useRouter } from "next/navigation";


const AuthContext = createContext();

export const useAuth = () => {
    return useContext(AuthContext);
};

export const AuthProvider = ({ children }) => {
    const router = useRouter();

    const [user, setUser] = useState(null);

    useEffect(() => {
        console.log("user: ", user)
    }, [user])



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
            console.log(jsonResponse);
            
            if (!responseFromLogin.ok) {

                let errorString = "";
                Object.entries(jsonResponse).forEach(([key, value]) => {
                    errorString += (`${key}:`, value)+"\n";
                });

                throw new Error(errorString);
            }

            //set the user globally from the token
            const response = await fetch("/auth/user", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                },
                credentials: "include",
                cache: "no-store"
            }); // front-end server endpoint
            const userData = await response.json();
            
            setUser(userData);
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
        <AuthContext.Provider value={{ login, logout, user }}>
            {children}
        </AuthContext.Provider>
    );



};



