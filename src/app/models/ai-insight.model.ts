/**
 * AI Insight response model.
 */
export interface AIInsight {
    insight: string;
    weaknesses: string[];
    suggestions: string[];
    summary: string;
    studyConsistency: string;
    dsaStats: string;
    topicProgress: { [key: string]: string };
}

/**
 * AI Insight request model.
 */
export interface AIInsightRequest {
    mode: string;
}
