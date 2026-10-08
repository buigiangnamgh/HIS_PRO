import { Component, ViewEncapsulation, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { TranslateService } from 'ng2-translate';

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: '<router-outlet></router-outlet>',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  constructor(private router: Router
  ,public translate: TranslateService
  ) {
	  // translate.addLangs(["vi", "en"]);
    // translate.setDefaultLang('vi');
    translate.use('vi');
	const isSearchReport = localStorage.getItem('objectstore__IsSearchReport');    
	if(isSearchReport!=undefined && isSearchReport!=null && isSearchReport=='1'){
		console.log('isSearchReport in Init' );
	}
	else{
		localStorage.removeItem('objectstore__searchReport');
	}
	localStorage.setItem('objectstore__IsSearchReport', '');
	console.log('AppComponent Init ' + new Date());
    // let browserLang = translate.getBrowserLang();
    // translate.use(browserLang.match(/vi|en/) ? browserLang : 'vi');
  }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });
  }
}
