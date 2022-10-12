import { Input, OnDestroy } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/core/models/photo.model';
import { PhotoService } from 'src/app/core/services/photo.service';
import {NgbModal, ModalDismissReasons} from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FileUploadService } from 'src/app/core/services/file-upload.service';
import { SubscriptionLike } from 'rxjs';
import { Guid } from 'guid-typescript';

@Component({
  selector: 'app-photo-manage',
  templateUrl: './photo-manage.component.html',
  styleUrls: ['./photo-manage.component.css'],
  providers: [PhotoService]
})

export class PhotoManageComponent implements OnInit, OnDestroy {
  private subscription: SubscriptionLike;
  closeResult = '';
  photos: Photo[];
  constructor(
    private photoService: PhotoService,
    private modalService: NgbModal,
    private toastr: ToastrService,
    private uploadService: FileUploadService) {  }

  openLg(content) {
    this.modalService.open(content, { size: 'lg' }).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  openSm(content) {
    this.modalService.open(content).result.then((result) => {
      this.closeResult = `Closed with: ${result}`;
    }, (reason) => {
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }

  upload($event) {
    this.uploadService.upload($event.event).subscribe(() => {
        this.modalService.dismissAll();
        this.getPhotos();
        this.toastr.success('Photo saved!'); }
    );
  }
  deletePhoto(id: Guid) {
    this.photoService.deletePhoto(id).subscribe(() => {
      this.getPhotos();
      this.toastr.success('Photo deleted!');
    });
  }
  ngOnInit(): void {
    this.getPhotos();
  }

  getPhotos() {
    this.subscription = this.photoService.getPhotos().subscribe(data => this.photos = data);
  }

  ngOnDestroy(): void {
      if (this.subscription) {
        this.subscription.unsubscribe();
        this.subscription = null;
      }
    }
}
