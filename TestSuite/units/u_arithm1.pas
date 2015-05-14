unit u_arithm1;
procedure Test;
var a : integer;
    b : byte;
    c : char;
    d : smallint;
    e : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    
begin
 a := 456; assert(a = 456);
 b := 124; assert(b = 124);
 c := 'd'; assert(c = 'd');
 d := 567; assert(d = 567);
 e := -78; assert(e = -78);
 w := 7457; assert(w = 7457);
 lw := 312344; assert(lw = 312344);
 li := -1000000; assert(li = -1000000);
 ui := 6778778778; assert(ui = 6778778778);
 
 a := 20; b := 40; assert(a+b = 60);
 a := 20; d := 40; assert(a+d = 60);
 a := 20; e := 40; assert(a+e = 60);
 a := 20; w := 40; assert(a+w = 60);
 a := 20; lw := 40; assert(a+lw = 60);
 a := 20; li := 40; assert(a+li = 60);
 a := 20; ui := 40; assert(a+ui = 60);
 b := 20; b := 40; assert(b+b = 80);
 b := 20; d := 40; assert(b+d = 60);
 b := 20; e := 40; assert(b+e = 60);
 b := 20; w := 40; assert(b+w = 60);
 b := 20; lw := 40; assert(b+lw = 60);
 b := 20; li := 40; assert(b+li = 60);
 b := 20; ui := 40; assert(b+ui = 60);
 
 d := 20; b := 40; assert(d+b = 60);
 d := 20; d := 40; assert(d+d = 80);
 d := 20; e := 40; assert(d+e = 60);
 d := 20; w := 40; assert(d+w = 60);
 d := 20; lw := 40; assert(d+lw = 60);
 d := 20; li := 40; assert(d+li = 60);
 d := 20; ui := 40; assert(d+ui = 60);
 
 w := 20; b := 40; assert(w+b = 60);
 w := 20; d := 40; assert(w+d = 60);
 w := 20; e := 40; assert(w+e = 60);
 w := 20; w := 40; assert(w+w = 80);
 w := 20; lw := 40; assert(w+lw = 60);
 w := 20; li := 40; assert(w+li = 60);
 w := 20; ui := 40; assert(w+ui = 60);
 
 e := 20; b := 40; assert(a+b = 60);
 e := 20; d := 40; assert(a+d = 60);
 e := 20; e := 40; assert(e+e = 80);
 e := 20; w := 40; assert(e+w = 60);
 e := 20; lw := 40; assert(e+lw = 60);
 e := 20; li := 40; assert(e+li = 60);
 e := 20; ui := 40; assert(e+ui = 60);
 
 lw := 20; b := 40; assert(lw+b = 60);
 lw := 20; d := 40; assert(lw+d = 60);
 lw := 20; e := 40; assert(lw+e = 60);
 lw := 20; w := 40; assert(lw+w = 60);
 lw := 20; lw := 40; assert(lw+lw = 80);
 lw := 20; li := 40; assert(lw+li = 60);
 lw := 20; ui := 40; assert(lw+ui = 60);
 
 li := 20; b := 40; assert(li+b = 60);
 li := 20; d := 40; assert(li+d = 60);
 li := 20; e := 40; assert(li+e = 60);
 li := 20; w := 40; assert(li+w = 60);
 li := 20; lw := 40; assert(li+lw = 60);
 li := 20; li := 40; assert(li+li = 80);
 li := 20; ui := 40; assert(li+ui = 60);
 
 ui := 20; b := 40; assert(ui+b = 60);
 ui := 20; d := 40; assert(ui+d = 60);
 ui := 20; e := 40; assert(ui+e = 60);
 ui := 20; w := 40; assert(ui+w = 60);
 ui := 20; lw := 40; assert(ui+lw = 60);
 ui := 20; li := 40; assert(ui+li = 60);
 ui := 20; ui := 40; assert(ui+ui = 80);
 
 a := 1; a := a + 2; assert(a=3);
 b := 1; b := b + 2; assert(b=3);
 d := 1; d := d + 2; assert(d=3);
 w := 1; w := w + 2; assert(w=3);
 e := 1; e := e + 2; assert(e=3);
 lw := 1; lw := lw + 2; assert(lw=3);
 li := 1; li := li + 2; assert(li=3);
 ui := 1; ui := ui + 2; assert(ui=3);
