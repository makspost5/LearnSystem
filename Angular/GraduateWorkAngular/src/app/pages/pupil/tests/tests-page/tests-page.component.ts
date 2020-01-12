import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tests-page',
  templateUrl: './tests-page.component.html',
  styleUrls: ['./tests-page.component.css'],
  providers: [SubjectService]
})
export class TestsPageComponent implements OnInit {

  testRows: TestRow[] = [];

  constructor(private http: SubjectService, private _router: Router) {
    this.http.controllerGetPupilTestRows().subscribe((data: Array<Object>) => {
      for (let row of data) {
        let newTestRow: TestRow = row as TestRow;
        this.testRows.push(newTestRow);
      }
      console.log(this.testRows);
    });
  }

  ngOnInit() {
  }


  selectTest(id) {
    this._router.navigate(['pupil-page/test/' + id]);
  }
}

export class TestRow {
  ID: number;
  Name: string;
  Subject: string;
  TestType: string;
  TeacherName: string;
  Mark: number;
}
