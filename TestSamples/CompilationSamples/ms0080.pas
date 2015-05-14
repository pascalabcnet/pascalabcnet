function TrimLeft(s: string): string;
var cc: array of char;
begin
  SetLength(cc,1);
  cc[0] := ' ';
  Result := s.TrimStart(cc);
end;

begin

end.