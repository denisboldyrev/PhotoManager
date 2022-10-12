import { Component, OnInit} from '@angular/core';
import { HomeComponent } from 'src/app/home/home.component';
import { Router } from '@angular/router';
import { AlbumService } from 'src/app/core/services/album.service';
import { ToastrService } from 'ngx-toastr';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-album-manage',
  templateUrl: './album-manage.component.html',
  styleUrls: ['./album-manage.component.css']
})
export class AlbumManageComponent extends HomeComponent implements OnInit {
  router: Router;

  constructor(router: Router, albumService: AlbumService, private toastr: ToastrService) {
    super(albumService);
    this.router = router;
  }

  goToCreateAlbumPage(): void {
    const navigationDetails: string[] = ['manage/albums/create'];
    this.router.navigate(navigationDetails);
  }
  deleteAlbum(id: Guid) {
    this.albumService.deleteAlbum(id).subscribe(() => {
      this.getAlbums();
      this.toastr.success('Album deleted!');
    });
  }
}
