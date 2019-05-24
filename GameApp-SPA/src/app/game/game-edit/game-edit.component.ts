import { Component, OnInit, ViewChild, HostListener } from '@angular/core';
import { User } from 'src/app/models/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/services/alertify.service';
import { NgForm } from '@angular/forms';
import { UserService } from 'src/app/services/user.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-game-edit',
  templateUrl: './game-edit.component.html',
  styleUrls: ['./game-edit.component.css']
})
export class GameEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  gameUrl: string;
  // canDeactivated guard could only guard the navigation within this website
  // if we want to prevent user close the website or sth, we need host listener to monitor
  // the web browser for us.
  @HostListener('window:beforeunload', ['$event'])
  unloadNotification($event: any) {
    if (this.editForm.dirty) {
      $event.returnValue = true;
    }
  }

  constructor(private route: ActivatedRoute, private alertify: AlertifyService,
    private userService: UserService, private authService: AuthService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    this.authService.currentGameUrl.subscribe(gameUrl => this.gameUrl = gameUrl);
  }

  updateUser() {
    this.userService.updateUser(this.authService.decodedToken.nameid, this.user).subscribe(next => {
      this.alertify.success('update successfully!');
    // param would reset the form with data of user remains
    this.editForm.reset(this.user);
    }, error => {
      this.alertify.error(error);
    });
  }

  updateMainGame(gameUrl) {
    this.user.gameUrl = gameUrl;
  }

}
