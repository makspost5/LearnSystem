import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule }   from '@angular/common/http';

import { RouterModule, Routes} from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { ReactiveFormsModule } from '@angular/forms';
import { MainTeacherPageComponent } from './pages/teacher/main-teacher-page/main-teacher-page.component';
import { TeacherGuard } from './services/TeacherGuard.service';
import { TeacherCoursesPageComponent } from './pages/teacher/courses/teacher-courses-page/teacher-courses-page.component';
import { TeacherTestsPageComponent } from './pages/teacher/courses/teacher-tests-page/teacher-tests-page.component';
import { TeacherTestCreationPageComponent } from './pages/teacher/courses/teacher-test-creation-page/teacher-test-creation-page.component'
import { FormsModule } from '@angular/forms';
import { TeacherCourseCreationPageComponent } from './pages/teacher/courses/teacher-course-creation-page/teacher-course-creation-page.component';
import { CreateSubjectSectionComponent } from './pages/teacher/courses/create-subject-section/create-subject-section.component';
import { SafePipe } from './safe.pipe';
import { MainPupilPageComponent } from './pages/pupil/main-pupil-page/main-pupil-page.component';
import { TestsPageComponent } from './pages/pupil/tests/tests-page/tests-page.component';
import { CurrentTestPageComponent } from './pages/pupil/tests/current-test-page/current-test-page.component';
import { PupilGuard } from './services/PupilGuard';
import { AdminGuard } from './services/AdminGuard';
import { PupilAllCoursesComponent } from './pages/pupil/course/pupil-all-courses/pupil-all-courses.component';
import { PupilCoursePageComponent } from './pages/pupil/course/pupil-course-page/pupil-course-page.component';
import { PupilSectionPageComponent } from './pages/pupil/course/pupil-section-page/pupil-section-page.component';
import { PupilBlockPageComponent } from './pages/pupil/course/pupil-block-page/pupil-block-page.component';
import { AllResultsComponent } from './pages/teacher/results/all-results/all-results.component';
import { ResultsComponent } from './pages/pupil/results/results.component';
import { MainPageComponent } from './pages/admin/main-page/main-page.component';

const appRout: Routes = [
    {path: '', component: LoginPageComponent},
    {path: 'teacher-page', component: MainTeacherPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/all-courses', component: TeacherCoursesPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/tests', component: TeacherTestsPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/create-test', component: TeacherTestCreationPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/update-test/:id', component: TeacherTestCreationPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/update-course/:id', component: TeacherCourseCreationPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/create-course', component: TeacherCourseCreationPageComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/create-course/:courseId/create-section/:level', component: CreateSubjectSectionComponent, canActivate: [TeacherGuard]},
    {path: 'teacher-page/results', component: AllResultsComponent, canActivate: [TeacherGuard]},
    {path: 'pupil-page', component: MainPupilPageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/tests', component: TestsPageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/test/:id', component: CurrentTestPageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/courses/:id', component: PupilCoursePageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/courses/:courseId/section/:id', component: PupilSectionPageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/courses/:courseId/section/:sectionId/sectionblock/:id', component: PupilBlockPageComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/results', component: ResultsComponent, canActivate: [PupilGuard]},
    {path: 'pupil-page/courses', component: PupilAllCoursesComponent, canActivate: [PupilGuard]},
    {path: 'admin', component: MainPageComponent, canActivate: [TeacherGuard]}
];

@NgModule({
  declarations: [
    AppComponent,
    LoginPageComponent,
    MainTeacherPageComponent,
    TeacherCoursesPageComponent,
    TeacherTestsPageComponent,
    TeacherTestCreationPageComponent,
    TeacherCourseCreationPageComponent,
    CreateSubjectSectionComponent,
    SafePipe,
    MainPupilPageComponent,
    TestsPageComponent,
    CurrentTestPageComponent,
    PupilAllCoursesComponent,
    PupilCoursePageComponent,
    PupilSectionPageComponent,
    PupilBlockPageComponent,
    AllResultsComponent,
    ResultsComponent,
    MainPageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    RouterModule.forRoot(appRout),
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule
  ],
  providers: [TeacherGuard, PupilGuard, AdminGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
