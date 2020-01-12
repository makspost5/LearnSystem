import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-teacher-tests-page',
  templateUrl: './teacher-tests-page.component.html',
  styleUrls: ['./teacher-tests-page.component.css'],
  providers: [SubjectService]
})
export class TeacherTestsPageComponent implements OnInit {

  testRows: TestRow[] = [];

  constructor(private http: SubjectService, private _router: Router) {
    this.http.controllerGetTestRows().subscribe((data: Array<Object>) => {
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
    this._router.navigate(['teacher-page/update-test/' + id]);
  }

  deleteTest(index) {
    this.http.controllerDeleteTest(this.testRows[index].ID).subscribe(data => {
      console.log(data);
      this.testRows.splice(index, 1);
    });
  }

  createTest(){
    this._router.navigate(['teacher-page/create-test']);
  }
}

export class TestRow {
  ID: number;
  Name: string;
  Subject: string;
  TestType: string;
  Available: boolean;
}
