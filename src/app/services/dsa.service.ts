import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DSAProblem, CreateDSAProblemRequest, UpdateDSAProblemRequest } from '../models/dsa-problem.model';
import { environment } from '../../environments/environment';

@Injectable({
    providedIn: 'root'
})
export class DSAService {
    private apiUrl = `${environment.apiUrl}/api/dsa`;

    constructor(private http: HttpClient) { }

    /**
     * Get all DSA problems
     */
    getAllProblems(): Observable<DSAProblem[]> {
        return this.http.get<DSAProblem[]>(this.apiUrl);
    }

    /**
     * Get a specific DSA problem by ID
     */
    getProblemById(id: number): Observable<DSAProblem> {
        return this.http.get<DSAProblem>(`${this.apiUrl}/${id}`);
    }

    /**
     * Filter problems by difficulty
     */
    getProblemsByDifficulty(difficulty: string): Observable<DSAProblem[]> {
        let params = new HttpParams().set('difficulty', difficulty);
        return this.http.get<DSAProblem[]>(`${this.apiUrl}/filter`, { params });
    }

    /**
     * Filter problems by pattern
     */
    getProblemsByPattern(pattern: string): Observable<DSAProblem[]> {
        let params = new HttpParams().set('pattern', pattern);
        return this.http.get<DSAProblem[]>(`${this.apiUrl}/filter`, { params });
    }

    /**
     * Get problems by date range
     */
    getProblemsByDateRange(startDate: Date, endDate: Date): Observable<DSAProblem[]> {
        let params = new HttpParams()
            .set('startDate', startDate.toISOString())
            .set('endDate', endDate.toISOString());
        return this.http.get<DSAProblem[]>(`${this.apiUrl}/filter`, { params });
    }

    /**
     * Create a new DSA problem
     */
    createProblem(request: CreateDSAProblemRequest): Observable<DSAProblem> {
        return this.http.post<DSAProblem>(this.apiUrl, request);
    }

    /**
     * Update an existing DSA problem
     */
    updateProblem(request: UpdateDSAProblemRequest): Observable<DSAProblem> {
        return this.http.put<DSAProblem>(this.apiUrl, request);
    }

    /**
     * Delete a DSA problem
     */
    deleteProblem(id: number): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }
}
