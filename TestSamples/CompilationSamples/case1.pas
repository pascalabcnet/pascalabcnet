function Test: integer;
begin
Result := 3;
end;

begin
case Test of
1,2 : writeln(2);
3 : writeln(4);
end;

end.