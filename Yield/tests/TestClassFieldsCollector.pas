type A = class;

type A = class
  field1: integer;
  field2: integer;
  
  procedure fun1;
  begin
    field1 += 1;
  end;
  
  procedure fun2;
end;

type B = class
end;

procedure A.fun2;
begin
  field1 += 1;
  field2 += 4;
end;

procedure fun3;
begin
end;

begin
end.