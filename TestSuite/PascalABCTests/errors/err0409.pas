type
  I1 = interface
    property X: byte read;
  end;
  
  T = class(I1)
  public
    property T.X: byte read 0;
  end;
  
begin
  var x := new T();
  Writeln(I1(x).X);
end.