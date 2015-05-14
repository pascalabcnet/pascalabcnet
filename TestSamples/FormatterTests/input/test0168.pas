uses test0168u;
    
begin
 assert(integer(dig)=2); 
 hue := Red or Yellow;
 assert(hue = MultiHue(1 or 4));
end.