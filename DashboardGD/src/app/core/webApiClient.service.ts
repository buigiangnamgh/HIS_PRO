import { HttpClient, HttpHeaders } from '@angular/common/http';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable, of, throwError as observableThrowError} from 'rxjs';
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/catch';
import {catchError, tap, map} from 'rxjs/operators';
import {LoggerService} from './log/logger.service';
import { Router, ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';

@Injectable()
export class WebApiClientService  {  

    public constructor(private httpClient: HttpClient, private router: Router) {        
    }

    public get<T>(domain: string, url: string, param: string): Observable<T> {
        let encryptParam:string; 
        if(param != null)
            encryptParam = btoa(unescape(encodeURIComponent(param)));   
        return this.httpClient.request<T>("GET", domain + url + '?param=' + encryptParam)
        .pipe(
            map(res => res),
            tap(treatments => LoggerService.log(`fetched get `+ domain + url)),
            catchError(this.handleError(domain + url, {} as T))
        );
    }
    public getparams<T>(domain: string, url: string, param: string): Observable<T> {
        let encryptParam:string; 
        if(param != undefined && param != null)
            encryptParam = '?params=' + btoa(unescape(encodeURIComponent(param)));   
		else
			encryptParam = '';
        return this.httpClient.request<T>("GET", domain + url + encryptParam)
        .pipe(
            map(res => res),
            tap(treatments => LoggerService.log(`fetched get `+ domain + url)),
            catchError(this.handleError(domain + url, {} as T))
        );
    }
    public post<T>(domain: string, url: string, body: any): Observable<T> {
        let data = '';
        if(body == null)
            body = data;
        return this.httpClient.request<T>('POST', domain + url, {body})
        .pipe(
            map(res => res as T), // or any other operator
            tap(treatments => LoggerService.log(`fetched post ` + domain + url)),
            catchError(this.handleError(domain + url, {} as T))
        );
    }

    public put<T>(domain: string, url: string, body: string): Observable<T> {
        let data = '';
        if(body == null)
            body = data;
        return this.httpClient.request<T>('PUT', domain + url, {body})
        .pipe(
            map(res => res as T), // or any other operator
            tap(treatments => LoggerService.log(`fetched put ` + domain + url)),
            catchError(this.handleError(domain + url, {} as T))
        );
    }
    
    public handleError<T>(operation = 'operation', result?: T) {
        return (error: any): Observable<T> => {
    
            const token = localStorage.getItem('tokencode');
            if (token != undefined && token != null && token != '') {
               // TODO: send the error to remote logging infrastructure
                console.error(error); // log to console instead
        
                var stt = error.status; 
                if(stt===401) {
                    alert('Phiên làm việc đã hết hạn, vui lòng đăng nhập lại');
                    localStorage.clear();          
                    this.router.navigate(['/login']);
                }

                // TODO: better job of transforming error for user consumption
                //LoggerService.log(`${operation} failed: ${error.message}`);
            }          
    
            // Let the app keep running by returning an empty result.
            return of(result as T);
        };
    }
}

