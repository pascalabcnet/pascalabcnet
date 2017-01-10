begin
  (var l, var m, var n):=(3,5,2);
  var A:=MatrRandom(l,m,1,50); A.Println(5); Writeln;
  var B:=MatrRandom(m,n,1,50); B.Println(5); Writeln;
  var C:=MatrFill(l,n,0);
  for var i:=0 to l-1 do
    for var j:=0 to n-1 do
      C[i,j]:=A.Row(i).Zip(B.Col(j),(x,y)->x*y).Sum;
end.