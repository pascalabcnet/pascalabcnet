//nopabcrtl
uses NUnitABC;

type TClass = class
public 
[Test, Combinatorial]
static procedure TestPrime4([ValuesAttribute(1,2,3,4)] a: real);
begin
end;
end;

begin
  TClass.TestPrime4(2);
end.