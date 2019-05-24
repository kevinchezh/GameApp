import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Game} from "../../models/Game";
import { FileUploader } from 'ng2-file-upload';
import {environment} from "../../../environments/environment";
import {AuthService} from "../../services/auth.service";
import {UserService} from "../../services/user.service";
import {AlertifyService} from "../../services/alertify.service";

@Component({
  selector: 'app-game-editor',
  templateUrl: './game-editor.component.html',
  styleUrls: ['./game-editor.component.css']
})
export class GameEditorComponent implements OnInit {
  @Input() games:Game[];
  @Output() getMemberGameChange = new EventEmitter<string>();
  currentMainGame: Game;
  uploader:FileUploader;
  hasBaseDropZoneOver:boolean = false;
  baseUrl = environment.apiUrl;
  constructor(private authService: AuthService,
              private userService: UserService,
              private alertify: AlertifyService) { }

  ngOnInit() {
    this.initializeUploader();
  }

  initializeUploader() {
    this.uploader = new FileUploader({
      url: this.baseUrl + "users/" + this.authService.decodedToken.nameid + "/games",
      authToken: "Bearer " + localStorage.getItem('token'),
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      // need to click a button to upload
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024 // max 10MB
    });

    this.uploader.onAfterAddingFile = (file) => {file.withCredentials = false};
    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if(response) {
        const res: Game = JSON.parse(response);
        const game = {
          id: res.id,
          url: res.url,
          dateAdded: res.dateAdded,
          description: res.description,
          isMain: res.isMain
        };
        this.games.push(game);
      }
    }
  }

  public fileOverBase(e:any):void {
    this.hasBaseDropZoneOver = e;
  }

  setMainGame(game: Game) {
    this.userService.setMainGame(this.authService.decodedToken.nameid,
      game.id).subscribe(()=> {
        //filter method would filter by condition and return an array of satisfied objects
        this.currentMainGame = this.games.filter(g => g.isMain === true)[0];
        this.currentMainGame.isMain = false;
        game.isMain = true;
        // this.getMemberGameChange.emit(game.url);
        this.authService.changeMemberGame(game.url);
        // after update the Main game in memory, need to reflex that on the real current user
        this.authService.currentUser.gameUrl = game.url;
        // also update the localstorage
        localStorage.setItem('user', JSON.stringify(this.authService.currentUser));
    },(error)=> {
        this.alertify.error(error);
    })
  }

  deleteGame(id: number) {
    this.alertify.confirm('Are you sure you want to delete this game?', () => {
      this.userService.deleteGame(this.authService.decodedToken.nameid, id).subscribe(() => {
        //delete the game from the array in this component as well
        this.games.splice(this.games.findIndex(g => g.id == id) , 1);
        this.alertify.success("Deletion successful");
      }, (error) => {
        this.alertify.error("Deletion failed");
      });
    });
  }
}
