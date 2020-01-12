import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { ActivatedRoute, Router } from '@angular/router';
import { GroupModel, Subject } from '../teacher-test-creation-page/teacher-test-creation-page.component';
import {Location} from '@angular/common';

@Component({
  selector: 'app-teacher-course-creation-page',
  templateUrl: './teacher-course-creation-page.component.html',
  styleUrls: ['./teacher-course-creation-page.component.css'],
  providers: [SubjectService]
})
export class TeacherCourseCreationPageComponent implements OnInit {

  hierarchys: HierarchyLevel[] = [];
  modeParameters: boolean = true;
  courseParameters: CourseParameters;
  groupModels: GroupModel[] = [];
  subjects: Subject[] = [];

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService, private _router: Router, private _location: Location) {

    this.http.controllerGetSubjects().subscribe((data: Array<Object>) => {
      for (let subject of data) {
        let newSubject: Subject = subject as Subject;
        this.subjects.push(newSubject);
      }
      console.log(this.subjects);
    });

    this.courseParameters = new CourseParameters();
    var id = activateRoute.snapshot.params['id'];
    if (!id) {
      this.http.controllerGetSubjectGroups().subscribe((data: Array<Object>) => {
        for (let group of data) {
          let newGroupModel: GroupModel = group as GroupModel;
          this.groupModels.push(newGroupModel);
        }
        console.log(this.groupModels);
      });

      var finishLevel = new HierarchyLevel();
      finishLevel.level = 0;
      finishLevel.subjectSections = [];

      var newSection = new SubjectSection();
      newSection.SubjectSectionID = -1;
      finishLevel.subjectSections.push(newSection);

      this.hierarchys.push(finishLevel);
    } else {
      this.http.controllerGetUpdateCourseData(id).subscribe(data => {
        this.courseParameters = data['courseParameters'] as CourseParameters;

        for (let group of data['groupModels']) {
          let newGroupModel: GroupModel = group as GroupModel;
          this.groupModels.push(newGroupModel);
        }
        console.log(this.groupModels);
      });

      this.http.getSubjectsSections(id).subscribe((data: Array<Object>) => {
        let index: number = 0;
        let hierarchy: HierarchyLevel = new HierarchyLevel();
        var isLes = false;
        hierarchy.level = index;
        for (let subject of data) {
          let newSubject: SubjectSection = subject as SubjectSection;
          if (newSubject.HierarchyLevel !== index) {
            if (newSubject.HierarchyLevel < index) {
              for (let lvl of this.hierarchys) {
                if (newSubject.HierarchyLevel === lvl.level) {
                  lvl.subjectSections.push(newSubject);
                  isLes = true;
                  break;
                }
              }
            } else {

              index++;
              this.hierarchys.push(hierarchy);
              hierarchy = new HierarchyLevel();
              hierarchy.level = index;
            }
          }

          if (!isLes) {
            hierarchy.subjectSections.push(newSubject);
          }
          isLes = false;
        }

        if (hierarchy.subjectSections.length == 1) {
          var rightSection = new SubjectSection();
          rightSection.SubjectSectionID = -1;
          hierarchy.subjectSections.push(rightSection);
        }

        if (hierarchy.subjectSections.length !== 0) {
          this.hierarchys.push(hierarchy);
        }

        var finishLevel = new HierarchyLevel();
        finishLevel.level = this.hierarchys.length;
        finishLevel.subjectSections = [];

        var newSection = new SubjectSection();
        newSection.SubjectSectionID = -1;
        finishLevel.subjectSections.push(newSection);

        this.hierarchys.push(finishLevel);

        console.log(this.hierarchys);
      });
    }

    // this.http.getSubjectsSection(activateRoute.snapshot.params['id']).subscribe((data: Array<Object>) => {
    //   let index: number = 0;
    //   let hierarchy: HierarchyLevel = new HierarchyLevel();
    //   hierarchy.level = index;
    //   for (let subject of data) {
    //     let newSubject : SubjectSection = subject as SubjectSection;
    //     if (newSubject.hierarchyLevel !== index) {
    //       index++;
    //       this.hierarchys.push(hierarchy);
    //       hierarchy = new HierarchyLevel();
    //       hierarchy.level = index;
    //     }
    //     hierarchy.subjectSections.push(newSubject);
    //   }

    //   if (hierarchy.subjectSections.length !== 0) {
    //     this.hierarchys.push(hierarchy);
    //   }
    //   console.log(this.hierarchys);
    // });
  }

  changeMode() {
    this.modeParameters = !this.modeParameters;
  }

  cancelButton() {
    this._location.back();
    console.log(this.hierarchys);
  }

  saveButton() {
    this.http.teacherPostCourse(this.courseParameters, this.groupModels).subscribe((data: number) => {
      this.courseParameters.id = data;
      console.log(data);
    });
    console.log(this.hierarchys);
  }

  changeAvailable(i) {
    this.groupModels[i].isAvailable = !this.groupModels[i].isAvailable;
  }

  addSection(level) {
    this._router.navigate(['teacher-page/create-course/' + this.courseParameters.id + '/create-section/' + level]);
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

export class CourseParameters {
  id: number = 0;
  name: string;
  description: string;
  subjectID: number = 0;
  icon: any;
}
