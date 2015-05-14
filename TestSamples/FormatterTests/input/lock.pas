var o:object;

begin
  o := new object;
  lock o do
    writeln('xx');
end.