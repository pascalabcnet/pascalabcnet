procedure P() := Writeln(1); 

procedure P(x: byte) := Writeln(2); 

begin
  var x: System.Delegate := P; 
  x.DynamicInvoke(); // 1
end.