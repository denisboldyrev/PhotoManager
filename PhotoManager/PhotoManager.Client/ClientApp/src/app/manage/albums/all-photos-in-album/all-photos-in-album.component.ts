import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { SubscriptionLike } from 'rxjs';
import { Photo } from 'src/app/core/models/photo.model';
import { AlbumService } from 'src/app/core/services/album.service';

@Component({
  selector: 'app-all-photos-in-album',
  templateUrl: './all-photos-in-album.component.html',
  styleUrls: ['./all-photos-in-album.component.css']
})
export class AllPhotosInAlbumComponent implements OnInit, OnDestroy {
  private subscription: SubscriptionLike;
  photos: Photo[];
  id: Guid;
  constructor(private albumService: AlbumService, private activeRoute: ActivatedRoute) { }

  ngOnInit() {
    this.id = Guid.parse(this.activeRoute.snapshot.params['id']);
    this.subscription = this.albumService.getAllPhotosFromAlbum(this.id)
          .subscribe(data => this.photos = data);
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
