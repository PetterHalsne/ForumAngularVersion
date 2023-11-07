import { Component, OnInit } from '@angular/core';
import { ITopic } from './topic';

import { Router } from '@angular/router';
import { TopicService } from './topics.service';
@Component({
  selector: 'app-topics-component',
  templateUrl: './topics.component.html',
  styleUrls: ['./topics.component.css']
})

export class TopicsComponent implements OnInit {
  viewTitle: string = 'Table';

  topics: ITopic[] = [];

  constructor(
    private _router: Router,
    private _topicService: TopicService) { }

  private _listFilter: string = '';
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    console.log('In setter:', value);
    this.filteredTopics = this.performFilter(value);
  }

  deleteTopic(topic: ITopic): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${topic.TopicName}"?`);
    if (confirmDelete) {
      this._topicService.deleteTopic(topic.TopicId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this.filteredTopics = this.filteredTopics.filter(i => i !== topic);
            }
          },
          (error) => {
            console.error('Error deleting item:', error);
          });
    }
  }

  getTopics(): void {
    this._topicService.getTopics()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.topics = data;
        this.filteredTopics = this.topics;
      }
      );
  }

  filteredTopics: ITopic[] = this.topics;

  performFilter(filterBy: string): ITopic[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.topics.filter((topic: ITopic) =>
      topic.TopicName.toLocaleLowerCase().includes(filterBy));
  }



  navigateToTopicform() {
    this._router.navigate(['/topicform']);
  }

  ngOnInit(): void {
    this.getTopics();
  }
}