end;

procedure Test2;
var a : integer;
    b : byte;
    c : char;
    d : smallint;
    e : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    
procedure Nested;
begin
 a := 456; assert(a = 456);
 b := 124; assert(b = 124);
 c := 'd'; assert(c = 'd');
 d := 567; assert(d = 567);
 e := -78; assert(e = -78);
 w := 7457; assert(w = 7457);
 lw := 312344; assert(lw = 312344);
 li := -1000000; assert(li = -1000000);
 ui := 6778778778; assert(ui = 6778778778);
 
 a := 20; b := 40; assert(a+b = 60);
 a := 20; d := 40; assert(a+d = 60);
 a := 20; e := 40; assert(a+e = 60);
 a := 20; w := 40; assert(a+w = 60);
 a := 20; lw := 40; assert(a+lw = 60);
 a := 20; li := 40; assert(a+li = 60);
 a := 20; ui := 40; assert(a+ui = 60);
 
 b := 20; b := 40; assert(b+b = 80);
 b := 20; d := 40; assert(b+d = 60);
 b := 20; e := 40; assert(b+e = 60);
 b := 20; w := 40; assert(b+w = 60);
 b := 20; lw := 40; assert(b+lw = 60);
 b := 20; li := 40; assert(b+li = 60);
 b := 20; ui := 40; assert(b+ui = 60);
 
 d := 20; b := 40; assert(d+b = 60);
 d := 20; d := 40; assert(d+d = 80);
 d := 20; e := 40; assert(d+e = 60);
 d := 20; w := 40; assert(d+w = 60);
 d := 20; lw := 40; assert(d+lw = 60);
 d := 20; li := 40; assert(d+li = 60);
 d := 20; ui := 40; assert(d+ui = 60);
 
 w := 20; b := 40; assert(w+b = 60);
 w := 20; d := 40; assert(w+d = 60);
 w := 20; e := 40; assert(w+e = 60);
 w := 20; w := 40; assert(w+w = 80);
 w := 20; lw := 40; assert(w+lw = 60);
 w := 20; li := 40; assert(w+li = 60);
 w := 20; ui := 40; assert(w+ui = 60);
 
 e := 20; b := 40; assert(a+b = 60);
 e := 20; d := 40; assert(a+d = 60);
 e := 20; e := 40; assert(e+e = 80);
 e := 20; w := 40; assert(e+w = 60);
 e := 20; lw := 40; assert(e+lw = 60);
 e := 20; li := 40; assert(e+li = 60);
 e := 20; ui := 40; assert(e+ui = 60);
 
 lw := 20; b := 40; assert(lw+b = 60);
 lw := 20; d := 40; assert(lw+d = 60);
 lw := 20; e := 40; assert(lw+e = 60);
 lw := 20; w := 40; assert(lw+w = 60);
 lw := 20; lw := 40; assert(lw+lw = 80);
 lw := 20; li := 40; assert(lw+li = 60);
 lw := 20; ui := 40; assert(lw+ui = 60);
 
 li := 20; b := 40; assert(li+b = 60);
 li := 20; d := 40; assert(li+d = 60);
 li := 20; e := 40; assert(li+e = 60);
 li := 20; w := 40; assert(li+w = 60);
 li := 20; lw := 40; assert(li+lw = 60);
 li := 20; li := 40; assert(li+li = 80);
 li := 20; ui := 40; assert(li+ui = 60);
 
 ui := 20; b := 40; assert(ui+b = 60);
 ui := 20; d := 40; assert(ui+d = 60);
 ui := 20; e := 40; assert(ui+e = 60);
 ui := 20; w := 40; assert(ui+w = 60);
 ui := 20; lw := 40; assert(ui+lw = 60);
 ui := 20; li := 40; assert(ui+li = 60);
 ui := 20; ui := 40; assert(ui+ui = 80);
 
 a := 1; a := a + 2; assert(a=3);
 b := 1; b := b + 2; assert(b=3);
 d := 1; d := d + 2; assert(d=3);
 w := 1; w := w + 2; assert(w=3);
 e := 1; e := e + 2; assert(e=3);
 lw := 1; lw := lw + 2; assert(lw=3);
 li := 1; li := li + 2; assert(li=3);
 ui := 1; ui := ui + 2; assert(ui=3);
