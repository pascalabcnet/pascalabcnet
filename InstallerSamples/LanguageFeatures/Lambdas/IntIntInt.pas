type int = integer;

begin
  var f: int -> int -> int := i -> j -> i*j;
  MatrGen(10,10,(i,j)->f(i)(j)).Println;  
end.