type TBase = class
constructor Create(i : integer);
begin
  
end;
constructor Create(r : real);
begin
  
end;
end;

type TDer = class(TBase)
constructor Create(s : string);
begin
  inherited;
end;
end;

begin
  
end.