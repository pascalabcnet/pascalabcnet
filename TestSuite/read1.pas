var
  read: integer;

procedure test;
begin
  assign(input, 'read1.txt');
  readln(read);
  assert(read = 45);
end;
begin
  //TODO
end.