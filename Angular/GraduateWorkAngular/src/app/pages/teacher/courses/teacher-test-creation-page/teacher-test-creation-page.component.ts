import { Component, OnInit } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import {Location} from '@angular/common';

@Component({
  selector: 'app-teacher-test-creation-page',
  templateUrl: './teacher-test-creation-page.component.html',
  styleUrls: ['./teacher-test-creation-page.component.css'],
  providers: [SubjectService]
})
export class TeacherTestCreationPageComponent implements OnInit {

  private startId: number = -1;

  answerTypes: AnswerType[] = [];
  testTypes: TestType[] = [];
  subjects: Subject[] = [];
  groupModels: GroupModel[] = [];

  selectedQuestionNumber: number = -1;
  public questionForm: FormGroup;
  testParameters: TestParameters;

  questions: Question[];

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService, private _router: Router, private _location: Location) {
    this.testParameters = new TestParameters();
    this.questions = [];
    var id = activateRoute.snapshot.params['id'];

    if (id) {
      this.startId = id;

      this.http.controllerGetUpdateTestData(id).subscribe(data => {
        this.testParameters = data['testParameters'] as TestParameters;
        for (let question of data['questions']) {
          let newQuestion: Question = question as Question;

          this.questions.push(newQuestion);
        }

        for (let i = 0; i < this.questions.length; i++) {
          this.selectedQuestionNumber = i;
          this.questiontTypeChange(this.questions[i].selectedTypeValue.toString(), false);
        }

        console.log(data);
        for (let group of data['groupModels']) {
          let newGroupModel: GroupModel = group as GroupModel;
          this.groupModels.push(newGroupModel);
        }
        console.log(this.groupModels);

        this.selectedQuestionNumber = -1;
      })
    }

    this.selectedQuestionNumber = -1;

    this.questionForm = new FormGroup({
      title: new FormControl('', Validators.required),
      questionType: new FormControl('disabled', Validators.required)
    });

    this.http.controllerGetAnswerTypes().subscribe((data: Array<Object>) => {
      for (let answerType of data) {
        let newAnswerType: AnswerType = answerType as AnswerType;
        if (newAnswerType.AnswerTypeID !== 6) {
          this.answerTypes.push(newAnswerType);
        }
      }
      console.log(this.answerTypes);
    });

    this.http.controllerGetTestTypes().subscribe((data: Array<Object>) => {
      for (let testType of data) {
        let newTestType: TestType = testType as TestType;
        this.testTypes.push(newTestType);
      }
      console.log(this.testTypes);
    });

    this.http.controllerGetSubjects().subscribe((data: Array<Object>) => {
      for (let subject of data) {
        let newSubject: Subject = subject as Subject;
        this.subjects.push(newSubject);
      }
      console.log(this.subjects);
    });

    if (this.startId === -1) {
      this.http.controllerGetSubjectGroups().subscribe((data: Array<Object>) => {
        for (let group of data) {
          let newGroupModel: GroupModel = group as GroupModel;
          this.groupModels.push(newGroupModel);
        }
        console.log(this.groupModels);
      });
    }
  }

  addQuestion() {
    this.questions.push(new Question());
    this.selectedQuestionNumber = this.questions.length - 1;

    console.log("Добавлен вопрос " + this.selectedQuestionNumber);
  }

  saveTest() {
    var mode = '';
    if (this.startId === -1) {
      mode = 'create';
    } else {
      mode = 'update';
    }

    this.http.teacherPostTest(this.testParameters, this.questions, this.groupModels, mode).subscribe(data => {
      console.log(data);
      this._router.navigate(['teacher-page/tests']);
    });

    // TODO
    console.log(this.questions);
  }

  ngOnInit() {
  }

  questiontTypeChange(typeValue, mode = true) {
    if (mode) {
      for (let i in this.questions[this.selectedQuestionNumber].answers) {
        this.questions[this.selectedQuestionNumber].answers[i].isRight = false;
      }
    }

    this.questions[this.selectedQuestionNumber].selectedTypeValue = typeValue;
    switch (typeValue) {
      case "1": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isOne = true;
        break;
      }
      case "2": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isSeveral = true;
        break;
      }
      case "3": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isOrder = true;
        break;
      }
      case "4": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isText = true;
        break;
      }
      case "5": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isMatching = true;
        break;
      }
      case "6": {
        this.questions[this.selectedQuestionNumber].questionType = new QuestionType();
        this.questions[this.selectedQuestionNumber].questionType.isSpecial = true;
        break;
      }
    }

    console.log(this.questions[this.selectedQuestionNumber]);
  }

  addAnswer() {
    if (this.questions[this.selectedQuestionNumber].questionType.isOne || this.questions[this.selectedQuestionNumber].questionType.isSeveral) {
      this.questions[this.selectedQuestionNumber].answers.push(new Answer());
    } else if (this.questions[this.selectedQuestionNumber].questionType.isOrder) {
      this.questions[this.selectedQuestionNumber].answerOrder.push(new AnswerOrder());
    } else if (this.questions[this.selectedQuestionNumber].questionType.isMatching) {
      this.questions[this.selectedQuestionNumber].answerMatching.push(new AnswerMatching());
    }
  }

  radioButtonClicked(index) {
    for (let i in this.questions[this.selectedQuestionNumber].answers) {
      this.questions[this.selectedQuestionNumber].answers[i].isRight = false;
    }
    this.questions[this.selectedQuestionNumber].answers[index].isRight = true;
  }

  changeQuestion(index) {
    this.selectedQuestionNumber = index;
  }

  deleteQuestion(index) {
    if (this.selectedQuestionNumber === index) {
      this.selectedQuestionNumber = -1;
    }
    this.questions.splice(index, 1);
  }

  orderAnswerClick(index) {
    if (this.questions[this.selectedQuestionNumber].answerOrder[index].number === 0) {
      this.questions[this.selectedQuestionNumber].answerOrder[index].number = this.questions[this.selectedQuestionNumber].orderAnswersIndexer;
      this.questions[this.selectedQuestionNumber].orderAnswersIndexer++;
    } else {
      this.questions[this.selectedQuestionNumber].orderAnswersIndexer = this.questions[this.selectedQuestionNumber].answerOrder[index].number;
      this.questions[this.selectedQuestionNumber].answerOrder[index].number = 0;

      this.questions[this.selectedQuestionNumber].answerOrder.forEach(element => {
        if (element.number >= this.questions[this.selectedQuestionNumber].orderAnswersIndexer)
          element.number = 0;
      });
    }

    console.log(index);
  }

  showQuestions() {
    console.log(this.questions);
    this._location.back();
  }

  getQuestionName(index): string {
    var q = this.questions[index];
    if (!q.body) {
      return "Новый вопрос";
    }
    return q.body.substring(0, 25);
  }

  changeAvailable(index) {
    this.groupModels[index].isAvailable = !this.groupModels[index].isAvailable;
  }
}

