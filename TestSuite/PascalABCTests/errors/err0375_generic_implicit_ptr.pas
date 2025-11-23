type
  t1 = class
    static function operator implicit<T>(val: T): t1;
    begin
      Writeln('rec');
    end;
   
  end;
  
  t2<T> = class end;
  
begin
  var i := 5;
  var a: t1 := @i;
end.