import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';

@Component({
  selector: 'app-all-results',
  templateUrl: './all-results.component.html',
  styleUrls: ['./all-results.component.css'],
  providers: [SubjectService]
})
export class AllResultsComponent implements OnInit {

  isDataLoaded = false;
  isTest: boolean = true;
  testResults: TestResultModel[] = [];
  courseResults: CourseResultModel[] = [];
  personResultModelCourse: PersonResultModelCourse[] = [];
  personResultModelTest: PersonResultModelTest[] = [];
  selectedTestNumber: number = 0;
  selectedCourseNumber: number = 0;

  constructor(private http: SubjectService) {
    this.http.controllerGetTeacherResultsCourse().subscribe((data: Array<CourseResultModel>) => {
      this.courseResults = data;
      console.log(this.courseResults);
    });

    this.http.controllerGetTeacherResultsTest().subscribe((data: Array<TestResultModel>) => {
      this.testResults = data;
      this.isDataLoaded = true;
      console.log(this.testResults);
    });
  }

  ngOnInit() {
  }

  linkClick(isTest) {
    console.log(isTest);
    this.isTest = isTest;
  }

  getGroups(index, isTest) {
    var groups = '';
    if (isTest) {
      for (var gr of this.testResults[index].GroupResultModels) {
        groups += gr.Name + ' '
      }
    } else {
      for (var gr of this.courseResults[index].GroupResultModels) {
        groups += gr.Name + ' '
      }
    }

    return groups;
  }

  selectTest(index) {
    this.selectedTestNumber = index;
  }

  selectCourse(index) {
    this.selectedCourseNumber = index;
  }

  groupChange(value) {
    if (this.isTest) {
      this.http.controllerGetTeacherResultsTestByGroup(value, this.testResults[this.selectedTestNumber].TestID).subscribe((data: PersonResultModelTest[]) => {
        this.personResultModelTest = data;
        console.log(this.personResultModelTest);
      });
    } else {
      this.http.controllerGetTeacherResultsCourseByGroup(value, this.courseResults[this.selectedCourseNumber].SubjectCourseID).subscribe((data: PersonResultModelCourse[]) => {
        this.personResultModelCourse = data;
        console.log(this.personResultModelCourse);
      });
    }
    console.log(value);
  }
}

export class TestResultModel {
  TestID: number;
  Name: string;
  Subject: string;
  TypeTest: string;
  GroupResultModels: GroupResultModel[];
}

export class GroupResultModel {
  GroupID: number;
  Name: string;
}

export class CourseResultModel {
  SubjectCourseID: number;
  Name: string;
  Subject: string;
  SubjectSectionCount: number;
  GroupResultModels: GroupResultModel[];
}

export class PersonResultModelCourse {
  Name: string;
  SectionName: string;
  LastTime: string;
}

export class PersonResultModelTest {
  Name: string;
  Grade: number;
  Finish: string;
}