end;

begin
 a := 456; assert(a = 456);
 b := 124; assert(b = 124);
 c := 'd'; assert(c = 'd');
 d := 567; assert(d = 567);
 e := -78; assert(e = -78);
 w := 7457; assert(w = 7457);
 lw := 312344; assert(lw = 312344);
 li := -1000000; assert(li = -1000000);
 ui := 6778778778; assert(ui = 6778778778);
 
 a := 20; b := 40; assert(a+b = 60);
 a := 20; d := 40; assert(a+d = 60);
 a := 20; e := 40; assert(a+e = 60);
 a := 20; w := 40; assert(a+w = 60);
 a := 20; lw := 40; assert(a+lw = 60);
 a := 20; li := 40; assert(a+li = 60);
 a := 20; ui := 40; assert(a+ui = 60);
 
 b := 20; b := 40; assert(b+b = 80);
 b := 20; d := 40; assert(b+d = 60);
 b := 20; e := 40; assert(b+e = 60);
 b := 20; w := 40; assert(b+w = 60);
 b := 20; lw := 40; assert(b+lw = 60);
 b := 20; li := 40; assert(b+li = 60);
 b := 20; ui := 40; assert(b+ui = 60);
 
 d := 20; b := 40; assert(d+b = 60);
 d := 20; d := 40; assert(d+d = 80);
 d := 20; e := 40; assert(d+e = 60);
 d := 20; w := 40; assert(d+w = 60);
 d := 20; lw := 40; assert(d+lw = 60);
 d := 20; li := 40; assert(d+li = 60);
 d := 20; ui := 40; assert(d+ui = 60);
 
 w := 20; b := 40; assert(w+b = 60);
 w := 20; d := 40; assert(w+d = 60);
 w := 20; e := 40; assert(w+e = 60);
 w := 20; w := 40; assert(w+w = 80);
 w := 20; lw := 40; assert(w+lw = 60);
 w := 20; li := 40; assert(w+li = 60);
 w := 20; ui := 40; assert(w+ui = 60);
 
 e := 20; b := 40; assert(a+b = 60);
 e := 20; d := 40; assert(a+d = 60);
 e := 20; e := 40; assert(e+e = 80);
 e := 20; w := 40; assert(e+w = 60);
 e := 20; lw := 40; assert(e+lw = 60);
 e := 20; li := 40; assert(e+li = 60);
 e := 20; ui := 40; assert(e+ui = 60);
 
 lw := 20; b := 40; assert(lw+b = 60);
 lw := 20; d := 40; assert(lw+d = 60);
 lw := 20; e := 40; assert(lw+e = 60);
 lw := 20; w := 40; assert(lw+w = 60);
 lw := 20; lw := 40; assert(lw+lw = 80);
 lw := 20; li := 40; assert(lw+li = 60);
 lw := 20; ui := 40; assert(lw+ui = 60);
 
 li := 20; b := 40; assert(li+b = 60);
 li := 20; d := 40; assert(li+d = 60);
 li := 20; e := 40; assert(li+e = 60);
 li := 20; w := 40; assert(li+w = 60);
 li := 20; lw := 40; assert(li+lw = 60);
 li := 20; li := 40; assert(li+li = 80);
 li := 20; ui := 40; assert(li+ui = 60);
 
 ui := 20; b := 40; assert(ui+b = 60);
 ui := 20; d := 40; assert(ui+d = 60);
 ui := 20; e := 40; assert(ui+e = 60);
 ui := 20; w := 40; assert(ui+w = 60);
 ui := 20; lw := 40; assert(ui+lw = 60);
 ui := 20; li := 40; assert(ui+li = 60);
 ui := 20; ui := 40; assert(ui+ui = 80);
 
 a := 1; a := a + 2; assert(a=3);
 b := 1; b := b + 2; assert(b=3);
 d := 1; d := d + 2; assert(d=3);
 w := 1; w := w + 2; assert(w=3);
 e := 1; e := e + 2; assert(e=3);
 lw := 1; lw := lw + 2; assert(lw=3);
 li := 1; li := li + 2; assert(li=3);
 ui := 1; ui := ui + 2; assert(ui=3);
 Nested;
