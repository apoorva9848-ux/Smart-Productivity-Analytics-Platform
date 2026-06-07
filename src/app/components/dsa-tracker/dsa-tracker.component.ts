import { Component, OnInit } from '@angular/core';
import { DSAService } from '../../services/dsa.service';
import { DSAProblem, CreateDSAProblemRequest } from '../../models/dsa-problem.model';

@Component({
    selector: 'app-dsa-tracker',
    templateUrl: './dsa-tracker.component.html',
    styleUrls: ['./dsa-tracker.component.scss']
})
export class DSATrackerComponent implements OnInit {
    problems: DSAProblem[] = [];
    loading = false;
    error: string | null = null;
    selectedFilter: 'all' | 'difficulty' | 'pattern' = 'all';
    filterValue: string = '';
    difficulties = ['Easy', 'Medium', 'Hard'];
    patterns = ['Array', 'String', 'Hashing', 'Sliding Window', 'Two Pointers', 'DFS', 'BFS', 'Dynamic Programming'];

    constructor(private dsaService: DSAService) { }

    ngOnInit(): void {
        this.loadAllProblems();
    }

    loadAllProblems(): void {
        this.loading = true;
        this.error = null;
        this.dsaService.getAllProblems().subscribe({
            next: (data) => {
                this.problems = data.sort((a, b) =>
                    new Date(b.dateSolved).getTime() - new Date(a.dateSolved).getTime()
                );
                this.loading = false;
            },
            error: (err) => {
                this.error = 'Failed to load problems';
                this.loading = false;
                console.error(err);
            }
        });
    }

    onProblemCreated(problem: DSAProblem): void {
        this.problems.unshift(problem);
        this.loadAllProblems();
    }

    onProblemUpdated(problem: DSAProblem): void {
        const index = this.problems.findIndex(p => p.id === problem.id);
        if (index !== -1) {
            this.problems[index] = problem;
        }
        this.loadAllProblems();
    }

    onProblemDeleted(id: number): void {
        this.problems = this.problems.filter(p => p.id !== id);
    }

    applyFilter(): void {
        if (this.selectedFilter === 'all') {
            this.loadAllProblems();
        } else if (this.selectedFilter === 'difficulty' && this.filterValue) {
            this.loading = true;
            this.dsaService.getProblemsByDifficulty(this.filterValue).subscribe({
                next: (data) => {
                    this.problems = data;
                    this.loading = false;
                },
                error: (err) => {
                    this.error = 'Failed to filter problems';
                    this.loading = false;
                    console.error(err);
                }
            });
        } else if (this.selectedFilter === 'pattern' && this.filterValue) {
            this.loading = true;
            this.dsaService.getProblemsByPattern(this.filterValue).subscribe({
                next: (data) => {
                    this.problems = data;
                    this.loading = false;
                },
                error: (err) => {
                    this.error = 'Failed to filter problems';
                    this.loading = false;
                    console.error(err);
                }
            });
        }
    }

    resetFilter(): void {
        this.selectedFilter = 'all';
        this.filterValue = '';
        this.loadAllProblems();
    }

    getStatistics() {
        return {
            total: this.problems.length,
            easy: this.problems.filter(p => p.difficulty === 'Easy').length,
            medium: this.problems.filter(p => p.difficulty === 'Medium').length,
            hard: this.problems.filter(p => p.difficulty === 'Hard').length
        };
    }
}
