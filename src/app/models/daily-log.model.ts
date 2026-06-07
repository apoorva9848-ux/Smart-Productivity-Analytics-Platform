/**
 * DailyLog Model
 * Represents a date entry for activity grouping.
 */
export interface DailyLog {
    id: number;
    date: Date | string;
}

export type ActivityType = 'DSA' | 'Theory' | 'Project' | 'Resume' | 'Other';

export interface Activity {
    id: number;
    dailyLogId: number;
    date: Date | string;
    type: ActivityType;
    description: string;
    durationInMinutes?: number;
    createdAt: Date | string;
}

export interface CreateActivityRequest {
    date: Date | string;
    type: ActivityType;
    description: string;
    durationInMinutes?: number;
}
