import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BsDatepickerModule, BsDropdownModule, TabsModule} from 'ngx-bootstrap';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule} from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { AuthService } from './services/auth.service';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { ErrorInterceptorProvide } from './services/error.interceptor';
import { AlertifyService } from './services/alertify.service';
import { MemberListComponent } from './member-list/member-list.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { GameListComponent } from './game/game-list/game-list.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { AuthGuard } from './_guard/auth.guard';
import { UserService } from './services/user.service';
import { GameCardComponent } from './game/game-card/game-card.component';
import { JwtModule } from '@auth0/angular-jwt';
import { GameDetailComponent } from './game/game-detail/game-detail.component';
import { NgxGalleryModule } from 'ngx-gallery';
import { GameDetailResolver } from './reslover/game-detail.resolver';
import { GameListResolver } from './reslover/game-list.resolver';
import { GameEditComponent } from './game/game-edit/game-edit.component';
import { GameEditResolver } from './reslover/game-edit.resolver';
import { PreventUnsavedChanges } from './_guard/prevent-unsaved-changes.guard';
import { GameEditorComponent } from './game/game-editor/game-editor.component';
import { FileUploadModule } from "ng2-file-upload";
export function tokenGetter() {
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      HomeComponent,
      RegisterComponent,
      MemberListComponent,
      ListsComponent,
      MessagesComponent,
      GameListComponent,
      GameCardComponent,
      GameDetailComponent,
      GameEditComponent,
      GameEditorComponent
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      TabsModule.forRoot(),
      NgxGalleryModule,
      // routes set up
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            // this would set a token along side with http request
            tokenGetter: tokenGetter,
            // this is the domain we want to add token
            whitelistedDomains: ['localhost:5000'],
            // the domain that do not set token with
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      }),
     FileUploadModule,

   ],
   providers: [
      AuthService,
      ErrorInterceptorProvide,
      AlertifyService,
      AuthGuard,
      UserService,
      GameDetailResolver,
      GameListResolver,
      GameEditResolver,
      PreventUnsavedChanges
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
