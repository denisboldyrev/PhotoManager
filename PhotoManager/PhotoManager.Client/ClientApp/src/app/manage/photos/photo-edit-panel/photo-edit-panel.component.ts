import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { Photo } from 'src/app/core/models/photo.model';

@Component({
  selector: 'app-photo-edit-panel',
  templateUrl: './photo-edit-panel.component.html',
  styleUrls: ['./photo-edit-panel.component.css']
})
export class PhotoEditPanelComponent implements OnInit {
  @Input() photo: Photo;
  @Output() btnClick = new EventEmitter();
  constructor(private router: Router) { }

  ngOnInit(): void {
  }
  onClick() {
    this.btnClick.emit();
  }
}
