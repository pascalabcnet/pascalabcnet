// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
unit DMZadan;

interface

procedure a1;
procedure a2;
procedure a3;
procedure a4;
procedure a5;
procedure a6;

procedure c1;
procedure c2;
procedure c3;
procedure c4;
procedure c5;
procedure c6;
procedure c7;
procedure c8;
procedure c9;
procedure c10;
procedure c11;
procedure c12;
procedure c13;
procedure c14;
procedure c15;
procedure c16;
procedure c17;
procedure c18;
procedure c19;
procedure c20;
procedure c21;
procedure c22;
procedure c23;
procedure c24;
procedure c25;
procedure c26;

procedure cc1;
procedure cc2;
procedure cc3;
procedure cc4;
procedure cc5;
procedure cc6;
procedure cc7;
procedure cc8;
procedure cc9;
procedure cc10;
procedure cc11;
procedure cc12;
procedure cc13;
procedure cc14;
procedure cc15;
procedure cc16;

procedure p1;
procedure p2;
procedure p3;
procedure p4;

procedure pp1;
procedure pp2;
procedure pp3;
procedure pp4;
procedure pp5;
procedure pp6;
procedure pp7;
procedure pp8;
procedure pp9;
procedure pp10;
procedure pp11;
procedure pp12;
procedure pp13;
procedure pp14;
procedure pp15;
procedure pp16;
procedure pp17;
procedure pp18;
procedure pp19;

procedure examen1;
procedure examen2;
procedure examen3;
procedure examen4;
procedure examen5;
procedure examen6;
procedure examen7;
procedure examen8;
procedure examen9;
procedure examen10;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses DMTaskMaker,DrawManField;

procedure rect(a1,a2: integer);
begin
  DoPenDown;
  DoOnVector(a2,0);
  DoOnVector(0,a1);
  DoOnVector(-a2,0);
  DoOnVector(0,-a1);
  DoPenUp;
end;

procedure kDoOnVector(a: integer);
begin
  DoPenDown;
  DoOnVector(a,0);
  DoOnVector(0,a);
  DoOnVector(-a,0);
  DoOnVector(0,-a);
  DoPenUp;
end;

procedure oct;
begin
  DoPenDown;
  DoOnVector(1,0);
  DoOnVector(1,1);
  DoOnVector(0,1);
  DoOnVector(-1,1);
  DoOnVector(-1,0);
  DoOnVector(-1,-1);
  DoOnVector(0,-1);
  DoOnVector(1,-1);
  DoPenUp;
end;

procedure oct1(a: integer);
begin
  DoPenDown;
  DoOnVector(a,0);DoOnVector(1,1);
  DoOnVector(0,a);DoOnVector(-1,1);
  DoOnVector(-a,0);DoOnVector(-1,-1);
  DoOnVector(0,-a);DoOnVector(1,-1);
  DoPenUp;
end;

procedure oct2(a: integer);
begin
  DoPenDown;
  DoOnVector(1,0);DoOnVector(a,a);
  DoOnVector(0,1);DoOnVector(-a,a);
  DoOnVector(-1,0);DoOnVector(-a,-a);
  DoOnVector(0,-1);DoOnVector(a,-a);
  DoPenUp;
end;

procedure parket1;
begin
  DoPenDown;
  DoOnVector(2,2);DoOnVector(1,-1);
  DoOnVector(-2,-2);DoOnVector(-1,1);
  DoPenUp;
end;

procedure line(a,b: integer);
begin
  DoPenDown;
  DoOnVector(a,b);
  DoOnVector(-a,-b);
  DoPenUp;
end;

procedure kvDoOnVector(a,b,c: integer);
begin
  kDoOnVector(a);
  DoOnVector(b,c);
end;

procedure tri(a,b: integer);
begin
  DoPenDown;
  DoOnVector(a,0);
  DoOnVector(-a,b);
  DoOnVector(0,-b);
  DoPenUp;
end;

procedure center;
begin
  DoToPoint(15,10);
end;

procedure StandardField;
begin
  TaskText('Задание StandardField. Стандартное поле');
  Field(30,20);
end;

procedure a1;
begin
  TaskText('Задание a1. Начертить, используя команды ToPoint(2,3) и OnVector(1,2)');
  Field(5,6);
  DoToPoint(2,3);
  DoPenDown;
  DoOnVector(1,2);
end;

procedure a2;
var x0,y0: integer;
begin
  TaskText('Задание a2. Использовать четыре раза команду ToPoint(5,4)и команды OnVector');
  Field(9,7);
  x0:=5;
  y0:=4;
  DoToPoint(x0,y0);
  DoPenDown;
  DoOnVector(3,1);
  DoToPoint(x0,y0);
  DoOnVector(2,-3);
  DoToPoint(x0,y0);
  DoPenDown;
  DoOnVector(-4,2);
  DoToPoint(x0,y0);
  DoPenDown;
  DoOnVector(-2,-2);
end;

procedure a3;
begin
  TaskText('Задание a3. Начертите указанные линии');
  Field(11,6);
  DoToPoint(2,1);
  DoPenDown; DoOnVector(4,0);
  DoPenUp;
  DoToPoint(3,2);
  DoPenDown; DoOnVector(0,3);
  DoPenUp;
  DoToPoint(4,4);
  DoPenDown; DoOnVector(5,-1);
  DoPenUp;
end;

