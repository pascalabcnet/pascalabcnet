type
  c1 = class
    seq1: sequence of integer;
    constructor create;
    begin
      var a := Arr(3.1, 3.7);
      seq1 := a.Select(b -> Round(b));
    end;
  end;

begin
  var obj := new c1;
  assert(obj.seq1.ToArray[1] = 4);
end.
