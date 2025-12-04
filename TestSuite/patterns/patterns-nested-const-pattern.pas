begin 
  var a := 3;
  var b := 2;
   match a with
    1: assert(false);
    2: assert(false);
    3: 
    match b with
      1: assert(false);
      2: exit;
    end;
   end;
   assert(false);
end.