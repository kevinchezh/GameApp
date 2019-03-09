import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService, private altertify: AlertifyService) { }

  ngOnInit() {

  }

  register() {
    this.authService.register(this.model).subscribe(() => {
      this.altertify.success('registration successful');
    }, error => {
      this.altertify.error(error);
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
