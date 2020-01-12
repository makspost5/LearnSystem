import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-pupil-section-page',
  templateUrl: './pupil-section-page.component.html',
  styleUrls: ['./pupil-section-page.component.css'],
  providers: [SubjectService]
})
export class PupilSectionPageComponent implements OnInit {

  subjectBlocks: SectionBlock[] = [];
  count: number = 0;

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService) {
    http.getSectionBlocksBySubjectSectionId(activateRoute.snapshot.params['id']).subscribe((data: Array<Object>) => {
      let isPassedLast = true;
      for (let subject of data) {
        let newSubject : SectionBlock = subject as SectionBlock;
        newSubject.isAvailable = isPassedLast;
        isPassedLast = newSubject.isPassed;
        this.subjectBlocks.push(newSubject);
        this.count++;
      }
      console.log(this.subjectBlocks);
    });
  }

  ngOnInit() {
  }

}

export class SectionBlock {
  SectionBlockID: number;
  isPassed: boolean;
  Name: string;
  Position: number;
  SubjectSectionID: number;
  isAvailable: boolean;
}
