import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { DashboardService } from '../../services/dashboard.service';
import { ResumeService } from '../../services/resume.service';
import { AIService } from '../../services/ai.service';
import { DashboardSummary, Distribution, HeatmapEntry, Insight, TrendEntry } from '../../models/dashboard.model';
import { ResumeAnalysisResult } from '../../models/resume.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  summary: DashboardSummary | null = null;
  heatmap: HeatmapEntry[] = [];
  trends: TrendEntry[] = [];
  distribution: Distribution | null = null;
  insight: Insight | null = null;
  loading = false;
  error: string | null = null;
  selectedMonth = new Date().toISOString().slice(0, 7);

  resumeAnalysis: ResumeAnalysisResult | null = null;
  resumeFile: File | null = null;
  jobDescription = '';
  resumeLoading = false;
  resumeError: string | null = null;

  constructor(
    private dashboardService: DashboardService,
    private resumeService: ResumeService
    , private aiService: AIService
  ) { }
  aiSuggestions: string[] = [];

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {
    this.loading = true;
    this.error = null;

    const month = this.selectedMonth;

    forkJoin({
      summary: this.dashboardService.getSummary(month),
      heatmap: this.dashboardService.getHeatmap(),
      trends: this.dashboardService.getTrends(month),
      distribution: this.dashboardService.getDistribution(month),
      insight: this.dashboardService.getInsights(month)
    }).subscribe({
      next: result => {
        this.summary = result.summary;
        this.heatmap = result.heatmap;
        this.trends = result.trends;
        this.distribution = result.distribution;
        this.insight = result.insight;
      },
      error: err => {
        this.error = err?.error?.message || 'Unable to load dashboard data.';
      },
      complete: () => {
        this.loading = false;
      }
    });
  }

  onResumeFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files || input.files.length === 0) {
      this.resumeFile = null;
      return;
    }

    this.resumeFile = input.files[0];
    this.resumeError = null;
  }

  analyzeResume(): void {
    if (!this.resumeFile) {
      this.resumeError = 'Please upload a resume file to continue.';
      return;
    }

    if (!this.jobDescription.trim()) {
      this.resumeError = 'Please paste a job description to analyze.';
      return;
    }

    this.resumeLoading = true;
    this.resumeError = null;
    this.resumeAnalysis = null;

    this.resumeService.analyzeResume(this.resumeFile, this.jobDescription.trim()).subscribe({
      next: result => {
        this.resumeAnalysis = result;
        // After resume analysis, fetch AI suggestions and merge
        this.aiService.getWeeklyInsights('normal').subscribe({
          next: ai => {
            this.aiSuggestions = ai.suggestions || [];
          },
          error: () => {
            this.aiSuggestions = [];
          }
        });
      },
      error: err => {
        this.resumeError = err?.error?.message || 'Failed to analyze resume. Please try again.';
      },
      complete: () => {
        this.resumeLoading = false;
      }
    });
  }

  get trendPath(): string {
    if (!this.trends?.length) {
      return '';
    }

    const values = this.trends.map(t => t.score);
    const max = Math.max(...values, 1);
    const min = Math.min(...values, 0);
    const width = 520;
    const height = 180;
    const xStep = width / Math.max(values.length - 1, 1);

    return values.map((score, index) => {
      const x = index * xStep;
      const y = height - ((score - min) / (max - min || 1)) * height;
      return `${index === 0 ? 'M' : 'L'} ${x.toFixed(2)} ${y.toFixed(2)}`;
    }).join(' ');
  }

  get distributionSegments(): { label: string; value: number; color: string; percentage: number; offset: number; dashArray: string }[] {
    if (!this.distribution) {
      return [];
    }
    if (!this.distribution) {
      return [];
    }

    const segments = [
      { label: 'DSA', value: this.distribution.dsa, color: '#5b8def' },
      { label: 'Theory', value: this.distribution.theory, color: '#f59e0b' },
      { label: 'Project', value: this.distribution.project, color: '#10b981' },
      { label: 'Resume', value: this.distribution.resume, color: '#ec4899' },
      { label: 'Other', value: this.distribution.other, color: '#a855f7' }
    ];

    const total = segments.reduce((sum, segment) => sum + segment.value, 0) || 1;
    const radius = 60;
    const circumference = 2 * Math.PI * radius;
    let offset = 0;

    return segments.map(segment => {
      const percentage = segment.value / total;
      const dash = `${(percentage * circumference).toFixed(2)} ${(circumference - percentage * circumference).toFixed(2)}`;
      const result = {
        ...segment,
        percentage: Math.round(percentage * 100),
        offset: -offset,
        dashArray: dash
      };
      offset += percentage * circumference;
      return result;
    });
  }

  get mergedRecommendations(): string[] {
    const resumeRecs = this.resumeAnalysis?.recommendations || [];
    const combined = [...(this.aiSuggestions || []), ...resumeRecs];
    // unique preserving order
    return combined.filter((v, i, a) => a.findIndex(x => x === v) === i);
  }

  trackByDate(index: number, item: HeatmapEntry): string {
    return item.date;
  }
}
