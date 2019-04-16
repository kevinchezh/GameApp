import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { GameListComponent } from './game/game-list/game-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guard/auth.guard';
import { GameDetailComponent } from './game/game-detail/game-detail.component';
import { GameDetailResolver } from './reslover/game-detail.resolver';
import { GameListResolver } from './reslover/game-list.resolver';
import { GameEditComponent } from './game/game-edit/game-edit.component';
import { GameEditResolver } from './reslover/game-edit.resolver';
import { PreventUnsavedChanges } from './_guard/prevent-unsaved-changes.guard';

export const appRoutes: Routes = [
    {path : '', component: HomeComponent},
    // rather than write canActivate:[AuthGuard] to every routes, we can put all
    // these routes under a parent route and all we need to do is guard the parent path

    // why path is ""?, the rule of matching child path is localhost/parentchild
    // means the path should the the cancatination of parent route and child route,
    // so if parent path is "hi", then to access games page, the path should be
    // /higames
    // by set parent path to be "", this would not affect the children path
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            {path : 'games', component: GameListComponent, resolve: {users: GameListResolver}},
            {path : 'games/:id', component: GameDetailComponent, resolve: {user: GameDetailResolver}},
            {path : 'game/edit', component: GameEditComponent, resolve: {user: GameEditResolver},
        canDeactivate: [PreventUnsavedChanges]},
            {path : 'messages', component: MessagesComponent},
            {path : 'lists', component: ListsComponent},
        ]
    },
    // order is important, especially for wild cards
    {path : '**', redirectTo: '', pathMatch : 'full'},
];
