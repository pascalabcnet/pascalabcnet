uses test0106u;
var t : TClass;

begin
t := new TClass;
t.OnClick += MyProc;
t.RaiseMethod;
assert(i=8);
end.