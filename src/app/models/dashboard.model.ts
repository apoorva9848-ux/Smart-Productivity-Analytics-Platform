export interface HeatmapEntry {
    date: string;
    score: number;
}

export interface DashboardSummary {
    totalScore: number;
    activeDays: number;
    totalActivities: number;
    dsaCount: number;
    theoryCount: number;
    projectCount: number;
    resumeCount: number;
    otherCount: number;
    missedDays: number;
    longestInactivityStreak: number;
}

export interface TrendEntry {
    date: string;
    score: number;
}

export interface Distribution {
    dsa: number;
    theory: number;
    project: number;
    resume: number;
    other: number;
}

export interface Insight {
    insight: string;
    type: 'warning' | 'success' | 'info';
}

export interface DistributionSegment {
    label: string;
    value: number;
    color: string;
    percentage: number;
    offset: number;
    dashArray: string;
}
