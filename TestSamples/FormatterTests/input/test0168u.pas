unit test0168u;
uses System;
type 
TDigits = (One=1, Two=2, Three=3);
[FlagsAttribute]
MultiHue = (Black=0, Red=1, Green=2, Yellow=4, Blue=8);

var dig : TDigits:=Two;
    hue : MultiHue;
    
begin
end.