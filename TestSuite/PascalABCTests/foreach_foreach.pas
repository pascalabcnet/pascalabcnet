begin
  
  foreach var a1 in Arr&<object> do
    foreach var a2 in Arr&<object>.Select(o->o as object) do
    begin
      
      var p: procedure := ()->
      begin
        var o: object;
        var f := o is byte(var b);
      end;
      
    end;
  Assert(1=1);
end.