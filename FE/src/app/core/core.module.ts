import { NgModule, Optional, SkipSelf } from '@angular/core';

@NgModule({
  providers: [
    // Các service singleton toàn app đăng ký ở đây
  ],
})
export class CoreModule {
  // Ngăn chặn việc import CoreModule nhiều lần
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error(
        'CoreModule đã được import rồi. Vui lòng chỉ import CoreModule trong AppModule thôi!'
      );
    }
  }
}