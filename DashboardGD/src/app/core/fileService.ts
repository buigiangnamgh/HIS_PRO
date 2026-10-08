import { Injectable } from '@angular/core';
import { RequestOptions, ResponseContentType } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { InterceptorService } from 'ng2-interceptors';

import { getFileNameFromResponseContentDisposition, saveFile } from './file-download-helper';

@Injectable()
export class FileService {
    constructor(
        private http: InterceptorService,
    ) {}

    downloadFile(url: string) {       
        const options = new RequestOptions({responseType: ResponseContentType.Blob });

        // Process the file downloaded
        this.http.get(url, options).subscribe(res => {
            const fileName = getFileNameFromResponseContentDisposition(res);
            saveFile(res.blob(), fileName);
        });
    }
}