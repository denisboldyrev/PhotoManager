import { Output, EventEmitter } from '@angular/core';
import { Component, Input } from '@angular/core';
import { Album } from 'src/app/core/models/album.model';
import { ClipboardService } from 'ngx-clipboard';
import { Guid } from 'guid-typescript';
import { environment, apiPaths } from 'src/environments/environment';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-album-edit-panel',
  templateUrl: './album-edit-panel.component.html',
  styleUrls: ['./album-edit-panel.component.css']
})
export class AlbumEditPanelComponent {
  @Input() album: Album;
  @Output() btnClick = new EventEmitter();
  constructor(
    private clipboardApi: ClipboardService,
    private toastr: ToastrService
    ) {}

  onClick() {
    this.btnClick.emit();
  }

  shareLink(id: Guid) {
    const linkToAlbum = `${environment.clientUrl}${apiPaths.albums}/${id}`;
    this.clipboardApi.copy(linkToAlbum);
    this.toastr.success('Link copied to clipboard!');
 }
}
