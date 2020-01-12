import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SubjectService } from 'src/app/services/subject.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import {Location} from '@angular/common';

@Component({
  selector: 'app-create-subject-section',
  templateUrl: './create-subject-section.component.html',
  styleUrls: ['./create-subject-section.component.css'],
  providers: [SubjectService]
})
export class CreateSubjectSectionComponent implements OnInit {

  private startId: number = -1;

  answerTypes: AnswerType[] = [];

  @ViewChild('theoryInput') divContainer: ElementRef;

  selectedBlockNumber: number = -1;
  subjectSectionParameters: SubjectSectionParameters;

  sectionBlocks: SectionBlock[];

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService, private _router: Router, private _location: Location) {
    this.subjectSectionParameters = new SubjectSectionParameters();
    this.sectionBlocks = [];
    var id = activateRoute.snapshot.params['id'];

    if (id) {
      // this.startId = id;

      // this.http.controllerGetUpdateTestData(id).subscribe(data => {
      //   this.testParameters = data['testParameters'] as TestParameters;
      //   for (let question of data['questions']) {
      //     let newQuestion: Question = question as Question;

      //     this.questions.push(newQuestion);
      //   }

      //   for (let i = 0; i < this.questions.length; i++) {
      //     this.selectedQuestionNumber = i;
      //     this.questiontTypeChange(this.questions[i].selectedTypeValue.toString(), false);
      //   }

      //   console.log(data);
      //   for (let group of data['groupModels']) {
      //     let newGroupModel: GroupModel = group as GroupModel;
      //     this.groupModels.push(newGroupModel);
      //   }
      //   console.log(this.groupModels);

      //   this.selectedQuestionNumber = -1;
      // })
    } else {
      this.subjectSectionParameters.SubjectCourseID = activateRoute.snapshot.params['courseId'];
      this.subjectSectionParameters.HierarchyLevel = activateRoute.snapshot.params['level'];
    }

    this.selectedBlockNumber = -1;

    this.http.controllerGetAnswerTypes().subscribe((data: Array<Object>) => {
      for (let answerType of data) {
        let newAnswerType: AnswerType = answerType as AnswerType;
        if (newAnswerType.AnswerTypeID !== 6) {
          this.answerTypes.push(newAnswerType);
        }
      }
      console.log(this.answerTypes);
    });
  }

  addBlock() {
    this.sectionBlocks.push(new SectionBlock());
    this.selectedBlockNumber = this.sectionBlocks.length - 1;

    console.log("Добавлен блок " + this.selectedBlockNumber);
  }

  addQuestion() {
    this.sectionBlocks[this.selectedBlockNumber].Questions.push(new Question);
    this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber = this.sectionBlocks[this.selectedBlockNumber].Questions.length - 1;

    console.log("Добавлен вопрос " + this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber);
  }

  saveSection() {
    this.http.teacherPostSubjectSection(this.subjectSectionParameters.toModel(this.sectionBlocks)).subscribe(data => {
      console.log(data);
      this._router.navigate(['teacher-page/update-course/' + this.subjectSectionParameters.SubjectCourseID]);
    });

    // var mode = '';
    // if (this.startId === -1) {
    //   mode = 'create';
    // } else {
    //   mode = 'update';
    // }

    // this.http.teacherPostTest(this.testParameters, this.questions, this.groupModels, mode).subscribe(data => {
    //   console.log(data);
    //   this._router.navigate(['teacher-page/tests']);
    // });

    // // TODO
    // console.log(this.questions);
  }

  ngOnInit() {
  }

  questiontTypeChange(typeValue, mode = true) {
    if (mode) {
      for (let i in this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers) {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers[i].isRight = false;
      }
    }

    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].selectedTypeValue = typeValue;
    switch (typeValue) {
      case "1": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isOne = true;
        break;
      }
      case "2": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isSeveral = true;
        break;
      }
      case "3": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isOrder = true;
        break;
      }
      case "4": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isText = true;
        break;
      }
      case "5": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isMatching = true;
        break;
      }
      case "6": {
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType = new QuestionType();
        this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isSpecial = true;
        break;
      }
    }

    console.log(this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber]);
  }

  addAnswer() {
    if (this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isOne || this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isSeveral) {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers.push(new Answer());
    } else if (this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isOrder) {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder.push(new AnswerOrder());
    } else if (this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].questionType.isMatching) {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerMatching.push(new AnswerMatching());
    }
  }

  radioButtonClicked(index) {
    for (let i in this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers) {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers[i].isRight = false;
    }
    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answers[index].isRight = true;
  }

  changeBlock(index) {
    if (index !== -1) {
      this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber = 0;
    }

    this.selectedBlockNumber = index;
  }

  changeQuestion(index) {
    this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber = index;
  }

  deleteQuestion(index) {
    if (this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber === index) {
      this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber = -1;
    }
    this.sectionBlocks[this.selectedBlockNumber].Questions.splice(index, 1);
  }

  deleteBlock(index) {
    if (this.selectedBlockNumber === index) {
      this.selectedBlockNumber = -1;
    }
    this.sectionBlocks.splice(index, 1);
  }

  orderAnswerClick(index) {
    if (this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder[index].number === 0) {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder[index].number = this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].orderAnswersIndexer;
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].orderAnswersIndexer++;
    } else {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].orderAnswersIndexer = this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder[index].number;
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder[index].number = 0;

      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].answerOrder.forEach(element => {
        if (element.number >= this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].orderAnswersIndexer)
          element.number = 0;
      });
    }

    console.log(index);
  }

  showBlocks() {
    console.log(this.sectionBlocks);
    this._location.back();
  }

  getQuestionName(index): string {
    var q = this.sectionBlocks[this.selectedBlockNumber].Questions[index];
    if (!q.body) {
      return "Новый вопрос";
    }
    return q.body.substring(0, 25);
  }

  changeCreationMode(mode) {
    this.sectionBlocks[this.selectedBlockNumber].CreationMode = mode;
  }

  inputTheory(innerHTML : string) {
    innerHTML = innerHTML.replace(/  /g, '&nbsp;&nbsp;');
    innerHTML = innerHTML.replace(/ <span/g, '<span>&nbsp;</span><span');
    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.Body = innerHTML;
    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.BodyString = innerHTML;
  }

  format() {
    this.http.formatHTML(this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.Body).subscribe((data: string) => {
      this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.BodyString = data;
      this.inputTheoryString(data);
    });
  }

  inputTheoryString(innerText) {
    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.BodyString = innerText;
    this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.Body = innerText.replace(/\n/g, '').replace(/\r/g, '').replace(/\t/g, '');
    this.fullInnerHtml();
  }

  fullInnerHtml() {
    this.divContainer.nativeElement.innerHTML = this.sectionBlocks[this.selectedBlockNumber].Questions[this.sectionBlocks[this.selectedBlockNumber].SelectedQuestionNumber].theory.Body;
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
  selectedTypeValue: number = 0;
  orderAnswersIndexer = 1;
  questionType: QuestionType = new QuestionType();
  answers: Answer[] = [];
  answerOrder: AnswerOrder[] = [];
  answerMatching: AnswerMatching[] = [];
  theory: Theory = new Theory();

  public toModel(position): any {
    var questionModel = new QuestionModel();

    questionModel.Body = this.body;
    questionModel.AnswerTypeID = this.selectedTypeValue;
    questionModel.QuestionID = this.id;
    questionModel.Theory = this.theory.toModel(position, this.id);

    questionModel.Answer = [];
    this.answers.forEach(answer => {
      questionModel.Answer.push(answer.toModel(this.id));
    });

    questionModel.AnswerOrder = [];
    this.answerOrder.forEach(answer => {
      questionModel.AnswerOrder.push(answer.toModel(this.id));
    });

    questionModel.AnswerMatching = [];
    this.answerMatching.forEach(answer => {
      questionModel.AnswerMatching.push(answer.toModel(this.id));
    });

    return questionModel;
  }
}

