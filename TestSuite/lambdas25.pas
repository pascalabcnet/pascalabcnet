var
  f: function: sequence of char := 
    ()-> 'aboba'.Select(x-> x);
  v: function: function: integer := () -> () -> 1;
  
begin
  assert(f().First = 'a');
  assert(v()() = 1);
end.