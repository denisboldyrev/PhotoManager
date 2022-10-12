import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { AlbumComponent } from './core/components/album/album.component';
import { AlbumEditComponent } from './manage/albums/album-edit/album-edit.component';
import { NgxBootstrapIconsModule} from 'ngx-bootstrap-icons';
import { pencil, trash, share, threeDotsVertical, camera } from 'ngx-bootstrap-icons';
import { PhotosComponent } from './core/components/photo/photos/photos.component';
import { PhotoComponent } from './core/components/photo/photo.component';
import { PhotoEditComponent } from './manage/photos/photo-edit/photo-edit.component';
import { AlbumManageComponent } from './manage/albums/album-manage.component';
import { AlbumEditPanelComponent } from './manage/albums/album-edit-panel/album-edit-panel.component';
import { AllPhotosInAlbumComponent } from './manage/albums/all-photos-in-album/all-photos-in-album.component';
import { PhotoManageComponent } from './manage/photos/photo-manage.component';
import { AlbumFormComponent } from './core/components/album-form/album-form.component';
import { AlbumCreateComponent } from './manage/albums/album-create/album-create.component';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthInterceptorService } from './core/services/auth-interceptor.service';
import { SigninRedirectCallbackComponent } from './signin-redirect-callback/signin-redirect-callback.component';
import { SignoutRedirectCallbackComponent } from './signout-redirect-callback/signout-redirect-callback.component';
import { ClipboardModule } from 'ngx-clipboard';
import { AllAlbumsModalComponent } from './core/components/all-albums-modal/all-albums-modal.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { UploadImagesComponent } from './core/components/upload-images/upload-images.component';
import { AlbumsComponent } from './core/components/album/albums/albums.component';
import { PhotoEditPanelComponent } from './manage/photos/photo-edit-panel/photo-edit-panel.component';
import { HttpErrorInterceptorService } from './core/services/http-error-interceptor.service';

const icons = {
  pencil,
  trash,
  share,
  threeDotsVertical,
  camera
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AlbumComponent,
    AlbumManageComponent,
    AlbumFormComponent,
    AlbumCreateComponent,
    AlbumEditComponent,
    AlbumEditPanelComponent,
    PhotoComponent,
    PhotosComponent,
    AllPhotosInAlbumComponent,
    PhotoEditComponent,
    PhotoManageComponent,
    SigninRedirectCallbackComponent,
    SignoutRedirectCallbackComponent,
    AllAlbumsModalComponent,
    UploadImagesComponent,
    AlbumsComponent,
    PhotoEditPanelComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    NgxBootstrapIconsModule.pick(icons),
    ReactiveFormsModule,
    BrowserAnimationsModule,
    ClipboardModule,
    NgbModule,
    ToastrModule.forRoot({
      timeOut: 2000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    RouterModule.forRoot([
    { path: '', component: HomeComponent, pathMatch: 'full' },
    { path: 'albums/:id', component: AllPhotosInAlbumComponent },
    { path: 'signin-callback', component: SigninRedirectCallbackComponent },
    { path: 'signout-callback', component: SignoutRedirectCallbackComponent },
    { path: 'manage/albums', component: AlbumManageComponent },
    { path: 'manage/albums/:id/photos', component: AllPhotosInAlbumComponent },
    { path: 'manage/albums/create', component: AlbumCreateComponent },
    { path: 'manage/albums/edit/:id', component: AlbumEditComponent },
    { path: 'manage/photos', component: PhotoManageComponent },
    { path: 'manage/photos/:id', component: AlbumFormComponent },
], { relativeLinkResolution: 'legacy' })
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptorService, multi: true  }],
  bootstrap: [AppComponent]
})
export class AppModule { }