export class Answer {
  id: number = 0;
  body: string;
  isRight: boolean = false;

  public toModel(questionId) {
    var answerModel = new AnswerModel();

    answerModel.AnswerID = this.id;
    answerModel.IsRight = this.isRight;
    answerModel.Body = this.body;
    answerModel.QuestionID = questionId;

    return answerModel;
  }
}

export class AnswerOrder {
  id: number = 0;
  body: string;
  number: number = 0;

  public toModel(questionId): any {
    var answerOrderModel = new AnswerOrderModel();

    answerOrderModel.QuestionID = questionId;
    answerOrderModel.Body = this.body;
    answerOrderModel.AnswerOrderID = this.id;
    answerOrderModel.SerialNumber = this.number;

    return answerOrderModel;
  }
}

export class AnswerMatching {
  id: number = 0;
  LeftParth: string;
  RightParth: string;

  public toModel(questionId) {
    var answerMatchingModel = new AnswerMatchingModel();

    answerMatchingModel.QuestionID = questionId;
    answerMatchingModel.LeftParth = this.LeftParth;
    answerMatchingModel.RightParth = this.RightParth;
    answerMatchingModel.AnswerMatchingID = this.id;

    return answerMatchingModel;
  }
}

export class SubjectSectionParameters {
  SubjectSectionID: number = 0;
  Name: string;
  SubjectCourseID: number;
  HierarchyLevel: number;
  image: any;//TODO

