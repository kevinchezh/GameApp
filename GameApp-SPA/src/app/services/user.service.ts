import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';

// with jwtmodule, it could add token to request automatically, so we dont need to
// config ourselfves

// why this? every request here need authorization in the header in order to pass
// the guard in back end. So we reterive the token in localStorage and send it back
// along side with request
// const httpOptions = {
//   headers: new HttpHeaders({
//     // the space after Bear is important
//     'Authorization': 'Bearer ' + localStorage.getItem('token')
//   })
// };


@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.apiUrl;
constructor(private http: HttpClient) { }

getUsers(): Observable<User[]> {
  // htt.get() would reture a Observable<Object>, so we need to specify the specific object
  // type in order to cast it.
  return this.http.get<User[]>(this.baseUrl + 'users');
}

getUser(id): Observable<User> {
  return this.http.get<User>(this.baseUrl + 'users/' + id);
}

updateUser(id: number, user: User) {
  return this.http.put(this.baseUrl + 'users/' + id, user);
}


}


