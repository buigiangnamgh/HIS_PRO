import { ParamData } from "./apidata.model";

export class AutData {
    Success: boolean;
    Data: TokenData;
    Param: ParamData;
}

export class TokenData {
    ValidAddress: string;
    TokenCode: string;
    RenewCode: string;
    LoginTime: string;
    ExpireTime: string;
    LoginAddress: string;
    User: User;
}

export class User {
    ApplicationCode: string;
    Email: string;
    GCode: string;
    LoginName: string;
    Mobile: string;
    UserName: string;
}