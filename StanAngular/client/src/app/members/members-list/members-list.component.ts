import { MembersService } from './../../_services/members.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';

@Component({
  selector: 'app-members-list',
  templateUrl: './members-list.component.html',
  styleUrls: ['./members-list.component.css']
})
export class MembersListComponent implements OnInit {
  users: any;
  members: Member[];
  
  constructor(private http: HttpClient, private memberService: MembersService) { }

  ngOnInit(): void {
    this.getUsers();
    this.loadMembers();
  }

  getUsers(){
    this.http.get('https://localhost:44329/api/users').subscribe(response => {
      this.users = response;
    }, error => {
      console.log(error);
    })
  }

  loadMembers() {
    this.memberService.getMembers().subscribe(members => {
      this.members = members;
      if (members) console.log("radi")
      console.log(members)
      
    })
  }

}
