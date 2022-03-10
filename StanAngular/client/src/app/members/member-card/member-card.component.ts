import { Member } from './../../_models/member';
import { Component, Input, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
@Input() member: Member;
@Input() url: Photo["url"];
  constructor() { }

  ngOnInit(): void {
  }
 
}
