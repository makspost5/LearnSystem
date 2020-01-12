import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubjectService } from 'src/app/services/subject.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-current-test-page',
  templateUrl: './current-test-page.component.html',
  styleUrls: ['./current-test-page.component.css'],
  providers: [SubjectService]
})
export class CurrentTestPageComponent implements OnInit {

  question: QuestionModel[] = [];
  selectedQuestionIndex: number = -1;
  testResult: Result = new Result();
  isQuestionFulled = false;
  orderAnswersIndexer: number = 0;

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService, private _router: Router, private _location: Location) {
    this.testResult.TestID = activateRoute.snapshot.params['id'];

    this.http.controllerGetQuestionsByTestId(this.testResult.TestID).subscribe((data: Array<Object>) => {
      for (let row of data) {
        let newQuestion: QuestionModel = row as QuestionModel;
        this.question.push(newQuestion);
      }
      console.log(this.question);
      this.selectedQuestionIndex = 0;
      for (let i = 0; i < this.question.length; i++){
        this.createQuestionResult(i);
      }

      console.log(this.question[this.selectedQuestionIndex].AnswerTypeID);
    });
  }

  ngOnInit() {
  }

  actionButtonClick() {
    if (this.selectedQuestionIndex + 1 === this.question.length) {
      this.http.pupilSetResult(this.testResult).subscribe(data => {
        console.log(data);
        this._location.back();
      });
    } else {
      this.selectedQuestionIndex++;
    }
    console.log(this.testResult);
  }

  private createQuestionResult(index) {
    this.orderAnswersIndexer = 0;
    var questionResult = new QuestionResult();
    questionResult.QuestionID = this.question[index].QuestionID;

    switch (this.question[index].AnswerTypeID) {
      case 1:
        for (let answer of this.question[index].Answer) {
          var answerResult = new AnswerResult();
          answerResult.IsSelected = false;
          answerResult.AnswerID = answer.AnswerID;
          questionResult.AnswerResult.push(answerResult);
        }
        break;
      case 2:
        for (let answer of this.question[index].Answer) {
          var answerResult = new AnswerResult();
          answerResult.IsSelected = false;
          answerResult.AnswerID = answer.AnswerID;
          questionResult.AnswerResult.push(answerResult);
        }
        break;
      case 3:
        for (let answer of this.question[index].AnswerOrder) {
          var answerOrderResult = new AnswerOrderResult();
          answerOrderResult.SelectedSerialNumber = 0;
          answerOrderResult.AnswerOrderID = answer.AnswerOrderID;
          questionResult.AnswerOrderResult.push(answerOrderResult);
        }
        break;
      case 5:
        for (let answer of this.question[index].AnswerMatching) {
          var answerMatchingResult = new AnswerMatchingResult();
          answerMatchingResult.SelectedLeftParth = answer.LeftParth;
          answerMatchingResult.SelectedRightParth = '';
          answerMatchingResult.AnswerMatchingID = answer.AnswerMatchingID;
          questionResult.AnswerMatchingResult.push(answerMatchingResult);
        }
        break;
      case 6:
        questionResult.CustomAnswer.push(new CustomAnswer());
        break;
    }

    this.testResult.QuestionResult.push(questionResult);
    this.isQuestionFulled = true;
  }

  radioButtonClicked(i) {
    console.log(i);

    for (let index = 0; index < this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult.length; index++)
    {
      if (index === i) {
        this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[i].IsSelected = !this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[i].IsSelected;
      } else {
        this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[index].IsSelected = false;
      }
    }
  }

  orderAnswerClick(i) {
    if (this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber === 0) {
      this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber = this.orderAnswersIndexer;
      this.orderAnswersIndexer++;
    } else {
      this.orderAnswersIndexer = this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber;
      this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber = 0;

      this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult.forEach(element => {
        if (element.SelectedSerialNumber >= this.orderAnswersIndexer)
          element.SelectedSerialNumber = 0;
      });
    }
  }

  selectQuestion(i){
    this.selectedQuestionIndex = i;
  }
}

export class QuestionModel {
  QuestionID: number = 0;
  Body: string;
  AnswerTypeID: number;
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

export class AnswerMatchingResult {
  AnswerMatchingID: number;
  SelectedLeftParth: string;
  SelectedRightParth: string;
}

export class AnswerOrderResult {
  AnswerOrderID: number;
  SelectedSerialNumber: number = 0;
}

export class AnswerResult {
  AnswerID: number;
  IsSelected: boolean;
}

export class QuestionResult {
  CustomAnswer: CustomAnswer [] = [];
  AnswerResult: AnswerResult[] = [];
  QuestionID: number;
  AnswerMatchingResult: AnswerMatchingResult[] = [];
  AnswerOrderResult: AnswerOrderResult[] = [];
}

export class CustomAnswer {
  Body: string;
}

export class Result {
  QuestionResult: QuestionResult[] = [];
  TestID: number;
}
