uses events4u;
var t : TClass;

begin
t := new TClass;
t.OnClick += MyProc;
t.RaiseMethod;
end.