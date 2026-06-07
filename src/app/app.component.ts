import { Component } from '@angular/core';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.scss']
})
export class AppComponent {
    title = 'Productivity Tracker';
    activeTab: 'daily' | 'dsa' | 'topics' | 'dashboard' | 'insights' = 'dashboard';

    setActiveTab(tab: 'daily' | 'dsa' | 'topics' | 'dashboard' | 'insights') {
        this.activeTab = tab;
    }
}
