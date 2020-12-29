import {State} from './state';
import {HopArrival} from './hopArrival';

export class TrackingInformation {
  state: State;
  visitedHops: HopArrival[];
  futureHops: HopArrival[];
}
