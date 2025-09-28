"use client";

import { useRouter } from "next/navigation";
import { useState } from "react";

export default function RegisterCommonForm() {
  const router = useRouter();

  const [form, setForm] = useState({
    email: "",
    password: "",
    firstName: "",
    middleName: "",
    firstSurname: "",
    secondSurname: "",
    roles: [] as string[],
  });

  const [error, setError] = useState<string | null>(null);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleRoleToggle = (role: string) => {
    setForm(prev => {
      const exists = prev.roles.includes(role);
      return {
        ...prev,
        roles: exists ? prev.roles.filter(r => r !== role) : [...prev.roles, role],
      };
    });
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (form.roles.length === 0) {
      setError("Please select at least one role.");
      return;
    }
    setError(null);

    // Save to sessionStorage for step 2
    sessionStorage.setItem("register-common", JSON.stringify(form));
    router.push("/register/details");
  };

  return (
    <main className="min-h-screen bg-gradient-to-br from-violet-200 via-white to-violet-100 flex items-center justify-center">
    <div className="max-w-md mx-auto bg-white shadow-lg rounded-2xl p-8 mt-10 border border-yugiviolet">
      <h2 className="text-2xl font-bold text-center text-yugiviolet mb-6">
        Create Your Account
      </h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <input name="email" value={form.email} onChange={handleChange} type="email" required placeholder="Email" className="input" />
        <input name="password" value={form.password} onChange={handleChange} type="password" required placeholder="Password" className="input" />
        <input name="firstName" value={form.firstName} onChange={handleChange} type="text" required placeholder="First Name" className="input" />
        <input name="middleName" value={form.middleName} onChange={handleChange} type="text" placeholder="Middle Name (optional)" className="input" />
        <input name="firstSurname" value={form.firstSurname} onChange={handleChange} type="text" required placeholder="First Surname" className="input" />
        <input name="secondSurname" value={form.secondSurname} onChange={handleChange} type="text" required placeholder="Second Surname" className="input" />

        <div className="space-y-2">
          <label className="font-semibold text-yugiviolet">Select Role(s):</label>
          <div className="flex gap-4">
            <label>
              <input type="checkbox" checked={form.roles.includes("Player")} onChange={() => handleRoleToggle("Player")} />
              <span className="ml-2 text-yugiviolet">Player</span>
            </label>
            <label>
              <input type="checkbox" checked={form.roles.includes("Sponsor")} onChange={() => handleRoleToggle("Sponsor")} />
              <span className="ml-2 text-yugiviolet">Sponsor</span>
            </label>
          </div>
        </div>

        {error && <p className="text-red-600 text-sm">{error}</p>}

        <button type="submit" className="bg-yugiviolet hover:bg-yugiviolet-dark text-white font-semibold py-2 px-4 rounded-lg w-full">
          Next →
        </button>
      </form>
      </div>
    </main>
  );
}
