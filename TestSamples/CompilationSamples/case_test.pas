var k : integer;
begin
k:= 77;
case k of
1..4: writeln(3);
5..8, 26,55,77,89 : writeln(2);
10..14 : writeln(1);
else writeln(0);
end;
k := 112;
case k of 
1, 12, 17, 23, 47 : writeln(1);
3,6,26,55,77,89 : writeln(2);
0,52,112,212 : writeln(3);
end;
end.