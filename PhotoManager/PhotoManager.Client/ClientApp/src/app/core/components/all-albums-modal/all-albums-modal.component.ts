import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SubscriptionLike } from 'rxjs';
import { Album } from '../../models/album.model';
import { AlbumService } from '../../services/album.service';

@Component({
  selector: 'app-all-albums-modal',
  templateUrl: './all-albums-modal.component.html',
  styleUrls: ['./all-albums-modal.component.css']
})
export class AllAlbumsModalComponent implements OnInit, OnDestroy {
  private subscription: SubscriptionLike;
  albums: Album[];
  constructor(private albumService: AlbumService) { }
  ngOnInit() {
    this.subscription = this.albumService.getAlbums()
      .subscribe(data => this.albums = data);
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
