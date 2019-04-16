import { Injectable } from '@angular/core';
import { GameEditComponent } from '../game/game-edit/game-edit.component';
import { CanDeactivate } from '@angular/router';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<GameEditComponent> {
    canDeactivate(component: GameEditComponent) {
        if (component.editForm.dirty) {
            return confirm('Are you sure want to continue? Any unsaved changes will be lost');
        }
        return true;
    }
}
