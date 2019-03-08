import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  constructor(private http: HttpClient) { }
  // users: any;
  ngOnInit() {
    // this.getUsers();
  }

  registerToggle() {
    this.registerMode = true;
  }

  // getUsers() {
  //   this.http.get('http://localhost:5000/api/values').subscribe(res => {
  //     this.users = res;
  //   }, error => {
  //     console.log(error);
  //   });
  // }

  cancelRegisterMode(registerMode: boolean) {
    this.registerMode = registerMode;
  }

}
