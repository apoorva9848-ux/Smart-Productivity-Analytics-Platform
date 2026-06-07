import { Component, OnInit, Output, EventEmitter, OnDestroy, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { DailyTrackerService } from '../../../services/daily-tracker.service';
import { CreateActivityRequest, ActivityType } from '../../../models/daily-log.model';

@Component({
    selector: 'app-daily-log-form',
    templateUrl: './daily-log-form.component.html',
    styleUrls: ['./daily-log-form.component.scss']
})
export class DailyLogFormComponent implements OnInit, OnDestroy {
    @Input() selectedDate = new Date().toISOString().split('T')[0];
    @Output() onSuccess = new EventEmitter<void>();

    activityForm!: FormGroup;
    isSubmitting = false;
    successMessage: string | null = null;
    errorMessage: string | null = null;

    private destroy$ = new Subject<void>();

    constructor(
        private fb: FormBuilder,
        private dailyTrackerService: DailyTrackerService
    ) { }

    ngOnInit(): void {
        this.initializeForm();
    }

    private initializeForm(): void {
        this.activityForm = this.fb.group({
            type: ['DSA', [Validators.required]],
            description: ['', [Validators.required, Validators.minLength(5)]],
            durationInMinutes: [null]
        });
    }

    submitForm(): void {
        if (this.activityForm.invalid) {
            this.errorMessage = 'Please fill all required fields correctly';
            return;
        }

        this.isSubmitting = true;
        this.errorMessage = null;
        this.successMessage = null;

        const formValue = this.activityForm.value;
        const request: CreateActivityRequest = {
            date: this.selectedDate,
            type: formValue.type as ActivityType,
            description: formValue.description.trim(),
            durationInMinutes: formValue.durationInMinutes || undefined
        };

        this.dailyTrackerService.createActivity(request)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: () => {
                    this.successMessage = 'Activity added successfully!';
                    this.activityForm.reset({ type: 'DSA', description: '', durationInMinutes: null });
                    this.isSubmitting = false;
                    this.dailyTrackerService.loadActivities(new Date(this.selectedDate));
                    this.onSuccess.emit();
                    setTimeout(() => this.successMessage = null, 3000);
                },
                error: (error) => {
                    this.errorMessage = error.error?.message || 'Failed to add activity';
                    this.isSubmitting = false;
                    console.error('Error adding activity:', error);
                }
            });
    }

    quickAdd(type: ActivityType, description: string, durationInMinutes?: number): void {
        this.isSubmitting = true;
        this.errorMessage = null;
        this.successMessage = null;

        const request: CreateActivityRequest = {
            date: this.selectedDate,
            type,
            description,
            durationInMinutes
        };

        this.dailyTrackerService.createActivity(request)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: () => {
                    this.successMessage = 'Quick activity added!';
                    this.isSubmitting = false;
                    this.dailyTrackerService.loadActivities(new Date(this.selectedDate));
                    this.onSuccess.emit();
                    setTimeout(() => this.successMessage = null, 3000);
                },
                error: (error) => {
                    this.errorMessage = error.error?.message || 'Failed to add activity';
                    this.isSubmitting = false;
                    console.error('Error adding quick activity:', error);
                }
            });
    }

    getControl(name: string) {
        return this.activityForm.get(name);
    }

    hasError(fieldName: string): boolean {
        const field = this.getControl(fieldName);
        return !!(field && field.invalid && (field.dirty || field.touched));
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
