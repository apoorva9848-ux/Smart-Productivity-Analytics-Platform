import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ResumeAnalysisResult } from '../models/resume.model';

@Injectable({
    providedIn: 'root'
})
export class ResumeService {
    private apiUrl = `${environment.apiUrl}/api/resume`;

    constructor(private http: HttpClient) { }

    analyzeResume(file: File, jobDescription: string): Observable<ResumeAnalysisResult> {
        const formData = new FormData();
        formData.append('resumeFile', file, file.name);
        formData.append('jobDescription', jobDescription);
        return this.http.post<ResumeAnalysisResult>(`${this.apiUrl}/analyze`, formData);
    }
}
