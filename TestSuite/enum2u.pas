unit enum2u;

procedure Test;
type Digits = (one, two, three, four);
var dig : Digits;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);
end;

procedure Test2;
type Digits = (one, two, three, four);
var dig : Digits;
procedure Nested;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);
end;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);
Nested;
end;

end.