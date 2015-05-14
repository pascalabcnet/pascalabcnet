type TRec2 = record a, b : real; end;
type TRec = record a : integer; r : TRec2; end;

const rec : TRec = (a:2;r:(a:1.3;b:1.5));

begin
rec.a := 34;
end.