import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Parcel} from '../models/parcel';
import {Observable} from 'rxjs';
import {NewParcelInfo} from '../models/newParcelInfo';
import {TrackingInformation} from '../models/trackingInformation';

@Injectable({
  providedIn: 'root'
})
export class ParcelApiService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  postParcel(parcel: Parcel): Observable<NewParcelInfo> {
    return this.http.post<NewParcelInfo>(`${this.apiUrl}/parcel`, parcel);
  }

  getTrackingInformation(trackingId: string): Observable<TrackingInformation> {
    return this.http.get<TrackingInformation>(`${this.apiUrl}/parcel/${trackingId}`);
  }

  reportDelivery(trackingId: string): Observable<string> {
    return this.http.post(`${this.apiUrl}/parcel/${trackingId}/reportDelivery`, null, {responseType: 'text'});
  }

  reportHop(trackingId: string, hopCode: string): Observable<string> {
    return this.http.post(`${this.apiUrl}/parcel/${trackingId}/reportHop/${hopCode}`, null, {responseType: 'text'});
  }

}
