import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {map} from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  baseUrl = 'http://localhost:5000/api/auth/';
  // this helper could check whether current token exsits or expired and also could
  // decode the token to get some information like username(from header)
  jwtHelper = new JwtHelperService();
  decodedToken: any;
constructor(private http: HttpClient) { }

login(model: any) {
  return this.http.post(this.baseUrl + 'login', model)
  .pipe(
    map((res: any) => {
      const user = res;
      if (user) {
        localStorage.setItem('token', user.token);
        // decode token to get user name
        this.decodedToken = this.jwtHelper.decodeToken(user.token);
        // username is decodedToken.unique_name which can be found in console log
        // console.log(this.decodedToken);
      }
    })
  );

}
register(model: any) {
    return this.http.post(this.baseUrl + 'register', model);
}

loggedIn() {
  const token = localStorage.getItem('token');
  // isTokenExpired would return true if token is not exsits or expired
  return !this.jwtHelper.isTokenExpired(token);
}

}
