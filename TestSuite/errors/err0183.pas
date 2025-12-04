type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator explicit(t,t2 : TClass):integer;
begin
  Result := t.a;  
end;

end;

begin
  
end.