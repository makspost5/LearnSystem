import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-pupil-course-page',
  templateUrl: './pupil-course-page.component.html',
  styleUrls: ['./pupil-course-page.component.css'],
  providers: [SubjectService]
})
export class PupilCoursePageComponent implements OnInit {

  

  hierarchys: HierarchyLevel[] = [];

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService) {
    this.http.getSubjectsSections(activateRoute.snapshot.params['id']).subscribe((data: Array<Object>) => {
      console.log(data);
      let index: number = 0;
      let hierarchy: HierarchyLevel = new HierarchyLevel();
      hierarchy.level = index;
      for (let subject of data) {
        let newSubject : SubjectSection = subject as SubjectSection;
        if (newSubject.HierarchyLevel !== index) {
          index++;
          this.hierarchys.push(hierarchy);
          hierarchy = new HierarchyLevel();
          hierarchy.level = index;
        }
        hierarchy.subjectSections.push(newSubject);
      }

      if (hierarchy.subjectSections.length !== 0) {
        this.hierarchys.push(hierarchy);
      }
      console.log(this.hierarchys);
    });
  }

  ngOnInit() {
  }

}

export class HierarchyLevel {
  level: number;
  subjectSections: SubjectSection[] = [];
}

export class SubjectSection {
  HierarchyLevel: number;
  SubjectSectionID: number;
  image: string;
  Name: string;
  subjectID: number;
  SubjectCourseID: number;
}