procedure a4;
begin
  TaskText('Задание a4. Начертить квадраты');
  Field(9,7);
  DoToPoint(3,1);
  kDoOnVector(1);
  DoToPoint(5,3);
  kDoOnVector(3);
  DoToPoint(1,3);
  kDoOnVector(2);
end;

procedure a5;
begin
  TaskText('Задание a5. Начертить несколько прямоугольников');
  Field(8,8);
  DoToPoint(4,1);rect(2,3);
  DoToPoint(1,1);rect(5,1);
  DoToPoint(3,4);rect(3,4);
end;

procedure a6;
begin
  TaskText('Задание a6. Начертить цифры');
  Field(11,6);
  DoToPoint(2,2);
  DoOnVector(0,1);DoPenDown;DoOnVector(1,1);DoOnVector(0,-2);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoOnVector(1,0);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(1,0);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(0,2);DoPenUp;
  DoOnVector(1,0);
end;

procedure c1;
var i: integer;
begin
  TaskText('Задание c1. Начертить, используя цикл');
  Field(17,5);
  DoToPoint(1,3);
  DoPenDown;
  for i:=1 to 15 do
  begin
    DoOnVector(0,-1);
    DoOnVector(1,1);
  end;
  DoPenUp;
end;

procedure c2;
var i: integer;
begin
  TaskText('Задание c2. Начертить, используя цикл');
  Field(22,5);
  DoToPoint(1,3);
  DoPenDown;
  for i:=1 to 10 do
  begin
    DoOnVector(1,-1);
    DoOnVector(1,1);
  end;
  DoPenUp;
end;

procedure c3;
var i: integer;
begin
  TaskText('Задание c3. Начертить, используя цикл');
  Field(18,7);
  DoToPoint(2,5);
  for i:=1 to 15 do
  begin
    line(0,-3);
    DoOnVector(1,0);
  end
end;

procedure c4;
var i: integer;
begin
  TaskText('Задание c4. Начертить, используя цикл');
  Field(21,5);
  DoToPoint(1,2);
  for i:=1 to 10 do
    kvDoOnVector(1,2,0);
end;

procedure c5;
var i: integer;
begin
  TaskText('Задание c5. Начертить, используя цикл');
  Field(12,12);
  DoToPoint(1,11);
  for i:=1 to 10 do
  begin
    DoPenDown;
    DoOnVector(1,0);
    DoOnVector(-1,-1);
    DoOnVector(0,1);
    DoPenUp;
    DoOnVector(1,-1);
  end
end;

procedure c6;
var i: integer;
begin
  TaskText('Задание c6. Начертить, используя цикл');
  Field(13,14);
  DoToPoint(1,3);
  for i:=1 to 6 do
  begin
    DoPenDown;
    DoOnVector(1,0);
    DoOnVector(0,-2);
    DoOnVector(-1,0);
    DoOnVector(0,2);
    DoPenUp;
    DoOnVector(2,2)
  end
end;

procedure c7;
var i: integer;
begin
  TaskText('Задание c7. Начертить, используя цикл');
  Field(30,20);
  DoToPoint(2,1);
  for i:=1 to 10 do
    kvDoOnVector(5,2,1);
end;

procedure c8;
var i: integer;
begin
  TaskText('Задание c8. Начертить, используя цикл');
  Field(29,5);
  DoToPoint(2,1);
  DoPenDown;
  for i:=1 to 9 do
  begin
    oct;
    DoOnVector(3,0);
  end
end;

procedure c9;
var i: integer;
begin
  TaskText('Задание c9. Начертить, используя циклы');
  Field(25,14);
  DoToPoint(2,12);
  DoPenDown;
  for i:=1 to 10 do
  begin
    DoOnVector(1,0);
    DoOnVector(0,-1);
  end;
  DoOnVector(1,0);
  for i:=1 to 10 do
  begin
    DoOnVector(0,1);
    DoOnVector(1,0);
  end;
  DoPenUp;
end;

procedure c10;
var i: integer;
begin
  TaskText('Задание c10. Начертить, используя цикл');
  Field(14,12);
  DoToPoint(2,1);
  DoPenDown;
  for i:=1 to 10 do
  begin
    DoOnVector(0,i);
    DoOnVector(1,-i);
  end;
  DoPenUp;
end;

procedure c11;
var i: integer;
begin
  TaskText('Задание c11. Начертить, используя цикл');
  Field(14,12);
  DoToPoint(2,1);
  for i:=1 to 10 do
  begin
    DoPenDown;
    DoOnVector(0,i);
    DoPenUp;
    DoOnVector(1,-i);
  end;
end;

procedure c12;
var i: integer;
begin
  TaskText('Задание c12. Начертить, используя цикл');
  Field(14,12);
  DoToPoint(2,10);
  for i:=1 to 10 do
  begin
    DoPenDown;
    DoOnVector(i,0);
    DoPenUp;
    DoOnVector(-i,-1);
  end;
end;

procedure c13;
var i: integer;
begin
  TaskText('Задание c13. Начертите, используя цикл');
  Field(25,10);
  DoToPoint(2,2);
  DoPenDown;
  for i:=1 to 6 do
  begin
    DoOnVector(0,i);
    DoOnVector(i,-i);
  end;
  DoPenUp;
end;

procedure c14;
var i: integer;
begin
  TaskText('Задание c14. Начертить, используя цикл');
  Field(14,12);
  DoToPoint(2,1);
  for i:=1 to 10 do
    kDoOnVector(i);
