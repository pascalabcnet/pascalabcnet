unit u_classprops1;
type TClass = class
class a : integer := 4;
class constructor Create;
begin
assert(a=4);
end;

class property Prop1 : integer read a write a;
end;

var t : TClass;
begin
t := new TClass();
TClass.Prop1 := 6;
assert(TClass.Prop1=6);
assert(TClass.a = 6);
end.