// issue#2640
type
  CustomClass = class public x := 5; end;
  EnumSynonym = System.Enum;
  
  t1 = class
    
    procedure OK1<T>(o: T); virtual; where T: CustomClass; begin Writeln(o) end;
    
    procedure OK2<T>(o: T); virtual; where T: CustomClass, constructor; begin Writeln(o) end;
    
    procedure OK3<T>(o: T); virtual; where T: CustomClass, constructor; begin Writeln(o) end;
    
    procedure OK4<T>(o: T); virtual; where T: CustomClass; begin Writeln(o) end;
    
    procedure OK5<T>(o: T); virtual; where T: EnumSynonym, record; begin Writeln(o) end;
    
  end;
  t2 = class(t1)
    
    procedure OK1<T>(o: T); override; begin Writeln(o) end;
    
    procedure OK2<T>(o: T); override; where T: CustomClass; begin Writeln(o) end;
    
    procedure OK3<T>(o: T); override; where T: constructor; begin Writeln(o) end;
    
    procedure OK4<T>(o: T); override; where T: class; begin Writeln(o) end;
    
    procedure OK5<T>(o: T); override; where T: record; begin Writeln(o) end;
    
  end;
  
begin
  var a := new t1;
  var b := new t2;
  
  a.OK1(new CustomClass);
  b.OK1(0);
  Writeln('~'*30);
  
  a.OK2(new CustomClass);
  b.OK2(new CustomClass);
  Writeln('~'*30);
  
  a.OK3(new CustomClass);
  b.OK3(new object);
  Writeln('~'*30);
  
  a.OK4(new CustomClass);
  b.OK4(new object);
  Writeln('~'*30);
  
  var my_enum: (ME_1, ME_2);
  a.OK5(my_enum);
  b.OK5(my_enum);
  Writeln('~'*30);
  
end.