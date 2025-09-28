import React from "react";
import RegisterSponsorForm from "@/app/components/RegisterSponsorForm";

export default function RegisterPage() {
  return (
    <main className="min-h-screen bg-gradient-to-br from-violet-200 via-white to-violet-100 flex items-center justify-center">
      <RegisterSponsorForm />
    </main>
  );
}


// I have tow roles on my application, player that has an address with many parameters; sponsor that has an IBAN. I want a full register component such that the parameters that are common for both roles are showed first and a selector for choosing which role to register with, both roles selected is possible, then a button that redirects to another component that has everything to complete the registration form for the role(s) that were chowed. I need it to be implemented these functionalities in a way such that all information is retrived at the end and sent to the backend, and if everything was ok redirect to a login page, else show red as it is used to be on production ready web sites and with a style as beautiful as the one of the component I will share with you, and that uses the ApiClient that I will share you next.