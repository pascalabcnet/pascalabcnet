uses test0149u;
var t : TClass;
    i : integer;
    r : real;
    
begin
t := 202;
assert(t.a = 202);
i := integer(t);
assert(i=202);
var c : char := char(t);
assert(c=char(202));
t := 112.23;
assert(t.a = 112);
end.