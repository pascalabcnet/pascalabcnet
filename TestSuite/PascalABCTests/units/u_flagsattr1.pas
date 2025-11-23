unit u_flagsattr1;
uses System;
type 
TDigits = (One=1, Two=2, Three=3);
[FlagsAttribute]
MultiHue = (Black=0, Red=1, Green=2, Yellow=4, Blue=8);

var dig : TDigits:=Two;
    hue : MultiHue;
    
begin
 assert(integer(dig)=2); 
 hue := Red or Yellow;
 assert(hue = MultiHue(1 or 4));
end.