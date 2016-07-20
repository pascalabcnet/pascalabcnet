type 
  A = class
    v := 2;
    class function f: sequence of integer;
    begin
      var a := 1;
      yield a+v;
    end;
  end;

begin
  A.f.Print;
end.