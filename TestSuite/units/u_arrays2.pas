unit u_arrays2;
type TArr = array[1..4] of integer;
     TMatr = array[1..3,1..3] of integer;
     
const a : TArr = (2,3,4,5);
      a2 : TMatr = ((1,2,3),(4,5,6),(7,8,9));
      
var b : TArr := (2,3,4,5);
    b2 : TMatr := ((1,2,3),(4,5,6),(7,8,9));
    
begin
assert(a[3] = 4);
assert(b[3] = 4);
assert(a2[2,2] = 5);
assert(b2[2,2] = 5);
end.