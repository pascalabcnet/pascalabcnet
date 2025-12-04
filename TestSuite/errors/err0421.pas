//!Нельзя преобразовать тип function(i: integer): string к string
function f(i: integer): string;
begin
  
end;

procedure p(s: string);
begin
  
end;
begin
  p(f);
end.