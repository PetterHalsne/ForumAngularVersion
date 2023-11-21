import { Component, OnInit } from '@angular/core';
import { IPost } from './post';
import { ActivatedRoute, Router } from '@angular/router';
import { PostService } from './posts.service';


@Component({
    selector: 'app-posts-component',
    templateUrl: './posts.component.html',
    styleUrls: ['./posts.component.css']
})

export class PostsComponent implements OnInit {
  viewTitle: string = 'Post';
  private _listfilter: string = "";

  posts: IPost[] = [];
    
  constructor(
      private _router: Router,
      private _postService: PostService,
      private _route: ActivatedRoute
  ) { }

  get listFilter(): string {
      return this._listfilter;
  }

  set listFilter(value: string) {
    this._listfilter = value;
    console.log('In setter:', value);
    this.filteredPosts = this.performFilter(value);
  }

  deletePost(post: IPost): void {
    const confirmDelete = confirm(`Are you sure you want to delete "${post.PostTitle}"?`);
    if (confirmDelete) {
      this._postService.deletePost(post.PostId)
        .subscribe(
          (response) => {
            if (response.success) {
              console.log(response.message);
              this.filteredPosts = this.filteredPosts.filter(i => i !== post);
            }
          },
          (error) => {
            console.error('Error deleting post:', error);
          });
    }
  }
  


  getPosts(): void {
    this._postService.getPosts()
      .subscribe(data => {
        console.log('All', JSON.stringify(data));
        this.posts = data;
        this.filteredPosts = this.posts;
      }
      );
  }

  filteredPosts: IPost[] = this.posts;

  performFilter(filterBy: string): IPost[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.posts.filter((post: IPost) =>
      post.PostTitle.toLocaleLowerCase().includes(filterBy));
  }

  ngOnInit(): void {
      console.log('PostComponent created');
      this.getPosts();
  }
}
