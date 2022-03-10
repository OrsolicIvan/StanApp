import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  registerMode2 = false;
  registerShow = true;

  constructor() { }

  ngOnInit(): void {
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
    this.registerShow = !this.registerShow;
  }
  registerToggle2(){
    this.registerMode2 = !this.registerMode2;
    this.registerShow = !this.registerShow;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;  
    this.registerShow = !this.registerShow;
  }
  cancelRegisterMode2(event: boolean) {
    this.registerMode2 = event;  
    this.registerShow = !this.registerShow;
  }

}
