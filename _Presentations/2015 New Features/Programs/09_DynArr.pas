var a: array of integer;

begin
  a := Seq(2,5,7,10);
  writeln(a);
  a := SeqRandom();
  a.Println;
  var b := Seq('a','e','i','o','u','y');
  writeln(b);
  a := SeqFill(666,5);
  writeln(a);
  a := SeqFill(10,i->i*i+1);
  writeln(a);
//  a := ReadSeqInteger(5); 
  a := SeqGen(1,x->x*2,10);  
  a.Println;
end.