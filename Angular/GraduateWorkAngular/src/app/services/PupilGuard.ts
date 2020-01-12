import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';

@Injectable()
export class PupilGuard implements CanActivate {

  constructor(private _router: Router) {
  }

  canActivate() {
    if (localStorage.getItem('role') !== 'pupil') {
      switch (localStorage.getItem('role')) {
          case 'teacher': {
              this._router.navigate(['teacher-page']);
              break;
          }
          case 'admin': {
              this._router.navigate(['']);
              break;
          }
      }
  }

  return localStorage.getItem('role') === 'pupil';
  }
}