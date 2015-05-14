unit u_tostring2;
var a : integer := 3;
    b : char := 'r';
    c : 1..4 := 2;
    d : (red, green, blue) := green;
    e : boolean := true;
    
begin
assert(a.ToString()='3');
assert(b.ToString()='r');
assert(c.ToString()='2');
assert(d.ToString()='green');
assert(e.ToString()='True');
end.