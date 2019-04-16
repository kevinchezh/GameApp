import { Injectable } from '@angular/core';
import { User } from '../models/user';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { AuthService } from '../services/auth.service';
@Injectable()
export class GameEditResolver implements Resolve<User> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService,
        private authService: AuthService) {}
    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // decodedToken: is the obejct we decode from token string, which include nameid(user id)
        return this.userService.getUser(this.authService.decodedToken.nameid).pipe(
            catchError(error => {
                this.alertify.error('Problem retrieving your data');
                this.router.navigate(['/games']);
                return of(null);
            })
        );
    }
}
