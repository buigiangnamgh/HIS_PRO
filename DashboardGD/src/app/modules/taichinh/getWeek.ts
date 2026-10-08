import { data } from "../../model/apidata.model";

export function getWeek() {


  //  return currentWeekNumber();
  return 0;
}

export function addDays(dateObj, numDays) {
  dateObj.setDate(dateObj.getDate() + numDays);
  let x=dateObj.getDate();
  return dateObj;
}



export function ConvertdateToString(date,time) {
  let x = ""
  if (date.getMonth()+1 < 10) {
    if (date.getDate() < 10) {
      x = date.getFullYear() + "0" + (date.getMonth()+1) + "0" + date.getDate()+time;
    } else {
      x = date.getFullYear() + "0" + (date.getMonth()+1) +""+ date.getDate()+time;
    }
  } else {
    if (date.getDate() < 10) {
      console.log(date.getMonth(),date.getFullYear(),date.getDate());
      x = date.getFullYear() +""+ (date.getMonth()+1) + "0" + date.getDate()+time;
    } else {
      let m =date.getMonth()+1;
      let d =date.getDate();
      let y =date.getFullYear();
      x = y +""+ m +""+ d +""+time;
    }
  }
  return x;
}
export function MondayofWeek(year,week){
  let FirstDay=new Date();
  FirstDay.setFullYear(year,0,1);
  switch(FirstDay.getDay()){
    case 1 :{ 
      FirstDay.setDate(FirstDay.getDate()+(week-1)*7);//là thứ 2
      break;
    }
    case 2:{
      FirstDay.setDate(FirstDay.getDate()+6+(week-1)*7);
      break;
    }
    case 3:{
      FirstDay.setDate(FirstDay.getDate()+5+(week-1)*7);
      break;
    }
    case 4:{
      FirstDay.setDate(FirstDay.getDate()+4+(week-1)*7);
      break;
    }
    case 5:{
      FirstDay.setDate(FirstDay.getDate()+3+(week-1)*7);
      break;
    }
    case 6:{
      FirstDay.setDate(FirstDay.getDate()+2+(week-1)*7);
      break;
    }
    case 0:{
      FirstDay.setDate(FirstDay.getDate()+1+(week-1)*7);
      break;
    }
  }
  return FirstDay;
}
//thứ 2:52
//thứ 3:51 thêm 6
//thứ 4:51 thêm 5
//thứ 5:51 thêm 4
//thứ 6:51 thêm 3
//thứ 7:51 thêm 2
//cn : 52 thêm 1
