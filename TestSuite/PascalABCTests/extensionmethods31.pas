var a,b,c,d: integer;

type 
  AA = class
    procedure p(i: integer);
    begin
      a := 1;
    end;
  end;
  
procedure p(Self: AA; i,j: integer); extensionmethod;
begin
  b := 2;
end;

procedure p<T>(self: T; i,j,k: integer); extensionmethod;
begin
  c := 3;
end;

procedure p<T>(self: T; i: T); extensionmethod;
begin
  d := 4;
end;
  
begin
  var a1 := new AA;
  a1.p(1);
  a1.p(1,2);
  a1.p(1,2,3);
  a1.p(a1);
  Assert((a,b,c,d)=(1,2,3,4));
end.  