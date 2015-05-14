const max : byte = 2;

var i : integer;
    b : byte;
    c : char;
    sm : smallint;
    sh : shortint;
    w : word;
    lw : longword;
    li : longint;
    ui : uint64;
    col : (one, two, three, four, five);
    d1 : 1..4;
    d2 : byte(1)..byte(4);
    d3 : shortint(1)..byte(4);
    d4 : smallint(1)..smallint(4);
    d5 : word(1)..word(4);
    d6 : longword(1)..longword(4);
    d7 : longint(1)..longint(4);
    d8 : uint64(1)..uint64(4);
    
procedure Test(var i : integer);
begin

end;

begin
//Test(4);
i := 2;
Inc(i);
Inc(b);
c := 'k';
Inc(c,4);
Inc(sm,b);
Inc(sh);
Inc(w);
Inc(lw);
Inc(li);
Inc(ui);
col := two;
//Inc(col,6);
Inc(d1,Succ(d1));
Inc(d2);
Inc(d3);
Inc(d4);
Inc(d5);
Inc(d6);
Inc(d7);
Inc(d8);
Dec(d1);
Dec(d2);
Dec(d3);
Dec(d4);
Dec(d5);
Dec(d6);
Dec(d7);
Dec(d8);

col := two;

case col of
one: writeln('one');
two: writeln('two');
end;
d1 := 2;
case d1 of
1: writeln(2);
max: writeln(3);
end;
end.