end;

procedure c15;
var i: integer;
begin
  TaskText('Задание c15. Начертить, используя цикл');
  Field(30,11);
  DoToPoint(1,2);
  for i:=1 to 7 do
    kvDoOnVector(i,i,0);
end;

procedure c16;
var i: integer;
begin
  TaskText('Задание c16. Начертить, используя цикл');
  Field(30,17);
  DoToPoint(1,2);
  for i:=1 to 14 do
  begin
    DoPenDown;
    DoOnVector(2*i,0);
    DoPenUp;
    DoOnVector(-2*i,1);
  end;
end;

procedure c17;
var i: integer;
begin
  TaskText('Задание c17. Начертить, используя цикл');
  Field(30,17);
  DoToPoint(1,2);
  for i:=0 to 13 do
  begin
    DoPenDown;
    DoOnVector(2*i+1,0);
    DoPenUp;
    DoOnVector(-(2*i+1),1);
  end;
end;

procedure c18;
var i: integer;
begin
  TaskText('Задание c18. Начертить, используя цикл');
  Field(12,19);
  DoToPoint(2,9);
  for i:=1 to 9 do
  begin
    DoPenDown;
    DoOnVector(0,2*i-1);
    DoPenUp;
    DoOnVector(1,-2*i);
  end;
end;

procedure c19;
var i: integer;
begin
  TaskText('Задание c19. Начертить, используя цикл');
  Field(12,20);
  DoToPoint(10,10);
  DoPenDown;
  for i:=0 to 16 do
    line(-8,8-i);
  DoPenUp;
end;

procedure c20;
var i: integer;
begin
  TaskText('Задание c20. Начертить, используя цикл');
  Field(10,10);
  DoToPoint(2,2);
  for i:=0 to 6 do
  begin
    line(i,6-i);
    DoOnVector(0,1);
  end;
end;

procedure c21;
var i: integer;
begin
  TaskText('Задание c21. Начертить, используя цикл');
  Field(13,18);
  DoToPoint(1,1);DoPenDown;
  for i:=0 to 8 do
    line(10,2*i);
  DoPenUp;
end;

procedure c22;
var i: integer;
begin
  TaskText('Задание c22. Начертить, используя цикл');
  Field(19,19);
  DoToPoint(1,1);
  DoPenDown;
  for i:=0 to 16 do
  begin
    DoOnVector(i,16-i);
    DoToPoint(1,1);
  end;
  DoPenUp;
end;

procedure c23;
var i: integer;
begin
  TaskText('Задание c23. Начертить, используя цикл');
  Field(20,14);
  DoToPoint(1,7);
  DoPenDown;
  for i:=1 to 6 do
  begin
    DoOnVector(1,i);
    DoOnVector(0,-2*i);
    DoOnVector(1,i);
    DoOnVector(1,0);
  end;
  DoPenUp;
end;

procedure c24;
var i: integer;
begin
  TaskText('Задание c24. Начертить, используя цикл');
  Field(19,17);
  DoToPoint(9,8);
  DoPenDown;
  for i:=1 to 7 do
  begin
    kDoOnVector(2*i-1);
    DoOnVector(-1,-1);
  end;
  DoPenUp;
end;

procedure c25;
var i,k: integer;
begin
  TaskText('Задание c25. Начертить, используя цикл');
  Field(14,14);
  DoToPoint(1,7);
  k:=1;
  DoPenDown;
  for i:=1 to 12 do
  begin
    DoOnVector(1,i*k);
    k:=-k;
  end;
  DoPenUp;
end;

procedure c26;
var i: integer;
begin
  TaskText('Задание c26. Начертить график параболы');
  Field(26,6);
  DoPenDown;
  for i:=1 to 5 do
    DoToPoint(i*i,i);
end;

procedure cc1;
var i,j: integer;
begin
  TaskText('Задание cc1. Начертить, используя вложенные циклы');
  Field(22,6);
  DoToPoint(1,1);DoPenDown;
  for i:=1 to 4 do
  begin
    for j:=1 to 5 do
    begin
      DoOnVector(1,i);
      DoOnVector(0,-i);
    end;
  end;
  DoPenUp;
end;

procedure cc2;
var i,j: integer;
begin
  TaskText('Задание cc2. Начертить, используя вложенные циклы');
  Field(22,6);
  DoToPoint(1,1);DoPenDown;
  for i:=1 to 5 do
  begin
    for j:=1 to 4 do
    begin
      DoOnVector(1,j);
      DoOnVector(0,-j);
    end;
  end;
  DoPenUp;
end;

procedure cc3;
var i,j: integer;
begin
  TaskText('Задание cc3. Начертить, используя вложенные циклы');
  Field(30,9);
  DoToPoint(1,1);DoPenDown;
  for i:=1 to 7 do
  begin
    for j:=1 to i do
    begin
      DoOnVector(1,j);
      DoOnVector(0,-j);
    end;
  end;
  DoPenUp;
end;

procedure cc4;
var i,j: integer;
begin
  TaskText('Задание cc4. Начертить, используя вложенные циклы');
  Field(30,9);
  DoToPoint(1,1);
  for i:=1 to 7 do
  begin
    for j:=1 to i do
    begin
      DoPenDown;
      DoOnVector(0,i);
      DoPenUp;
      DoOnVector(1,-i);
    end;
  end;
end;

