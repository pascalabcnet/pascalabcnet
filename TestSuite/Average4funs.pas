{function Average1<TSource>(Self: sequence of TSource; selector: TSource -> integer): real; extensionmethod;
begin
  
end;

function Average1<TSource>(Self: sequence of TSource; selector: TSource -> real): real; extensionmethod;
begin
end;}

begin
  var pupils : array of integer := (1,2,3);
  var r := pupils.Average(p->p);
  Assert(abs(r - 2)<0.000001);
end.