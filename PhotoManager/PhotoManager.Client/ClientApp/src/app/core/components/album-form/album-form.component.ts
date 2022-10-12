import { OnInit } from '@angular/core';
import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, Validators, FormBuilder} from '@angular/forms';
import { Guid } from 'guid-typescript';
import { Album } from 'src/app/core/models/album.model';
import { AlbumValidatorService } from 'src/app/core/services/album-validator.service';

@Component({
  selector: 'app-album-form',
  templateUrl: './album-form.component.html',
  styleUrls: ['./album-form.component.css']
})
export class AlbumFormComponent implements OnInit {
  @Input() formData: FormGroup;
  @Output() submitBtnClick = new EventEmitter();
  album: Album;
  albumForm: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private titleValidator: AlbumValidatorService) {}
  ngOnInit(): void {
    this.albumForm  = this.formBuilder.group({
      id: [Guid.EMPTY],
      title: ['', {
        validators: [
          Validators.required,
          Validators.maxLength(15),
          Validators.pattern('^[a-zA-Z0-9\' \'-\'_\s]*'),
        ],
        updateOn: 'blur'
      },
      ],
      description: ['']
    });
    if (this.formData) {
      this.albumForm.patchValue(this.formData);
      const oldTitle = this.albumForm.controls['title'].value;
      this.albumForm.controls['title'].addAsyncValidators([this.titleValidator.uniqueTitleValidator(oldTitle)]);
    } else {
      this.albumForm.controls['title'].addAsyncValidators([this.titleValidator.uniqueTitleValidator(null)]);
    }
  }
  onClick() {
     if (this.albumForm.valid) {
       this.album = this.albumForm.value;
       this.submitBtnClick.emit(this.album);
     }
  }
  get title() {
    return this.albumForm.get('title');
  }
  get description() {
    return this.albumForm.get('description');
  }
}
