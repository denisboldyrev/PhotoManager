import { Guid } from 'guid-typescript';
import { Photo } from './photo.model';

export class Album {
    id: Guid;
    title = '';
    description = '';
    photos: Photo[] = [];
    constructor() {}
}
