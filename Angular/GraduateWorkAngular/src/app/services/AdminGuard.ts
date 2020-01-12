import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';

@Injectable()
export class AdminGuard implements CanActivate {

  constructor(private _router: Router) {
  }

  canActivate() {
    if (localStorage.getItem('role') !== 'admin') {
      switch (localStorage.getItem('role')) {
          case 'teacher': {
              this._router.navigate(['']);
              break;
          }
          case 'pupil': {
              this._router.navigate(['pupil-page']);
              break;
          }
      }
  }

  return localStorage.getItem('role') === 'admin';
  }
}