import { ApiData, data } from "../../model/apidata.model";


export class department {    
    DEPARTMENT_CODE: string; 
    DEPARTMENT_NAME: string;
	EXECUTE_LOGINNAME: string;
	EXECUTE_USERNAME: string;
    AMOUNT: number;
}

export class relativeDepartment {    
    DEPARTMENT_CODE: string; 
    DEPARTMENT_NAME: string;
	Child:Array<department>;
}

export class bacsikhamdt {    
    DATA_DETAIL:
	{
		SINGER_DATA: any;
		Report0:Array<department>;
	};
	
}

export class bacsikhamDatas extends ApiData {
    Data: bacsikhamdt;
}

