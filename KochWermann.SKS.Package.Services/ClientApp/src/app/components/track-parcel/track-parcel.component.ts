import { Component, OnInit } from '@angular/core';
import {ParcelApiService} from '../../services/parcelApi.service';
import {TrackingInformation} from '../../models/trackingInformation';

@Component({
  selector: 'app-track-parcel',
  templateUrl: './track-parcel.component.html',
  styleUrls: ['./track-parcel.component.scss']
})
export class TrackParcelComponent implements OnInit {
  trackingId: string;
  isLoading = false;
  errorMessage: string;
  trackingInfo: TrackingInformation;
  displayedColumns = ['id', 'code', 'description', 'dateTime'];

  constructor(private parcelApiService: ParcelApiService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.isLoading = true;
    this.trackingInfo = undefined;
    this.errorMessage = undefined;

    this.parcelApiService.getTrackingInformation(this.trackingId).subscribe((res) => {
      this.trackingInfo = res;
      this.isLoading = false;
      console.log(res);
    }, (error) => {
      this.errorMessage = error.error.errorMessage;
      this.isLoading = false;
    });
  }

}
