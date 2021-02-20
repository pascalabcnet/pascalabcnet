begin
  Range(1,10).Println;
  Range(1,20,2).Println(',');
  Range('a','z').Println;
  Range(1.0,2.0,10).Println;
  
  var sum := Range(1,10,2).Select(i -> i*i).Sum();

  sum := 0;
  var i := 1;
  while i<=10 do
  begin
    sum += i*i;
    i += 2;
  end;
end.