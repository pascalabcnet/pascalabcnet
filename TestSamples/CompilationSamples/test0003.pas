var b : byte;
    c : char;
    i : integer;
    sm : smallint;
    sh : shortint;
    w : word;
    lw : longword;
    li : longint;
    ui : uint64;
    s : single;
    r : real;
    f : boolean;
    str : string;
    
begin
 c := char(b); c := char(i); c := char(sm); c := char(sh); c := char(w); c := char(lw);
 c := char(li); c := char(ui);
 b := byte(c); i := integer(c); sm := smallint(c); sh := shortint(c); w := word(c); lw := longword(c);
 li := longint(c); ui := uint64(c);
end.