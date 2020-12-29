import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './components/nav/nav.component';
import {MatButtonModule} from '@angular/material/button';
import { TrackParcelComponent } from './components/track-parcel/track-parcel.component';
import { SubmitParcelComponent } from './components/submit-parcel/submit-parcel.component';
import { ReportComponent } from './components/report/report.component';
import {FormsModule} from '@angular/forms';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {HttpClientModule} from '@angular/common/http';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatTableModule} from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    TrackParcelComponent,
    SubmitParcelComponent,
    ReportComponent
  ],
    imports: [
      BrowserModule,
      HttpClientModule,
      AppRoutingModule,
      BrowserAnimationsModule,
      MatButtonModule,
      FormsModule,
      MatFormFieldModule,
      MatInputModule,
      MatProgressSpinnerModule,
      MatTableModule
    ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
