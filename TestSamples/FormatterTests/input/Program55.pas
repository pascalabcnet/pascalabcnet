procedure Test(s : string[2]);
begin
writeln(s);
end;

type 
  TRec = record
          name : string[20];
          age : integer;
         end;
         
var s : string[5];
    s3 : ShortString;
    s1 : string;
    //f : file of ShortString;
    f : file of string[5];
    f2 : file of TRec;
    r,r2 : TRec;
    f3 : Text;
    
begin
Assign(f3,'out.txt');
SetLength(s,23);
Assign(f2,'out.dat');
Rewrite(f2);
r.age := 23;
r.name := 'Popov';
Write(f2,r);
r.age := 45;
r.name := 'Vatov';
Write(f2,r);
r.age := 17;
r.name := 'Komov';
Write(f2,r);
Close(f2);
Reset(f2);
Seek(f2,2);
Read(f2,r2);
Close(f2);
Assign(f,'out.dat');
s := 'abd';
Rewrite(f);
Write(f,s);
Close(f);
Reset(f);
Read(f,s);
Close(f);
s1 := 'absdefghjk';
s := '';
s1 := 'abcdefgh';
s1[4] := #0;
writeln(s1);
writeln(s1[7]);
s[0] := '3';
s[2] := 'h';
s[3] := #0;
s[4] := 'd';
Test('rrrrr');
s1 := 'abcdefghij';
s := s1;
s[3] := '2';
writeln(integer(s[0]));
s := 'adf';
writeln(s <> s1);
s := s + 'kldrrr';
writeln(integer(s[0]));
s := s1;
s := s3;
s := 'aaa';
end.