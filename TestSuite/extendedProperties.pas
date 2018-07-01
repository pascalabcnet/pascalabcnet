type 
  B = class
    x: real;
  end;
  A = class
    b1 := new B;
    property X: real read b1.x write begin b1.x := value; Print(value); end;
  end;

var a1 := new A;

begin
  a1.X := 2;
  a1.X.Print;
  Assert(a1.X = 2);
end.