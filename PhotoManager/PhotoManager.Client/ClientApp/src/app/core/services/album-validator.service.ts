import { Injectable } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AlbumService } from './album.service';

@Injectable({
  providedIn: 'root'
})
export class AlbumValidatorService  {
  constructor(private albumService: AlbumService) { }
  uniqueTitleValidator(title: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<ValidationErrors | null> => {
      return this.albumService.uniqueTitleValidate(control.value).pipe(
        map((res: boolean) => {
          if (control.value === title) {
            return null;
          }
          return res ? {titleAlreadyExists: true} : null;
        })
      );
    };
  }
}

