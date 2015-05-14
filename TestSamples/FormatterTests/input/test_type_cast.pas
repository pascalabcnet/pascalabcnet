var o:object;r:real;
begin
  o:=1.1;
  r:=real(o);
  write(r);
  o:=new Exception('');
  o:=Exception(o);
  readln;
end.