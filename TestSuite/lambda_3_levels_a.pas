begin
  var a := 1;
  
  var l0: procedure :=
  ()->
  begin
    var l1: procedure := ()->Assert(777=777);
   
    var l2: integer->()->() :=
      i-> 
        begin
          Assert(666=666);
          Result := ()-> Assert(a+i=8);
        end;
    l1;
    l2(7)();
  end;
  l0();
end.