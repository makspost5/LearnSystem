import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SubjectService } from 'src/app/services/subject.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-pupil-block-page',
  templateUrl: './pupil-block-page.component.html',
  styleUrls: ['./pupil-block-page.component.css'],
  providers: [SubjectService]
})
export class PupilBlockPageComponent implements OnInit {

  question: QuestionModel[] = [];
  selectedQuestionIndex: number = -1;
  orderAnswersIndexer: number = 0;
  isQuestion = false;
  isFieldAll = false;

  answers: boolean[] = [];
  answersOrder: number[] = [];
  answersMaching: string[] = [];

  constructor(private activateRoute: ActivatedRoute, private http: SubjectService, private _router: Router, private _location: Location) {
    this.http.controllerGetQuestionsBySectionBlockId(activateRoute.snapshot.params['id']).subscribe((data: Array<Object>) => {
      for (let row of data) {
        let newQuestion: QuestionModel = row as QuestionModel;
        this.question.push(newQuestion);
      }
      console.log(this.question);
      this.selectedQuestionIndex = 0;
      this.createAnswers(this.selectedQuestionIndex);

      let a = 0;

      for (let question of this.question) {
        a++;
        this.http.controllerGetTheoryBody(question.QuestionID).subscribe(data => {
          console.log(data);
          this.question.find(q => q.QuestionID === data["id"]).Theory.Body = data["body"];
          if (a === this.question.length) {
            this.isFieldAll = true;
          }
        });
      }

      console.log(this.question[this.selectedQuestionIndex].AnswerTypeID);
    });
  }

  ngOnInit() {
  }

  actionButtonClick() {
    if (this.isQuestion && !this.checkAnswers(this.selectedQuestionIndex)) {
      alert("Ответ не правильный");
      return;
    }

    if (this.selectedQuestionIndex === this.question.length - 1 && this.isQuestion) {
      this.http.controllerCloseBlock(this.activateRoute.snapshot.params['id']).subscribe(data => {
        console.log("done");
        this._location.back();
      });
    } else {
      this.isQuestion = !this.isQuestion;
      if (!this.isQuestion) {
        this.createAnswers(this.selectedQuestionIndex + 1);
        this.selectedQuestionIndex++;
      }
    }
  }

  checkAnswers(index) {
    console.log(this.question[index].Answer);
    console.log(this.answers);


    switch (this.question[index].AnswerTypeID) {
      case 1:
      case 2:
        for (let i = 0; i < this.question[index].Answer.length; i++) {
          if (this.question[index].Answer[i].IsRight !== this.answers[i]) {
            return false;
          }
        }
        return true;
      case 3:
        for (let i = 0; i < this.question[index].AnswerOrder.length; i++) {
          if (this.question[index].AnswerOrder[i].SerialNumber !== this.answersOrder[i]) {
            return false;
          }
        }
        return true;
      case 5:
        for (let i = 0; i < this.question[index].AnswerMatching.length; i++) {
          if (this.question[index].AnswerMatching[i].RightParth !== this.answersMaching[i]) {
            return false;
          }
        }
        return true;
    }
  }

  createAnswers(i) {
    this.isFieldAll = false;
    switch (this.question[i].AnswerTypeID) {
      case 1:
      case 2:
        this.answers = [];
        for (let answ of this.question[i].Answer) {
          this.answers.push(false);
        }
        break;

      case 3:
        this.answersOrder = [];
        for (let answ of this.question[i].AnswerOrder) {
          this.answersOrder.push(0);
        }
        break;
      case 5:
        this.answersMaching = [];
        for (let answ of this.question[i].AnswerMatching) {
          this.answersMaching.push('');
        }
        break;
    }

    this.isFieldAll = true;
  }

  radioButtonClicked(i) {
    this.createAnswers(this.selectedQuestionIndex);

    this.answers[i] = true;

    console.log(i);

    // for (let index = 0; index < this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult.length; index++)
    // {
    //   if (index === i) {
    //     this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[i].IsSelected = !this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[i].IsSelected;
    //   } else {
    //     this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerResult[index].IsSelected = false;
    //   }
    // }
  }

  orderAnswerClick(i) {
    // if (this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber === 0) {
    //   this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber = this.orderAnswersIndexer;
    //   this.orderAnswersIndexer++;
    // } else {
    //   this.orderAnswersIndexer = this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber;
    //   this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult[i].SelectedSerialNumber = 0;

    //   this.testResult.QuestionResult[this.selectedQuestionIndex].AnswerOrderResult.forEach(element => {
    //     if (element.SelectedSerialNumber >= this.orderAnswersIndexer)
    //       element.SelectedSerialNumber = 0;
    //   });
    // }
  }

  selectQuestion(i, isQuestion) {
    if (this.isQuestion && !this.checkAnswers(this.selectedQuestionIndex)) {
      alert("Ответ не правильный");
      return;
    }

    this.isQuestion = isQuestion;
    this.createAnswers(i);
    this.selectedQuestionIndex = i;
  }
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

export class TheoryModel {
  QuestionID: number;
  Body: string;
  SectionBlockID: number;
  Position: number;
}