end;

var a : integer;
    b : byte;
    c : char;
    d : smallint;
    e : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    
begin
 a := 456; assert(a = 456);
 b := 124; assert(b = 124);
 c := 'd'; assert(c = 'd');
 d := 567; assert(d = 567);
 e := -78; assert(e = -78);
 w := 7457; assert(w = 7457);
 lw := 312344; assert(lw = 312344);
 li := -1000000; assert(li = -1000000);
 ui := 6778778778; assert(ui = 6778778778);
 
 a := 20; b := 40; assert(a+b = 60);
 a := 20; d := 40; assert(a+d = 60);
 a := 20; e := 40; assert(a+e = 60);
 a := 20; w := 40; assert(a+w = 60);
 a := 20; lw := 40; assert(a+lw = 60);
 a := 20; li := 40; assert(a+li = 60);
 a := 20; ui := 40; assert(a+ui = 60);
 
 b := 20; b := 40; assert(b+b = 80);
 b := 20; d := 40; assert(b+d = 60);
 b := 20; e := 40; assert(b+e = 60);
 b := 20; w := 40; assert(b+w = 60);
 b := 20; lw := 40; assert(b+lw = 60);
 b := 20; li := 40; assert(b+li = 60);
 b := 20; ui := 40; assert(b+ui = 60);
 
 d := 20; b := 40; assert(d+b = 60);
 d := 20; d := 40; assert(d+d = 80);
 d := 20; e := 40; assert(d+e = 60);
 d := 20; w := 40; assert(d+w = 60);
 d := 20; lw := 40; assert(d+lw = 60);
 d := 20; li := 40; assert(d+li = 60);
 d := 20; ui := 40; assert(d+ui = 60);
 
 w := 20; b := 40; assert(w+b = 60);
 w := 20; d := 40; assert(w+d = 60);
 w := 20; e := 40; assert(w+e = 60);
 w := 20; w := 40; assert(w+w = 80);
 w := 20; lw := 40; assert(w+lw = 60);
 w := 20; li := 40; assert(w+li = 60);
 w := 20; ui := 40; assert(w+ui = 60);
 
 e := 20; b := 40; assert(a+b = 60);
 e := 20; d := 40; assert(a+d = 60);
 e := 20; e := 40; assert(e+e = 80);
 e := 20; w := 40; assert(e+w = 60);
 e := 20; lw := 40; assert(e+lw = 60);
 e := 20; li := 40; assert(e+li = 60);
 e := 20; ui := 40; assert(e+ui = 60);
 
 lw := 20; b := 40; assert(lw+b = 60);
 lw := 20; d := 40; assert(lw+d = 60);
 lw := 20; e := 40; assert(lw+e = 60);
 lw := 20; w := 40; assert(lw+w = 60);
 lw := 20; lw := 40; assert(lw+lw = 80);
 lw := 20; li := 40; assert(lw+li = 60);
 lw := 20; ui := 40; assert(lw+ui = 60);
 
 li := 20; b := 40; assert(li+b = 60);
 li := 20; d := 40; assert(li+d = 60);
 li := 20; e := 40; assert(li+e = 60);
 li := 20; w := 40; assert(li+w = 60);
 li := 20; lw := 40; assert(li+lw = 60);
 li := 20; li := 40; assert(li+li = 80);
 li := 20; ui := 40; assert(li+ui = 60);
 
 ui := 20; b := 40; assert(ui+b = 60);
 ui := 20; d := 40; assert(ui+d = 60);
 ui := 20; e := 40; assert(ui+e = 60);
 ui := 20; w := 40; assert(ui+w = 60);
 ui := 20; lw := 40; assert(ui+lw = 60);
 ui := 20; li := 40; assert(ui+li = 60);
 ui := 20; ui := 40; assert(ui+ui = 80);
 
 a := 1; a := a + a; assert(a=2);
 b := 1; b := b + b; assert(b=2);
 d := 1; d := d + d; assert(d=2);
 w := 1; w := w + w; assert(w=2);
 e := 1; e := e + e; assert(e=2);
 lw := 1; lw := lw + lw; assert(lw=2);
 li := 1; li := li + li; assert(li=2);
 ui := 1; ui := ui + ui; assert(ui=2);
 Test;
 Test2;
end.