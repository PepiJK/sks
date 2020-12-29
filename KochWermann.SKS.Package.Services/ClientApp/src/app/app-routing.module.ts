import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {TrackParcelComponent} from './components/track-parcel/track-parcel.component';
import {SubmitParcelComponent} from './components/submit-parcel/submit-parcel.component';
import {ReportComponent} from './components/report/report.component';

const routes: Routes = [
  {path: 'submit-parcel', component: SubmitParcelComponent},
  {path: 'track-parcel', component: TrackParcelComponent},
  {path: 'report', component: ReportComponent},
  {path: '', redirectTo: '/submit-parcel', pathMatch: 'full'},
  {path: '**', redirectTo: '/submit-parcel'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
