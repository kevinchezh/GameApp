/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GameEditComponent } from './game-edit.component';

describe('GameEditComponent', () => {
  let component: GameEditComponent;
  let fixture: ComponentFixture<GameEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GameEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GameEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
