import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { SubscriptionLike } from 'rxjs';
import { Album } from 'src/app/core/models/album.model';
import { AlbumService } from 'src/app/core/services/album.service';

@Component({
  selector: 'app-album-edit',
  templateUrl: './album-edit.component.html',
  styleUrls: ['./album-edit.component.css']
})
export class AlbumEditComponent implements OnInit, OnDestroy {
  id: Guid;
  album: Album;
  formData: Album;
  isLoaded = false;
  private subscriptions: Array<SubscriptionLike> = [];

  constructor(
    private albumService: AlbumService,
    private router: Router,
    private activeRoute: ActivatedRoute,
    private toastr: ToastrService,
    ) {
      this.id = Guid.parse(this.activeRoute.snapshot.params['id']);
   }
  ngOnInit() {
      const result = this.albumService.getAlbum(this.id).subscribe((data) => {
        this.formData = data;
        this.isLoaded = true;
      });
      this.subscriptions.push(result);
  }
  save(album: Album) {
    const result =  this.albumService.editAlbum(this.id, album).subscribe(() =>  {
      this.router.navigateByUrl('/manage/albums');
      this.toastr.success('Album edited!');
    });
    this.subscriptions.push(result);
  }

  ngOnDestroy(): void {
   if (this.subscriptions.length > 0) {
      this.subscriptions.forEach(subscription => {
        if (subscription) {
          subscription.unsubscribe();
          subscription = null;
      }});
    }
  }
}
