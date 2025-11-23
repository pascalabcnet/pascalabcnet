uses u_defaultparams1; 
begin 
assert(test3=4);
assert(test(2)=3);
assert(test(2,4)=4);
var j : integer := test2;
assert(j=4);
var i := test2;
assert(i = 4);
var s := 'abcd';
assert(s.test=6);
var k := s.test;
assert(k = 6);
tt(ff);
assert(ArrRandomReal <> nil);
assert(ArrRandomReal.Any = true);
var obj := new TClass;
var l := obj.f;
assert(l = 2);
end.