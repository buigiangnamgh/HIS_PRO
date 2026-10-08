import { ApiData } from './apidata.model';

export class Profile {
    ID: number;
    PERSON_CODE: string;
    FIRST_NAME: string;
    LAST_NAME: string;
    NICK_NAME: string;
    DOB: number;
    IS_HAS_NOT_DAY_DOB: number;
    GENDER_ID: number;
    GENDER_NAME: string;
    BLOOD_ABO_CODE: string;
    BLOOD_RH_CODE: string;
    CAREER_NAME: string;
    BORN_PROVINCE_CODE: string;
    BORN_PROVINCE_NAME: string;
    BHYT_NUMBER: string;
    CMND_NUMBER: string;
    CMND_DATE: string;
    CMND_PLACE: string;
    CMND_CCCD_NUMBER: string;
    CMND_CCCD_DATE: string;
    CMND_CCCD_PLACE: string;
    CCCD_NUMBER: string;
    CCCD_DATE: string;
    CCCD_PLACE: string;
    RELATIVE_NAME: string;
    RELATIVE_TYPE: string;
    RELATIVE_CMND_NUMBER: string;
    RELATIVE_ADDRESS: string;
    RELATIVE_MOBILE: string;
    RELATIVE_PHONE: string;
    RELATIVE_NAME_TYPE: string;
    MOTHER_CODE: string;
    MOTHER_NAME: string;
    FATHER_CODE: string;
    FATHER_NAME: string;
    EMAIL: string;
    MOBILE: string;
    PHONE: string;
    NATIONAL_NAME: string;
    ETHNIC_NAME: string;
    RELIGION_NAME: string;
    HOUSEHOLD_CODE: string;
    PROVINCE_NAME: string;
    HOUSEHOLD_RELATION_NAME: string;
    DISTRICT_NAME: string;
    COMMUNE_NAME: string;
    ADDRESS: string;
    HT_PROVINCE_NAME: string;
    HT_DISTRICT_NAME: string;
    HT_COMMUNE_NAME: string;
    HT_ADDRESS: string;
    CARD_CODE: string;
    VIR_ADDRESS: string;
    VIR_PERSON_NAME: string;
    VIR_HT_ADDRESS: string;
    HID_GENDER: string;
}

export class ProfileData extends ApiData {
    Data: Profile;
}

export class ProfileDatas extends ApiData {
    Data: Array<Profile>;
}
