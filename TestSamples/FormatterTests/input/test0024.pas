procedure Test2;
var f1 : file of char;
    f2 : file of integer;
    f3 : file of byte;
    f4 : file of smallint;
    f5 : file of shortint;
    f6 : file of word;
    f7 : file of longword;
    f8 : file of int64;
    f9 : file of uint64;
    f10 : file of real;
    f11 : file of single;
    //f12 : file of string;
    
    c : char;
    i : integer;
    b : byte;
    sm : smallint;
    sh : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    r : real;
    sn : single;
  
procedure Nested;
begin
Assign(f1,'test.dat');
Rewrite(f1);
Write(f1,'a');
Write(f1,'d');
Close(f1);
Reset(f1);
Read(f1,c); assert(c='a');
Read(f1,c); assert(c='d');
Close(f1);

Assign(f2,'test.dat');
Rewrite(f2);
Write(f2,35);
Write(f2,36);
Close(f2);
Reset(f2);
Read(f2,i); assert(i=35);
Read(f2,i); assert(i=36);
Close(f2);

Assign(f3,'test.dat');
Rewrite(f3);
Write(f3,35);
Write(f3,36);
Close(f3);
Reset(f3);
Read(f3,b); assert(b=35);
Read(f3,b); assert(b=36);
Close(f3);

Assign(f4,'test.dat');
Rewrite(f4);
Write(f4,35);
Write(f4,36);
Close(f4);
Reset(f4);
Read(f4,sm); assert(sm=35);
Read(f4,sm); assert(sm=36);
Close(f4);

Assign(f5,'test.dat');
Rewrite(f5);
Write(f5,35);
Write(f5,36);
Close(f5);
Reset(f5);
Read(f5,sh); assert(sh=35);
Read(f5,sh); assert(sh=36);
Close(f5);

Assign(f6,'test.dat');
Rewrite(f6);
Write(f6,35);
Write(f6,36);
Close(f6);
Reset(f6);
Read(f6,w); assert(w=35);
Read(f6,w); assert(w=36);
Close(f6);

Assign(f7,'test.dat');
Rewrite(f7);
Write(f7,35);
Write(f7,36);
Close(f7);
Reset(f7);
Read(f7,lw); assert(lw=35);
Read(f7,lw); assert(lw=36);
Close(f7);

Assign(f8,'test.dat');
Rewrite(f8);
Write(f8,35);
Write(f8,36);
Close(f8);
Reset(f8);
Read(f8,li); assert(li=35);
Read(f8,li); assert(li=36);
Close(f8);

Assign(f9,'test.dat');
Rewrite(f9);
Write(f9,35);
Write(f9,36);
Close(f9);
Reset(f9);
Read(f9,ui); assert(ui=35);
Read(f9,ui); assert(ui=36);
Close(f9);

Assign(f10,'test.dat');
Rewrite(f10);
Write(f10,3.5);
Write(f10,3.6);
Close(f10);
Reset(f10);
Read(f10,r); assert(r=3.5);
Read(f10,r); assert(r=3.6);
Close(f10);

Assign(f11,'test.dat');
Rewrite(f11);
Write(f11,3.5);
Close(f11);
Reset(f11);
Read(f11,sn); assert(sn=3.5);
Close(f11);
end;

begin
Assign(f1,'test.dat');
Rewrite(f1);
Write(f1,'a');
Write(f1,'d');
Close(f1);
Reset(f1);
Read(f1,c); assert(c='a');
Read(f1,c); assert(c='d');
Close(f1);

Assign(f2,'test.dat');
Rewrite(f2);
Write(f2,35);
Write(f2,36);
Close(f2);
Reset(f2);
Read(f2,i); assert(i=35);
Read(f2,i); assert(i=36);
Close(f2);

Assign(f3,'test.dat');
Rewrite(f3);
Write(f3,35);
Write(f3,36);
Close(f3);
Reset(f3);
Read(f3,b); assert(b=35);
Read(f3,b); assert(b=36);
Close(f3);

Assign(f4,'test.dat');
Rewrite(f4);
Write(f4,35);
Write(f4,36);
Close(f4);
Reset(f4);
Read(f4,sm); assert(sm=35);
Read(f4,sm); assert(sm=36);
Close(f4);

