type 
  X = class
    v := 2;
  end;

var v := 666;

type 
  A = class(X)
    function f: sequence of integer;
    begin
      var a := 1;
      yield a+v;
    end;
  end;

begin
  var a1 := new A;
  Assert(a1.f.Print.SequenceEqual(Seq(3)));
end.