procedure cc5;
var i,j: integer;
begin
  TaskText('Задание cc5. Начертить, используя вложенные циклы');
  Field(16,8);
  DoToPoint(1,1);
  DoPenDown;
  for j:=1 to 5 do
  begin
    for i:=0 to 6 do
    begin
      DoPenDown;
      DoOnVector(2,i);
      DoPenUp;
      DoOnVector(-2,-i);
    end;
    DoOnVector(3,0);
  end;
  DoPenUp;
end;

procedure cc6;
var i,j: integer;
begin
  TaskText('Задание cc6. Начертить, используя вложенные циклы');
  Field(16,8);
  DoToPoint(1,1);
  DoPenDown;
  for j:=1 to 5 do
  begin
    for i:=j to 6 do
    begin
      DoPenDown;
      DoOnVector(2,i);
      DoPenUp;
      DoOnVector(-2,-i);
    end;
    DoOnVector(3,0);
  end;
  DoPenUp;
end;

procedure cc7;
var i,j: integer;
begin
  TaskText('Задание cc7. Начертить, используя вложенные циклы');
  Field(16,11);
  DoToPoint(1,1);
  DoPenDown;
  for j:=1 to 5 do
  begin
    for i:=j to 2*j-1 do
    begin
      DoPenDown;
      DoOnVector(2,i);
      DoPenUp;
      DoOnVector(-2,-i);
    end;
    DoOnVector(3,0);
  end;
  DoPenUp;
end;

procedure cc8;
var i,j: integer;
begin
  TaskText('Задание cc8. Начертить, используя вложенные циклы');
  Field(21,7);
  DoToPoint(1,1);
  DoPenDown;
  for j:=1 to 5 do
  begin
    for i:=0 to 5 do
    begin
      DoPenDown;
      DoOnVector(j,i);
      DoPenUp;
      DoOnVector(-j,-i);
    end;
    DoOnVector(j+1,0);
  end;
  DoPenUp;
end;

procedure cc9;
var i,j: integer;
begin
  TaskText('Задание cc9. Начертить, используя вложенные циклы');
  Field(18,18);
  for i:=0 to 3 do
  begin
    DoToPoint(1,1);
    DoPenDown;
    for j:=1 to 5 do
      DoOnVector(j,6-j-i);
    DoPenUp;
  end
end;

procedure cc10;
var i,j: integer;
begin
  TaskText('Задание cc10. Начертить, используя вложенные циклы');
  Field(15,11);
  DoToPoint(2,1);
  for j:=0 to 4 do
  begin
    for i:=0 to 10 do
    begin
      DoPenDown;
      DoOnVector(1,1);
      DoOnVector(0,-1);
      DoPenUp;
    end;
    DoOnVector(-11,2);
  end;
end;

procedure cc11;
var i,j: integer;
begin
  TaskText('Задание cc11. Начертить, используя вложенные циклы');
  Field(27,22);
  DoToPoint(2,21);
  for j:=1 to 7 do
  begin
    for i:=1 to 8 do
    begin
      DoPenDown;
      DoOnVector(2,0);
      DoOnVector(-1,-2);
      DoOnVector(-1,2);
      DoPenUp;
      DoOnVector(3,0);
    end;
    DoOnVector(-3*8,-3);
  end
end;

procedure cc12;
var i,j: integer;
begin
  TaskText('Задание cc12. Начертить, используя вложенные циклы');
  Field(13,11);
  DoToPoint(2,1);
  for j:=0 to 4 do
  begin
    for i:=0 to j do
    begin
      DoPenDown;
      DoOnVector(1,1);
      DoOnVector(0,-1);
      DoPenUp;
    end;
    DoOnVector(-j-1,2);
  end;
end;

procedure cc13;
var i,j: integer;
begin
  TaskText('Задание cc13. Начертить, используя вложенные циклы');
  Field(13,11);
  DoToPoint(2,1);
  for j:=0 to 4 do
  begin
    for i:=0 to 2*j do
    begin
      DoPenDown;
      DoOnVector(1,1);
      DoOnVector(0,-1);
      DoPenUp;
    end;
    DoOnVector(-2*j-1,2);
  end;
end;

procedure cc14;
var i,j: integer;
begin
  TaskText('Задание cc14. Начертить, используя вложенные циклы');
  Field(11,11);
  DoToPoint(1,2);
  for j:=0 to 4 do
  begin
    for i:=0 to 2*j do
    begin
      DoPenDown;
      DoOnVector(1,0);
      DoOnVector(0,-1);
      DoPenUp;
    end;
    DoOnVector(-2*j-1,2*j+3);
  end;
end;

procedure cc15;
var n,i,j: integer;
begin
  TaskText('Задание cc15. Начертить, используя вложенные циклы');
  Field(11,9);
  n:=8;
  for j:=0 to n do
  begin
    for i:=0 to n do
    begin
      DoToPoint(0,i);
      DoPenDown;
      DoToPoint(10,j);
      DoPenUp;
    end;
  end;
end;

procedure cc16;
var n,i,j: integer;
begin
  TaskText('Задание cc16. Начертить, используя вложенные циклы');
  Field(11,9);
  n:=8;
  for j:=0 to n div 2 do
  begin
    for i:=0 to n div 2-1 do
    begin
      DoToPoint(0,2*i+1);
      DoPenDown;
      DoToPoint(10,2*j);
      DoPenUp;
    end;
  end;
end;

