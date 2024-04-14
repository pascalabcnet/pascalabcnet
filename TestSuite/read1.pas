var
  read: integer;
begin
  assign(input, 'read1.txt');
  readln(read);
  assert(read = 45);
end.