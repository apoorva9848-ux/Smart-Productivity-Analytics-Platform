import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Activity, ActivityType } from '../../../models/daily-log.model';

@Component({
    selector: 'app-daily-logs-table',
    templateUrl: './daily-logs-table.component.html',
    styleUrls: ['./daily-logs-table.component.scss']
})
export class DailyLogsTableComponent {
    @Input() activities: Activity[] = [];
    @Input() selectedDate: string = '';
    @Output() activityDeleted = new EventEmitter<number>();

    /**
     * Get icon for activity type
     */
    getActivityIcon(type: ActivityType): string {
        switch (type) {
            case 'DSA':
                return '🧩';
            case 'Theory':
                return '📚';
            case 'Project':
                return '💻';
            case 'Resume':
                return '📄';
            case 'Other':
                return '📝';
            default:
                return '📝';
        }
    }

    /**
     * Delete activity
     */
    deleteActivity(activityId: number): void {
        if (confirm('Are you sure you want to delete this activity?')) {
            this.activityDeleted.emit(activityId);
        }
    }
}
