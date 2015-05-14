unit u_arrays3;
type TArr = array of integer;
     TMatr = array of array of integer;
     
const a : TArr = (2,3,4,5);
      a2 : TMatr = ((1,2,3),(4,5,6),(7,8,9));
      
var b : TArr := (2,3,4,5);
    b2 : TMatr := ((1,2,3),(4,5,6),(7,8,9));
    a3 : TArr;
    
begin
assert(a[2] = 4);
assert(b[2] = 4);
assert(a2[1,1] = 5);
assert(b2[1,1] = 5);
a3 := new integer[4];
a3[2] := 111;
assert(a3[2] = 111);
SetLength(a3,8);
assert(a3[2] = 111);
end.