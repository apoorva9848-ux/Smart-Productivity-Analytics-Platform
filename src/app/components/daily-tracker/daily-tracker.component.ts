import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DailyTrackerService } from '../../services/daily-tracker.service';
import { Activity } from '../../models/daily-log.model';

@Component({
    selector: 'app-daily-tracker',
    templateUrl: './daily-tracker.component.html',
    styleUrls: ['./daily-tracker.component.scss']
})
export class DailyTrackerComponent implements OnInit, OnDestroy {
    activities: Activity[] = [];
    selectedDate = new Date().toISOString().split('T')[0];
    isLoading = false;
    errorMessage: string | null = null;

    private destroy$ = new Subject<void>();

    constructor(private dailyTrackerService: DailyTrackerService) { }

    ngOnInit(): void {
        this.loadActivities(this.selectedDate);
    }

    loadActivities(dateString: string): void {
        this.isLoading = true;
        this.errorMessage = null;

        this.dailyTrackerService.activities$
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: (activities) => {
                    this.activities = activities;
                    this.isLoading = false;
                },
                error: (error) => {
                    this.errorMessage = 'Failed to load activities';
                    this.isLoading = false;
                    console.error('Error loading activities:', error);
                }
            });

        const date = new Date(dateString);
        this.dailyTrackerService.loadActivities(date);
    }

    onActivityAdded(): void {
        this.loadActivities(this.selectedDate);
    }

    onActivityDeleted(activityId: number): void {
        this.dailyTrackerService.deleteActivity(activityId).subscribe({
            next: () => {
                this.loadActivities(this.selectedDate);
            },
            error: (error) => {
                this.errorMessage = 'Failed to delete activity';
                console.error('Error deleting activity:', error);
            }
        });
    }

    onDateChange(event: Event): void {
        const target = event.target as HTMLInputElement;
        if (target.value) {
            this.selectedDate = target.value;
            this.loadActivities(this.selectedDate);
        }
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
