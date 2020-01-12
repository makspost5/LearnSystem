import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teacher-courses-page',
  templateUrl: './teacher-courses-page.component.html',
  styleUrls: ['./teacher-courses-page.component.css'],
  providers: [SubjectService]
})
export class TeacherCoursesPageComponent implements OnInit {

  courseRows: CourseRow[] = [];

  constructor(private http: SubjectService, private _router: Router) {
    this.http.controllerGetCourseRows().subscribe((data: Array<Object>) => {
      for (let row of data) {
        let newCourseRow: CourseRow = row as CourseRow;
        this.courseRows.push(newCourseRow);
      }
      console.log(this.courseRows);
    });
  }

  ngOnInit() {
  }

  selectCourse(id) {
    this._router.navigate(['teacher-page/update-course/' + id]);
  }

  deleteCourse(index) {
    this.http.controllerDeleteCourse(this.courseRows[index].ID).subscribe(data => {
      console.log(data);
      this.courseRows.splice(index, 1);
    });
  }

  createCourse(){
    this._router.navigate(['teacher-page/create-course']);
  }

}

export class CourseRow {
  ID: number;
  Name: string;
  Subject: string;
  SectionsCount: number;
  Available: boolean;
}