Assign(f5,'test.dat');
Rewrite(f5);
Write(f5,35);
Write(f5,36);
Close(f5);
Reset(f5);
Read(f5,sh); assert(sh=35);
Read(f5,sh); assert(sh=36);
Close(f5);

Assign(f6,'test.dat');
Rewrite(f6);
Write(f6,35);
Write(f6,36);
Close(f6);
Reset(f6);
Read(f6,w); assert(w=35);
Read(f6,w); assert(w=36);
Close(f6);

Assign(f7,'test.dat');
Rewrite(f7);
Write(f7,35);
Write(f7,36);
Close(f7);
Reset(f7);
Read(f7,lw); assert(lw=35);
Read(f7,lw); assert(lw=36);
Close(f7);

Assign(f8,'test.dat');
Rewrite(f8);
Write(f8,35);
Write(f8,36);
Close(f8);
Reset(f8);
Read(f8,li); assert(li=35);
Read(f8,li); assert(li=36);
Close(f8);

Assign(f9,'test.dat');
Rewrite(f9);
Write(f9,35);
Write(f9,36);
Close(f9);
Reset(f9);
Read(f9,ui); assert(ui=35);
Read(f9,ui); assert(ui=36);
Close(f9);

Assign(f10,'test.dat');
Rewrite(f10);
Write(f10,3.5);
Write(f10,3.6);
Close(f10);
Reset(f10);
Read(f10,r); assert(r=3.5);
Read(f10,r); assert(r=3.6);
Close(f10);

Assign(f11,'test.dat');
Rewrite(f11);
Write(f11,3.5);
Close(f11);
Reset(f11);
Read(f11,sn); assert(sn=3.5);
Close(f11);
Nested;
end;

procedure Test;
var f1 : file of char;
    f2 : file of integer;
    f3 : file of byte;
    f4 : file of smallint;
    f5 : file of shortint;
    f6 : file of word;
    f7 : file of longword;
    f8 : file of int64;
    f9 : file of uint64;
    f10 : file of real;
    f11 : file of single;
    //f12 : file of string;
    
    c : char;
    i : integer;
    b : byte;
    sm : smallint;
    sh : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    r : real;
    sn : single;
    
begin
Assign(f1,'test.dat');
Rewrite(f1);
Write(f1,'a');
Write(f1,'d');
Close(f1);
Reset(f1);
Read(f1,c); assert(c='a');
Read(f1,c); assert(c='d');
Close(f1);

Assign(f2,'test.dat');
Rewrite(f2);
Write(f2,35);
Write(f2,36);
Close(f2);
Reset(f2);
Read(f2,i); assert(i=35);
Read(f2,i); assert(i=36);
Close(f2);

Assign(f3,'test.dat');
Rewrite(f3);
Write(f3,35);
Write(f3,36);
Close(f3);
Reset(f3);
Read(f3,b); assert(b=35);
Read(f3,b); assert(b=36);
Close(f3);

Assign(f4,'test.dat');
Rewrite(f4);
Write(f4,35);
Write(f4,36);
Close(f4);
Reset(f4);
Read(f4,sm); assert(sm=35);
Read(f4,sm); assert(sm=36);
Close(f4);

Assign(f5,'test.dat');
Rewrite(f5);
Write(f5,35);
Write(f5,36);
Close(f5);
Reset(f5);
Read(f5,sh); assert(sh=35);
Read(f5,sh); assert(sh=36);
Close(f5);

Assign(f6,'test.dat');
Rewrite(f6);
Write(f6,35);
Write(f6,36);
Close(f6);
Reset(f6);
Read(f6,w); assert(w=35);
Read(f6,w); assert(w=36);
Close(f6);

Assign(f7,'test.dat');
Rewrite(f7);
Write(f7,35);
Write(f7,36);
Close(f7);
Reset(f7);
Read(f7,lw); assert(lw=35);
Read(f7,lw); assert(lw=36);
Close(f7);

