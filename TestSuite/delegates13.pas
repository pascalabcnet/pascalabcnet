var i: integer;

type
  t0 = abstract class
    procedure p0; abstract;
    procedure p2; virtual;
    begin
      
    end;
    procedure p3;
    begin
      i := 3;
    end;
  end;
  
  t1 = class(t0)
    procedure p0; override;
    begin
      i := 1;
    end;
    procedure p2; override;
    begin
      i := 2;
    end;
  end;
  
procedure p1(p: Action0);
begin
  p();
end;

begin
  var a := new t1 as t0;
  p1(a.p0);
  assert(i = 1);
  p1(a.p2);
  assert(i = 2);
  p1(a.p3);
  assert(i = 3);
end.