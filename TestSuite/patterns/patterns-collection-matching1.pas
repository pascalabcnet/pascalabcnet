begin
  var a := Arr(1, 9, 8, 7, 2, 3, 4, 5); 
  var c: procedure := () -> begin
    match a with
      [.., 4, var x]: assert(x = 5);
    end;
  end;
  
  c;
end.