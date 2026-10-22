library pcu_only_library;

uses u_pcu_only_middle;

function Answer: integer;
begin
  Result := Calculate(10);
end;

end.
