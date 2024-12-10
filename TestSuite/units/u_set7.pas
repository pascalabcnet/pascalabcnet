unit u_set7;

var 
  b : byte:=1;
  sh : shortint:=2;
  sm : smallint:=3;
  w : word:=4;
  i : integer:=5;
  lw : longword:=6;
  li : int64:=7;
  ui : uint64:=8;
  s1 : set of uint64;
  s2 : set of integer;
  
begin
assert([b,sh,sm,w,i,lw,li,ui]=SetOf(1,2,3,4,5,6,7,8));
Include(s1,integer.MaxValue);
s1.Add(longword.MaxValue);
s1 += [int64.MaxValue,uint64.MaxValue];
assert(s1=[integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
s1 := [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue];
end.
