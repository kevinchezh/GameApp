import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { UserService } from 'src/app/services/user.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { ActivatedRoute } from '@angular/router';
import { load } from '@angular/core/src/render3';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-game-detail',
  templateUrl: './game-detail.component.html',
  styleUrls: ['./game-detail.component.css']
})
export class GameDetailComponent implements OnInit {
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService, private altertify: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    });
    // console.log(this.user);
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];

    this.galleryImages = this.getImages();

  }
  getImages() {
    const imageUrls = [];
    console.log(this.user);
    for (let i = 0; i < this.user.games.length; i++) {
      imageUrls.push({
        small: this.user.games[i].url,
        medium: this.user.games[i].url,
        big: this.user.games[i].url,
        description: this.user.games[i].description
      });
    }
    // console.log(imageUrls);
    return imageUrls;
  }
  loadUser() {
    // + will cast string to number
    // get the id param in activated route and then remember getUser return an observable
    this.userService.getUser(+this.route.snapshot.params['id']).subscribe((user: User) => {
      // console.log(user);
      this.user = user;
      // console.log(this.user);
    }, (error) => {
      this.altertify.error(error);
    });
  }

}
