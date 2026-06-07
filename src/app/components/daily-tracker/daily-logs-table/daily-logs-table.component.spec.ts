import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DailyLogsTableComponent } from './daily-logs-table.component';

describe('DailyLogsTableComponent', () => {
    let component: DailyLogsTableComponent;
    let fixture: ComponentFixture<DailyLogsTableComponent>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            declarations: [DailyLogsTableComponent]
        })
            .compileComponents();

        fixture = TestBed.createComponent(DailyLogsTableComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
