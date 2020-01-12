import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { SubjectService } from 'src/app/services/subject.service';
import * as $ from 'jquery';
import { Router } from '@angular/router'; 

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css'],
  providers: [SubjectService]
})
export class LoginPageComponent implements OnInit {

  public authForm: FormGroup;
  public registerForm: FormGroup;

  courses: Course[] = [];
  faculties: Faculty[] = [];
  groups: Group[] = [];

  messageClick() {
    $('form').animate({ height: "toggle", opacity: "toggle" }, "slow");
  }

  SignIn() {
    const controls = this.authForm.controls;

    /** Проверяем форму на валидность */
    if (this.authForm.invalid) {
      /** Если форма не валидна, то помечаем все контролы как touched*/
      Object.keys(controls)
        .forEach(controlName => controls[controlName].markAsTouched());

      /** Прерываем выполнение метода*/
      return;
    }

    this.http.login(this.authForm.controls['email'].value, this.authForm.controls['password'].value).subscribe(responseData => {
      localStorage.setItem('access_token', responseData['access_token']);
      localStorage.setItem('role', responseData['role']);
      console.log(localStorage.getItem('access_token'));
      console.log(localStorage.getItem('role'));

      switch(responseData['role']) {
        case 'teacher': {
          this._router.navigate(['teacher-page']);
          break;
        }
        case 'admin': {

          break;
        }
        case 'pupil': {
          this._router.navigate(['pupil-page']);
          break;
        }
      }
    });
  }

  Register() {
    const controls = this.registerForm.controls;

    if (!this.confirmPasswordValidator()) {
      $('#confirmPassword').addClass('password-invalid');
      $('#passwordRegister').addClass('password-invalid');
    }

    /** Проверяем форму на валидность */
    if (this.registerForm.invalid) {
      /** Если форма не валидна, то помечаем все контролы как touched*/
      Object.keys(controls)
        .forEach(controlName => controls[controlName].markAsTouched());

      if (this.registerForm.controls["group"].value == 'disabled') {
        $('#groupSelector').addClass('group-invalid');
      }


      /** Прерываем выполнение метода*/
      return;
    }


    /** TODO: Обработка данных формы */
    console.log(this.registerForm.value);
  }

  isControlInvalid(controlName: string): boolean {
    const control = this.authForm.controls[controlName];

    const result = control.invalid && control.touched;

    return result;
  }

  isControlInvalidRegister(controlName: string): boolean {
    const control = this.registerForm.controls[controlName];

    const result = control.invalid && control.touched;

    return result;
  }

  confirmPasswordValidator(): boolean {
    return this.registerForm.controls["confirmPassword"].value === this.registerForm.controls["passwordRegister"].value;
  }

  constructor(private http: SubjectService, private _router: Router) {
    this.authForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.email]),
      password: new FormControl('', Validators.required),
    });

    this.registerForm = new FormGroup({
      emailRegister: new FormControl('', [Validators.required, Validators.email]),
      passwordRegister: new FormControl('', [Validators.required, Validators.minLength(8)]),
      userName: new FormControl('', Validators.required),
      confirmPassword: new FormControl('', [Validators.required, Validators.minLength(8)]),
      group: new FormControl('disabled', Validators.required),
    });

    this.http.controllerGetCourses().subscribe((data: Array<Object>) => {
      for (let course of data) {
        let newCourse: Course = course as Course;
        this.courses.push(newCourse);
      }
      console.log(this.courses);
    });

    this.http.controllerGetFaculties().subscribe((data: Array<Object>) => {
      for (let faculty of data) {
        let newFaculty: Faculty = faculty as Faculty;
        this.faculties.push(newFaculty);
      }
      console.log(this.faculties);
    });

    this.http.controllerGetGroups().subscribe((data: Array<Object>) => {
      for (let group of data) {
        let newGroup: Group = group as Group;
        this.groups.push(newGroup);
      }
      console.log(this.groups);
    });
  }

  ngOnInit() {
  }

}

export class Course {
  CourseID: number;
  Name: string;
}

export class Faculty {
  FacultyID: number;
  Name: string;
}

export class Group {
  GroupID: number;
  CourseID: number;
  FacultyID: number;
  GroupNumber: number;
}
