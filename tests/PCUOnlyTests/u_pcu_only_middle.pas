unit u_pcu_only_middle;

interface

function Calculate(value: integer): integer;

implementation

uses u_pcu_only_leaf;

function Calculate(value: integer): integer;
begin
  Result := Transform(Identity(value));
end;

end.
