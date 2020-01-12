import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';

@Component({
  selector: 'app-pupil-all-courses',
  templateUrl: './pupil-all-courses.component.html',
  styleUrls: ['./pupil-all-courses.component.css'],
  providers: [SubjectService]
})
export class PupilAllCoursesComponent implements OnInit {
  subjects: SubjectCurse[] = [];

  constructor(private http: SubjectService) {
    this.http.getSubjectCourses().subscribe((data: Array<Object>) => {
      for (let subject of data) {
        let newSubject : SubjectCurse = subject as SubjectCurse;
        this.subjects.push(newSubject);
      }
      console.log(this.subjects);
    });
  }

  ngOnInit() {

  }

}

export class SubjectCurse {
  Name: string;
  Description: string;
  Image: string;
  Teacher: string;
  Subject: string;
  Id: number;
}