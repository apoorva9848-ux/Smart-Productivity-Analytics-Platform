import { Component, Input, Output, EventEmitter } from '@angular/core';
import { DSAService } from '../../../services/dsa.service';
import { DSAProblem, UpdateDSAProblemRequest } from '../../../models/dsa-problem.model';

@Component({
    selector: 'app-dsa-problems-table',
    templateUrl: './dsa-problems-table.component.html',
    styleUrls: ['./dsa-problems-table.component.scss']
})
export class DSAProblemsTableComponent {
    @Input() problems: DSAProblem[] = [];
    @Output() problemUpdated = new EventEmitter<DSAProblem>();
    @Output() problemDeleted = new EventEmitter<number>();

    editingId: number | null = null;
    editingData: any = {};
    loading = false;
    error: string | null = null;

    difficulties = ['Easy', 'Medium', 'Hard'];

    constructor(private dsaService: DSAService) { }

    startEdit(problem: DSAProblem): void {
        this.editingId = problem.id;
        this.editingData = { ...problem };
    }

    cancelEdit(): void {
        this.editingId = null;
        this.editingData = {};
    }

    saveEdit(id: number): void {
        if (!this.editingData.name.trim()) {
            this.error = 'Problem name cannot be empty';
            return;
        }

        this.loading = true;
        const request: UpdateDSAProblemRequest = {
            id: id,
            name: this.editingData.name,
            dateSolved: this.editingData.dateSolved,
            link: this.editingData.link,
            difficulty: this.editingData.difficulty,
            pattern: this.editingData.pattern,
            notes: this.editingData.notes
        };

        this.dsaService.updateProblem(request).subscribe({
            next: (problem) => {
                this.problemUpdated.emit(problem);
                this.editingId = null;
                this.editingData = {};
                this.loading = false;
            },
            error: (err) => {
                this.error = err.error?.message || 'Failed to update problem';
                this.loading = false;
            }
        });
    }

    deleteProblem(id: number): void {
        if (confirm('Are you sure you want to delete this problem?')) {
            this.loading = true;
            this.dsaService.deleteProblem(id).subscribe({
                next: () => {
                    this.problemDeleted.emit(id);
                    this.loading = false;
                },
                error: (err) => {
                    this.error = 'Failed to delete problem';
                    this.loading = false;
                }
            });
        }
    }

    getDifficultyClass(difficulty: string): string {
        return difficulty.toLowerCase();
    }

    getDateString(date: any): string {
        return new Date(date).toISOString().split('T')[0];
    }

    setDateFromEvent(event: Event): void {
        const target = event.target as HTMLInputElement | null;
        if (target?.value) {
            this.editingData.dateSolved = new Date(target.value);
        }
    }
}
