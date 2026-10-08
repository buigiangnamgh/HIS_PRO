import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Import Containers
import { DefaultLayoutComponent } from './containers';
import { EmptyLayoutComponent } from './containers/empty-layout';
import { P404Component } from './modules/error/404.component';
import { P500Component } from './modules/error/500.component';
import { AuthGuard } from './authen/authen.guard';
import { AppConfig } from './config/app.config';

export const routes: Routes = [
  {
    path: '',
    redirectTo: AppConfig.routes.default,
    pathMatch: 'full',
    canActivate: [AuthGuard]
  },
  {
    path: '',
    component: EmptyLayoutComponent,
    data: {
      title: 'Other Page'
    },
    children: [
      {
        path: 'login',
        loadChildren: () => import('./modules/login/login.module').then(m => m.LoginModule)
      },
      {
        path: 'forgotpassword',
        loadChildren: () => import('./modules/forgotpassword/forgotpassword.module').then(m => m.ForgotPasswordModule)
      },
    ]
  },
  {
    path: '404',
    component: P404Component,
    data: {
      title: 'Page 404'
    }
  },
  {
    path: '500',
    component: P500Component,
    data: {
      title: 'Page 500'
    }
  },
  {
    path: '',
    component: DefaultLayoutComponent,
    data: {
      title: 'Trang chủ'
    },
    children: [
      {
        path: 'home',
        loadChildren: () => import('./modules/home/home.module').then(m => m.TrangChuModule)
      },
      {
        path: 'homepage',
        loadChildren: () => import('./modules/homepage/homepage.module').then(m => m.HomePageModule)
      },
      {
        path: 'canlamsang',
        loadChildren: () => import('./modules/canlamsang/canlamsang.module').then(m => m.canlamsangModule)
      },
      {
        path: 'khamcapcuu',
        loadChildren: () => import('./modules/khamcapcuu/khamcapcuu.module').then(m => m.KhamCapCuuModule)
      },
      {
        path: 'vaovien',
        loadChildren: () => import('./modules/vaovien/vaovien.module').then(m => m.VaoVienModule)
      },
      {
        path: 'ravien',
        loadChildren: () => import('./modules/ravien/ravien.module').then(m => m.RaVienModule)
      },
      {
        path: 'dangnamvien',
        loadChildren: () => import('./modules/dangnamvien/dangnamvien.module').then(m => m.DangNamVienModule)
      },
      {
        path: 'taichinh',
        loadChildren: () => import('./modules/taichinh/taichinh.module').then(m => m.TaiChinhModule)
      },
      {
        path: 'hiendiennt',
        loadChildren: () => import('./modules/hiendiennt/hiendiennt.module').then(m => m.hiendienntModule)
      },
      {
        path: 'thutienkham',
        loadChildren: () => import('./modules/thutienkham/thutienkham.module').then(m => m.ThuTienKhamModule)
      },
      {
        path: 'khambenhtiepdon',
        loadChildren: () => import('./modules/khambenhtiepdon/khambenhtiepdon.module').then(m => m.khambenhtiepdonModule)
      },
      {
        path: 'khambenh',
        loadChildren: () => import('./modules/khambenh/khambenh.module').then(m => m.KhamBenhModule)
      },
      {
        path: 'trungbinhkham',
        loadChildren: () => import('./modules/trungbinhkham/trungbinhkham.module').then(m => m.trungbinhkhamModule)
      },
      {
        path: 'thoigiankham',
        loadChildren: () => import('./modules/thoigiankham/thoigiankham.module').then(m => m.thoigiankhamModule)
      },
      {
        path: 'bacsikham',
        loadChildren: () => import('./modules/bacsikham/bacsikham.module').then(m => m.bacsikhamModule)
      },
      {
        path: 'sieuam',
        loadChildren: () => import('./modules/sieuam/sieuam.module').then(m => m.sieuamModule)
      },
      {
        path: 'giaiphaubenh',
        loadChildren: () => import('./modules/giaiphaubenh/giaiphaubenh.module').then(m => m.giaiphaubenhModule)
      },
      {
        path: 'sinhthiettucthi',
        loadChildren: () => import('./modules/sinhthiettucthi/sinhthiettucthi.module').then(m => m.sinhthiettucthiModule)
      },
      {
        path: 'mau',
        loadChildren: () => import('./modules/mau/mau.module').then(m => m.mauModule)
      },
      {
        path: 'dtshpt',
        loadChildren: () => import('./modules/dtshpt/dtshpt.module').then(m => m.dtshptModule)
      },
      {
        path: 'tmcanthiep',
        loadChildren: () => import('./modules/tmcanthiep/tmcanthiep.module').then(m => m.tmcanthiepModule)
      },
      {
        path: 'phauthuat',
        loadChildren: () => import('./modules/phauthuat/phauthuat.module').then(m => m.phauthuatModule)
      },
      {
        path: 'ngoaiphongmo',
        loadChildren: () => import('./modules/ngoaiphongmo/ngoaiphongmo.module').then(m => m.ngoaiphongmoModule)
      },
      {
        path: 'xetnghiemsinhhoa',
        loadChildren: () => import('./modules/xetnghiemsinhhoa/xetnghiemsinhhoa.module').then(m => m.xetnghiemsinhhoaModule)
      },
      {
        path: 'xetnghiemhuyethoc',
        loadChildren: () => import('./modules/xetnghiemhuyethoc/xetnghiemhuyethoc.module').then(m => m.xetnghiemhuyethocModule)
      },
      {
        path: 'xetnghiemvisinh',
        loadChildren: () => import('./modules/xetnghiemvisinh/xetnghiemvisinh.module').then(m => m.xetnghiemvisinhModule)
      },
      {
        path: 'chitietdoanhthu',
        loadChildren: () => import('./modules/chitietdoanhthu/chitietdoanhthu.module').then(m => m.ChiTietDoanhThuModule)
      },
      {
        path: 'noisoi',
        loadChildren: () => import('./modules/noisoi/noisoi.module').then(m => m.noisoiModule)
      },
      {
        path: 'thuthuat',
        loadChildren: () => import('./modules/thuthuat/thuthuat.module').then(m => m.thuthuatModule)
      }
    ],
    canActivate: [AuthGuard]
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
