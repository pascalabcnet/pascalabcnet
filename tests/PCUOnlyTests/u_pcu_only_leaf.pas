unit u_pcu_only_leaf;

interface

function Transform(value: integer): integer;
function Identity<T>(value: T): T;

implementation

function Transform(value: integer): integer;
begin
  Result := Seq(value, value + 1).Select(x -> x * 2).Sum;
end;

function Identity<T>(value: T): T;
begin
  Result := value;
end;

end.
