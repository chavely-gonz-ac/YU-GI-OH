"use client";

import React, { useState } from "react";
import { ApiClient, RegisterUserRequest, IRegisterUserRequest } from "@/api/ApiClient";

// import "@/tailwind.config.js"

const api = new ApiClient(process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5234");

export default function RegisterForm() {
  // Use the generated interface for state (plain data)
  const [form, setForm] = useState<IRegisterUserRequest>({
    email: "",
    password: "",
    firstName: "",
    middleName: "",
    firstSurname: "",
    secondSurname: "",
    // keep roles as default Player; API expects string[]
    roles: ["Sponsor"],
    country: undefined,
    region: undefined,
    city: undefined,
    streetType: undefined,
    streetName: undefined,
    building: undefined,
    apartment: undefined,
    iBAN: "",
  });

  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState<string | null>(null);

  // Generic input handler — cast at the end to satisfy TS
  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...(prev as any), [name]: value } as IRegisterUserRequest));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setMessage(null);

    try {
      // Convert the plain data into the NSwag class instance before calling the ApiClient
      // This satisfies ApiClient.register(body: RegisterUserRequest)
      const payload = new RegisterUserRequest(form as any); // constructor copies properties

      await api.register(payload); // call NSwag client
      setMessage("✅ Registration successful. Please check your email to confirm.");
      // reset (you can keep more fields if needed)
      setForm({
        email: "",
        password: "",
        firstName: "",
        middleName: "",
        firstSurname: "",
        secondSurname: "",
        roles: ["Sponsor"],
        iBAN: "",
      });
    } catch (err: any) {
      console.error(err);
      // If ApiClient throws an ApiException, you can surface its message / status
      setMessage(err?.message ?? "❌ Registration failed. Try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-md mx-auto bg-white shadow-lg rounded-2xl p-8 mt-10 border border-yugiviolet">
      <h2 className="text-2xl font-bold text-center text-yugiviolet mb-6">Register</h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="email"
          value={form.email ?? ""}
          onChange={handleChange}
          type="email"
          required
          placeholder="Email"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="password"
          value={form.password ?? ""}
          onChange={handleChange}
          type="password"
          required
          placeholder="Password"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="firstName"
          value={form.firstName ?? ""}
          onChange={handleChange}
          type="text"
          required
          placeholder="First name"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="middleName"
          value={form.middleName ?? ""}
          onChange={handleChange}
          type="text"
          placeholder="Middle Name (optional)"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="firstSurname"
          value={form.firstSurname ?? ""}
          onChange={handleChange}
          type="text"
          required
          placeholder="First surname"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="secondSurname"
          value={form.secondSurname ?? ""}
          onChange={handleChange}
          type="text"
          required
          placeholder="Second surname"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <input
          name="iBAN"
          value={form.iBAN ?? ""}
          onChange={handleChange}
          type="text"
          placeholder="IBAN"
          className="w-full border rounded-lg px-4 py-2 focus:outline-none focus:ring-2 focus:ring-yugiviolet"
        />

        <button
          type="submit"
          disabled={loading}
          className="w-full bg-yugiviolet hover:bg-yugiviolet-dark text-white font-semibold py-2 px-4 rounded-lg transition "
        >
          {loading ? "Registering..." : "Register"}
        </button>
      </form>

      {message && <p className="text-center mt-4 text-sm text-gray-700">{message}</p>}
    </div>
  );
}
