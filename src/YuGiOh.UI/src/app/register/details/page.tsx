"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { ApiClient, RegisterUserRequest, IRegisterUserRequest, Address } from "@/api/ApiClient";

const api = new ApiClient(process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5234");

export default function RegisterDetailsForm() {
  const router = useRouter();
  const [commonData, setCommonData] = useState<any>(null);
  const [countries, setCountries] = useState<any[]>([]);
  const [states, setStates] = useState<any[]>([]);
  const [cities, setCities] = useState<any[]>([]);
  const [streetTypes, setStreetTypes] = useState<any[]>([]);

const [form, setForm] = useState<IRegisterUserRequest>({
  roles: [],
  iBAN: "",
  address: new Address({ 
    countryIso2: "", 
    stateIso2: "", 
    cityIso2: "", 
    streetTypeId: 0,
    streetName: "", 
    buildingName: "", 
    apartment: "" 
  }),
});

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const saved = sessionStorage.getItem("register-common");
    if (!saved) {
      router.push("/register/common");
      return;
    }
    const parsed = JSON.parse(saved);
    setCommonData(parsed);
    setForm(prev => ({ ...prev, ...parsed }));
  }, [router]);

  useEffect(() => {
    api.getCountries()
      .then(data => {
        console.log("Countries loaded:", data);
        setCountries(data);
      })
      .catch(console.error);
  }, []);

  useEffect(() => {
    api.getStreetTypes()
      .then(data => {
        console.log("StreetTypes loaded:", data);
        setStreetTypes(data);
      })
      .catch(console.error);
  }, []);


const handleChange = (
  e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>
) => {
  const { name, value } = e.target;
  // Decide the finalValue: cast to number if it's streetTypeId
  const finalValue = name === "streetTypeId" ? Number(value) : value;


  if (form.address && name in form.address) {
    console.log(name);
    setForm(prev => ({
      ...prev,
      address: new Address({
        ...prev.address,
        [name]: finalValue,
      }),
    }));
  } else {
    setForm(prev => ({
      ...prev,
      [name]: finalValue,
    }));
  }
};


  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const payload = new RegisterUserRequest(form);
      await api.register(payload);
      router.push("/login");
    } catch (err: any) {
      console.error(err);
      setError(err?.message ?? "❌ Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  if (!commonData) return null;

  return (
    <main className="min-h-screen bg-gradient-to-br from-violet-200 via-white to-violet-100 flex items-center justify-center">
    <div className="max-w-md mx-auto bg-white shadow-lg rounded-2xl p-8 mt-10 border border-yugiviolet">
      <h2 className="text-2xl font-bold text-center text-yugiviolet mb-6">
        Complete Your Registration
      </h2>

      <form onSubmit={handleSubmit} className="space-y-4">
{form.roles?.includes("Player") && (
  <div>
    <h3 className="font-semibold mb-2 text-yugiviolet">Player Address</h3>

    {/* Country selection */}
    <select
      name="countryIso2"
      onChange={async (e) => {
        handleChange(e);
        const selectedCountry = e.target.value;
        if (selectedCountry) {
          const states = await api.getStates(selectedCountry);
          setStates(states);
          setCities([]); // clear cities when country changes
        } else {
          setStates([]);
          setCities([]);
        }
      }}
      className="input"
      value={form.address?.countryIso2 || ""}
    >
      <option value="">Select Country</option>
      {countries.map((c) => (
        <option key={c.iso2} value={c.iso2}>
          {c.name}
        </option>
      ))}
    </select>

    {/* State selection */}
    {states.length > 0 && (
      <select
        name="stateIso2"
        onChange={async (e) => {
          handleChange(e);
          const selectedState = e.target.value;
          if (selectedState && form.address?.countryIso2) {
            const cities = await api.getCities(form.address.countryIso2, selectedState);
            setCities(cities);
          } else {
            setCities([]);
          }
        }}
        className="input mt-2"
        value={form.address?.stateIso2 || ""}
      >
        <option value="">Select State</option>
        {states.map((s) => (
          <option key={s.iso2} value={s.iso2}>
            {s.name}
          </option>
        ))}
      </select>
    )}

    {/* City selection */}
    {cities.length > 0 && (
      <select
        name="cityIso2"
        onChange={handleChange}
        className="input mt-2"
        value={form.address?.cityIso2 || ""}
      >
        <option value="">Select City</option>
        {cities.map((c) => (
          <option key={c.name} value={c.name}>
            {c.name}
          </option>
        ))}
      </select>
    )}

    {/* StreetType selection */}
    <select
      name="streetTypeId"
      onChange={async (e) => {
        handleChange(e);
      }}
      className="input"
      value={form.address?.streetTypeId || 0}
    >
      <option value="">Select Street Type</option>
      {streetTypes.map((c) => (
        <option key={c.id} value={c.id}>
          {c.name}
        </option>
      ))}
    </select>

    {/* Street info */}
    <input
      name="streetName"
      onChange={handleChange}
      placeholder="Street Name"
      className="input mt-2"
    />
    <input
      name="buildingName"
      onChange={handleChange}
      placeholder="Building Name"
      className="input mt-2"
    />
    <input
      name="apartment"
      onChange={handleChange}
      placeholder="Apartment"
      className="input mt-2"
    />
  </div>
)}


        {form.roles?.includes("Sponsor") && (
          <div>
            <h3 className="font-semibold mb-2 text-yugiviolet">Sponsor Information</h3>
            <input name="iBAN" value={form.iBAN ?? ""} onChange={handleChange} placeholder="IBAN" className="input" />
          </div>
        )}

        {error && <p className="text-red-600 text-sm text-red">{error}</p>}

        <button type="submit" disabled={loading} className="bg-yugiviolet hover:bg-yugiviolet-dark text-white font-semibold py-2 px-4 rounded-lg w-full">
          {loading ? "Registering..." : "Finish Registration"}
        </button>
      </form>
      </div>
    </main>
  );
}
