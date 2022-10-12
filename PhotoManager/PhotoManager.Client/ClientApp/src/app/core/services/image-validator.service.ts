import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class ImageValidatorService {
  constructor() { }
  fileExtValidator(type: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      const file = control.value;
      if ( file ) {
        const splitFileName = file.split('.');
        const extension = splitFileName[splitFileName.length - 1].toLowerCase();
        if (type.toLowerCase() !== extension) {
          return of({ requiredFileType: true });
        }
        return of(null);
      }
      return of(null);
    };
  }

  validateImage(image: File, allowedExtension: string, sizeLimit: number) {
    const splitFileName = image.name.split('.');
    const ext = splitFileName[splitFileName.length - 1].toLowerCase();
    if (allowedExtension.toLowerCase() !== ext) {
      return of({ requiredFileType: true });
    }
    const imgSize = image.size;
    if (imgSize > sizeLimit) {
      return of({ invalidSize: true });
    }
    return of(null);
  }
}
