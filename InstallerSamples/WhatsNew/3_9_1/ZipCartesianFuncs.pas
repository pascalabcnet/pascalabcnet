begin
  Zip(1..5,2..6).Println;
  Zip(1..5,2..6,(x,y) -> x+y).Println;
  Zip(1..5,2..6,3..7).Println;
  Zip(1..5,2..6,3..7,(x,y,z) -> x+y+z).Println;

  Cartesian(1..4,3..5).Println;
  Cartesian(1..4,3..5,(x,y) -> x*y).Println;
  Cartesian(1..2,4..5,6..7).Println;
  Cartesian(1..2,4..5,6..7,(x,y,z) -> x*y*z).Println;
end.