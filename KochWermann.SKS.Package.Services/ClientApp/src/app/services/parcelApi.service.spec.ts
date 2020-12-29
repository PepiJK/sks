import { TestBed } from '@angular/core/testing';

import { ParcelApiService } from './parcel-api.service';

describe('ApiService', () => {
  let service: ParcelApiService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ParcelApiService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
