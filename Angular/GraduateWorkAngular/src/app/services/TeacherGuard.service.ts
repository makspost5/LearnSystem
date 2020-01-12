import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { Router } from '@angular/router';

@Injectable()
export class TeacherGuard implements CanActivate {

    constructor(private _router: Router) {
    }

    canActivate() {
        if (localStorage.getItem('role') !== 'teacher') {
            switch (localStorage.getItem('role')) {
                case 'admin': {
                    this._router.navigate(['']);
                    break;
                }
                case 'pupil': {
                    this._router.navigate(['pupil-page']);
                    break;
                }
            }
        }

        return localStorage.getItem('role') === 'teacher';
    }
}