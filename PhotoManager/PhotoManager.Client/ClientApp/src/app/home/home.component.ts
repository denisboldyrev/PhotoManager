import { OnDestroy, OnInit } from '@angular/core';
import { Component } from '@angular/core';
import { SubscriptionLike } from 'rxjs';
import { Album } from '../core/models/album.model';
import { AlbumService } from '../core/services/album.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  providers: [AlbumService]
})
export class HomeComponent implements OnInit, OnDestroy {
  subscription: SubscriptionLike;
  albums: Album[];
  albumService: AlbumService;
  constructor(albumService: AlbumService) {
    this.albumService = albumService;
  }

  ngOnInit(): void {
    this.getAlbums();
  }
  getAlbums() {
    this.subscription = this.albumService.getAlbums().subscribe(data => this.albums = data);
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
