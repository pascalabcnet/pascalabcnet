begin
  var a := Arr(1..4);
  repeat
    a.Println;
  until not NextPermutation(a);
end.