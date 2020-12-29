import { Component, OnInit } from '@angular/core';
import {ParcelApiService} from '../../services/parcelApi.service';

@Component({
  selector: 'app-report-parcel',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.scss']
})
export class ReportComponent implements OnInit {
  hopTrackingId: string;
  hopCode: string;
  deliveryTrackingId: string;
  isLoading = false;
  successMessage: string;
  errorMessage: string;

  constructor(private parcelApiService: ParcelApiService) { }

  ngOnInit(): void {
  }

  onSubmitHop(): void {
    this.isLoading = true;
    this.errorMessage = undefined;
    this.successMessage = undefined;

    this.parcelApiService.reportHop(this.hopTrackingId, this.hopCode).subscribe((res) => {
      this.successMessage = res;
      this.isLoading = false;
    }, (error) => {
      this.errorMessage = (JSON.parse(error.error)).errorMessage;
      this.isLoading = false;
    });
  }

  onSubmitDelivery(): void {
    this.isLoading = true;
    this.errorMessage = undefined;
    this.successMessage = undefined;

    this.parcelApiService.reportDelivery(this.deliveryTrackingId).subscribe((res) => {
      this.successMessage = res;
      this.isLoading = false;
    }, (error) => {
      this.errorMessage = (JSON.parse(error.error)).errorMessage;
      this.isLoading = false;
    });
  }

}
