import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { DashboardSummary, Distribution, HeatmapEntry, Insight, TrendEntry } from '../models/dashboard.model';

@Injectable({
    providedIn: 'root'
})
export class DashboardService {
    private baseUrl = `${environment.apiUrl}/api/dashboard`;

    constructor(private http: HttpClient) { }

    getSummary(month: string): Observable<DashboardSummary> {
        const params = new HttpParams().set('month', month);
        return this.http.get<DashboardSummary>(`${this.baseUrl}/summary`, { params });
    }

    getTrends(month: string): Observable<TrendEntry[]> {
        const params = new HttpParams().set('month', month);
        return this.http.get<TrendEntry[]>(`${this.baseUrl}/trends`, { params });
    }

    getDistribution(month: string): Observable<Distribution> {
        const params = new HttpParams().set('month', month);
        return this.http.get<Distribution>(`${this.baseUrl}/distribution`, { params });
    }

    getInsights(month: string): Observable<Insight> {
        const params = new HttpParams().set('month', month);
        return this.http.get<Insight>(`${this.baseUrl}/insights`, { params });
    }

    getHeatmap(): Observable<HeatmapEntry[]> {
        return this.http.get<HeatmapEntry[]>(`${this.baseUrl}/heatmap`);
    }
}
