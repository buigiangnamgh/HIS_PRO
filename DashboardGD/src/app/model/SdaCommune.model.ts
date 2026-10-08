import { ApiData } from './apidata.model';
import { STICKY_DIRECTIONS } from '@angular/cdk/table';

export class SdaCommune {
    CREATE_TIME: number;
    CREATOR: string;
    COMMUNE_CODE: string;
    COMMUNE_NAME: string;
    GROUP_CODE: string;
    ID: number;
    INITIAL_NAME: string;
    IS_ACTIVE: number;
    IS_DELETE: number;
    MODIFIER: string;
    MODIFY_TIME: number;
    DISTRICT_ID: number;
}

export class SdaCommuneData extends ApiData {
    Data: SdaCommune;
}

export class SdaCommuneDatas extends ApiData {
    Data: Array<SdaCommune>;
}
