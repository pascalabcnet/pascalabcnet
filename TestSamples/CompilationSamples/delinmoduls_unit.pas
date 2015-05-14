unit delinmoduls_unit;

type
  del=procedure(i:integer);

var 
  d:del;

procedure dr(i:integer);
begin
end;

procedure test;
begin
  d:=dr;
  d(1);
end;

end.