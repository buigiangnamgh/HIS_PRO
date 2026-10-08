import { Injectable } from '@angular/core';
import { NgModule } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpEventType } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import {AppConfig} from './config/app.config';

export class AuthInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    console.debug('Before____'+ JSON.stringify(req));

    var token = localStorage.getItem('tokencode');

    if (token) {
      console.debug('has token');
      localStorage.setItem('loginAuthData', null);
        req = req.clone({
          setHeaders: { 
              'Content-Type': 'application/json; charset=UTF-8',
              'TokenCode': token,
              'ApplicationCode': AppConfig.common.AppLICATION_CODE,
              'Accept': 'application/json, text/plain, */*',
			  'Access-Control-Allow-Origin': '*',
             }
        });
    }    

    if (!req.headers.has('Content-Type')) {
        req = req.clone({ headers: req.headers.set('Content-Type', 'application/json; charset=UTF-8') });
    }

    req = req.clone({ headers: req.headers.set('Accept', 'application/json') });

    if(req.url.indexOf('/api/Token/Login')> -1){
      var loginAuthData = localStorage.getItem('loginAuthData');
      req = req.clone({
        setHeaders: { 
            'Content-Type': 'application/json; charset=UTF-8',
            'Authorization': 'Basic ' + loginAuthData,        
            'Accept': 'application/json, text/plain, */*',
			'Access-Control-Allow-Origin': '*',
          }
      });
    }
    else if(req.url.indexOf('/api/Token/GetAuthenticated')> -1)
    {
      req = req.clone({
        setHeaders: { 
            'Content-Type': 'application/json; charset=UTF-8',
            'TokenCode': token,
            'ApplicationCode': AppConfig.common.AppLICATION_CODE,
            'Accept': 'application/json, text/plain, */*',
			'Access-Control-Allow-Origin': '*',
           }
      });
    }
	// else if(req.url.indexOf('/dkna.vn/api')> -1)
    // {		
        // req = req.clone({
            // setHeaders: { 
				 // 'Content-Type': 'application/json',            
				 // 'Accept': 'application/json, text/plain, */*',
				 // 'Access-Control-Allow-Origin': '*',
				 // 'Access-Control-Allow-Methods': 'GET, POST',
				 // 'Access-Control-Allow-Headers': 'Origin, X-Requested-With',
            // }
        // });
    // }

    console.debug('After____'+ JSON.stringify(req));

    // if(token===undefined || token===null)
    //     token='94b0105163670140eb77539fe8e01f08cda9da7f6e9f9355fe02282fe12c494c74d9cea63c2a0612e752675e4abd58d866b58824323ec65f97eb7c0dd013f734';
    // const mathe = localStorage.getItem('mathe');
    // const macanhan = localStorage.getItem('macanhan');
    // let param = '{   "ApiData": { CARD_CODE: ' + '\'' + mathe + '\' } }';
    // if (mathe == null || mathe == ''){
    //     param = '{   "ApiData": { PERSON_CODE: ' + '\'' + macanhan + '\' } }';
    // }       
    
    // const requestOptions = {
    //     params: new HttpParams()
    //   };
    //   requestOptions.params.set('Content-Type', 'application/json');
    //   requestOptions.params.set('TokenCode', token);

    // const authReq = req.clone({
    //   setHeaders: { 
    //       'Content-Type': 'application/json; charset=UTF-8',
    //       'TokenCode': token,
    //       'ApplicationCode': 'HSK',
    //       'Accept': 'application/json, text/plain, */*',
    //      }
    // });
    return next.handle(req);
  }

}