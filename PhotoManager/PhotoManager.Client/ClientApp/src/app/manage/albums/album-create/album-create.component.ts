import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { SubscriptionLike } from 'rxjs';
import { Album } from 'src/app/core/models/album.model';
import { AlbumService } from 'src/app/core/services/album.service';

@Component({
  selector: 'app-album-create',
  templateUrl: './album-create.component.html',
  styleUrls: ['./album-create.component.css']
})
export class AlbumCreateComponent implements OnInit, OnDestroy {
  private subscription: SubscriptionLike;
  isLoaded = false;
  album: Album = new Album();
  constructor(
    private albumService: AlbumService,
    private router: Router,
    private toastr: ToastrService) { }
  ngOnInit(): void {
    this.isLoaded = true;
  }

  save(album: Album) {
      this.subscription = this.albumService.createAlbum(album)
        .subscribe(() => {
          this.router.navigateByUrl('/manage/albums');
          this.toastr.success('Album created!');
        });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
      this.subscription = null;
    }
  }
}
