import { Component, OnDestroy } from '@angular/core';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AIService } from '../../services/ai.service';
import { AIInsight } from '../../models/ai-insight.model';

@Component({
    selector: 'app-ai-insights',
    templateUrl: './ai-insights.component.html',
    styleUrls: ['./ai-insights.component.css']
})
export class AIInsightsComponent implements OnDestroy {
    insight: AIInsight | null = null;
    isLoading = false;
    error: string | null = null;
    hasGenerated = false;
    selectedMode = 'normal';

    private destroy$ = new Subject<void>();

    constructor(private aiService: AIService) { }

    /**
     * Generate weekly insights by calling the backend API.
     */
    generateInsights(): void {
        this.isLoading = true;
        this.error = null;
        this.insight = null;

        this.aiService.getWeeklyInsights(this.selectedMode)
            .pipe(takeUntil(this.destroy$))
            .subscribe({
                next: (data) => {
                    this.insight = data;
                    this.hasGenerated = true;
                    this.isLoading = false;
                },
                error: (err) => {
                    console.error('Error generating insights:', err);
                    this.error = 'Failed to generate insights. Please try again.';
                    this.isLoading = false;
                }
            });
    }

    getWeaknessLabel(weakness: string): string {
        const parts = weakness.split(/:\s*/);
        return parts.length > 1 ? parts[0] : 'Weakness';
    }

    getWeaknessDetail(weakness: string): string {
        const parts = weakness.split(/:\s*/);
        return parts.length > 1 ? `: ${parts.slice(1).join(':')}` : ` ${weakness}`;
    }

    getSuggestionIcon(suggestion: string): string {
        if (/\b(STOP|SOLVE|COMPLETE|PICK|ADD)\b/i.test(suggestion)) {
            return '🎯';
        }
        return '💡';
    }

    ngOnDestroy(): void {
        this.destroy$.next();
        this.destroy$.complete();
    }
}
