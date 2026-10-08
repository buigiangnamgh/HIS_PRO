export default class Utils {

    // public static convertStringToDate(pString): string {
    //     if (typeof pString == 'undefined') return '';
    //     var stringArr = pString.split("T");
    //     var dateArr = stringArr[0].split("-");
    //     return dateArr[2] + '/' + dateArr[1] + '/' + dateArr[0];
    // }
    // public static convertStringToDateTime(pString): string {
    //     if (typeof pString == 'undefined') return '';
    //     var stringArr = pString.split("T");
    //     var dateArr = stringArr[0].split("-");
    //     var timeArr = stringArr[1].split("+");
    //     return dateArr[2] + '/' + dateArr[1] + '/' + dateArr[0] + ' ' + timeArr[0];
    // }

    public static formatUndefined(str): string {
        return (typeof str != 'undefined') ? str : "";
    }

    /**
     * @author nhandh
     * with format dd/MM/yyyy
     */
    public static convertStringToDate(sDate: string): Date {
        let strs: Array<string> = sDate.split('/');
        let day: number = parseInt(strs[0]);
        let month: number = parseInt(strs[1]) - 1;
        let year: number = parseInt(strs[2]);
        return new Date(year, month, day);
    }

    /**
     * function getDateIntance with format dd/MM/yyyy
     * @author nhandh
     */
    public static getDateIntance(d: Date): Date {
        d.setHours(0);
        d.setMilliseconds(0);
        d.setMinutes(0);
        d.setSeconds(0);
        return d;
    }

    /**
     * function compareDate with format dd/MM/yyyy
     * @author nhandh
     * @argument d1, d2
     */
    public static compareDate(d1: Date, d2: Date): number {
        let date1: Date = this.getDateIntance(d1);
        let date2: Date = this.getDateIntance(d2);
        let t1: number = date1.getTime();
        let t2: number = date2.getTime();
        if (t1 == t2) return 0;
        return t1 > t2 ? 1 : -1;
    }


    /**
     * @author nhandh
     * get by id with array
     */
    public static getById(objs, id): any {
        if (objs == null || objs.length == 0) {
            return null;
        }
        for (var i in objs) {
            if (objs[i].id == id)
                return objs[i];
        }
        return null;
    }

    public static floatWithFixDecimal(number: number, decimal: number): string {
        let dec = Math.pow(10, decimal);
        let r1 = Math.round(number * dec);
        let r2 = r1 / dec;
        let r3 = parseFloat(r2.toString());
        let r4 = r3.toFixed(decimal);
        return r4; 
    }
}
