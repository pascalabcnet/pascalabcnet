type
  t1 = class
    
    function f1: sequence of byte;
    begin
      
      var p: function: sequence of integer := ()->
      |(1,2)|.Select(\(a,b)->3);
      yield p().ToArray()[0];
    end;
    
  end;
  
begin 
  var o := new t1;
  assert(o.f1.ToArray[0] = 3);
end.