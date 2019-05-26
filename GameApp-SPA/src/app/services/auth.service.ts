import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject} from "rxjs";
import {map} from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import {User} from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = environment.apiUrl + 'auth/';
  // this helper could check whether current token exsits or expired and also could
  // decode the token to get some information like username(from header)
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  gameUrl = new BehaviorSubject<string>('../../assets/user.png');
  currentGameUrl = this.gameUrl.asObservable();
constructor(private http: HttpClient) { }

changeMemberGame(gameUrl: string) {
  // update game url observable
  this.gameUrl.next(gameUrl);
}

login(model: any) {
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((res: any) => {
      const user = res;
      if (user) {
        localStorage.setItem('token', user.token);
        localStorage.setItem('user', JSON.stringify(user.user));
        // decode token to get user name
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        this.currentUser = user.user;
        this.changeMemberGame(this.currentUser.gameUrl);
        // username is decodedToken.unique_name which can be found in console log
        // console.log(this.decodedToken);
      }
    })
  );

}
register(user: User) {
    return this.http.post(this.baseUrl + 'register', user);
}

loggedIn() {
  const token = localStorage.getItem('token');
  // isTokenExpired would return true if token is not exsits or expired
  return !this.jwtHelper.isTokenExpired(token);
}

}
