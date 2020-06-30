// #2180
type
  t1 = class end;
  
function f1(self: t1; x: integer): integer; extensionmethod;
begin
  Result := x * x;
end;

type
  t2 = class(t1)
    procedure test;
    begin
      Assert(f1(4)=16);
      Assert(Self.f1(4)=16);
    end;
  end;

begin
  t2.Create.test;
end.
