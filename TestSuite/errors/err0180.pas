type TClass = class
a : integer;
constructor(a : integer);
begin
  self.a := a;
end;

class function operator implicit(a : integer):real;
begin
  Result := new TClass(a);  
end;

end;

begin
  
end.