import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DailyTrackerComponent } from './components/daily-tracker/daily-tracker.component';
import { DailyLogFormComponent } from './components/daily-tracker/daily-log-form/daily-log-form.component';
import { DailyLogsTableComponent } from './components/daily-tracker/daily-logs-table/daily-logs-table.component';
import { DSATrackerComponent } from './components/dsa-tracker/dsa-tracker.component';
import { DSAProblemFormComponent } from './components/dsa-tracker/dsa-problem-form/dsa-problem-form.component';
import { DSAProblemsTableComponent } from './components/dsa-tracker/dsa-problems-table/dsa-problems-table.component';
import { TopicTrackerComponent } from './components/topic-tracker/topic-tracker.component';
import { AIInsightsComponent } from './components/ai-insights/ai-insights.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';

@NgModule({
    declarations: [
        AppComponent,
        DailyTrackerComponent,
        DailyLogFormComponent,
        DailyLogsTableComponent,
        DSATrackerComponent,
        DSAProblemFormComponent,
        DSAProblemsTableComponent,
        TopicTrackerComponent,
        AIInsightsComponent,
        DashboardComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        HttpClientModule,
        ReactiveFormsModule,
        FormsModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