procedure p1;
begin
  TaskText('Задание p1. Начертить цифры почтового индекса, составив для каждой процедуру');
  Field(23,6);
  DoToPoint(2,2);
  DoPenDown;DoOnVector(1,0);DoOnVector(0,2);DoOnVector(-1,0);DoOnVector(0,-2);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(0,1);DoPenDown;DoOnVector(1,1);DoOnVector(0,-2);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoOnVector(1,0);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(1,0);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(0,2);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(1,0);DoPenDown;DoOnVector(-1,0);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,0);DoPenUp;DoOnVector(1,2);
  DoOnVector(1,0);
  DoOnVector(1,0);DoPenDown;DoOnVector(-1,-1);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,1);DoOnVector(-1,0);DoOnVector(1,1);DoPenUp;
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(0,-1);DoPenUp;DoOnVector(1,2);
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(0,-2);DoOnVector(-1,0);DoOnVector(0,2);DoOnVector(0,-1);DoOnVector(1,0);DoPenUp;DoOnVector(0,1);
  DoOnVector(1,0);
  DoOnVector(1,-1);DoPenDown;DoOnVector(-1,0);DoOnVector(0,1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoPenUp;
end;

procedure p2;
  procedure okno(a,b: integer);
  begin
    rect(b,a);
    DoOnVector(1,1);
    rect(b-2,a div 2-1);
    rect(b-2,a div 2-1);
    DoOnVector(a div 2,0);
    rect(b-2-a div 2,a div 2-1);
    DoOnVector(0,b-1-a div 2);
    rect(a div 2-1,a div 2-1)
  end;
begin
  TaskText('Задание p2. Составить процедуру Win и начертить три окна');
  Field(38,16);
  DoToPoint(3,2);okno(9,12);
  DoToPoint(15,2);okno(9,12);
  DoToPoint(26,2);okno(9,12);
end;

procedure p3;
  procedure okno(a,b: integer);
  begin 
    rect(b,a);
    DoOnVector(1,1);
    rect(b-2,a div 2-1);
    rect(b-2,a div 2-1);
    DoOnVector(a div 2,0);
    rect(b-2-a div 2,a div 2-1);
    DoOnVector(0,b-1-a div 2);
    rect(a div 2-1,a div 2-1)
  end;
begin
  TaskText('Задание p3. Составить процедуру SmallHouse и начертить два дома. Использовать процедуру Win');
  Field(72,40);
  DoToPoint(4,4);rect(22,30);
  DoToPoint(8,9);okno(9,12);
  DoToPoint(21,9);okno(9,12);
  DoToPoint(4,26);
  DoPenDown;DoOnVector(15,8);DoOnVector(15,-8);
  DoPenUp;
  DoToPoint(38,4);rect(22,30);
  DoToPoint(42,9);okno(9,12);
  DoToPoint(55,9);okno(9,12);
  DoToPoint(38,26);
  DoPenDown;DoOnVector(15,8);DoOnVector(15,-8);
  DoPenUp;
end;

procedure p4;
  procedure okno1(a,b: integer);
  begin
    rect(b,a);
    DoOnVector(1,1);
    rect(b-2,a div 2-1);
    rect(b-2,a div 2-1);
    DoOnVector(a div 2,0);
    rect(b-2-a div 2,a div 2-1);
    DoOnVector(0,b-1-a div 2);
    rect(a div 2-1,a div 2-1);
    DoOnVector(a div 2,a div 2);
    DoOnVector(-a,-b);
  end;
  procedure dom3;
  begin
    DoOnVector(0,6);okno1(9,12);
    DoOnVector(12,0);okno1(9,12);
    DoOnVector(-12,16);okno1(9,12);
    DoOnVector(12,0);okno1(9,12);
    DoOnVector(-12,16);okno1(9,12);
    DoOnVector(12,0);okno1(9,12);
  end;
begin
  TaskText('Задание p4. Составить процедуру BigHouse и начертить два дома. Использовать процедуру Win');
  Field(83,70);
  DoToPoint(4,4);rect(56,33);
  DoToPoint(2,60);rect(2,37);
  DoToPoint(10,4);dom3;
  DoToPoint(46,4);rect(56,33);
  DoToPoint(44,60);rect(2,37);
  DoToPoint(52,4);dom3;
end;

procedure pp1;
begin
  TaskText('Задание pp1. Начертить квадраты, составив процедуру Square(a)');
  Field(16,11);
  DoToPoint(2,1); kDoOnVector(1);
  DoToPoint(8,8); kDoOnVector(2);
  DoToPoint(3,4); kDoOnVector(3);
  DoToPoint(10,2); kDoOnVector(4);
end;

procedure pp2;
var i: integer;
begin
  TaskText('Задание pp2. Начертить, используя процедуру Square(a)');
  Field(21,10);
  DoToPoint(2,1);
  for i:=1 to 8 do
    kDoOnVector(i);
  DoToPoint(18,8);
  for i:=1 to 8 do
  begin
    kDoOnVector(i);
    DoOnVector(-1,-1);
  end;
end;

procedure pp3;
var i: integer;
begin
  TaskText('Задание pp3. Начертить, используя процедуру Square(a)');
  Field(30,16);
  DoToPoint(1,1);
  for i:=1 to 7 do
  begin
    kDoOnVector(i);
    DoOnVector(i,1);
  end;
end;

procedure pp4;
var i: integer;
begin
  TaskText('Задание pp4. Начертить, используя процедуру Square(a)');
  Field(30,20);
  DoToPoint(14,9);
  for i:=1 to 8 do
  begin
    kDoOnVector(2*i-1);
    DoOnVector(-1,-1)
  end;
end;

procedure pp5;
begin
  TaskText('Задание pp5. Начертить, составив процедуру Rect(w,h), где w - ширина, а h - высота прямоугольника');
  Field(18,12);
  DoToPoint(2,1);rect(2,5);
  DoToPoint(12,3);rect(3,4);
  DoToPoint(4,6);rect(4,3);
  DoToPoint(9,4);rect(5,1);
end;

procedure pp6;
var i: integer;
begin
  TaskText('Задание pp6. Начертить, используя процедуру Rect(w,h)');
  Field(16,16);
  DoToPoint(2,2);
  for i:=1 to 12 do
  begin
    rect(i,1);
    DoOnVector(1,0);
  end
end;

procedure pp7;
var i: integer;
begin
  TaskText('Задание pp7. Начертить, используя процедуру Rect(w,h)');
  Field(30,9);
  DoToPoint(1,1);
  for i:=1 to 7 do
  begin
    rect(8-i,i);
    DoOnVector(i,0);
  end
end;

procedure pp8;
  procedure elka(aa: integer);
  var i: integer;
  begin
    DoPenDown;
    DoOnVector(0,aa+1);
    for i:=1 to aa do
    begin
      line(-1,-1);
      line(1,-1);
      DoOnVector(0,-1);
    end;
    DoOnVector(0,-1);
  end;
var j: integer;
begin
  TaskText('Задание pp8. Начертить, составив процедуру Fir(h), где h - высота елки');
  Field(28,14);
  DoToPoint(2,1);
  for j:=1 to 9 do
  begin
    elka(2+j);
    DoOnVector(3,0);
  end
end;

procedure pp9;
  procedure tri1(d: integer);
  begin
    DoPenDown;
    DoOnVector(-d,-d);
    DoOnVector(2*d,0);
    DoOnVector(-d,d);
    DoPenUp;
  end;
var j: integer;
begin
  TaskText('Задание pp9. Начертить, составив процедуру Tri(h), где h - высота треугольника. Рисование треугольника начинать с верхней точки');
  Field(14,17);
  DoToPoint(7,16);
  for j:=1 to 5 do
  begin
    tri1(j);
    DoOnVector(0,-j);
  end
end;

procedure pp10;
  procedure tri1(d: integer);
  begin
    DoPenDown;
    DoOnVector(-d,-d);
    DoOnVector(2*d,0);
    DoOnVector(-d,d);
    DoPenUp;
  end;
  procedure elka1(dd: integer);
  var j: integer;
  begin
    for j:=1 to dd do
    begin
      tri1(j);
      DoOnVector(0,-j);
    end
  end;
begin
  TaskText('Задание pp10. Используя процедуру Tri(a), составьте процедуру Fir1(n), где n - количество прямоугольников, образующих елку');
  Field(30,17);
  DoToPoint(5,11);
  elka1(4);
  DoToPoint(16,16);
  elka1(5);
  DoToPoint(26,7);
  elka1(3);
end;

procedure pp11;
begin
  TaskText('Задание pp11. Начертить, составив процедуру Triangle(a,b), где a и b - длины горизонтального и вертикального катетов');
  Field(26,11);
  DoToPoint(2,2);tri(3,5);DoToPoint(7,2);tri(2,7);DoToPoint(11,2);tri(4,3);DoToPoint(17,2);tri(7,4);
end;

procedure pp12;
begin
  TaskText('Задание pp12. Начертить, используя процедуру Triangle(a,b) с a=+3,-3; b=+5,-5');
  Field(21,9);
  DoToPoint(2,2); tri(3,5);
  DoOnVector(8,0); tri(-3,5);
  DoOnVector(1,5); tri(3,-5);
  DoOnVector(8,0); tri(-3,-5);
end;

procedure pp13;
  procedure sn(x,y: integer);
  begin
    DoPenDown;
    DoOnVector(x,3*y);
    DoOnVector(0,-y);
    DoOnVector(x,y);
    DoOnVector(-x,-2*y);
    DoOnVector(2*x,y);
    DoOnVector(-x,-y);
    DoOnVector(x,0);
    DoOnVector(-3*x,-y);
    DoPenUp;
  end;
begin
  TaskText('Задание pp13. Начертить, составив процедуру рисования лепестка. Использовать идею зеркального отражения');
  Field(19,11);
  DoToPoint(5,6);sn(1,1);sn(1,-1);sn(-1,1);sn(-1,-1);
  DoToPoint(14,6);sn(1,1);
  DoOnVector(-1,0);sn(-1,1);
  DoOnVector(0,-1);sn(-1,-1);
  DoOnVector(1,0);sn(1,-1);
end;

procedure pp14;
  procedure horiz(w,h: integer);
  var i: integer;
  begin
    for i:=1 to h do
      rect(i,w);
  end;
begin
  TaskText('Задание pp14. Начертить, составив процедуру Horiz(w,h), где w - ширина, а h - высота заштрихованного прямоугольника');
  Field(17,13);
  DoToPoint(2,2); horiz(8,5);
  DoToPoint(12,4); horiz(3,4);
  DoToPoint(5,8); horiz(5,3);
end;

procedure pp15;
  procedure vert(w,h: integer);
  var i: integer;
  begin
    for i:=1 to w do
      rect(h,i);
  end;
begin
  TaskText('Задание pp15. Начертить, составив процедуру Vert(w,h), где w - ширина, а h - высота заштрихованного прямоугольника');
  Field(17,13);
  DoToPoint(2,2); vert(8,5);
  DoToPoint(12,4); vert(3,4);
  DoToPoint(5,8); vert(5,3);
end;

procedure pp16;
  procedure setka(aa,bb: integer);
  var i: integer;
  begin
    for i:=0 to aa do
    begin
      line(bb,0);
      DoOnVector(0,1);
    end;
    DoOnVector(0,-aa-1);
    for i:=0 to bb do
    begin
      line(0,aa);
      DoOnVector(1,0);
    end;
  end;
begin
  TaskText('Задание pp16. Начертить, составив процедуру Setka(w,h), где w - ширина, а h - высота заштрихованного прямоугольника');
  Field(21,15);
  DoToPoint(1,1);setka(5,7);
  DoToPoint(2,7);setka(3,8);
  DoToPoint(9,2);setka(2,4);
  DoToPoint(12,6);setka(8,8);
end;

procedure pp17;
  procedure spir(a: integer);
  var i,j: integer;
  begin
    DoPenDown;
    i:=1;
    for j:=1 to a do
    begin
      DoOnVector(i,0);
      DoOnVector(0,i);
      i:=i+1;
      DoOnVector(-i,0);
      DoOnVector(0,-i);
      i:=i+1;
    end;
    DoPenUp;
  end;
begin
  TaskText('Задание pp17. Начертить, составив процедуру Spir(n), где n - количество витков спирали. Рисование спирали начинать из ее центра');
  Field(26,14);
  DoToPoint(7,7); spir(5);
  DoToPoint(20,6); spir(4);
end;

procedure pp18;
begin
  TaskText('Задание pp18. Начертить цифры почтового индекса, составив процедуру Digit(n). Использовать процедуры из задания DMp1');
  Field(23,12);
  DoToPoint(2,2);
  DoOnVector(1,1);DoPenDown;DoOnVector(-1,0);DoOnVector(0,1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,2);
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(0,-2);DoOnVector(-1,0);DoOnVector(0,2);DoOnVector(0,-1);DoOnVector(1,0);DoPenUp;DoOnVector(0,1);
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(0,-1);DoPenUp;DoOnVector(1,2);
  DoOnVector(1,0);
  DoOnVector(1,0);DoPenDown;DoOnVector(-1,-1);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,1);DoOnVector(-1,0);DoOnVector(1,1);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(1,0);DoPenDown;DoOnVector(-1,0);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,0);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(0,2);DoPenUp;
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(1,0);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(0,2);DoPenDown;DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoOnVector(1,0);DoPenUp;DoOnVector(0,2);
  DoOnVector(1,0);
  DoOnVector(0,-1);DoPenDown;DoOnVector(1,1);DoOnVector(0,-2);DoPenUp;
  DoOnVector(1,0);
  DoPenDown;DoOnVector(0,2);DoOnVector(1,0);DoOnVector(0,-2);DoOnVector(-1,0);DoPenUp;
  DoToPoint(2,6);
  DoPenDown;DoOnVector(1,1);DoOnVector(0,-2);DoPenUp;
  DoOnVector(1,2);
  DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(1,0);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,0);
  DoOnVector(1,2);DoPenDown;DoOnVector(-1,0);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,0);DoPenUp;DoOnVector(1,0);
  DoOnVector(1,2);
  DoPenDown;DoOnVector(1,0);DoOnVector(-1,-1);DoOnVector(0,-1);DoPenUp;DoOnVector(1,2);
  DoOnVector(1,-2);
  DoOnVector(1,1);DoPenDown;DoOnVector(-1,0);DoOnVector(0,1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoPenUp;DoOnVector(1,2);
  DoToPoint(2,10);
  DoOnVector(0,-2);
  DoPenDown;DoOnVector(0,2);DoOnVector(1,0);DoOnVector(0,-2);DoOnVector(-1,0);DoPenUp;
  DoOnVector(2,2);
  DoPenDown;DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(-1,-1);DoOnVector(1,0);DoPenUp;DoOnVector(0,2);
  DoOnVector(1,-2);
  DoOnVector(0,2);DoPenDown;DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,-1);DoOnVector(0,2);DoPenUp;
  DoOnVector(1,0);
  DoOnVector(1,0);DoPenDown;DoOnVector(-1,-1);DoOnVector(0,-1);DoOnVector(1,0);DoOnVector(0,1);DoOnVector(-1,0);DoOnVector(1,1);DoPenUp;
  DoOnVector(1,0);
  DoPenDown;DoOnVector(1,0);DoOnVector(0,-2);DoOnVector(-1,0);DoOnVector(0,2);DoOnVector(0,-1);DoOnVector(1,0);DoPenUp;DoOnVector(0,1);
