procedure Test2(var s : string[5]);
begin
  s[3] := '3';
end;

procedure Test(s : string[5]);
begin
  assert(s='abcd');
  Test2(s);
  assert(s='ab3d');
end;

type TRec = record
      s1 : string[5];
      s2 : string[7];
      s3 : string[4];
     end;
     
var s1 : string[4];
    s2 : string[2];
    s3 : ShortString;
    s4,s7 : string;
    s5 : string[10];
    s6 : string[5];
    arr : array[1..5] of string[8];
    f : file of TRec;
    r,r2 : TRec;
    p : ^string[12];
    prec : ^TRec;
    
begin
New(p);
p^ := 'abc'; assert(p^='abc');
SetLength(p^,5); assert(p^='abc  ');
p^[4] := '2'; assert(p^[4]='2');
Dispose(p);
s1 := 'abcdefg'; assert(s1 = 'abcd');
s2 := s1; assert(s2 = 'ab');
s3 := 'abcdefg'; assert(s3 = 'abcdefg');
s4 := s1 + s2; assert(s4 = 'abcdab');
s3 := s4; assert(s3 = s4);
s1 := 'ab'; 
s2 := 'c'; 
s4 := 'def';
s1 := s1+s2+s4; assert(s1='abcd');
s1 := 'a'; s2 := 'ab'; 
assert(s1<>s2);
assert(s1<s2); assert(s2>s1);
assert(s1<s4);
s1 := 'ab'; s2 := 'ab'; assert(s1=s2);
s4 := s1; s3 := s1; assert(s4=s2); assert(s3=s2);
assert(s1.Length=2);
s1 := 'abcd'; s1[3] := '2'; assert(s1[3] = '2'); assert(s1 = 'ab2d');
s5 := '123'; SetLength(s5,6); assert(s5='123   ');
s6 := 'abcd';
SetLength(s2,4);
//s2[4] := 'r';
Test(s6);
assert(s6='abcd');

arr[1] := 'adgg'; assert(arr[1]='adgg');
SetLength(arr[1],6); assert(arr[1]='adgg  ');

Assign(f,'test4.dat');
Rewrite(f);
r.s1 := 'AAAAA';
r.s2 := 'BBBBB';
r.s3 := 'CCCCC';
Write(f,r);
r.s1 := '1111';
r.s2 := '2222';
r.s3 := '3333';
Write(f,r);
Close(f);
Reset(f);
Seek(f,1);
writeln(eof(f));
Read(f,r2);
assert(r.s1=r2.s1);
assert(r.s2=r2.s2);
assert(r.s3=r2.s3);
Close(f);
s5 := 'abcd'; Insert('1234',s5,3); assert(s5='ab1234cd');
Delete(s5,3,4); assert(s5='abcd');
s1 := 'abc'; Insert('123',s1,3); assert(s1='ab12');
Delete(s1,3,4); assert(s1='ab');
end.