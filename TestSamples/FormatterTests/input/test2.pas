

Type Te=class
 function GetIndex(i:integer):integer;
 procedure SetIndex(i,j:integer);
 property index[i:integer]:integer read GetIndex write SetIndex;default;
 constructor create;
 begin
 
 end;
End;

Type Tc=array[1..10] of Te;

Type Tb=class
 function c:Tc;
 constructor create;
 begin

 end;
End;

Type Ta=class
 function GetB:Tb;
 procedure SetB(bb:Tb);
 property b: Tb read GetB write SetB;
 constructor create;
 begin

 end;
End;

function Te.GetIndex(i:integer):integer;
begin
 GetIndex:=88;
end;


procedure Te.SetIndex(i,j:integer);
begin

end;

function Tb.c:Tc;
var
 cc:Tc;
begin
 cc[1]:=Te.create;
 c:=cc;
end;

function Ta.GetB:Tb;
var
 b:Tb;
begin
 b:=Tb.create;
 GetB:=b;
end;

procedure Ta.SetB(bb:Tb);
begin

end;

var
i:integer;
a:Ta;

begin

a:=Ta.create;

i:=a.b.c[1][2];
writeln(i);

end.

