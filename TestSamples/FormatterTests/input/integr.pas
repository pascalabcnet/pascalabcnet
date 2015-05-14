
uses System;

function f(x:real):real;
 begin
  f:=Math.Exp(x);
 end;

function abs(x : real): real;
begin
 abs := Math.Abs(x);
end;

function INTEGR(a,b,eps:real;k,NMAX:integer):real;
 var
  IT,N:integer;
  SH,SH1,S0,S1,S2,x,h,S:real;
  is_vch:boolean;
  l : integer;

 begin
  h:=(b-a)/k;
  SH:=0;
  IT:=0;
  S0:=f(a)+f(b);

  SH1:=0;

  S1:=0;
  S2:=0;
  x:=a+2*h;

  while x<b-h/2 do
   begin
    S2:=S2+f(x);
    x:=x+2*h;
   end;
  S2:=S1+S2;
 SH:=SH1;
 IT:=IT+1;

 S1:=0;
 x:=a+h;

 while b-h/2>x do
  begin
   S1:=S1+f(x);
   x:=x+2*h;
  end;

 SH1:=(S0+4*S1+2*S2)*h/3;

 h:=h/2; l := 3;
 
while (abs(SH-SH1)/15>eps) and (IT<=NMAX) do
begin
 S2:=S1+S2;
 SH:=SH1;
 IT:=IT+1;

 S1:=0;
 x:=a+h;

 while x<b-h/2 do
  begin
   S1:=S1+f(x);
   x:=x+2*h;
  end;

 SH1:=(S0+4*S1+2*S2)*h/3;

 h:=h/2;
end;

if abs(SH-SH1)/15 <= eps then
   is_vch:=true
  else
   is_vch:=false;
   S:=SH1;
   INTEGR:=S;
end;

var
 a,b,eps:real;
 MAXITER,k:integer;
 S:real;
begin
  a:=0;
  b:=1.0;
  eps:=0.0001;
  k := 5;
  MAXITER:=10000;
  S:=INTEGR(a,b,eps,k,MAXITER); 
  writeln(S);
end.
