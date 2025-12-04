type TDiap = 1..4;
     Tstring = string[3];
     TColor = (red, green, blue);
     
procedure Test(s1 : set of string; s2 : set of TDiap; params arr : array of set of TString);
begin
end;

var s : set of TString := ['ag','b'];
    f : file of Tstring;
    s1,s2 : TString;
    s3 : set of byte := [2,3,4];
    
begin
assert('b' in s);
Assign(f,'test.dat');
Rewrite(f);
Write(f,'s','q');
Write(f,'t');
Close(f);
Reset(f);
Read(f,s1,s2);
assert(s1='s');
assert(s2='q');
Read(f,s1);
assert(s1='t');
Close(f);
assert(2 in [1..3,'a'..'d',false..true]);
assert('b' in [1..3,'a'..'d',false..true]);
assert(blue in [2.3,1..5,green..blue,'d','frgg','k'..'t',red]);
end.