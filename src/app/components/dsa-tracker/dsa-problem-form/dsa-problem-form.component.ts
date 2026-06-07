import { Component } from '@angular/core';
import { DSAService } from '../../../services/dsa.service';
import { DSAProblem, CreateDSAProblemRequest } from '../../../models/dsa-problem.model';
import { Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'app-dsa-problem-form',
    templateUrl: './dsa-problem-form.component.html',
    styleUrls: ['./dsa-problem-form.component.scss']
})
export class DSAProblemFormComponent {
    @Output() problemCreated = new EventEmitter<DSAProblem>();

    formData: CreateDSAProblemRequest = {
        name: '',
        dateSolved: new Date(),
        link: '',
        difficulty: 'Easy',
        pattern: '',
        notes: ''
    };

    difficulties = ['Easy', 'Medium', 'Hard'];
    patterns = ['Array', 'String', 'Hashing', 'Sliding Window', 'Two Pointers', 'DFS', 'BFS', 'Dynamic Programming', 'Greedy', 'Math'];
    submitted = false;
    loading = false;
    error: string | null = null;
    success: string | null = null;

    constructor(private dsaService: DSAService) { }

    onSubmit(): void {
        this.submitted = true;
        this.error = null;
        this.success = null;

        if (!this.isFormValid()) {
            return;
        }

        this.loading = true;
        this.dsaService.createProblem(this.formData).subscribe({
            next: (problem) => {
                this.success = 'Problem added successfully!';
                this.problemCreated.emit(problem);
                this.resetForm();
                this.loading = false;
                setTimeout(() => this.success = null, 3000);
            },
            error: (err) => {
                this.error = err.error?.message || 'Failed to add problem';
                this.loading = false;
                console.error(err);
            }
        });
    }

    isFormValid(): boolean {
        return this.formData.name.trim() !== '';
    }

    resetForm(): void {
        this.formData = {
            name: '',
            dateSolved: new Date(),
            link: '',
            difficulty: 'Easy',
            pattern: '',
            notes: ''
        };
        this.submitted = false;
    }

    getDateString(): string {
        return this.formData.dateSolved instanceof Date
            ? this.formData.dateSolved.toISOString().split('T')[0]
            : new Date(this.formData.dateSolved).toISOString().split('T')[0];
    }

    setDate(dateString: string): void {
        this.formData.dateSolved = new Date(dateString);
    }

    setDateFromEvent(event: Event): void {
        const target = event.target as HTMLInputElement | null;
        if (target?.value) {
            this.formData.dateSolved = new Date(target.value);
        }
    }
}
