import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ImageValidatorService } from '../../services/image-validator.service';

@Component({
  selector: 'app-upload-images',
  templateUrl: './upload-images.component.html',
  styleUrls: ['./upload-images.component.css']
})
export class UploadImagesComponent implements OnInit {
  @Output() uploadBtnClick = new EventEmitter();
  file?: File = null;
  previews: string[] = [];
  uploadImgForm: FormGroup;
  errors: any;
  constructor(
    private formBuilder: FormBuilder,
    private imageValidator: ImageValidatorService) { }
    ngOnInit(): void {
    this.uploadImgForm = this.formBuilder.group({
      image:  [ null,
      [ Validators.required ],
      ]
    });
  }
  selectFiles(event: any): void {
    this.file = event.target.files[0];
    this.previews = [];
    const reader = new FileReader();

    const result = this.imageValidator.validateImage(this.file, 'jpg', 500000);
    if (result !== null) {
      result.subscribe(error => this.errors = error);
      this.image.setErrors(this.errors);
    }

    if (this.uploadImgForm.valid) {
      reader.onload = (e: any) => {
        this.uploadImgForm.patchValue({
          imgUpload: reader.result
        });
        console.log(e.target.result);
        this.previews.push(e.target.result);
      };
      reader.readAsDataURL(this.file);
    }
  }

  onClick() {
    if (this.file) {
      this.uploadBtnClick.emit({event: this.file});
     }
  }

  get image() {
    return this.uploadImgForm.get('image');
  }
}
