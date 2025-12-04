unit u_case2;

var c : char;
    b : boolean;
    d : (red, green);
    e : 1..3;
    f : 'b'..'f';
    
begin
c := 'a';
case c of
'a' : assert(c='a');
'b' : assert(c='b');
end;
c := 'b';
case c of
'a'..'c' : assert(c='b');
'd' : assert(c='d');
end;
b := true;
case b of
true : assert(b=true);
false : assert(b=false);
end;
case b of
false..true : assert(b=true);
end;
d := green;
case d of
red : assert(d=red);
green : assert(d=green);
end;
e := 2;
case e of
1 : assert(e=1);
2..3 : assert(e=2);
end;
f := 'd';
case f of
'a','b','c' : assert(f<>'d');
'd','e' : assert(f='d');
end;

end.