type
  TClass = class
    function F() := 0;
  end;

begin
  var ff1 := new TClass;
  var x := ff1?.F;
  Assert(x=0);
end.