var SosediA:array[0..10,0..10] of integer;
begin
SosediA[1][2]:=10;
case SosediA[1][2] of
   0..1,4..9:writeln(1);
   3:writeln(2);
end;
readln;
end.