import { ApiData, data } from "../../model/apidata.model";

export class khambenh {    
    EXECUTE_DEPARTMENT_NAME: string;
	EXECUTE_ROOM_NAME: string;
    COUNT_EXAM: number;
    COUNT_EXAM_BHYT: number;
    COUNT_TREATMENT: number;
    COUNT_TREATMENT_BHYT: number;
    COUNT_TREATMENT_IN: number;
    COUNT_TREATMENT_IN_BHYT: number;
	COUNT_DONE_DIV: number;
	COUNT_DONE_EXAM: number;
}

export class department {    
    EXECUTE_DEPARTMENT_NAME: string;
	EXECUTE_ROOM_NAME: string;
    COUNT_EXAM: number;
    COUNT_EXAM_BHYT: number;
    COUNT_TREATMENT: number;
    COUNT_TREATMENT_BHYT: number;
    COUNT_TREATMENT_IN: number;
    COUNT_TREATMENT_IN_BHYT: number;
	COUNT_DONE_DIV: number;
	COUNT_DONE_EXAM: number;
}

export class khambenhdt {    
    DATA_DETAIL:
	{
		Report:	Array<khambenh>;  
		SINGER_DATA: any;
		Department:Array<department>;
	};
	
}

export class khambenhDatas extends ApiData {
    Data: khambenhdt;
}

