import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import {
    Topic,
    CreateTopicRequest,
    UpdateTopicRequest,
    TopicProgress
} from '../models/topic.model';

@Injectable({
    providedIn: 'root'
})
export class TopicService {
    private apiUrl = `${environment.apiUrl}/api/topics`;

    constructor(private http: HttpClient) { }

    getAllTopics(): Observable<Topic[]> {
        return this.http.get<Topic[]>(this.apiUrl);
    }

    getTopicById(id: number): Observable<Topic> {
        return this.http.get<Topic>(`${this.apiUrl}/${id}`);
    }

    getTopicsByCategory(category: string): Observable<Topic[]> {
        return this.http.get<Topic[]>(`${this.apiUrl}/category/${category}`);
    }

    getProgress(): Observable<TopicProgress[]> {
        return this.http.get<TopicProgress[]>(`${this.apiUrl}/progress`);
    }

    createTopic(request: CreateTopicRequest): Observable<Topic> {
        return this.http.post<Topic>(this.apiUrl, request);
    }

    updateTopic(request: UpdateTopicRequest): Observable<Topic> {
        return this.http.put<Topic>(`${this.apiUrl}/${request.id}`, request);
    }

    deleteTopic(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}