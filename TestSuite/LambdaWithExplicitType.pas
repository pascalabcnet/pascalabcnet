begin
  var q := procedure(a,b: integer; c: real) -> begin
    //Print(a,b,c);
  end;
  var b := function(i: integer): integer -> i * 2;
  
  q(2,3,4);
  Assert(b(2)=4)
end.