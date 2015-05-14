type TClass = class
procedure Test(a : integer); abstract;
end;

TClass2 = class(TClass)
procedure Test(a : integer); override;
begin
end;
end;

TClass3 = class(TClass)
procedure Test2(k : real); abstract;
end;

TClass4 = class(TClass3)
procedure Test(a : integer); override;
begin
end;
procedure Test2(k : real); override;
begin
end;
end;

TClass5 = class(TClass2)
procedure Test3; abstract;
end;

TClass6 = class(TClass5)
procedure Test3; override;
begin
end;
end;

var t : TClass;
    t2 : TClass3;
    t3 : TClass5;
    
begin
t := new TClass2;
t.Test(2);
t2 := new TClass4;
t2.Test2(2.3);
t3 := new TClass6;
t3.Test3;
end.