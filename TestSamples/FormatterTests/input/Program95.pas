function mystrcat(s1,s2 : string): string; external 'PABCRtl.dll' name 'pabc_strcat';

var s : string;
begin
s := mystrcat('abc','efg');
writeln(s);
end.