import {Recipient} from './recipient';

export class Parcel {
  weight: number;
  recipient: Recipient;
  sender: Recipient;

  constructor() {
    this.recipient = new Recipient();
    this.sender = new Recipient();
  }
}
