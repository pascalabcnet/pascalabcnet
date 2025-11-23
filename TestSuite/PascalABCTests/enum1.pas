type Digits = (one, two, three, four);
    TDiap = one..three;

procedure Test;

var dig,dig3 : Digits;
    dig2,dig4 : TDiap;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);

dig2 := one;
Inc(dig2); assert(dig2 = two);
Dec(dig2); assert(dig2 = one);
dig2 := two;
assert(Succ(dig2)=three);
assert(Pred(dig2)=one);
assert(Ord(dig2)=1);

dig := two;
assert(dig < three);
assert(dig > one);
assert(dig <= four);
assert(dig >= two);
assert(dig <> three);
dig3 := four;
assert(dig < dig3);

dig2 := two;
assert(dig2 < three);
assert(dig2 > one);
assert(dig2 <= four);
assert(dig2 >= two);
assert(dig2 <> three);
dig4 := three;
assert(dig2 < dig4);

for i : Digits := two to four do
if i = three then assert(i=three);

for i : TDiap := one to three do
if i = two then assert(i=two);
end;

procedure Test2;
//type Digits = (one, two, three, four);
var dig,dig3 : Digits;
    dig2,dig4 : TDiap;
procedure Nested;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);

dig2 := one;
Inc(dig2); assert(dig2 = two);
Dec(dig2); assert(dig2 = one);
dig2 := two;
assert(Succ(dig2)=three);
assert(Pred(dig2)=one);
assert(Ord(dig2)=1);

dig := two;
assert(dig < three);
assert(dig > one);
assert(dig <= four);
assert(dig >= two);
assert(dig <> three);
dig3 := four;
assert(dig < dig3);

dig2 := two;
assert(dig2 < three);
assert(dig2 > one);
assert(dig2 <= four);
assert(dig2 >= two);
assert(dig2 <> three);
dig4 := three;
assert(dig2 < dig4);

for i : Digits := two to four do
if i = three then assert(i=three);

for i : TDiap := one to three do
if i = two then assert(i=two);
end;

begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);

dig2 := one;
Inc(dig2); assert(dig2 = two);
Dec(dig2); assert(dig2 = one);
dig2 := two;
assert(Succ(dig2)=three);
assert(Pred(dig2)=one);
assert(Ord(dig2)=1);

dig := two;
assert(dig < three);
assert(dig > one);
assert(dig <= four);
assert(dig >= two);
assert(dig <> three);
dig3 := four;
assert(dig < dig3);

dig2 := two;
assert(dig2 < three);
assert(dig2 > one);
assert(dig2 <= four);
assert(dig2 >= two);
assert(dig2 <> three);
dig4 := three;
assert(dig2 < dig4);

for i : Digits := two to four do
if i = three then assert(i=three);

for i : TDiap := one to three do
if i = two then assert(i=two);

Nested;
end;

//type Digits = (one, two, three, four);
var dig,dig3 : Digits;
    dig2,dig4 : TDiap;
begin
dig := two;
Inc(dig); assert(dig=three);
Dec(dig); assert(dig=two);
assert(Succ(dig)=three);
assert(Pred(dig)=one);
assert(Ord(dig)=1);

dig2 := one;
Inc(dig2); assert(dig2 = two);
Dec(dig2); assert(dig2 = one);
dig2 := two;
assert(Succ(dig2)=three);
assert(Pred(dig2)=one);
assert(Ord(dig2)=1);

dig := two;
assert(dig < three);
assert(dig > one);
assert(dig <= four);
assert(dig >= two);
assert(dig <> three);
dig3 := four;
assert(dig < dig3);

dig2 := two;
assert(dig2 < three);
assert(dig2 > one);
assert(dig2 <= four);
assert(dig2 >= two);
assert(dig2 <> three);
dig4 := three;
assert(dig2 < dig4);

for i : Digits := two to four do
if i = three then assert(i=three);

for i : TDiap := one to three do
if i = two then assert(i=two);
Test;
Test2;
end.