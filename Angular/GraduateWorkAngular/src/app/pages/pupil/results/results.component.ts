import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';

@Component({
  selector: 'app-results',
  templateUrl: './results.component.html',
  styleUrls: ['./results.component.css'],
  providers: [SubjectService]
})
export class ResultsComponent implements OnInit {

  pupilResultModelTest: PupilResultModelTest[] = [];
  isDataLoaded: boolean = false;

  constructor(private http: SubjectService) {
    this.http.controllerGetPupilResults().subscribe((data: PupilResultModelTest[]) => {
      this.pupilResultModelTest = data;
      console.log(this.pupilResultModelTest);
      this.isDataLoaded = true;
    });
  }

  ngOnInit() {
  }

}

export class PupilResultModelTest {
  Grade: number;
  Finish: string;
  Test: string;
  Subject: string;
  TypeTest: string;
}