type TRec = record
val: integer;
class function operator implicit(self: BigInteger): TRec;
begin
  Result.val := 3;
end;
end;

function operator implicit(self: BigInteger): List<integer>; extensionmethod;
begin
  Result := new System.Collections.Generic.List<integer>();
  Result.Add(2);
end;

begin
  var b: BigInteger := new BigInteger(2.5);
  var r: List<integer> := b;
  assert(r[0] = 2);
  var rec: TRec := b;
  assert(rec.val = 3);
end.