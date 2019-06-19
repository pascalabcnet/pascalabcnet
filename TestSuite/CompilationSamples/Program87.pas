type
  t0<T> = class end;
  t1 = class(t0<byte>)
    
    procedure p1;
    begin
      
      //обязательно лямбда
      var p: procedure := ()->
      begin
        
      end;
      
      //обязательно цикл for, loop или foreach
      //достаточно оставить 1 из следующих 3 строк чтоб воспроизвелось:
      for var i := 0 to 0 do
        p;
      foreach var o in Arr(0) do;
      loop 0 do;
      
      
      //а с repeat и while не воспроизводится:
      //while false do;
      //repeat until true;
    end;
    
  end;

begin 
  var o := new t1;
  o.p1;
end.