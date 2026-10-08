import { ApiData } from './apidata.model';
import { STICKY_DIRECTIONS } from '@angular/cdk/table';

export class SdaDistrict {
    CREATE_TIME: number;
    CREATOR: string;
    DISTRICT_CODE: string;
    DISTRICT_NAME: string;
    GROUP_CODE: string;
    ID: number;
    INITIAL_NAME: string;
    IS_ACTIVE: number;
    IS_DELETE: number;
    MODIFIER: string;
    MODIFY_TIME: number;
    PROVINCE_ID: number;
}

export class SdaDistrictData extends ApiData {
    Data: SdaDistrict;
}

export class SdaDistrictDatas extends ApiData {
    Data: Array<SdaDistrict>;
}
