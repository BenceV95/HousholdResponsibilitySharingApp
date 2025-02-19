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
            //set the token from the backend
            const responseFromLogin = await apiPost("/Auth/Login", { email, password });

            //set the user globally from the token
            const response = await fetch("/auth/user", { cache: "no-store" });
            const userData = await response.json();
            setUser(userData);
            return userData;
        } catch (error) {
            console.error("Failed to fetch user:", error);
            setUser(null);
            throw new Error("Failed to log in");
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



