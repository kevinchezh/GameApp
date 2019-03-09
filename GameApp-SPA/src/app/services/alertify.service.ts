import { Injectable } from '@angular/core';
declare let alertify: any;
@Injectable({
  providedIn: 'root'
})
export class AlertifyService {

constructor() { }

  confirm(message: string, okCallback: () => any) {
    // this is from docs, confirm function could take in a message and a function to
    // say what to show at confirm pop out
    alertify.confirm(message, function(e) {
      if (e) {
        okCallback();
      } else {}
    });
  }
  success(message: string) {
    alertify.success(message);
  }
  error(message: string) {
    alertify.error(message);
  }
  warning(message: string) {
    alertify.warning(message);
  }
  message(message: string) {
    alertify.message(message);
  }
}
