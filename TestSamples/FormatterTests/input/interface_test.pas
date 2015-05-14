uses
  interface_unit, interface_unit2, System;

type
  MyClass = class(MyInter1)
  public
    procedure p(k: integer);
    begin
      writeln(k*3);
    end;
    function Clone: object; 
    begin
      result := nil;
    end;
  end;
  
var
  k: MyClass;
  m: MyC;
  i: MyInter1;

begin
  k := new MyClass;
  k.p(2);
  m := new MyC;
  i := m;
  i.p(1);
  readln;
end.