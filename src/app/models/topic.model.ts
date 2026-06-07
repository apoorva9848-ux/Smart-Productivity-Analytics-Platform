export interface Topic {
    id: number;
    name: string;
    category: string;
    isCompleted: boolean;
    dateCompleted?: string;
    notes?: string;
}

export interface CreateTopicRequest {
    name: string;
    category: string;
    notes?: string;
}

export interface UpdateTopicRequest {
    id: number;
    name?: string;
    category?: string;
    isCompleted?: boolean;
    dateCompleted?: string;
    notes?: string;
}

export interface TopicProgress {
    overall: number;
    angular: number;
    dotnet: number;
    sql: number;
}

export interface CategoryProgress {
    completed: number;
    total: number;
    percentage: number;
}