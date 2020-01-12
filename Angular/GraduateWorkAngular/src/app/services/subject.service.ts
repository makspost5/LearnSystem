import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { TestParameters, Question, GroupModel } from '../pages/teacher/courses/teacher-test-creation-page/teacher-test-creation-page.component';
import { CourseParameters } from '../pages/teacher/courses/teacher-course-creation-page/teacher-course-creation-page.component';
import { QuestionModel, SubjectSectionModel } from '../pages/teacher/courses/create-subject-section/create-subject-section.component';

@Injectable()
export class SubjectService {

  rootUrl: string = 'http://127.0.0.1/WebApi';
  //'http://192.168.1.223/WebApi'; //guk
  //'http://192.168.0.103/WebApi'; //mtt 
  //'http://192.168.1.102/WebApi'; //mts

  constructor(private http: HttpClient) {

  }

  private token() {
    const headerSettings: { [name: string]: string | string[]; } = {};

    var token = localStorage.getItem('access_token');

    if (token) {
      headerSettings['Authorization'] = 'Bearer ' + token;
    }

    var newHeader = new HttpHeaders(headerSettings);

    return newHeader;
  }

  login(email: string, password: string) {
    let params = new HttpParams()
      .set('grant_type', 'password')
      .set('username', email)
      .set('password', password);

    return this.http.post(this.rootUrl + '/token', params);
  }

  teacherPostTest(testParameters: TestParameters, questions: Question[], groupModels: GroupModel[], mode: string) {
    const data = {
      testParameters,
      questions,
      groupModels,
      mode
    }

    console.log(data);

    return this.http.post(this.rootUrl + '/api/Tests', data, { headers: this.token() });
  }

  pupilSetResult(result) {
    return this.http.post(this.rootUrl + '/api/Results', result, { headers: this.token() });
  }

  teacherPostCourse(courseParameters: CourseParameters, groupModels: GroupModel[]) {
    const data = {
      courseParameters,
      groupModels,
    }

    console.log(data);

    return this.http.post(this.rootUrl + '/api/SubjectCourses', data, { headers: this.token() });
  }

  teacherPostSubjectSection(data: SubjectSectionModel) {

    console.log(data);

    return this.http.post(this.rootUrl + '/api/SubjectSections', data, { headers: this.token() });
  }

  formatHTML(html: string) {
    let params = new HttpParams()
      .set('html', html.replace(/<br>/g, '<br></br>'));

    return this.http.post(this.rootUrl + '/HTMLFormatter', params, { headers: this.token() });
  }

  controllerGetSubjectGroups() {
    return this.http.get(this.rootUrl + '/api/SubjectGroups', { headers: this.token() });
  }

  controllerDeleteTest(id) {
    return this.http.delete(this.rootUrl + '/api/Tests/' + id, { headers: this.token() });
  }

  controllerDeleteCourse(id) {
    return this.http.delete(this.rootUrl + '/api/SubjectCourses/' + id, { headers: this.token() });
  }

  controllerGetAnswerTypes() {
    return this.http.get(this.rootUrl + '/api/AnswerType');
  }

  controllerGetTestTypes() {
    return this.http.get(this.rootUrl + '/api/TestType', { headers: this.token() });
  }

  controllerGetSubjects() {
    return this.http.get(this.rootUrl + '/api/Subject', { headers: this.token() });
  }

  controllerGetTestRows() {
    return this.http.get(this.rootUrl + '/api/Test/TestRows', { headers: this.token() });
  }

  controllerGetPupilTestRows() {
    return this.http.get(this.rootUrl + '/api/Pupil/Test/TestRows', { headers: this.token() });
  }

  controllerGetQuestionsByTestId(id) {
    return this.http.get(this.rootUrl + '/api/QuestionsByTestId/' + id, { headers: this.token() });
  }

  controllerGetQuestionsBySectionBlockId(id) {
    return this.http.get(this.rootUrl + '/api/QuestionsBySectionBlockId/' + id, { headers: this.token() });
  }

  controllerGetTheoryBody(id) {
    return this.http.get(this.rootUrl + '/api/TheoryBody/' + id, { headers: this.token() });
  }

  controllerCloseBlock(id) {
    return this.http.post(this.rootUrl + '/api/Pupil/SubjectCourseFinish/' + id, null, { headers: this.token() });
  }

  controllerGetCourseRows() {
    return this.http.get(this.rootUrl + '/api/Courses/CourseRow', { headers: this.token() });
  }

  controllerGetUpdateTestData(id) {
    return this.http.get(this.rootUrl + '/api/Tests/UpdateData/' + id, { headers: this.token() });
  }

  controllerGetUpdateCourseData(id) {
    return this.http.get(this.rootUrl + '/api/Courses/UpdateData/' + id, { headers: this.token() });
  }

  controllerGetGroups() {
    return this.http.get(this.rootUrl + '/api/Groups');
  }

  controllerGetFaculties() {
    return this.http.get(this.rootUrl + '/api/Faculties');
  }

  controllerGetCourses() {
    return this.http.get(this.rootUrl + '/api/Courses');
  }

  getSubjectCourses() {
    return this.http.get(this.rootUrl + '/api/Pupil/SubjectCourses', { headers: this.token() });
  }

  getSubjectsSections(subjectId) {
    return this.http.get(this.rootUrl + '/api/SubjectSectionsByCourse/' + subjectId, { headers: this.token() });
  }

  getSectionBlocksBySubjectSectionId(subjectSectionId) {
    return this.http.get(this.rootUrl + '/api/SectionBlocks/' + subjectSectionId, { headers: this.token() });
  }

  getTheoryAndQuestionsBySectionBlockIdQuery(sectionBlockId) {
    return this.http.get('/api/SectionBlock/getTheoryAndQuestions/' + sectionBlockId);
  }

  verifyAnswers(ids: number[]) {
    return this.http.post('/api/Answer/verify', ids);
  }

  setSectionBlockPassed(id: number) {
    return this.http.get('/api/SectionBlock/setSectionBlockPassed/' + id).subscribe(data => {
      console.log(data);
    });
  }

  controllerGetTeacherResultsCourse() {
    return this.http.get(this.rootUrl + '/api/TeacherResults/Course', { headers: this.token() });
  }

  controllerGetTeacherResultsTest() {
    return this.http.get(this.rootUrl + '/api/TeacherResults/Test', { headers: this.token() });
  }

  controllerGetTeacherResultsTestByGroup(groupId, testId) {
    return this.http.get(this.rootUrl + '/api/TeacherResults/Test/' + groupId + '/' + testId, { headers: this.token() });
  }

  controllerGetTeacherResultsCourseByGroup(groupId, courseId) {
    return this.http.get(this.rootUrl + '/api/TeacherResults/Course/' + groupId + '/' + courseId, { headers: this.token() });
  }

  controllerGetPupilResults() {
    return this.http.get(this.rootUrl + '/api/PupilResults', { headers: this.token() });
  }
}
