import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import {FormBuilder, FormControl, FormGroup, Validators} from "@angular/forms";
import {BsDatepickerConfig} from "ngx-bootstrap";
import {User} from "../models/user";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  user: User;
  registerForm: FormGroup;
  // Partial: make all properties in T class optional
  bsConfig: Partial<BsDatepickerConfig>;
  constructor(private authService: AuthService, private altertify: AlertifyService,
              private fb: FormBuilder, private router: Router) { }

  ngOnInit() {

    // old fashion way to create an angular form

    // this.registerForm = new FormGroup({
    //   username: new FormControl('', Validators.required),
    //   password: new FormControl('', [Validators.required, Validators.minLength(4),
    //     Validators.maxLength(8)]),
    //   // need to apply our own defined logic to validate this one which is the content should be the same
    //   // with password
    //   confirmPassword: new FormControl('', Validators.required)
    // }, this.paswordMatchValidator);
    this.createRegisterForm();
    this.bsConfig = {
      containerClass: 'theme-dark-blue'
    };
  }

  createRegisterForm() {
    // more simpler way to create a reactive form using formBuilder which is an angular service
    this.registerForm = this.fb.group({
      gender: ['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4),
        Validators.maxLength(8)]],
      confirmPassword: ['', Validators.required],

    }, {
      // pass the customized validator
      validator: this.paswordMatchValidator
    });
  }

  // customized validator function
  paswordMatchValidator(g: FormGroup) {
    return g.get('password').value === g.get('confirmPassword').value ? null : {'mismatch':true};
  }

  register() {
    if(this.registerForm.valid) {
      // clones the properties in later one to front one
      this.user = Object.assign({}, this.registerForm.value);
      this.authService.register(this.user).subscribe(() => {
        this.altertify.success('Registration successful');
      }, (error) => {
        this.altertify.error(error);
      }, ()=> {
        //
        this.authService.login(this.user).subscribe(() => {
          this.router.navigate(['/games']);
        })
      })
    }
    // this.authService.register(this.model).subscribe(() => {
    //   this.altertify.success('registration successful');
    // }, error => {
    //   this.altertify.error(error);
    // });
    console.log(this.registerForm.value);
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