end;

procedure pp19;
  procedure rect1(a1,b1: integer);
  begin
    rect(b1,a1);
  end;
  procedure win(a,b,w: integer);
  var b2: integer;
  begin
    b2:=b-a-w;
    rect1(2*a+3*w,b+2*w);
    DoOnVector(w,w);
    rect1(a,b);
    DoOnVector(a+w,0);
    rect1(a,b2);
    DoOnVector(0,b2+w);
    rect1(a,a);
    DoOnVector(-a-2*w,-b2-2*w);
  end;
begin
  TaskText('Задание pp19. Начертить, составив процедуру Win(a,b,w), где a,b - размеры большего стекла, w - ширина рамы');
  Field(36,21);
  DoToPoint(2,2);
  win(3,10,1);
  DoToPoint(13,2);
  win(3,13,2);
  DoToPoint(27,2);
  win(2,7,1);
end;

procedure examen1;
var i: integer;
begin
  TaskText('Задание examen1. Начертить, используя процедуру с параметром Triangle(a)');
  Field(28,10);
  DoToPoint(1,2);
  for i:=1 to 6 do
  begin
    tri(i,i);
    DoOnVector(i+1,0);
  end
end;

procedure examen2;
  procedure gaz(n: integer);
  var j: integer;
  begin
    for j:=1 to n do
    begin
      DoPenDown;
      DoOnVector(2,0);
      DoOnVector(-1,1);
      DoOnVector(-1,-1);
      DoOnVector(2,0);
      DoPenUp;
    end;
  end;
