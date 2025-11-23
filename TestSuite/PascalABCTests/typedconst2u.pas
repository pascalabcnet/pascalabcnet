unit typedconst2u;
type TRec = record 
             a : integer; 
             b : array[1..3] of integer; 
            end;
     TDiap = 1..4;
     
const 
  a : set of byte = [1..4];
  b : TRec = (a:2;b:(3,4,5));
  c : array of char = ('a','b','c');
  d : array[1..3] of real = (1.3,1.5,2.6);
  e : set of char = ['a','b','c'];
  f : (one, two, three) = two;
  g = [one,three];
  h = one;
  i = 3;
  j = d;
  k = f;
  l = g;
  m = b;
  n : array[1..2,1..2] of integer = ((1,2),(2,3));
  //g : TDiap = 3;
  
end.