type
  T = record
    y := 1;
    function ToString: string; override := y.ToString;
  end;

function F(x: T?): string;
begin
  Result := x?.ToString();
end;

begin
  assert(f(new T)='1');
end.