export class QuestionType {
  isOne: boolean = false;
  isSeveral: boolean = false;
  isOrder: boolean = false;
  isText: boolean = false;
  isMatching: boolean = false;
  isSpecial: boolean = false;
}

export class AnswerType {
  AnswerTypeID: number;
  Name: string;
}

export class Question {
  id: number = 0;
  body: string;
  numberOfPoints: number = 1; //TODO
  selectedTypeValue: number = 0;
  orderAnswersIndexer = 1;
  questionType: QuestionType = new QuestionType();
  answers: Answer[] = [];
  answerOrder: AnswerOrder[] = [];
  answerMatching: AnswerMatching[] = [];
}

export class Answer {
  id: number = 0;
  body: string;
  isRight: boolean = false;
}

export class AnswerOrder {
  id: number = 0;
  body: string;
  number: number = 0;
}

export class AnswerMatching {
  id: number = 0;
  LeftParth: string;
  RightParth: string;
}

export class TestParameters {
  id: number = 0;
  name: string;
  typeTestID: number = 0;
  subjectID: number = 0;
  time: number;
  icon: any;
}

export class TestType {
  TypeTestID: number;
  Name: string;
}

export class Subject {
  SubjectID: number;
  Name: string;
}

export class GroupModel {
  id: number;
  name: string;
  subjectId: number;
  isAvailable: boolean = false;
}
