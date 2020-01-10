uses System;

type
  ICloneable<T> = interface(System.ICloneable)
    function Clone(): T;
  end;
  
  TClass{@class TClass<T>@}<T> = class
  end;
  
  TClass2{@class TClass2<T1,T2>@}<T1,T2> = class
  end;
  
begin
  var i: ICloneable{@interface ICloneable<T>@}<integer>;
  var i2: ICloneable{@interface ICloneable<T>@};
  var i3: System.ICloneable{@interface System.ICloneable@};
  
end.