var i: integer;
begin
  TaskText('Задание examen2. Начертить, используя процедуру с параметром Gazon(n)');
  Field(31,17);
  for i:=1 to 7 do
  begin
    DoToPoint(1,2*i);
    gaz(2*i);
  end
end;

procedure examen3;
var i: integer;
begin
  TaskText('Задание examen3. Начертить, используя процедуру с параметром Stolb(h)');
  Field(20,20);
  DoToPoint(1,1);
  for i:=1 to 9 do
  begin
    rect(2*i,1);
    DoOnVector(2,0);
  end
end;

procedure examen4;
  procedure ttt(i: integer);
  begin
    DoPenDown;
    DoOnVector(i,i);
    DoOnVector(-2*i,0);
    DoOnVector(i,-i);
    DoPenUp;
  end;
var j: integer;
begin
  TaskText('Задание examen4. Начертить, используя процедуру с параметром Tri(a)');
  Field(30,18);
  DoToPoint(15,1);
  for j:=1 to 8 do
  begin
    ttt(j);
    DoOnVector(0,1);
  end
end;

procedure examen5;
  procedure zab(n: integer);
  var i: integer;
  begin
    for i:=1 to n do
    begin
      DoPenDown;
      DoOnVector(0,2);
      DoOnVector(0,-2);
      DoPenUp;
      DoOnVector(1,0);
    end;
  end;
