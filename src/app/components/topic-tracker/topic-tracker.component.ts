import { Component, OnInit } from '@angular/core';
import { TopicService } from '../../services/topic.service';
import { Topic, TopicProgress, CreateTopicRequest, UpdateTopicRequest } from '../../models/topic.model';

@Component({
    selector: 'app-topic-tracker',
    templateUrl: './topic-tracker.component.html',
    styleUrls: ['./topic-tracker.component.scss']
})
export class TopicTrackerComponent implements OnInit {
    topics: Topic[] = [];
    progress: TopicProgress = {
        overall: 0,
        angular: 0,
        dotnet: 0,
        sql: 0
    };
    loading = false;
    error: string | null = null;

    categories = ['Angular', '.NET', 'SQL'];
    selectedCategory = 'Angular';

    // Add topic form
    showAddForm = false;
    newTopic: CreateTopicRequest = {
        name: '',
        category: 'Angular',
        notes: ''
    };

    // Edit topic form
    editingTopic: Topic | null = null;
    editTopic: UpdateTopicRequest = {
        id: 0,
        name: '',
        category: 'Angular',
        notes: ''
    };

    constructor(private topicService: TopicService) { }

    ngOnInit(): void {
        this.loadTopics();
        this.loadProgress();
    }

    loadTopics(): void {
        this.loading = true;
        this.topicService.getAllTopics().subscribe({
            next: (topics: Topic[]) => {
                this.topics = topics;
                this.loading = false;
            },
            error: (err: any) => {
                this.error = 'Failed to load topics';
                this.loading = false;
                console.error(err);
            }
        });
    }

    loadProgress(): void {
        this.topicService.getProgress().subscribe({
            next: (progressList: any[]) => {
                // Calculate progress from the list
                this.progress = this.calculateProgress(progressList);
            },
            error: (err: any) => {
                console.error('Failed to load progress', err);
            }
        });
    }

    calculateProgress(progressList: any[]): TopicProgress {
        const progress: TopicProgress = {
            overall: 0,
            angular: 0,
            dotnet: 0,
            sql: 0
        };

        let totalCompleted = 0;
        let totalTopics = 0;

        progressList.forEach((item: any) => {
            const category = item.category?.toLowerCase() || '';
            const percentage = item.percentage || 0;

            if (category.includes('angular')) {
                progress.angular = percentage;
            } else if (category.includes('.net') || category.includes('dotnet')) {
                progress.dotnet = percentage;
            } else if (category.includes('sql')) {
                progress.sql = percentage;
            }

            totalCompleted += item.completed || 0;
            totalTopics += item.total || 0;
        });

        progress.overall = totalTopics > 0 ? (totalCompleted / totalTopics) * 100 : 0;
        return progress;
    }

    getOverallProgress(): number {
        return Math.round(this.progress.overall * 10) / 10;
    }

    getProgressForCategory(category: string): any {
        const categoryTopics = this.topics.filter(t => t.category === category);
        const completed = categoryTopics.filter(t => t.isCompleted).length;
        const total = categoryTopics.length;
        const percentage = total > 0 ? (completed / total) * 100 : 0;

        return {
            completed,
            total,
            percentage: Math.round(percentage * 10) / 10
        };
    }

    getTopicsByCategory(category: string): Topic[] {
        return this.topics.filter(t => t.category === category);
    }

    toggleTopicCompletion(topic: Topic): void {
        const request: UpdateTopicRequest = {
            id: topic.id,
            name: topic.name,
            category: topic.category,
            isCompleted: !topic.isCompleted,
            dateCompleted: !topic.isCompleted ? new Date().toISOString().split('T')[0] : undefined,
            notes: topic.notes
        };

        this.topicService.updateTopic(request).subscribe({
            next: (updatedTopic: Topic) => {
                const index = this.topics.findIndex(t => t.id === updatedTopic.id);
                if (index !== -1) {
                    this.topics[index] = updatedTopic;
                }
                this.loadProgress(); // Refresh progress
            },
            error: (err: any) => {
                this.error = 'Failed to update topic';
                console.error(err);
            }
        });
    }

