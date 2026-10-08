import { ApiData } from './apidata.model';
import { STICKY_DIRECTIONS } from '@angular/cdk/table';

export class SdaProvice {
    APP_CREATOR: string;
    APP_MODIFIER: string;
    CREATE_TIME: number;
    CREATOR: string;
    GROUP_CODE: string;
    ID: number;
    IS_ACTIVE: number;
    IS_DELETE: number;
    MODIFIER: string;
    MODIFY_TIME: number;
    NATIONAL_ID: number;
    PROVINCE_CODE: string;
    PROVINCE_NAME: string;
    SEARCH_CODE: string;
}

export class SdaProviceData extends ApiData {
    Data: SdaProvice;
}

export class SdaProviceDatas extends ApiData {
    Data: Array<SdaProvice>;
}
