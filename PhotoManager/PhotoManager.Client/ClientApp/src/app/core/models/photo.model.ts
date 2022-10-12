import { Guid } from 'guid-typescript';

export class Photo {
    id: Guid;
    title = '';
    description = '';
    fileName = '';
    constructor() {}
}