    updateTopicNotes(topic: Topic, notes: string): void {
        const request: UpdateTopicRequest = {
            id: topic.id,
            name: topic.name,
            category: topic.category,
            isCompleted: topic.isCompleted,
            dateCompleted: topic.dateCompleted,
            notes: notes
        };

        this.topicService.updateTopic(request).subscribe({
            next: (updatedTopic: Topic) => {
                const index = this.topics.findIndex(t => t.id === updatedTopic.id);
                if (index !== -1) {
                    this.topics[index] = updatedTopic;
                }
            },
            error: (err: any) => {
                this.error = 'Failed to update topic notes';
                console.error(err);
            }
        });
    }

    updateTopicDate(topic: Topic, dateString: string): void {
        const request: UpdateTopicRequest = {
            id: topic.id,
            name: topic.name,
            category: topic.category,
            isCompleted: topic.isCompleted,
            dateCompleted: dateString || undefined,
            notes: topic.notes
        };

        this.topicService.updateTopic(request).subscribe({
            next: (updatedTopic: Topic) => {
                const index = this.topics.findIndex(t => t.id === updatedTopic.id);
                if (index !== -1) {
                    this.topics[index] = updatedTopic;
                }
            },
            error: (err: any) => {
                this.error = 'Failed to update topic date';
                console.error(err);
            }
        });
    }

    addTopic(): void {
        if (!this.newTopic.name.trim()) {
            this.error = 'Topic name is required';
            return;
        }

        this.topicService.createTopic(this.newTopic).subscribe({
            next: (topic: Topic) => {
                this.topics.push(topic);
                this.loadProgress(); // Refresh progress
                this.resetAddForm();
                this.showAddForm = false;
            },
            error: (err: any) => {
                this.error = 'Failed to add topic';
                console.error(err);
            }
        });
    }

    startEditTopic(topic: Topic): void {
        this.editingTopic = topic;
        this.editTopic = {
            id: topic.id,
            name: topic.name,
            category: topic.category,
            isCompleted: topic.isCompleted,
            dateCompleted: topic.dateCompleted,
            notes: topic.notes || ''
        };
    }

    cancelEdit(): void {
        this.editingTopic = null;
        this.editTopic = {
            id: 0,
            name: '',
            category: 'Angular',
            notes: ''
        };
    }

    saveEditTopic(): void {
        if (!this.editTopic.name || !this.editTopic.name.trim()) {
            this.error = 'Topic name is required';
            return;
        }

        this.topicService.updateTopic(this.editTopic).subscribe({
            next: (updatedTopic: Topic) => {
                const index = this.topics.findIndex(t => t.id === updatedTopic.id);
                if (index !== -1) {
                    this.topics[index] = updatedTopic;
                }
                this.loadProgress(); // Refresh progress
                this.cancelEdit();
            },
            error: (err: any) => {
                this.error = 'Failed to update topic';
                console.error(err);
            }
        });
    }

    deleteTopic(topic: Topic): void {
        if (confirm(`Are you sure you want to delete "${topic.name}"? This action cannot be undone.`)) {
            this.topicService.deleteTopic(topic.id).subscribe({
                next: () => {
                    this.topics = this.topics.filter(t => t.id !== topic.id);
                    this.loadProgress(); // Refresh progress
                },
                error: (err: any) => {
                    this.error = 'Failed to delete topic';
                    console.error(err);
                }
            });
        }
    }

    resetAddForm(): void {
        this.newTopic = {
            name: '',
            category: this.selectedCategory,
            notes: ''
        };
        this.error = null;
    }

    getDateString(date?: string): string {
        return date ? new Date(date).toISOString().split('T')[0] : '';
    }
}