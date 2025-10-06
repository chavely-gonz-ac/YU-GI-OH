"use client";
import { ApiClient, SwaggerException } from "@/api/ApiClient";

async function refreshToken(api: AuthenticatedApiClient) {
    const res = await api.refresh();

    const body = await (res as any);
    const accessToken = body.accessToken;

    api.setAccessToken(accessToken);
    localStorage.setItem("accessToken", accessToken);
}

export async function safeCall<T>(fn: () => Promise<T>, api: AuthenticatedApiClient): Promise<T> {
    try {
        return await fn();
    } catch (err: any) {
        if (err instanceof SwaggerException && err.status === 401) {
            await refreshToken(api);
            return fn(); // retry once with new token
        }
        throw err;
    }
}

export class AuthenticatedApiClient extends ApiClient {
    private accessToken: string | null = null;

    setAccessToken(token: string | null) {
        this.accessToken = token;
    }

    private withAuth(init?: RequestInit): RequestInit {
        const headers = new Headers(init?.headers || {});
        if (this.accessToken) {
            headers.set("Authorization", `Bearer ${this.accessToken}`);
        }
        return { ...init, headers };
    }

    // override async getCountries(signal?: AbortSignal): Promise<Country[]> {
    //     let url_ = this.baseUrl + "/api/CSC/GetCountries";
    //     url_ = url_.replace(/[?&]$/, "");

    //     let options_: RequestInit = this.withAuth({
    //         method: "GET",
    //         signal,
    //         headers: { "Accept": "text/plain" }
    //     });

    //     return this.http.fetch(url_, options_).then((_response: Response) => {
    //         return this.processGetCountries(_response);
    //     });
    // }

    // 🔁 Same override pattern for other endpoints that need auth
}
