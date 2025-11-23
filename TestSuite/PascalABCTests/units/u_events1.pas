unit u_events1;
type TProc = procedure(sender: object);
     TProc2 = procedure;
     
var i : integer;

type TClass = class
event OnClick : procedure(sender: object);
class event OnClick_s : TProc2;

procedure MyProc(sender : object);
begin
i := 9;
end;
constructor Create;
begin
OnClick += MyProc;
OnClick(self);
assert(i=9);
OnClick -= MyProc;
end;

procedure RaiseMethod;
begin
if OnClick <> nil then
OnClick(self);
end;

class procedure RaiseMethod2;
begin
OnClick_s;
end;
end;

procedure MyProc(sender : object);
begin
i := 8;
end;

procedure MyProc2;
begin
i := 7;
end;

var t : TClass;

begin
t := new TClass;
t.OnClick += MyProc;
t.RaiseMethod;
assert(i=8);
TClass.OnClick_s += MyProc2;
TClass.RaiseMethod2;
assert(i=7);
end.