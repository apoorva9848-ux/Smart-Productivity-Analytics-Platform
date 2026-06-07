import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AIInsight, AIInsightRequest } from '../models/ai-insight.model';

@Injectable({
    providedIn: 'root'
})
export class AIService {
    private apiUrl = `${environment.apiUrl}/api/ai`;

    constructor(private http: HttpClient) { }

    /**
     * Get weekly AI insights based on last 7 days of user activity.
     */
    getWeeklyInsights(mode: string = 'normal'): Observable<AIInsight> {
        const request: AIInsightRequest = { mode };
        return this.http.post<AIInsight>(`${this.apiUrl}/insights`, request);
    }
}
