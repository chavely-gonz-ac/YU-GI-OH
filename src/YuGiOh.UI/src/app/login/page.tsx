"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { ApiClient, AuthenticateCommand } from "@/api/ApiClient";
import { AuthenticatedApiClient } from "@/api/AuthenticatedApiClient";

const api = new AuthenticatedApiClient(process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5234");

export default function LoginPage() {
  const router = useRouter();
  const [form, setForm] = useState({ handler: "", password: "" });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const payload = new AuthenticateCommand(form);

      // Call the login API
      const response = await api.login(payload);

      const body = await (response as any); // adjust if NSwag typing is stricter
      const accessToken = body.accessToken;

      api.setAccessToken(accessToken);
      localStorage.setItem("accessToken", accessToken); // optional persistence

      // Redirect to home page
      router.push("/");
    } catch (err: any) {
      console.error(err);
      setError(err?.message ?? "❌ Login failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="min-h-screen bg-gradient-to-br from-violet-200 via-white to-violet-100 flex items-center justify-center">
      <div className="max-w-md mx-auto bg-white shadow-lg rounded-2xl p-8 mt-10 border border-yugiviolet">
        <h2 className="text-2xl font-bold text-center text-yugiviolet mb-6">
          Login
        </h2>

        <form onSubmit={handleSubmit} className="space-y-4">
          <input
            type="text"
            name="handler"
            value={form.handler}
            onChange={handleChange}
            placeholder="Email or Username"
            required
            className="input"
          />
          <input
            type="password"
            name="password"
            value={form.password}
            onChange={handleChange}
            placeholder="Password"
            required
            className="input"
          />

          {error && <p className="text-red-600 text-sm">{error}</p>}

          <button
            type="submit"
            disabled={loading}
            className="bg-yugiviolet hover:bg-yugiviolet-dark text-white font-semibold py-2 px-4 rounded-lg w-full"
          >
            {loading ? "Logging in..." : "Login"}
          </button>
        </form>
      </div>
    </main>
  );
}
