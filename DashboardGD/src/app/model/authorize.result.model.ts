import { AuthorizeControlModel } from "./authorize.control.model";
import { AuthorizeModuleModel } from "./authorize.module.model";
import { ApiData } from "./apidata.model";

export class AuthorizeResultModel {
    ControlInRoles: Array<AuthorizeControlModel>;
    ModuleInRoles: Array<AuthorizeModuleModel>;
}

export class AuthorizeApiResult extends ApiData {
    Data: AuthorizeResultModel;
}