  public toModel(sectionBlocks: SectionBlock[]): any {
    var subjectSectionModel = new SubjectSectionModel();

    subjectSectionModel.HierarchyLevel = this.HierarchyLevel;
    subjectSectionModel.Name = this.Name;
    subjectSectionModel.SubjectCourseID = this.SubjectCourseID;
    subjectSectionModel.SubjectSectionID = this.SubjectSectionID;
    subjectSectionModel.image = this.image;

    subjectSectionModel.SectionBlock = [];
    for (let i = 0; i < sectionBlocks.length; i++) {
      subjectSectionModel.SectionBlock.push(sectionBlocks[i].toModel(i, this.SubjectSectionID));
    }

    return subjectSectionModel;
  }
}

export class SectionBlock {
  SectionBlockID: number = 0;
  SubjectSectionID: number;
  Name: string;
  Position: number;
  Questions: Question[] = [];
  SelectedQuestionNumber: number = -1;
  CreationMode: boolean = true;
  isDesigner: boolean = true;

  public toModel(position, subjectSectionID): any {
    var sectionBlockModel = new SectionBlockModel();

    sectionBlockModel.SectionBlockID = this.SectionBlockID;
    sectionBlockModel.Name = this.Name;
    sectionBlockModel.Position = position;
    sectionBlockModel.SubjectSectionID = subjectSectionID;

    sectionBlockModel.Question = [];
    for (let i = 0; i < this.Questions.length; i++) {
      sectionBlockModel.Question.push(this.Questions[i].toModel(i));
    }

    return sectionBlockModel;
  }
}

export class Theory {
  Body: string = '';
  BodyString: string = '';
  SectionBlockID: number;

  public toModel(position, questionId): any {
    var theoryModel = new TheoryModel();

    theoryModel.Body = this.Body;
    theoryModel.SectionBlockID = this.SectionBlockID;
    theoryModel.Position = position;
    theoryModel.QuestionID = questionId;

    return theoryModel;
  }
}


/*MODELS*/

export class TheoryModel {
  Body: string;
  SectionBlockID: number;
  Position: number;
  QuestionID: number;
}

export class QuestionModel {
  QuestionID: number = 0;
  Body: string;
  AnswerTypeID: number;
  Theory: TheoryModel;
  Answer: AnswerModel[];
  AnswerOrder: AnswerOrderModel[];
  AnswerMatching: AnswerMatchingModel[];
}

export class AnswerModel {
  AnswerID: number;
  Body: string;
  IsRight: boolean;
  QuestionID: number;
}

export class AnswerOrderModel {
  AnswerOrderID: number;
  SerialNumber: number;
  QuestionID: number;
  Body: string;
}

export class AnswerMatchingModel {
  AnswerMatchingID: number;
  LeftParth: string;
  RightParth: string;
  QuestionID: number;
}

export class SectionBlockModel {
  SectionBlockID: number;
  Name: string;
  Position: number;
  SubjectSectionID: number;

  Question: QuestionModel[];
}

export class SubjectSectionModel {
  SubjectSectionID: number;
  Name: string;
  image: string; //TODO
  SubjectCourseID: number;
  HierarchyLevel: number;
  SectionBlock: SectionBlockModel[];
}
