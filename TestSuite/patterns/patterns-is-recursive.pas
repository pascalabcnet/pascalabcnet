type
  Pair<T1, T2> = class
    
    _a: T1; _b: T2;
    procedure Deconstruct(var a: T1; var b: T2);
    begin
      a := _a;
      b := _b;
    end;
  end;

begin
  var p := new Pair<integer, string>;
  p._a := 1;
  p._b := 'asd';
  if p is Pair<integer, string>(integer(var a), string(var b)) then
    Assert((a = 1) and (b = 'asd'))
  else
    Assert(false);
end.