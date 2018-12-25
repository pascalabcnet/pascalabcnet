begin
  var s: real->real := x -> x * x;
  var add: (real,real)->real := (x, y)-> x + y;
  
  // Делегат может хранить процедурную переменную любого типа!
  var d: System.Delegate := s;  
  d := add;
  match d with
    Func<real, real>(var i): Print(i(2));
    Func2<real, real, real>(var i): Print(i(2,3));
  end;
end.