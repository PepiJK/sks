import {Component, OnInit} from '@angular/core';
import {Parcel} from '../../models/parcel';
import {ParcelApiService} from '../../services/parcelApi.service';

@Component({
  selector: 'app-submit-parcel',
  templateUrl: './submit-parcel.component.html',
  styleUrls: ['./submit-parcel.component.scss']
})
export class SubmitParcelComponent implements OnInit {
  newParcel: Parcel = new Parcel();
  newTrackingId: string;
  errorMessage: string;
  isLoading = false;

  constructor(private parcelApiService: ParcelApiService) {
  }

  ngOnInit(): void {
  }

  onSubmit(): void {
    this.isLoading = true;
    this.newTrackingId = undefined;
    this.errorMessage = undefined;

    this.parcelApiService.postParcel(this.newParcel).subscribe((res) => {
      this.newTrackingId = res.trackingId;
      this.isLoading = false;
    }, (error) => {
      this.errorMessage = error.error.errorMessage;
      this.isLoading = false;
    });
  }

}
