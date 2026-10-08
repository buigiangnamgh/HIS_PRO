export class ApiData {
    Success: boolean;
    Param: ParamData;
    Data: any;
}
export class data {
    status: number;
    Param: ParamData;
    data: any;
}
export class ParamData {
    Messages: Array<string>;
    BugCodes: Array<string>;
    Start: number;
    Limit: number;
    Count: number;
    ModuleCode: string;
    HasException: boolean;
}