type
  t1=class
  
  end;
  t2=class
    class function operator*(a:t1;b:t2):t2;
    begin
    
    end;
  end;

begin
  var a:t1;
  var b:t2;
  var c{@var c: t2;@} := a*b;
end.