Assign(f8,'test.dat');
Rewrite(f8);
Write(f8,35);
Write(f8,36);
Close(f8);
Reset(f8);
Read(f8,li); assert(li=35);
Read(f8,li); assert(li=36);
Close(f8);

Assign(f9,'test.dat');
Rewrite(f9);
Write(f9,35);
Write(f9,36);
Close(f9);
Reset(f9);
Read(f9,ui); assert(ui=35);
Read(f9,ui); assert(ui=36);
Close(f9);

Assign(f10,'test.dat');
Rewrite(f10);
Write(f10,3.5);
Write(f10,3.6);
Close(f10);
Reset(f10);
Read(f10,r); assert(r=3.5);
Read(f10,r); assert(r=3.6);
Close(f10);

Assign(f11,'test.dat');
Rewrite(f11);
Write(f11,3.5);
Close(f11);
Reset(f11);
Read(f11,sn); assert(sn=3.5);
Close(f11);
end;

var f1 : file of char;
    f2 : file of integer;
    f3 : file of byte;
    f4 : file of smallint;
    f5 : file of shortint;
    f6 : file of word;
    f7 : file of longword;
    f8 : file of int64;
    f9 : file of uint64;
    f10 : file of real;
    f11 : file of single;
    //f12 : file of string;
    
    c : char;
    i : integer;
    b : byte;
    sm : smallint;
    sh : shortint;
    w : word;
    lw : longword;
    li : int64;
    ui : uint64;
    r : real;
    sn : single;
    
begin
Assign(f1,'test.dat');
Rewrite(f1);
Write(f1,'a');
Write(f1,'b');
Close(f1);
Reset(f1);
Read(f1,c); assert(c='a');
Read(f1,c); assert(c='b');
Close(f1);

Assign(f2,'test.dat');
Rewrite(f2);
Write(f2,35);
Write(f2,36);
Close(f2);
Reset(f2);
Read(f2,i); assert(i=35);
Read(f2,i); assert(i=36);
Close(f2);

Assign(f3,'test.dat');
Rewrite(f3);
Write(f3,35);
Write(f3,36);
Close(f3);
Reset(f3);
Read(f3,b); assert(b=35);
Read(f3,b); assert(b=36);
Close(f3);

Assign(f4,'test.dat');
Rewrite(f4);
Write(f4,35);
Write(f4,36);
Close(f4);
Reset(f4);
Read(f4,sm); assert(sm=35);
Read(f4,sm); assert(sm=36);
Close(f4);

Assign(f5,'test.dat');
Rewrite(f5);
Write(f5,35);
Write(f5,36);
Close(f5);
Reset(f5);
Read(f5,sh); assert(sh=35);
Read(f5,sh); assert(sh=36);
Close(f5);

Assign(f6,'test.dat');
Rewrite(f6);
Write(f6,35);
Write(f6,36);
Close(f6);
Reset(f6);
Read(f6,w); assert(w=35);
Read(f6,w); assert(w=36);
Close(f6);

Assign(f7,'test.dat');
Rewrite(f7);
Write(f7,35);
Write(f7,36);
Close(f7);
Reset(f7);
Read(f7,lw); assert(lw=35);
Read(f7,lw); assert(lw=36);
Close(f7);

Assign(f8,'test.dat');
Rewrite(f8);
Write(f8,35);
Write(f8,36);
Close(f8);
Reset(f8);
Read(f8,li); assert(li=35);
Read(f8,li); assert(li=36);
Close(f8);

Assign(f9,'test.dat');
Rewrite(f9);
Write(f9,35);
Write(f9,36);
Close(f9);
Reset(f9);
Read(f9,ui); assert(ui=35);
Read(f9,ui); assert(ui=36);
Close(f9);

Assign(f10,'test.dat');
Rewrite(f10);
Write(f10,3.5);
Write(f10,3.6);
Close(f10);
Reset(f10);
Read(f10,r); assert(r=3.5);
Read(f10,r); assert(r=3.6);
Close(f10);

Assign(f11,'test.dat');
Rewrite(f11);
Write(f11,3.5);
Close(f11);
Reset(f11);
Read(f11,sn); assert(sn=3.5);
Close(f11);
Test;
Test2;
end.