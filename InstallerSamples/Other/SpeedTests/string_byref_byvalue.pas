var ss:int64 := 0;

procedure p(s: string);
begin
  s[1] := '1';
end;

procedure q(var s: string);
begin
  s[1] := '1';
end;

begin
  var n := 100000000;
  var s := 'a'*n;
  MillisecondsDelta;
  loop 10 do
    p(s);
  Println(s[:3]);
  MillisecondsDelta.Println;
  loop 10 do
    q(s);
  Println(s[:3]);
  MillisecondsDelta.Println;
end.