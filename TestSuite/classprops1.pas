type 
TestEventHandler =  procedure(value: integer);

TClass = class
class event TestEvent: TestEventHandler;
class a : integer := 4;
class property Prop1 : integer read a write a;

class constructor Create;
begin
TestEvent+= TestEventHandler(set_Prop1);
assert(a=4);
TestEvent(5);
assert(a=5);
end;

end;

var t : TClass;
begin
t := new TClass();
TClass.Prop1 := 6;
assert(TClass.Prop1=6);
assert(TClass.a = 6);
end.