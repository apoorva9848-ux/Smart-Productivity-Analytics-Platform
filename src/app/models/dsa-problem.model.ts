export interface DSAProblem {
    id: number;
    name: string;
    dateSolved: Date;
    link?: string;
    difficulty: 'Easy' | 'Medium' | 'Hard';
    pattern?: string;
    notes?: string;
}

export interface CreateDSAProblemRequest {
    name: string;
    dateSolved: Date;
    link?: string;
    difficulty: 'Easy' | 'Medium' | 'Hard';
    pattern?: string;
    notes?: string;
}

export interface UpdateDSAProblemRequest extends CreateDSAProblemRequest {
    id: number;
}
