unit u_generics8;

var i: integer;
procedure p1<T>(a: array of T);
begin
  i := 1;  
end;
procedure p1<T>(a: T);
begin
  i := 2;
end;

procedure p2<T>(a: array[,] of T);
begin
  i := 1;  
end;
procedure p2<T>(a: T);
begin
  i := 2;
end;

end.