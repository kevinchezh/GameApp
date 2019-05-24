import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { AlertifyService } from '../services/alertify.service';
import { Router } from '@angular/router';
import { routerNgProbeToken } from '@angular/router/src/router_module';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {};
  gameUrl: string;
  constructor(public authService: AuthService, private alterfiyService: AlertifyService,
    private router: Router) { }

  ngOnInit() {
    this.authService.currentGameUrl.subscribe(gameUrl => this.gameUrl = gameUrl);
  }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alterfiyService.success('login successfully');
    }, error => {
      this.alterfiyService.error(error);
    },
    // complete method (like finally method)
    () => {
      this.router.navigate(['/games']);
    });
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.alterfiyService.message('logout');
    this.router.navigate(['']);
  }

}
