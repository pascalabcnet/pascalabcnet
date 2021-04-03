type
  I1 = interface
    property X: byte read;
  end;
  
  I2 = interface
    property X: byte read;
  end;
  
  T = class(I1, I2)
  public
    property I1.X: byte read 0;
    //I2.X не реализовано, но компиляция успешна
  end;
  
begin
  var x := new T();
  Writeln(I1(x).X);
  Writeln(I2(x).X);
end.