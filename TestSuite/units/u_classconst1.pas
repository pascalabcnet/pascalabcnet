unit u_classconst1;
type
  cls = class
    class function p: integer;
    const c : array[1..5] of integer = (1,2,3,4,5);
    begin
      Result := c[2];
    end;
  end;
    
begin
  assert(cls.p = 2);
end.