begin
  TaskText('Задание examen5. Начертить, используя процедуру с параметром Palki(n)');
  Field(23,14);
  DoToPoint(2,2);
  zab(10);
  DoToPoint(2,6);
  zab(15);
  DoToPoint(2,10);
  zab(20);
end;

procedure examen6;
var i: integer;
begin
  TaskText('Задание examen6. Начертить, используя процедуру с параметрами Rect(a,b)');
  Field(30,10);
  DoToPoint(2,2);
  for i:=1 to 6 do
  begin
    rect(7-i,i);
    DoOnVector(i+1,0);
  end
end;

procedure examen7;
  procedure zab1(n: integer);
  var i: integer;
  begin
    for i:=1 to n do
    begin
      DoPenDown;
      DoOnVector(0,2);
      DoOnVector(1,1);
      DoOnVector(1,-1);
      DoOnVector(0,-2);
      DoPenUp;
      DoOnVector(1,0);
    end;
  end;
begin
  TaskText('Задание examen7. Начертить, используя процедуру с параметром Zabor(n)');
  Field(27,19);
  DoToPoint(2,2); zab1(5);
  DoToPoint(2,6); zab1(6);
  DoToPoint(2,10); zab1(7);
  DoToPoint(2,14); zab1(8);
end;

procedure examen8;
  procedure veer1(n: integer);
  var i: integer;
  begin
    for i:=0 to n do
    begin
      DoPenDown;
      DoOnVector(i,n-i);
      DoOnVector(-i,-n+i);
      DoPenUp;
    end;
  end;
begin
  TaskText('Задание examen8. Начертить, используя процедуру с параметром Veer(n)');
  Field(20,12);
  DoToPoint(2,2);
  veer1(6);
  DoToPoint(10,2);
  veer1(8);
end;

procedure examen9;
 procedure row(n: integer);
 var i: integer;
 begin
   for i:=1 to n do
   begin
     kDoOnVector(1);
     DoOnVector(2,0);
   end;
 end;
begin
  TaskText('Задание examen9. Начертить, используя процедуру row(n)');
  Field(27,11);
  DoToPoint(2,2); row(6);
  DoToPoint(2,4); row(8);
  DoToPoint(2,6); row(10);
  DoToPoint(2,8); row(12);
end;

procedure examen10;
  procedure vert(w,h: integer);
  var i: integer;
  begin
    for i:=1 to w do
      rect(h,i);
  end;
begin
  TaskText('Задание examen10. Начертить, составив процедуру Vert(w,h), где w - высота, h - ширина');
  Field(17,13);
  DoToPoint(2,2); vert(6,4);
  DoToPoint(12,4); vert(3,5);
  DoToPoint(4,8); vert(5,2);
end;

procedure zzz;
begin
  Field(10,2);
  TaskText('Задание c1. Закрасить помеченные клетки');
end;

var __initialized := false;
var __finalized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    DrawManField.__InitModule__;
    DMTaskMaker.__InitModule__;
  end;
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    DrawManField.__FinalizeModule__;
    DMTaskMaker.__FinalizeModule__;
  end;
end;

initialization
  
finalization
end.