import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { Activity, CreateActivityRequest } from '../models/daily-log.model';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class DailyTrackerService {
    private apiUrl = `${environment.apiUrl}/api/activities`;
    private activitiesSubject = new BehaviorSubject<Activity[]>([]);
    public activities$ = this.activitiesSubject.asObservable();

    constructor(private http: HttpClient) {
        this.loadActivities(new Date());
    }

    loadActivities(date: Date): void {
        const formattedDate = this.formatDate(date);
        this.getActivitiesByDate(formattedDate).subscribe({
            next: (activities) => this.activitiesSubject.next(activities),
            error: (error) => console.error('Error loading activities:', error)
        });
    }

    getActivitiesByDate(date: string): Observable<Activity[]> {
        return this.http.get<Activity[]>(`${this.apiUrl}/${date}`);
    }

    createActivity(request: CreateActivityRequest): Observable<Activity> {
        return this.http.post<Activity>(this.apiUrl, request);
    }

    deleteActivity(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    getCachedActivities(): Activity[] {
        return this.activitiesSubject.value;
    }

    private formatDate(date: Date): string {
        return date.toISOString().split('T')[0];
    }
}
