procedure Test1(b : byte);
begin
end;

procedure Test2(c : char);
begin
end;

procedure Test3(i : integer);
begin
end;

procedure Test4(sm : smallint);
begin
end;

procedure Test5(sh : shortint);
begin
end;

procedure Test6(w : word);
begin
end;

procedure Test7(lw : longword);
begin
end;

procedure Test8(li: longint);
begin
end;

procedure Test9(ui : uint64);
begin
end;

procedure Test10(s : single);
begin
end;

procedure Test11(r : real);
begin
end;

procedure Test12(str : string);
begin
end;

procedure Test13(f : boolean);
begin
end;

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

Test1(1); Test2('a'); Test3(1234); Test4(23); Test5(67); Test6(22); Test7(88); Test8($00004567); Test9(76); 
Test10(3.14); Test10(2); Test11(2.71); Test11(3); Test12('asss'); Test12('w'); Test13(false);

Test1(b); Test2(c); Test3(i); Test4(sm); Test5(sh); Test6(w); Test7(lw); Test8(li); Test9(ui); 
Test10(s); Test11(r); Test12(str); Test13(f);

Test1(b); Test3(b); Test4(b); Test5(b); Test6(b); Test7(b); Test8(b); Test9(b); 
Test10(b); Test11(b);

Test1(sm); Test3(sm); Test4(sm); Test5(sm); Test6(sm); Test7(sm); Test8(sm); Test9(sm); 
Test10(sm); Test11(sm);

Test1(sh); Test3(sh); Test4(sh); Test5(sh); Test6(sh); Test7(sh); Test8(sh); Test9(sh); 
Test10(sh); Test11(sh);

Test1(w); Test3(w); Test4(w); Test5(w); Test6(w); Test7(w); Test8(w); Test9(w); 
Test10(w); Test11(w);

Test1(i); Test3(i); Test4(i); Test5(i); Test6(i); Test7(i); Test8(i); Test9(i); 
Test10(i); Test11(i);

Test1(lw); Test3(lw); Test4(lw); Test5(lw); Test6(lw); Test7(lw); Test8(lw); Test9(lw); 
Test10(lw); Test11(lw);

Test1(li); Test3(li); Test4(li); Test5(li); Test6(li); Test7(li); Test8(li); Test9(li); 
Test10(li); Test11(li);

Test1(ui); Test3(ui); Test4(ui); Test5(ui); Test6(ui); Test7(ui); Test8(ui); Test9(ui); 
Test10(ui); Test11(ui);

Test10(s); Test10(r);
Test11(s); Test11(r);

Test12(c);
end.