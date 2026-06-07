/**
 * Resume analysis result model.
 */
export interface ResumeAnalysisResult {
    matchScore: number;
    atsScore: number;
    missingSkills: string[];
    strongMatches: string[];
    recommendations: string[];
    summary: string;
}
