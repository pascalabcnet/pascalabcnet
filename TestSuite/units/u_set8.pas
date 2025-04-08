unit u_set8;
procedure Test(s : set of byte);
begin
  var i : byte := 10;
  assert(i in s);
end;

var b : byte:=1;
    sh : shortint:=1;
    sm : smallint:=1;
    w : word:=1;
    i : integer:=1;
    lw : longword:=1;
    li : int64:=1;
    ui : uint64:=1;
    
    s1 : set of byte;
    s2 : set of shortint;
    s3 : set of smallint;
    s4 : set of word;
    s5 : set of integer;
    s6 : set of longword;
    s7 : set of int64;
    s8 : set of uint64;
    
    s9 : set of 1..10000;
    s10 : set of 1..MaxInt;
    
begin
Include(s1,b);
Include(s2,sh);
Include(s3,sm);
Include(s4,w);
Include(s5,i);
Include(s6,lw);
Include(s7,li);
Include(s8,ui);
assert(sh in s1);
assert(sm in s1);
assert(w in s1);
assert(i in s1);
assert(lw in s1);
assert(li in s1);
assert(ui in s1);
s2 := [1,3];
s4 := [1,2];
//assert(s2*s4=s1);
//assert(s2*s4=s3);
//assert(s2*s4=s5);
//assert(s2*s4=s7);
//assert(s2*s4=s8);

s1 := [2..5];
assert(3 in s1);
s2 := [2..5];
assert(3 in s2);
s3 := [2..5];
assert(3 in s3);
s4 := [2..5];
assert(3 in s4);
s5 := [2..5];
assert(3 in s5);
s6 := [2..5];
assert(3 in s6);
s7 := [2..5];
assert(3 in s7);
s8 := [2..5];
assert(3 in s8);

{b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s1 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s2 := [b,sh,sm,w,i,lw,li,ui];
assert(s2=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s3 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s4 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s5 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s6 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s7 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

b := 1; sh := 2; sm := 3; w := 4; i := 5; lw := 6; li := 7; ui := 8;
s8 := [b,sh,sm,w,i,lw,li,ui];
assert(s1=[1,2,3,4,5,6,7,8]);

s1 := [1]; s2 := [2]; s3 := [3]; s4 := [4]; s5 := [5]; s6 := [6]; s7 := [7]; s8 := [8];
assert(s1+s2+s3+s4+s5+s6+s7+s8=[1,2,3,4,5,6,7,8]);}

s1 := [byte.MaxValue];
assert(byte.MaxValue in s1);
s2 := [shortint.MaxValue];
assert(shortint.MaxValue in s2);
s3 := [smallint.MaxValue];
assert(smallint.MaxValue in s3);
s4 := [word.MaxValue];
assert(word.MaxValue in s4);
s5 := [integer.MaxValue];
assert(integer.MaxValue in s5);
s6 := [longword.MaxValue];
assert(longword.MaxValue in s6);
s7 := [int64.MaxValue];
assert(int64.MaxValue in s7);
s8 := [uint64.MaxValue];
assert(uint64.MaxValue in s8);
s8 := [longword.MaxValue,uint64.MaxValue];
ui := longword.MaxValue;
assert(ui in s8);

assert(byte.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(shortint.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(word.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(shortint.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(integer.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(longword.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(int64.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);
assert(uint64.MaxValue in [byte.MaxValue,shortint.MaxValue,smallint.MaxValue,word.MaxValue,integer.MaxValue,longword.MaxValue,int64.MaxValue,uint64.MaxValue]);

Include(s9,byte.MaxValue); assert(byte.MaxValue in s9);
Include(s9,shortint.MaxValue); assert(shortint.MaxValue in s9);
Include(s9,smallint.MaxValue); //assert(not (smallint.MaxValue in s9));
Include(s9,word.MaxValue); //assert(not (word.MaxValue in s9));
Include(s9,integer.MaxValue); //assert(not (integer.MaxValue in s9));
Include(s9,longword.MaxValue); //assert(not (longword.MaxValue in s9));
Include(s9,int64.MaxValue); //assert(not (int64.MaxValue in s9));
Include(s9,uint64.MaxValue); //assert(not (uint64.MaxValue in s9));

Include(s10,byte.MaxValue); assert(byte.MaxValue in s10);
Include(s10,shortint.MaxValue); assert(shortint.MaxValue in s10);
Include(s10,smallint.MaxValue); assert((smallint.MaxValue in s10));
Include(s10,word.MaxValue); assert((word.MaxValue in s10));
Include(s10,integer.MaxValue); assert((integer.MaxValue in s10));
Include(s10,longword.MaxValue); //assert(not(longword.MaxValue in s10));
Include(s10,int64.MaxValue); //assert(not(int64.MaxValue in s10));
Include(s10,uint64.MaxValue); //assert(not(uint64.MaxValue in s10));

Test([10,11,12]);
end.