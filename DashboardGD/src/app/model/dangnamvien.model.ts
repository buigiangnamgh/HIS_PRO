import { ApiData } from './apidata.model';

export class DangNamVien {
    TREATMENT_CODE: string;
    GENDER_ID: number;
    BRANCH_ID: number;
    DEPARTMENT_ID: number;
    DEPARTMENT_CODE: string;
    DEPARTMENT_NAME: string;
    COUNT_OLD: number;
    COUNT_OLD_FEMALE: number;
    COUNT_OLD_BHYT: number;
    COUNT_OLD_LESS6: number;
    COUNT_OLD_MORE: number;
    NUM_BED: number;
    COUNT_NEW: number;
    COUNT_NEW_FROM_KKB: number;
    COUNT_EXAM: number;
    COUNT_CLINICAL_IN: number;
    COUNT_NEW_TE: number;
    COUNT_NEW_LESS6: number;
    COUNT_NEW_MORE6number: number;
    COUNT_NEW_EMERGENCY: number;
    COUNT_NEW_BHYT: number;
    COUNT_NEW_EMERGENCY_BHYT: number;
    COUNT_ALL_BHYT: number;
    DAY: number;
    DAY1: number;
    COUNT_EXP: number;
    COUNT_OUT_KHOI: number;
    COUNT_OUT_DO: number;
    COUNT_OUT_KTD: number;
    COUNT_OUT_NANG: number;
    COUNT_OUT_NANG_CHET: number;
    COUNT_OUT_CV: number;
    COUNT_OUT_XINRAVIEN: number;
    COUNT_OUT_TRON: number;
    COUNT_OUT_RAVIEN: number;
    COUNT_OUT_KHAC: number;
    COUNT_OUT_HEN: number;
    COUNT_OUT_CTCV: number;
    COUNT_OUT_CV_LEN: number;
    COUNT_DIE: number;
    COUNT_DIE_TE: number;
    COUNT_DIE_LESS5: number;
    COUNT_DIE_LESS1: number;
    COUNT_DIE_24: number;
    COUNT_END: number;
    COUNT_MOVE: number;
    POINT_DAY: number;
    REALITY_PATIENT_BED: number;
    THEORY_PATIENT_BED: number;
    COUNT_CHDP_IMP: number;
    TREATMENT_DAY_COUNT: number;
    COUNT_TOTAL_DATE_TREATMENT: number;
    COUNT_NEW_FROM_CC: number;
    COUNT_NEW_FROM_CC_BHYT: number;
    COUNT_NEW_FROM_C: number;
    COUNT_NEW_FROM_S: number;
    COUNT_NEW_FROM_S_BHYT: number;
    COUNT_NEW_FROM_C_BHYT: number;
    COUNT_DEATH_INFO: number;
    COUNT_DEATH_INFO_TE: number;
    COUNT_DEATH_INFO_24: number;
    COUNT_NEW_FEMALE: number;
    COUNT_EXP_BHYT: number;
    COUNT_EXP_FEMALE: number;
    COUNT_OUT_CV_BHYT: number;
    COUNT_OUT_CV_FEMALE: number;
    COUNT_OUT_TRON_FEMALE: number;
    COUNT_OUT_TRON_BHYT: number;
    COUNT_OUT_NANG_CHET_BHYT: number;
    COUNT_OUT_NANG_CHET_FEMALE: number;
    COUNT_END_FEMALE: number;
    COUNT_END_BHYT: number;
    CLINICAL_IN_TIME: string;
    IN_DATE: number;
    NEXT_DEPARTMENT_IN_TIME: string;
    DEPARTMENT_IN_TIME: string;
    OUT_TIME: string;
    TDL_PATIENT_TYPE_ID: string;
    TDL_PATIENT_DOB: number;
    TREATMENT_RESULT_ID: string;
    TREATMENT_END_TYPE_ID: string;
    NEXT_ID: number;
    ORDER: string;
}

export class DangNamVienDt extends ApiData {
    DATA_DETAIL:
	{
		Report:	Array<DangNamVien>;  
		SINGER_DATA: any;
	};
}

export class DangNamVienDatas extends ApiData {
    Data: DangNamVienDt;
}
