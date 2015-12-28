unit RobotZadan;

interface

procedure a1;
procedure a2;
procedure a3;
procedure a4;

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

procedure if1;
procedure if2;
procedure if3;
procedure if4;
procedure if5;
procedure if6;
procedure if7;
procedure if8;
procedure if9;
procedure if10;
procedure if11;

procedure w1;
procedure w2;
procedure w3;
procedure w4;
procedure w5;
procedure w6;
procedure w7;
procedure w8;
procedure w9;
procedure w10;
procedure w11;
procedure w12;
procedure w13;
procedure w14;
procedure w15;
procedure w16;
procedure w17;

procedure cif1;
procedure cif2;
procedure cif3;
procedure cif4;
procedure cif5;
procedure cif6;
procedure cif7;
procedure cif8;
procedure cif9;
procedure cif10;
procedure cif11;
procedure cif12;
procedure cif13;
procedure cif14;
procedure cif15;
procedure cif16;
procedure cif17;
procedure cif18;
procedure cif19;
procedure cif20;
procedure cif21;
procedure cif22;

procedure count1;
procedure count2;
procedure count3;
procedure count4;
procedure count5;
procedure count6;
procedure count7;
procedure count8;
procedure count9;
procedure count10;
procedure count11;
procedure count12;
procedure count13;
procedure count14;
procedure count15;
procedure count16;
procedure count17;

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
procedure cc17;
procedure cc18;
procedure cc19;

procedure mix1;
procedure mix2;
procedure mix3;
procedure mix4;
procedure mix5;
procedure mix6;
procedure mix7;
procedure mix8;
procedure mix9;
procedure mix10;

procedure p1;
procedure p2;
procedure p3;
procedure p4;
procedure p5;
procedure p6;
procedure p7;
procedure p8;
procedure p9;
procedure p10;
procedure p11;
procedure p12;
procedure p13;
procedure p14;
procedure p15;

procedure pp1;
procedure pp2;
procedure pp3;
procedure pp4;
procedure pp5;
procedure pp6;
procedure pp7;
procedure pp8;

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

implementation

uses RobotTaskMaker;

procedure HorizontalWall(x,y,len: integer);
begin
  RobotTaskMaker.HorizontalWall(x-1,y,len);
end;

procedure VerticalWall(x,y,len: integer);
begin
  RobotTaskMaker.VerticalWall(x,y-1,len);
end;

procedure a1;
begin
  TaskText('Задание a1. Закрасить помеченные клетки');
  Field(7,4);
  RobotBeginEnd(2,2,1,1);
  HorizontalWall(1,1,5);
  TagRect(2,1,5,1);
end;

procedure a2;
var n: integer;
begin
  TaskText('Задание a2. Закрасить помеченные клетки');
  n:=3;
  Field(n,n);
  RobotBeginEnd(1,1,1,1);
  TagRect(1,1,n,1);
  TagRect(n,1,n,n);
  TagRect(1,n,n,n);
  TagRect(1,1,1,n);
end;

procedure a3;
begin
  TaskText('Задание a3. Закрасить помеченные клетки');
  Field(4,2);
  RobotBeginEnd(1,1,4,1);
  TagRect(1,1,4,2);
  VerticalWall(1,1,1);
  VerticalWall(3,1,1);
  VerticalWall(2,2,1);
end;

procedure a4;
begin
  TaskText('Задание a4. Закрасить помеченные клетки');
  Field(4,3);
  RobotBeginEnd(1,3,1,3);
  Tag(2,3);Tag(3,3);Tag(4,3);
  VerticalWall(1,2,2);
  VerticalWall(2,2,2);
  VerticalWall(3,2,2);
end;

procedure c1;
begin
  TaskText('Задание c1. Дойти до правой клетки');
  Field(11,1);
  RobotBeginEnd(1,1,11,1);
end;

procedure c2;
begin
  TaskText('Задание c2. Закрасить помеченные клетки');
  Field(11,1);
  RobotBeginEnd(1,1,11,1);
  TagRect(1,1,10,1);
end;

procedure c3;
begin
  TaskText('Задание c3. Закрасить помеченные клетки');
  Field(11,1);
  RobotBeginEnd(1,1,11,1);
  TagRect(2,1,11,1);
end;

procedure c4;
begin
  TaskText('Задание c4. Закрасить помеченные клетки');
  Field(11,1);  
  RobotBeginEnd(1,1,11,1);  
  TagRect(1,1,11,1);
end;

procedure c5;
var i: integer;
begin
  TaskText('Задание c5. Закрасить помеченные клетки');
  Field(11,11);
  RobotBeginEnd(1,1,11,11);
  for i:=1 to 10 do
    Tag(i,i);
end;

procedure c6;
var i: integer;
begin
  TaskText('Задание c6. Закрасить помеченные клетки');
  Field(17,1);
  RobotBeginEnd(17,1,17,1);
  for i:=1 to 8 do
    Tag(2*i,1);
end;

procedure c7;
var i: integer;
begin
  TaskText('Задание c7. Закрасить помеченные клетки');
  Field(11,2);
  RobotBeginEnd(11,1,1,1);
  TagRect(2,1,11,2);
  for i:=1 to 5 do
  begin
    VerticalWall(2*i,1,1);
    VerticalWall(2*i-1,2,1);
  end
end;

procedure c8;
var i: integer;
begin
  TaskText('Задание c8. Закрасить помеченные клетки');
  Field(11,2);
  RobotBeginEnd(11,1,1,1);
  TagRect(1,1,11,1);
  for i:=1 to 10 do
    VerticalWall(i,1,1);
end;

procedure c9;
var i: integer;
begin
  TaskText('Задание c9. Закрасить помеченные клетки');
  Field(11,2);
  RobotBeginEnd(1,2,11,2);
  for i:=1 to 5 do
  begin
    Tag(2*i,1);
    Tag(2*i-1,2);
  end
end;

procedure c10;
begin
  TaskText('Задание c10. Закрасить помеченные клетки');
  Field(11,2);
  RobotBeginEnd(1,1,1,2);
  TagRect(1,1,10,2);
  HorizontalWall(1,1,10);
end;

procedure c11;
begin
  TaskText('Задание c11. Закрасить помеченные клетки');
  Field(10,2);  
  RobotBeginEnd(10,1,10,2);
  HorizontalWall(2,1,9);  
  TagRect(1,1,10,2);
end;

procedure c12;
begin
  TaskText('Задание c12. Закрасить помеченные клетки');
  Field(11,1);
  RobotBeginEnd(6,1,6,1);
  TagRect(1,1,11,1);
end;

procedure c13;
begin
  TaskText('Задание c13. Закрасить помеченные клетки');
  Field(9,9);
  RobotBeginEnd(9,1,9,1);
  TagRect(1,1,9,1);
  TagRect(1,9,9,9);
  TagRect(1,1,1,9);
  TagRect(9,1,9,9);
end;

procedure c14;
begin
  TaskText('Задание c14. Закрасить помеченные клетки');
  Field(9,9);
  RobotBeginEnd(1,5,1,5);
  Tag(5,1); Tag(4,2); Tag(3,3); Tag(2,4);
  Tag(1,5); Tag(2,6); Tag(3,7); Tag(4,8);
  Tag(5,9); Tag(6,8); Tag(7,7); Tag(8,6);
  Tag(9,5); Tag(8,4); Tag(7,3); Tag(6,2);
end;

procedure c15;
begin
  TaskText('Задание c15. Закрасить помеченные клетки');
  Field(11,11);
  RobotBeginEnd(6,6,6,6);
  TagRect(6,1,6,11);
  TagRect(1,6,11,6);
end;

procedure c16;
var i: integer;
begin
  TaskText('Задание c16. Закрасить помеченные клетки');
  Field(12,3);
  RobotBeginEnd(2,2,2,2);
  TagRect(2,2,11,2);
  for i:=1 to 5 do
  begin
    HorizontalWall(2*i,1,1);
    HorizontalWall(1+2*i,2,1);
    VerticalWall(2*i,2,1);
    VerticalWall(1+2*i,2,1);
  end;
  VerticalWall(1,2,1);
end;

procedure if1;
var r: integer;
begin
  TaskText('Задание if1. Обойти препятствие и закрасить клетку');
  Field(5,5);
  RobotBeginEnd(2,3,4,3);
  Tag(4,3);
  VerticalWall(3,3,1);
  r:=random(2);
  if r=0 then
  begin  
    HorizontalWall(3,2,1);   
    VerticalWall(2,1,2); 
  end
  else
  begin  
    HorizontalWall(3,3,1);  
    VerticalWall(2,4,2);
  end;
end;

procedure if2;
var r1,r2,r3,r4: integer;
begin
  TaskText('Задание if2. Закрасить клетки у стен (стены могут быть независимо слева, справа, сверху и снизу)');
  Field(5,5);
  RobotBeginEnd(3,3,3,3);
  r1:=random(2);
  r2:=random(2);
  r3:=random(2);
  r4:=random(2);
  Tag(3,3);
  if r1=0 then
  begin
    Tag(3,2);   
    HorizontalWall(3,1,1); 
  end;
  if r2=0 then
  begin
    Tag(2,3);   
    VerticalWall(1,3,1); 
  end;
  if r3=0 then
  begin
    Tag(3,4);   
    HorizontalWall(3,4,1); 
  end;
  if r4=0 then
  begin
    Tag(4,3);   
    VerticalWall(4,3,1); 
  end;
end;

procedure if3;
var n,r: integer;
begin
  TaskText('Задание if3. Закрасить клетку в противоположном углу (Робот может находиться в любом из углов)');
  n:=2;
  Field(n,n);
  r:=random(4);
  if r=0 then
  begin 
    RobotBegin(1,1); 
    Tag(n,n); 
    RobotEnd(n,n);  
  end;
  if r=1 then
  begin 
    RobotBegin(1,n); 
    Tag(n,1); 
    RobotEnd(n,1);
  end;
  if r=2 then
  begin 
    RobotBegin(n,1); 
    Tag(1,n); 
    RobotEnd(1,n);  
  end;
  if r=3 then
  begin 
    RobotBegin(n,n); 
    Tag(1,1); 
    RobotEnd(1,1);  
  end;
end;

procedure if4;
var r: integer;
begin
  TaskText('Задание if4. Закрасить клетку, противоположную стене (стена может располагаться слева, справа, сверху или снизу)');
  Field(5,5);
  RobotBegin(3,3);
  r:=random(4);
  if r=0 then
  begin
    Tag(2,3); 
    RobotEnd(2,3);   
    VerticalWall(3,3,1); 
  end;
  if r=1 then
  begin 
    Tag(4,3); 
    RobotEnd(4,3);   
    VerticalWall(2,3,1); 
  end;
  if r=2 then
  begin 
    Tag(3,2); 
    RobotEnd(3,2);   
    HorizontalWall(3,3,1); 
  end;
  if r=3 then
  begin 
    Tag(3,4); 
    RobotEnd(3,4);   
    HorizontalWall(3,2,1); 
  end;
end;

procedure if5;
var r: integer;
begin
  TaskText('Задание if5. Закрасить клетки в зависимости от наличия стены (стена может располагаться слева, справа, сверху или снизу)');
  Field(5,5);
  RobotBeginEnd(3,3,3,3);
  r:=random(4);
  if r=0 then
  begin
    Tag(2,3); 
    Tag(4,3);   
    HorizontalWall(3,2,1);
  end;
  if r=1 then
  begin
    Tag(2,3); 
    Tag(4,3);   
    HorizontalWall(3,3,1);
  end;
  if r=2 then
  begin
    Tag(3,2); 
    Tag(3,4);  
    VerticalWall(3,3,1);
  end;
  if r=3 then
  begin
    Tag(3,2); 
    Tag(3,4);   
    VerticalWall(2,3,1);
  end;
end;

procedure if6;
var r: integer;
begin
  TaskText('Задание if6. Закрасить клетку, противоположную закрашенной (закрашенная клетка может находиться слева, справа, сверху или снизу)');
  Field(5,5);
  RobotBegin(3,3);
  r:=random(4);
  if r=0 then
  begin 
    Tag(2,3); 
    RobotEnd(2,3); 
    MarkPainted(4,3);
  end;
  if r=1 then
  begin 
    Tag(4,3); 
    RobotEnd(4,3); 
    MarkPainted(2,3);
  end;
  if r=2 then
  begin 
    Tag(3,2); 
    RobotEnd(3,2); 
    MarkPainted(3,4);
  end;
  if r=3 then
  begin 
    Tag(3,4); 
    RobotEnd(3,4); 
    MarkPainted(3,2);
  end;
end;

procedure if7;
var r: integer;
begin
  TaskText('Задание if7. Закрасить клетку, противоположную углу');
  Field(4,4);
  r:=random(4);
  if r=0 then
  begin 
    Tag(3,3);   
    RobotBeginEnd(2,2,3,3);   
    VerticalWall(1,2,1);  
    HorizontalWall(2,1,1);
  end;
  if r=1 then
  begin 
    Tag(2,2);   
    RobotBeginEnd(3,3,2,2);   
    VerticalWall(3,3,1);  
    HorizontalWall(3,3,1);
  end;
  if r=2 then
  begin  
    Tag(2,3);   
    RobotBeginEnd(3,2,2,3);   
    VerticalWall(3,2,1);  
    HorizontalWall(3,1,1);
  end;
  if r=3 then
  begin  
    Tag(3,2);   
    RobotBeginEnd(2,3,3,2);   
    VerticalWall(1,3,1);  
    HorizontalWall(2,3,1);
  end;
end;

procedure if8;
var r1,r2: integer;
begin
  TaskText('Задание if8. Закрасить клетки, используя вложенные операторы if');
  Field(6,6);
  RobotBeginEnd(2,2,3,3);
  r1:=random(2);
  r2:=random(2);
  if r1=0 then
  begin 
    Tag(3,2); 
    Tag(4,2);   
    VerticalWall(1,2,1);
    if r2=0 then 
    begin   
      VerticalWall(4,2,1);
      RobotEnd(4,2); 
    end
    else
    begin 
      RobotEnd(5,2);
      Tag(5,2); 
    end; 
  end
  else
  begin 
    Tag(2,3); 
    Tag(2,4);   
    HorizontalWall(2,1,1);
    if r2=0 then
    begin   
      HorizontalWall(2,4,1);
      RobotEnd(2,4); 
    end
    else
    begin 
      RobotEnd(2,5);
      Tag(2,5); 
    end; 
  end;
end;

procedure if9;
var r: integer;
begin
  TaskText('Задание if9. Закрасить клетки в зависимости от наличия стен слева и справа');
  Field(5,5);
  RobotBeginEnd(3,3,3,3);
  r:=random(4);
  if r=0 then
  begin 
    Tag(3,2); 
    Tag(3,4);   
    HorizontalWall(2,2,1);  
    HorizontalWall(4,3,1);
  end;
  if r=1 then
  begin 
    Tag(3,3);  
    HorizontalWall(2,2,1);
  end;
  if r=2 then
  begin 
    Tag(3,3);  
    HorizontalWall(4,3,1);
  end;
  if r=3 then
  begin 
    Tag(3,3);
  end;
end;

procedure if10;
var r: integer;
begin
  TaskText('Задание if10. Закрасить клетки в зависимости от наличия закрашенных клеток слева и справа');
  Field(5,5);
  RobotBeginEnd(3,3,3,3);
  r:=random(4);
  if r=0 then
  begin 
    Tag(3,2);
    MarkPainted(2,3); 
  end;
  if r=1 then
  begin 
    Tag(3,2);
    MarkPainted(4,3); 
  end;
  if r=2 then
  begin 
    Tag(3,2);
    MarkPainted(4,3);
    MarkPainted(2,3); 
  end;
  if r=3 then
    Tag(3,3);
end;

procedure if11;
var r: integer;
begin
  TaskText('Задание if11. Закрасить первую незакрашенную клетку справа');
  Field(5,5);
  RobotBeginEnd(2,2,4,2);
  r:=random(3);
  if r=0 then
  begin 
    Tag(3,2); 
  end;
  if r=1 then
  begin 
    Tag(3,3);
    MarkPainted(3,2);
  end;
  if r=2 then
  begin 
    Tag(3,4);
    MarkPainted(3,2);
    MarkPainted(3,3);
  end;
end;

procedure w1;
var n: integer;
begin
  TaskText('Задание w1. Пройти коридор переменной длины');
  n:=random(7)+8;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
end;

procedure w2;
var n: integer;
begin
  TaskText('Задание w2. Закрасить коридор переменной длины');
  n:=random(7)+8;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
  TagRect(1,1,n,1);
end;

procedure w3;
var n: integer;
begin
  TaskText('Задание w3. Дойти до конца стены переменной длины');
  n:=random(8)+3;
  Field(12,3);  
  RobotBeginEnd(1,1,n+1,1);
  HorizontalWall(1,1,n);
end;

procedure w4;
var n,i: integer;
begin
  TaskText('Задание w4. Дойти до конца закрашенного ряда переменной длины');
  n:=random(7)+3;
  Field(10,3);  
  RobotBeginEnd(2,2,n+1,2);
  for i:=3 to n do
    MarkPainted(i,2);
end;

procedure w5;
var n,m,i,j: integer;
begin
  TaskText('Задание w5. Дойти до правого нижнего угла закрашенного прямоугольника переменного размера');
  n:=random(7)+3;
  m:=random(6)+3;
  Field(10,9);  
  RobotBeginEnd(2,2,n,m);
  for j:=2 to m do
  for i:=2 to n do
    MarkPainted(i,j); 
end;

procedure w6;
var n,m: integer;
begin
  TaskText('Задание w6. Найти в стене проход и попасть в правый нижний угол');
  n:=random(7)+2;
  m:=random(12)+2;
  Field(15,10);  
  RobotBeginEnd(1,1,15,10);
  HorizontalWall(1,n,m-1);  
  HorizontalWall(m+1,n,15-m);
end;

procedure w7;
var n,m,k: integer;
begin
  TaskText('Задание w7. Дойти до третьей закрашенной клетки. После первой закрашенной клетки надо идти вверх, после второй - вправо');
  n:=random(7)+2;
  m:=random(7)+1;
  k:=random(7)+1;
  Field(15,10);  
  RobotBeginEnd(1,8,n+k,8-m);
  MarkPainted(n,8);
  MarkPainted(n,8-m);
  MarkPainted(n+k,8-m);
end;

procedure w8;
var n1,n2,len: integer;
begin
  TaskText('Задание w8. Закрасить все клетки над стеной переменной длины');
  n1:=random(3)+2;
  n2:=n1+random(4);
  len:=random(5)+4;
  Field(13,5);  
  RobotBeginEnd(n2,2,n1-1,2);
  HorizontalWall(n1,2,len);
  TagRect(n1,2,n1+len-1,2);
end;

procedure w9;
var n: integer;
begin
  TaskText('Задание w9. Перепрыгнуть стену с закрашиванием');
  n:=random(7)+2;
  Field(15,10);  
  RobotBeginEnd(7,10,8,10);
  VerticalWall(7,11-n,n);
  TagRect(7,10-n,8,10);
end;

procedure w10;
var n1,n2,r1,r2: integer;
begin
  TaskText('Задание w10. Обойти препятствия');
  n1:=random(7)+2;
  n2:=random(7)+2;
  r1:=4+random(3);
  r2:=9+random(3);
  Field(15,10);  
  RobotBeginEnd(1,10,15,10);
  VerticalWall(r1,11-n1,n1);  
  TagRect(r1,10-n1,r1+1,10);
  VerticalWall(r2,11-n2,n2);  
  TagRect(r2,10-n2,r2+1,10);
end;

procedure w11;
var n1,len: integer;
begin
  TaskText('Задание w11. Закрасить все клетки вокруг стены');
  n1:=random(3)+3;
  len:=random(5)+4;
  Field(15,5);  
  RobotBeginEnd(n1-1,2,n1-1,2);
  HorizontalWall(n1,2,len);
  TagRect(n1-1,2,n1+len,3);
end;

procedure w12;
var n1,n2,len1,len2: integer;
begin
  TaskText('Задание w12. Закрасить все клетки вокруг прямоугольника');
  n1:=random(3)+3;
  len1:=random(5)+4;
  n2:=random(3)+3;
  len2:=random(5)+4;
  Field(14,14);  
  RobotBeginEnd(n1-1,n2,n1-1,n2);
  HorizontalWall(n1,n2,len1);
  HorizontalWall(n1,n2+len2,len1);
  VerticalWall(n1-1,n2+1,len2);
  VerticalWall(n1-1+len1,n2+1,len2);
  TagRect(n1-1,n2,n1+len1,n2);
  TagRect(n1-1,n2+len2+1,n1+len1,n2+len2+1);
  TagRect(n1-1,n2,n1-1,n2+len2);
  TagRect(n1+len1,n2,n1+len1,n2+len2);
end;

procedure w13;
var n,i,m: integer;
begin
  TaskText('Задание w13. Подняться до клетки, имеющей проход налево или проход направо');
  n:=random(7)+2;
  Field(5,10);RobotBegin(3,10);
  VerticalWall(2,9,2);  
  VerticalWall(3,9,2);
  RobotEnd(3,10-n-1);
  for i:=0 to n-2 do
  begin
    m:=random(3);
    if m=0 then VerticalWall(2,10-n+i,1);
    if m=1 then VerticalWall(3,10-n+i,1);
    if m=2 then 
    begin   
      VerticalWall(2,10-n+i,1);  
      VerticalWall(3,10-n+i,1);
    end;
    if m<>2 then
      RobotEnd(3,10-n+i);
  end;
end;

procedure w14;
var n,m,i: integer;
begin
  TaskText('Задание w14. Подняться до клетки, имеющей проход налево и проход направо');
  n:=random(7)+2;
  Field(5,10);RobotBegin(3,10);
  VerticalWall(2,9,2);  VerticalWall(3,9,2);
  RobotEnd(3,10-n-1);
  for i:=0 to n-2 do
  begin
    m:=random(4);
    if m=0 then 
      VerticalWall(2,10-n+i,1);
    if m=1 then 
      VerticalWall(3,10-n+i,1);
    if m=2 then 
    begin 
      VerticalWall(2,10-n+i,1);
      VerticalWall(3,10-n+i,1); 
    end;
    if m=3 then 
      RobotEnd(3,10-n+i);
  end;
end;

procedure w15;
var n,z,i: integer;
begin
  TaskText('Задание w15. Закрасить клетку под первой закрашенной клеткой, под которой свободно');
  Field(15,5);
  RobotBegin(2,3);
  HorizontalWall(2,3,2);
  z:=14;
  MarkPainted(14,3);
  for i:=3 to 12 do
  begin
    n:=random(4);
    if n=0 then 
    begin 
      MarkPainted(16-i,3);
      z:=16-i; 
    end;
    if n=1 then 
    begin 
      MarkPainted(16-i,3); 
      HorizontalWall(16-i,3,1); 
    end;
    if n=2 then 
      HorizontalWall(16-i,3,1); 
  end;
  RobotEnd(z,4);
  Tag(z,4);
end;

procedure w16;
var n,i: integer;
begin
  TaskText('Задание w16. Идти до конца забора переменной длины');
  n:=random(6)+8;
  Field(15,5);  
  RobotBeginEnd(2,3,n+1,3);
  for i:=2 to n do
    VerticalWall(i,3,1);
end;

procedure w17;
var n,i: integer;
begin
  TaskText('Задание w17. Идти до конца забора переменной длины');
  n:=random(6)+1;
  Field(15,5);  
  RobotBeginEnd(2,3,2*n+1,3);
  for i:=1 to n do
    VerticalWall(2*i,3,1);
end;

procedure cif1;
var n,i,r: integer;
begin
  TaskText('Задание cif1. Закрасить все клетки под стенами');
  n:=14;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r:=random(2);
    if r=1 then
    begin    
      HorizontalWall(i,1,1); 
      Tag(i,2);  
    end;  
  end;
end;

procedure cif2;
var n,i,r: integer;
begin
  TaskText('Задание cif2. Закрасить помеченные клетки');
  n:=14;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r:=random(3);
    if r=1 then
    begin    
      HorizontalWall(i,1,1); 
      Tag(i,2);
    end;
    if r=2 then
    begin    
      HorizontalWall(i,2,1);
      Tag(i,2);
    end;
  end;
end;

procedure cif3;
var n,i,r: integer;
begin
  TaskText('Задание cif3. Закрасить помеченные клетки');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r:=random(4);
    if r=1 then
    begin    
      HorizontalWall(i,1,1);   
      HorizontalWall(i,2,1); 
      Tag(i,2);  
    end;
    if r=2 then
      HorizontalWall(i,1,1);
    if r=3 then
      HorizontalWall(i,2,1);
  end;
end;

procedure cif4;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif4. Закрасить помеченные клетки');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if (r1=1) and (r2=0) then 
      Tag(i,2); 
  end;
end;

procedure cif5;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif5. Закрасить помеченные клетки');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if (r1=0) or (r2=1) then
      Tag(i,2); 
  end;
end;

procedure cif6;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif6. Закрасить помеченные клетки');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); r2:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if (r1=0) or (r2=0) then
      Tag(i,2); 
  end;
end;

procedure cif7;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif7. Закрасить помеченные клетки, используя два условных оператора в цикле');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1)
    else Tag(i,1);
    if r2=1 then
      HorizontalWall(i,2,1)
    else Tag(i,3);
  end;
end;

procedure cif8;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif8. Закрасить помеченные клетки, используя два условных оператора в цикле');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); r2:=random(2);
    if (r1=1) and (r2=0) then
    begin    
      HorizontalWall(i,1,1); 
      Tag(i,3); 
    end;
    if (r2=1) and (r1=0) then
    begin    
      HorizontalWall(i,2,1); 
      Tag(i,1); 
    end;
  end;
end;

procedure cif9;
var n,i,r1,r2: integer;
begin
  TaskText('Задание cif9. Закрасить помеченные клетки, используя три условных оператора в цикле');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); r2:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if (r1=1) and (r2=0) then
      Tag(i,3); 
    if (r2=1) and (r1=0) then
      Tag(i,1); 
    if (r1=1) and (r2=1) then
      Tag(i,2); 
  end;
end;

procedure cif10;
var n,i,r1,r2,r3: integer;
begin
  TaskText('Задание cif10. Закрасить помеченные клетки');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2); 
    r3:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if r3=1 then
      MarkPainted(i,2); 
    if (r3=1) and (r2=0) then
      Tag(i,3); 
  end;
end;

procedure cif11;
var n,i,r1,r2,r3: integer;
begin
  TaskText('Задание cif11. Закрасить помеченные клетки, используя в условии две операции and');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2); 
    r3:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if r3=1 then
      MarkPainted(i,2); 
    if (r3=1) and (r2=0) and (r1=1) then
      Tag(i,3); 
  end;
end;

procedure cif12;
var n,i,r1,r2,r3: integer;
begin
  TaskText('Задание cif12. Закрасить помеченные клетки, используя в условии операции and и or');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2); 
    r3:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1);
    if r2=1 then
      HorizontalWall(i,2,1);
    if r3=1 then
      MarkPainted(i,2); 
    if (r1=0) and ((r3=1) or (r2=0)) then
      Tag(i,1); 
  end;
end;

procedure cif13;
var n,i,r2,r3: integer;
begin
  TaskText('Задание cif13. Закрасить помеченные клетки, используя в условии две операции and и одну or');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r2:=random(2); 
    r3:=random(2);
    if r2=1 then
      HorizontalWall(i,2,1);
    if r3=1 then
      MarkPainted(i,2);
    if (r3=1) and (r2=1) or (r3=0) and (r2=0) then 
      Tag(i,1);
  end;
end;

procedure cif14;
var n,i,r2,r3: integer;
begin
  TaskText('Задание cif14. Закрасить помеченные клетки, используя вложенные условные операторы');
  n:=25;
  Field(n,4);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  HorizontalWall(1,2,1);
  for i:=2 to n-1 do
  begin  
    r2:=random(2); 
    r3:=random(2);
    if r2=1 then
      HorizontalWall(i,2,1);
    if r3=1 then
      MarkPainted(i,3);
    if (r3=1) and (r2=0) then
      Tag(i,4);
 end;
end;

procedure cif15;
var n,i,r2,r3: integer;
begin
  TaskText('Задание cif15. Закрасить помеченные клетки, используя вложенные условные операторы');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  HorizontalWall(1,2,1);
  for i:=2 to n-1 do
  begin  
    r2:=random(2); 
    r3:=random(2);
    if r2=1 then
      MarkPainted(i,1);
    if r3=1 then
      MarkPainted(i,3);
    if (r3=1) and (r2=1) then
      Tag(i,2);
  end;
end;

procedure cif16;
var n: integer;
begin
  TaskText('Задание cif16. Перейти в противоположный угол');
  Field(9,9);
  n:=random(4);
  if n=0 then 
    RobotBeginEnd(1,1,9,9);
  if n=1 then 
    RobotBeginEnd(1,9,9,1);
  if n=2 then 
    RobotBeginEnd(9,1,1,9);
  if n=3 then
    RobotBeginEnd(9,9,1,1);
end;

procedure cif17;
var n,m,s: integer;
begin
  TaskText('Задание cif17. Дойти до конца тупика переменного размера (вертикальный коридор может сворачивать направо или налево)');
  Field(15,10);
  RobotBeginEnd(8,10,8,1);
  n:=random(9)+2;
  m:=random(6)+2;
  s:=random(2);
  VerticalWall(8,10-n+2,n-1);
  VerticalWall(7,10-n+2,n-1);
  if s=0 then
  begin
    HorizontalWall(8-m,10-n+1,m);
    HorizontalWall(8-m,10-n,m+1);
    VerticalWall(8,10-n+1,1);
    VerticalWall(8-m-1,10-n+1,1);
    RobotEnd(8-m,10-n+1);
  end
  else
  begin
    HorizontalWall(9,10-n+1,m);
    HorizontalWall(8,10-n,m+1);
    VerticalWall(7,10-n+1,1);
    VerticalWall(8+m,10-n+1,1);
    RobotEnd(8+m,10-n+1);
  end;
end;

procedure cif18;
var n,c,i: integer;
begin
  TaskText('Задание cif18. Перейти в конец более короткого ряда');
  Field(15,4);
  RobotBegin(2,2);
  c:=random(2);
  n:=random(11)+1;
  for i:=3 to 3+n-1 do
  begin 
    MarkPainted(i,2);MarkPainted(i,3) 
  end;
  if c=0 then 
  begin 
    MarkPainted(3+n,2);
    RobotEnd(3+n,3); 
  end
  else
  begin 
    MarkPainted(3+n,3);
    RobotEnd(3+n,2);
  end
end;

procedure cif19;
var n1,n2: integer;
begin
  TaskText('Задание cif19. Перейти в конец более короткой стены');
  Field(15,4);
  RobotBegin(2,3);
  n1:=random(9)+4;
  n2:=random(9)+4;
  if n1=n2 then 
    n1:=n1+1;
  HorizontalWall(2,1,n1);
  HorizontalWall(2,3,n2);
  if (n1<n2) then
    RobotEnd(n1+2,2)
  else RobotEnd(n2+2,3);
end;

procedure cif20;
var n1,n2: integer;
begin
  TaskText('Задание cif20. Перейти в конец более длинной стены');
  Field(15,4);
  RobotBegin(2,3);
  n1:=random(9)+4;
  n2:=random(9)+4;
  if n1=n2 then 
    n1:=n1+1;
  HorizontalWall(2,1,n1);
  HorizontalWall(2,3,n2);
  if n1>n2 then 
    RobotEnd(n1+2,2)
  else RobotEnd(n2+2,3);
end;

procedure cif21;
var n,m,c: integer;
begin
  TaskText('Задание cif21. Перейти на вторую закрашенную клетку');
  Field(15,8);
  RobotBegin(8,2);
  c:=random(2);
  m:=random(6)+1;
  n:=random(5)+1;
  if c=0 then
  begin 
    MarkPainted(8-m,2);
    MarkPainted(8-m,2+n);
    RobotEnd(8-m,2+n); 
  end
  else
  begin 
    MarkPainted(8+m,2);
    MarkPainted(8+m,2+n);
    RobotEnd(8+m,2+n); 
  end
end;

procedure cif22;
var n1,n2,n,m,c: integer;
begin
  TaskText('Задание cif22. Найти выход и переместиться в левую верхнюю клетку поля ');
  Field(15,5);
  n1:=random(3)+2;
  n2:=random(10)+5;
  n:=random(n2-n1+1)+n1;
  m:=random(n2-n1+1);
  RobotBegin(n,3);
  c:=random(4);
  if c=0 then
  begin   
    HorizontalWall(n1,2,n2-n1+1);  
    HorizontalWall(n1,3,n2-n1+1);  
    VerticalWall(n1-1,3,1); 
  end;
  if c=1 then
  begin   
    HorizontalWall(n1,2,n2-n1+1);  
    HorizontalWall(n1,3,n2-n1+1);  
    VerticalWall(n2,3,1); 
  end;
  if c=2 then
  begin   
    HorizontalWall(n1,2,m);  
    HorizontalWall(n1+m+1,2,n2-n1-m);  
    HorizontalWall(n1,3,n2-n1+1);  VerticalWall(n2,3,1);  
    VerticalWall(n1-1,3,1); 
  end;
  if c=3 then
  begin   
    HorizontalWall(n1,3,m);  
    HorizontalWall(n1+m+1,3,n2-n1-m);  
    HorizontalWall(n1,2,n2-n1+1);  
    VerticalWall(n2,3,1);  
    VerticalWall(n1-1,3,1); 
  end;
end;

procedure count1;
var n: integer;
begin
  TaskText('Задание count1. Дойти до правой стены, закрасить клетку и вернуться. Использовать переменную-счетчик');
  Field(10,5);
  n:=random(7)+3;
  RobotBeginEnd(n,3,n,3); 
  Tag(10,3);
end;

procedure count2;
var n1,n2,m: integer;
begin
  TaskText('Задание count2. Закрасить правый нижний угол и вернуться. Использовать два счетчика');
  m:=8;
  Field(m,m);
  n1:=random(6)+2;
  n2:=random(6)+2;
  RobotBeginEnd(n1,n2,n1,n2); 
  Tag(m,m);
end;

procedure count3;
var n1,n2,m,t,i: integer;
begin
  TaskText('Задание count3. Переместить Робота в клетку, симметричную относительно диагонали');
  m:=10;
  Field(m,m);
  n1:=random(7)+2;
  n2:=random(7)+2;
  if n1=n2 then
    n1:=n1+1; 
  if n1>n2 then 
  begin 
    t:=n1; 
    n1:=n2; 
    n2:=t; 
  end;
  for i:=1 to m do
    MarkPainted(i,i); 
  RobotBeginEnd(n1,n2,n2,n1);
end;

procedure count4;
var n1,n2: integer;
begin
  TaskText('Задание count4. Обойти стену переменной длины');
  Field(10,4);
  n1:=random(5)+4;
  n2:=random(4)+1;
  HorizontalWall(1,2,n1);
  RobotBeginEnd(n2,2,n2,3);
end;

procedure count5;
var n,m,n1,m1: integer;
begin
  TaskText('Задание count5. Закрасить периметр и вернуться назад');
  n:=random(3)+8;
  m:=random(3)+8;
  Field(n,m);
  n1:=random(n-2)+2;
  m1:=random(m-2)+2;
  RobotBeginEnd(n1,m1,n1,m1);   
  TagRect(1,1,n,1);  
  TagRect(1,1,1,m);  
  TagRect(1,m,n,m);  
  TagRect(n,1,n,m);
end;

procedure count6;
var n,m: integer;
begin
  TaskText('Задание count6. Переместиться к ближайшей стене');
  Field(15,5);
  RobotBegin(8,3);
  n:=random(5)+2;
  m:=random(5)+2;
  if m=n then 
    m:=m+1;
  VerticalWall(8-m,2,3);
  VerticalWall(8+n-1,2,3);
  if m<n then 
    RobotEnd(8-m+1,3)
  else RobotEnd(8+n-1,3);
end;

procedure count7;
var n,l,r: integer;
begin
  TaskText('Задание count7. Обойти стену переменной длины (перегородка нахоится слева или справа)');
  Field(16,4);
  n:=random(3)+2;
  l:=random(12)+1;
  r:=random(l)+n;
  HorizontalWall(n,2,l);
  RobotBeginEnd(r,2,r,3);
  if l mod 2 = 0 then
    VerticalWall(n-1,1,2); 
  if l mod 2 = 1 then
    VerticalWall(n+l-1,1,2); 
end;

procedure count8;
var i,r,cc: integer;
begin
  TaskText('Задание count8. Остановиться на пятой закрашенной клетке');
  Field(21,3);
  RobotBeginEnd(2,2,1,2);
  cc:=0;
  for i:=2 to 20 do
  begin
    if i<16 then
      r:=random(2)
    else 
    begin 
      if cc<5 then 
        r:=1
      else r:=0;
    end;
    if r=1 then 
    begin 
      MarkPainted(i,2);
      cc:=cc+1;
      if cc=5 then 
        RobotEnd(i,2); 
    end;
  end;
end;

procedure count9;
var cc,i,r: integer;
begin
  TaskText('Задание count9. Остановиться на третьей закрашенной подряд клетке');
  Field(20,3);
  RobotBeginEnd(2,2,1,2);
  cc:=0;
  for i:=2 to 19 do
  begin
    if i<17 then
      r:=random(2)
    else  
    begin 
      if cc<3 then 
        r:=1
      else r:=0;
    end;
    if r=1 then 
    begin 
      MarkPainted(i,2);
      cc:=cc+1;
      if cc=3 then 
        RobotEnd(i,2); 
    end
    else
    if cc<3 then
      cc:=0;
  end;
end;

procedure count10;
var n,m: integer;
begin
  TaskText('Задание count10. Закрасить клетки до конца нижней стены, но не менее пяти');
  Field(11,3);
  n:=random(9)+1;
  if n<5 then 
    m:=5
  else m:=n;
  TagRect(2,2,m+1,2);
  RobotBeginEnd(2,2,m+2,2);
  HorizontalWall(2,2,n);
end;

procedure count11;
var n: integer;
begin
  TaskText('Задание count11. Если длина тупика больше 4, то закрасить его первую клетку');
  Field(11,3);
  n:=random(9)+1;
  RobotBeginEnd(1,2,2,2);
  HorizontalWall(2,2,n);
  HorizontalWall(2,1,n);
  VerticalWall(n+1,2,1);
  if n>4 then 
    Tag(2,2); 
end;

procedure count12;
var n,i: integer;
begin
  TaskText('Задание count12. Закрасить помеченные клетки в коридоре переменной длины');
  n:=random(4)+10;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
  for i:=1 to n div 2 do
    Tag(i*2,1); 
end;

procedure count13;
var n,i: integer;
begin
  TaskText('Задание count13. Закрасить помеченные клетки в коридоре переменной длины');
  n:=random(4)+10;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
  for i:=1 to n div 2 + 1 do
    Tag(i*2-1,1); 
end;

procedure count14;
var n,i: integer;
begin
  TaskText('Задание count14. Закрасить помеченные клетки в коридоре переменной длины');
  n:=random(4)+16;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
  for i:=1 to n div 3 do
    Tag(i*3,1);  
end;

procedure count15;
var n,i: integer;
begin
  TaskText('Задание count15. Закрасить помеченные клетки в коридоре переменной длины');
  n:=random(4)+16;
  Field(n,1);  
  RobotBeginEnd(1,1,n,1);
  for i:=1 to n div 3 + 1 do
    Tag(i*3-2,1); 
end;

procedure count16;
var n,i,f,z: integer;
begin
  TaskText('Задание count16. Закрасить все нечетные пролеты между закрашенными клетками');
  Field(25,3);  
  RobotBeginEnd(2,2,25,2);
  n:=2+random(5);
  f:=0;
  for i:=2 to 24 do
  begin 
    if i=n then 
    begin 
      MarkPainted(i,2);
      n:=i+2+random(5);
      if f=0 then 
      begin 
        f:=1;
        z:=i 
      end 
      else
      begin 
        f:=0;  
        TagRect(z+1,2,i-1,2); 
      end; 
    end;
  end;
  if f=1 then 
    TagRect(z+1,2,25,2);
end;

procedure count17;
var n,i,f: integer;
begin
  TaskText('Задание count17. Остановиться на первой клетке, нарушающей закономерность');
  Field(25,3);
  n:=random(20)+4;
  RobotBeginEnd(2,2,n,2);
  f:=0;
  for i:=2 to 24 do
  begin 
    if i<>n then 
    begin 
      if f=0 then 
        f:=1
      else f:=0;
    end;
    if f=1 then 
      HorizontalWall(i,2,1);
  end;
end;

procedure cc1;
var i: integer;
begin
  TaskText('Задание cc1. Закрасить помеченные клетки, используя вложенные циклы');
  Field(31,7);   
  RobotBeginEnd(1,1,31,7);
  for i:=1 to 6 do
    TagRect(5*i-4,i,5*i,i);
end;

procedure cc2;
var a,b,i: integer;
begin
  TaskText('Задание cc2. Закрасить помеченные клетки, используя вложенные циклы');
  Field(22,7);   
  RobotBeginEnd(1,1,22,7);
  a:=1;
  b:=1;
  for i:=1 to 6 do
  begin   
    TagRect(a,i,b,i);
    a:=b+1;
    b:=a+i; 
  end
end;

procedure cc3;
var a,b,i: integer;
begin
  TaskText('Задание cc3. Закрасить помеченные клетки, используя вложенный цикл с downto');
  Field(22,7);   
  RobotBeginEnd(1,1,22,7);
  a:=1;
  b:=6;
  for i:=1 to 6 do
  begin   
    TagRect(a,i,b,i);
    a:=b+1;
    b:=a+5-i;
  end
end;

procedure cc4;
var i: integer;
begin
  TaskText('Задание cc4. Закрасить помеченные клетки, используя во внешнем цикле два вложенных цикла');
  Field(11,10);
  RobotBeginEnd(1,1,1,10);
  TagRect(1,1,11,9);
  for i:=1 to 9 do
    HorizontalWall(2,i,10); 
end;

procedure cc5;
var i: integer;
begin
  TaskText('Задание cc5. Пройти лабиринт, используя во внешнем цикле два вложенных цикла');
  Field(14,9);
  RobotBeginEnd(1,1,1,9);
  for i:=1 to 4 do
  begin   
    HorizontalWall(1,2*i-1,13);  
    HorizontalWall(2,2*i,13); 
  end
end;

procedure cc6;
var i,n,j: integer;
begin
  TaskText('Задание cc6. Закрасить помеченные клетки');
  n:=10;
  Field(20,n);   RobotBeginEnd(1,n,20,n);
  j:=1;
  for i:=1 to 5 do
  begin   
    HorizontalWall(j,n-1,3);  
    VerticalWall(j+3,1,n-1);Tag(j+3,1);j:=j+4; 
  end
end;

procedure cc7;
var i: integer;
begin
  TaskText('Задание cc7. Закрасить помеченные клетки, используя во внешнем цикле два вложенных цикла');
  Field(10,10);
  RobotBeginEnd(1,1,1,10);
  for i:=1 to 9 do
    TagRect(1,i,i,i);  
end;

procedure cc8;
var i: integer;
begin
  TaskText('Задание cc8. Закрасить помеченные клетки, используя цикл с downto');
  Field(10,10);
  RobotBeginEnd(1,1,1,10);
  for i:=1 to 9 do
    TagRect(1,i,10-i,i); 
end;

procedure cc9;
var i: integer;
begin
  TaskText('Задание cc9. Закрасить помеченные клетки, используя во внешнем цикле два вложенных цикла');
  Field(10,10);  
  RobotBeginEnd(10,1,10,10);  
  TagRect(10,1,10,9);
  for i:=1 to 9 do
  begin   
    HorizontalWall(11-i,i,i);   
    HorizontalWall(1,i,9-i); 
  end;
end;

procedure cc10;
var i: integer;
begin
  TaskText('Задание cc10. Закрасить помеченные клетки');
  Field(15,8);
  RobotBeginEnd(1,1,1,8);
  for i:=1 to 7 do
    TagRect(1,i,2*i,i); 
end;

procedure cc11;
var i: integer;
begin
  TaskText('Задание cc11. Закрасить помеченные клетки');
  Field(14,8);
  RobotBeginEnd(1,1,1,8);
  for i:=1 to 7 do
    TagRect(1,i,2*i-1,i); 
end;

procedure cc12;
var i,p: integer;
begin
  TaskText('Задание cc12. Закрасить помеченные клетки');
  Field(33,7);
  RobotBeginEnd(1,1,1,7);
  p:=1;
  for i:=1 to 6 do
  begin   
    TagRect(1,i,p,i);
    p:=p*2; 
  end
end;

procedure cc13;
var i: integer;
begin
  TaskText('Задание cc13. Закрасить помеченные клетки');
  Field(14,9);
  RobotBeginEnd(1,1,9,9);
  for i:=1 to 4 do
  begin   
    HorizontalWall(2*i-1,2*i-1,15-2*i);  
    HorizontalWall(2*i+2,2*i,13);
    Tag(2*i+1,2*i); 
  end
end;

procedure cc14;
var i: integer;
begin
  TaskText('Задание cc14. Пройти лабиринт');
  Field(14,9);
  RobotBeginEnd(1,1,5,9);
  for i:=1 to 4 do
  begin   
    HorizontalWall(i,2*i-1,15-2*i);  
    HorizontalWall(i+2,2*i,14-2*i);  
    VerticalWall(i,2*i,2);  
    VerticalWall(14-i,2*i+1,2); 
  end
end;

procedure cc15;
var i,n: integer;
begin
  TaskText('Задание cc15. Закрасить помеченные клетки');
  n:=8;
  Field(15,9);   
  RobotBeginEnd(n,2,n,9);
  for i:=1 to 7 do
    TagRect(n-i+1,i+1,n+i-1,i+1); 
end;

procedure cc16;
var i,k,j: integer;
begin
  TaskText('Задание cc16. Закрасить помеченные клетки, используя вложенный цикл во вложенном цикле');
  Field(22,7);   
  RobotBeginEnd(2,6,22,6);
  j:=2;
  for k:=1 to 4 do
    for i:=1 to 5 do
    begin
      TagRect(j,6-i+1,j,6);
      j:=j+1;
    end;  
end;

procedure cc17;
var i,j,k: integer;
begin
  TaskText('Задание cc17. Закрасить помеченные клетки, используя вложенный цикл во вложенном цикле');
  Field(29,8);   
  RobotBeginEnd(2,7,28,7);
  j:=2;
  for k:=1 to 6 do
  begin
    for i:=1 to 6-k+1 do
    begin   
      TagRect(j,7-i+1,j,7);
      j:=j+1; 
    end;
    j:=j+1;
  end;
end;

procedure cc18;
var i: integer;
begin
  TaskText('Задание cc18. Закрасить помеченные клетки, используя цикл while внутри цикла for');
  Field(12,7);   RobotBeginEnd(1,1,1,7);
  for i:=1 to 8 do
    HorizontalWall(2,i,12); 
  VerticalWall(7,1,1);Tag(7,1);
  VerticalWall(9,2,1);Tag(9,2);
  VerticalWall(5,3,1);Tag(5,3);
  VerticalWall(4,4,1);Tag(4,4);
  VerticalWall(11,5,1);Tag(11,5);
  VerticalWall(2,6,1);Tag(2,6);
  VerticalWall(7,7,1);Tag(7,7);
end;

procedure cc19;
var i,n: integer;
begin
  TaskText('Задание cc19. Закрасить помеченные клетки. Высота поля может меняться');
  n:=random(4)+6;
  Field(11,n);
  RobotBeginEnd(1,1,1,n);
  for i:=1 to n do
  begin 
    Tag(11,i);  
    HorizontalWall(2,i,10);
  end
end;

procedure mix1;
var n1,n2,m: integer;
begin
  TaskText('Задание mix1. Закрасить прямоугольник переменного размера');
  m:=8;
  Field(m,m);
  n1:=random(6)+2;
  n2:=random(6)+2;
  RobotBeginEnd(n1,n2,m,m);   
  TagRect(n1,n2,m,m);
end;

procedure mix2;
var n1,n2,m,i,j,ij: integer;
begin
  TaskText('Задание mix2. Закрасить в шахматном порядке прямоугольник переменного размера');
  m:=8;
  Field(m,m);
  n1:=random(6)+2;
  n2:=random(6)+2;
  RobotBeginEnd(n1,n2,m,m);
  for i:=n1 to m do
  begin
    for j:=n2 to m do
    begin 
      ij:=i+j-n1-n2;
      if ij mod 2 = 0 then
        Tag(i,j); 
    end;
  end;
end;

procedure mix3;
var n1,n2,m,l: integer;
begin
  TaskText('Задание mix3. Закрасить квадрат, предварительно определив его размер');
  m:=10;
  Field(m,m);
  n1:=random(3)+2;
  n2:=random(3)+2;
  l:=random(4)+2;
  RobotBeginEnd(n1+l,n2,n1+l,n2);   
  TagRect(n1,n2,n1+l,n2+l); 
  MarkPainted(n1,n2+l);
end;

procedure mix4;
var n1,n2,m,m1,m2: integer;
begin
  TaskText('Задание mix4. Найти закрашенную клетку');
  m:=10;
  Field(m,m);
  n1:=random(m-2)+2;
  n2:=random(m-2)+2;
  m1:=random(m-2)+2;
  m2:=random(m-2)+2;
  if (n1=m1) and (n2=m2) then
    m1:=m1-1; 
  RobotBeginEnd(n1,n2,m1,m2); 
  MarkPainted(m1,m2);
end;

procedure mix5;
var n1,n2,m,m1,m2: integer;
begin
  TaskText('Задание mix5. Найти закрашенную клетку и закрасить клетки, находящиеся с ней на одной вертикали и горизонтали');
  m:=10;
  Field(m,m);
  n1:=random(m-2)+2;
  n2:=random(m-2)+2;
  m1:=random(m-2)+2;
  m2:=random(m-2)+2;
  if (n1=n2) and (m1=m2) then
     n1:=n1-1; 
  RobotBeginEnd(n1,n2,m1,m2); 
  MarkPainted(m1,m2);   
  TagRect(1,m2,m,m2);   
  TagRect(m1,1,m1,m);
end;

procedure mix6;
var i,c: integer;
begin
  TaskText('Задание mix6. Закрасить те клетки у правой стены, для которых закрашены клетки левой стены');
  Field(11,11);
  RobotBeginEnd(1,1,1,11);
  for i:=1 to 10 do
  begin 
    c:=random(2); 
    if c=1 then 
    begin 
      MarkPainted(1,i);
      Tag(11,i) 
    end
  end
end;

procedure mix7;
var i,c1,c2: integer;
begin
  TaskText('Задание mix7. Закрасить горизонтальные ряды, в которых закрашены левая и правая клетки');
  Field(11,11);
  RobotBeginEnd(1,1,1,11);
  for i:=1 to 10 do
    begin c1:=random(2); 
    if c1=1 then 
      MarkPainted(1,i); 
    c2:=random(2); 
    if c2=1 then
      MarkPainted(11,i); 
    if (c1=1) and (c2=1) then
      TagRect(2,i,10,i); 
  end
end;

procedure mix8;
var i,c1,c2,c3,c4: integer;
begin
  TaskText('Задание mix8. Закрасить горизонтальные ряды, в которых закрашены левая и правая клетки и еще ровно одна');
  Field(11,15);
  RobotBeginEnd(1,1,1,15);
  for i:=1 to 14 do
  begin
    c1:=random(2);
    if c1=1 then
      MarkPainted(1,i);
    c2:=random(2);
    if c2=1 then
      MarkPainted(11,i);
    c3:=random(2);
    if c3=1 then
    begin 
      c4:=random(9)+2; 
      MarkPainted(c4,i); 
    end;
    if (c1=1) and (c2=1) and (c3=1) then
      TagRect(2,i,10,i);
  end
end;

procedure mix9;
var n1,n2,m,l1,l2: integer;
begin
  TaskText('Задание mix9. Закрасить прямоугольник, предварительно определив его размер, и вернуться в начальную позицию. Закрашенная клетка находится левее и ниже Робота');
  m:=10;
  Field(m,m);
  n1:=random(3)+2;
  n2:=random(3)+2;
  l1:=random(4)+2;
  l2:=random(4)+2;
  RobotBeginEnd(n1+l1,n2,n1+l1,n2);   
  TagRect(n1,n2,n1+l1,n2+l2); 
  MarkPainted(n1,n2+l2);
end;

procedure mix10;
var n,n1,n2,m,l1,l2: integer;
begin
  TaskText('Задание mix10. Закрасить прямоугольник, предварительно определив его размер, и вернуться в начальную позицию');
  m:=10;
  Field(m,m);
  n1:=random(3)+2;
  n2:=random(3)+2;
  l1:=random(4)+2;
  l2:=random(4)+2;
  TagRect(n1,n2,n1+l1,n2+l2);
  n:=random(4);
  if n=0 then 
  begin   
    RobotBeginEnd(n1,n2,n1,n2); 
    MarkPainted(n1+l1,n2+l2);
  end;
  if n=1 then 
  begin   
    RobotBeginEnd(n1+l1,n2+l2,n1+l1,n2+l2); 
    MarkPainted(n1,n2);
  end;
  if n=2 then 
  begin   
    RobotBeginEnd(n1+l1,n2,n1+l1,n2);
    MarkPainted(n1,n2+l2);
  end;
  if n=3 then 
  begin   
    RobotBeginEnd(n1,n2+l2,n1,n2+l2); 
    MarkPainted(n1+l1,n2);
  end
end;

procedure p1;
 procedure KresTag(x,y: integer);
 begin   
   TagRect(x-1,y,x+1,y);  
   TagRect(x,y-1,x,y+1); 
 end;
begin
  TaskText('Задание p1. Закрасить помеченные клетки, составив процедуру Cross');
  Field(10,10);   
  RobotBeginEnd(3,7,5,3);
  KresTag(3,7);
  KresTag(8,6);
  KresTag(5,3);
end;

procedure p2;
 procedure my(n: integer);
 var i: integer;
 begin
   for i:=1 to n do
     TagRect(3*i-1,2,3*i,3); 
 end;
begin
  TaskText('Задание p2. Закрасить помеченные клетки, составив процедуру Square2');
  Field(19,4);   
  RobotBeginEnd(2,2,17,2);
  my(6);
end;

procedure p3;
var i: integer;
begin
  TaskText('Задание p3. Закрасить помеченные клетки, используя процедуру Square2');
  Field(12,12);   
  RobotBeginEnd(11,11,1,1);
  for i:=1 to 6 do
    TagRect(2*i-1,2*i-1,2*i,2*i); 
end;

procedure p4;
begin
  TaskText('Задание p4. Закрасить помеченные клетки, используя процедуру Square2. Оформить полученный алгоритм в виде процедуры Square4');
  Field(6,6);   
  RobotBeginEnd(2,2,2,2);
  TagRect(2,2,5,5);
end;

procedure p5;
begin
  TaskText('Задание p5. Закрасить помеченные клетки, используя процедуру Square4');
  Field(10,10);   
  RobotBeginEnd(2,2,2,2);
  TagRect(2,2,9,9);
end;

procedure p6;
var i: integer;
begin
  TaskText('Задание p6. Закрасить помеченные клетки, составив процедуру Row, в которой Робот закрашивает ряд и возвращается назад');
  Field(9,9);   
  RobotBeginEnd(1,2,1,8);
  for i:=1 to 4 do
    TagRect(1,2*i,11,2*i); 
end;

procedure p7;
var i: integer;
begin
  TaskText('Задание p7. Закрасить помеченные клетки, составив процедуру Column, в которой Робот закрашивает колонку и возвращается назад');
  Field(9,9);   
  RobotBeginEnd(2,1,8,1);
  for i:=1 to 4 do
    TagRect(2*i,1,2*i,11); 
end;

procedure p8;
var i: integer;
begin
  TaskText('Задание p8. Закрасить помеченные клетки, используя процедуры Row и Column');
  Field(9,9);   
  RobotBeginEnd(1,2,8,1);
  for i:=1 to 4 do
  begin   
    TagRect(1,2*i,11,2*i);
    TagRect(2*i,1,2*i,11);
  end;
end;

procedure p9;
var i,j: integer;
begin
  TaskText('Задание p9. Закрасить помеченные клетки, используя процедуры Row и Column');
  Field(15,15);   
  RobotBeginEnd(2,2,6,6);
  i:=2;
  for j:=1 to 3 do
  begin   
    TagRect(i,i,i+8,i);  
    TagRect(i,i,i,i+8);i:=i+2;
  end;
end;

procedure p10;
var i,j: integer;
begin
  TaskText('Задание p10. Закрасить помеченные клетки, составив процедуру Contour');
  Field(31,7);   
  RobotBeginEnd(2,2,26,2);
  i:=2;
  for j:=1 to 5 do
  begin   
    TagRect(i,2,i+4,2);  
    TagRect(i,2,i,6);  
    TagRect(i,6,i+4,6);  
    TagRect(i+4,2,i+4,6);
    i:=i+6;
  end;
end;

procedure p11;
var i,j: integer;
begin
  TaskText('Задание p11. Закрасить помеченные клетки, составив процедуру Punktir');
  Field(15,11);   
  RobotBeginEnd(1,1,1,11);
  for i:=1 to 7 do
     for j:=1 to 5 do
      Tag(2*i-1,2*j-1); 
end;

procedure p12;
var i,j: integer;
begin
  TaskText('Задание p12. Закрасить помеченные клетки, используя процедуру Punktir');
  Field(16,12);   
  RobotBeginEnd(2,2,3,11);
  for i:=1 to 7 do
    for j:=1 to 5 do
    begin 
      Tag(2*i,2*j);
      Tag(2*i+1,2*j+1); 
    end
end;

procedure p13;
var i,j: integer;
begin
  TaskText('Задание p13. Закрасить помеченные клетки, используя процедуру Punktir');
  Field(17,14);   
  RobotBeginEnd(2,2,2,14);
  for i:=1 to 5 do
    for j:=1 to 4 do
    begin 
      Tag(3*i-1,3*j-1);
      Tag(3*i,3*j);
      Tag(3*i+1,3*j+1);
    end
end;

procedure p14;
var i: integer;
begin
  TaskText('Задание p14. Закрасить помеченные клетки, составив процедуры Row1 и Row2 для двух разновидностей рядов');
  Field(17,15);   RobotBeginEnd(2,2,2,14);
  for i:=1 to 8 do
      TagRect(2*i,2,2*i,13);  
  for i:=1 to 8 do
    TagRect(2,2*i,16,2*i); 
end;

procedure p15;
var i,j,n: integer;
begin
  TaskText('Задание p15. Закрасить помеченные клетки, составив процедуры Row1 и Row2 для двух разновидностей рядов');
  n:=14;  
  Field(25,n+1);  
  RobotBeginEnd(2,2,2,n);
  for i:=1 to 8 do
  begin   
    TagRect(4*i-1,2,4*i-1,n);
    for j:=1 to 6 do
    begin
      Tag(4*i-2,2*j+1);
      Tag(4*i,2*j+1);
    end;  
  end
end;

procedure pp1;
var i: integer;
begin
  TaskText('Задание pp1. Закрасить помеченные клетки, составив процедуры с параметром RightN(n) и LeftN(n)');
  Field(12,7);   
  RobotBeginEnd(1,1,1,7);
  for i:=1 to 8 do
    HorizontalWall(2,i,12); 
  Tag(7,1);Tag(9,2);Tag(5,3);Tag(4,4);Tag(11,5);Tag(2,6);Tag(7,7);
end;

procedure pp2;
begin
  TaskText('Задание pp2. Закрасить помеченные клетки, составив процедуру с параметром PaintDown(n)');
  Field(10,10);  
  RobotBeginEnd(1,1,9,1);
  VerticalWall(2,2,9);  TagRect(1,1,1,7);
  VerticalWall(4,2,9);  TagRect(3,1,3,4);
  VerticalWall(6,2,9);  TagRect(5,1,5,9);
  VerticalWall(8,2,9);  TagRect(7,1,7,6);
  TagRect(9,1,9,8);
end;

procedure pp3;
var i,d: integer;
begin
  TaskText('Задание pp3. Пройти лабиринт. Составить процедуры с параметрами RightN(n), LeftN(n), UpN(n), DownN(n)');
  d:=9;
  Field(d,d);
  RobotBeginEnd(1,1,5,5);
  for i:=1 to 4 do
  begin   
    VerticalWall(i,i,d+1-2*i);  
    HorizontalWall(i+1,d-i,d-2*i);  
    VerticalWall(d-i,i+1,d-2*i);  
    HorizontalWall(i+2,i,d-1-2*i); 
  end
end;

procedure pp4;
begin
  TaskText('Задание pp4. Закрасить помеченные клетки, составив процедуру SymbolT(m,h), где m - половина ширины символа T, а h - его высота');
  Field(19,8);
  RobotBeginEnd(5,2,17,2);
  TagRect(2,2,8,2);   TagRect(5,2,5,5);
  TagRect(10,2,14,2);   TagRect(12,2,12,6);
  TagRect(16,2,18,2);   TagRect(17,2,17,7);
end;

procedure pp5;
begin
  TaskText('Задание pp5. Закрасить прямоугольник, составив процедуру Rect(w,h), где w-ширина, а h-высота прямоугольника');
  Field(7,10);   
  RobotBeginEnd(2,2,2,2);   
  TagRect(2,2,6,9);
end;

procedure pp6;
begin
  TaskText('Задание pp6. Закрасить несколько прямоугольников, вызывая процедуру Rect(w,h)');
  Field(18,12);   
  RobotBeginEnd(2,3,3,7);
  TagRect(2,3,6,5);
  TagRect(3,7,7,11);
  TagRect(9,2,10,7);
  TagRect(12,2,15,7);
  TagRect(10,9,17,11);
end;

procedure pp7;
var x1,x2,y1,y2: integer;
begin
  TaskText('Задание pp7. Закрасить помеченные клетки, составив процедуру Perimeter(w,h), где w-ширина, а h-высота прямоугольника');
  Field(13,11);   
  RobotBeginEnd(3,2,9,3);
  x1:=3; x2:=7; y1:=2; y2:=5;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=9; x2:=12; y1:=3; y2:=7;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=2; x2:=5; y1:=8; y2:=10;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=3; x2:=7; y1:=2; y2:=5;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
end;

procedure pp8;
begin
  TaskText('Задание pp8. Закрасить помеченные клетки, составив процедуру SymbolH(w,h,t), где w-ширина, h-высота символа, а t - расстояние от верхней границы до горизонтальной линии');
  Field(16,8);
  RobotBeginEnd(2,2,13,1);
  TagRect(2,2,2,7);   TagRect(5,2,5,7);   TagRect(3,4,5,4);
  TagRect(7,3,7,7);   TagRect(11,3,11,7);   TagRect(8,6,10,6);
  TagRect(13,1,13,7);   TagRect(15,1,15,7); Tag(14,2);
end;

procedure examen1;
var i,j: integer;
begin
  TaskText('Задание examen1. Закрасить помеченные клетки, составив процедуру Punktir');
  Field(15,11);   
  RobotBeginEnd(1,1,1,11);
  for i:=1 to 7 do
    for j:=1 to 5 do
      Tag(2*i-1,2*j-1); 
end;

procedure examen2;
var i,j: integer;
begin
  TaskText('Задание examen2. Закрасить помеченные клетки, используя процедуру Punktir');
  Field(16,12);   
  RobotBeginEnd(2,2,3,11);
  for i:=1 to 7 do
  begin
    for j:=1 to 5 do
    begin 
      Tag(2*i,2*j);
      Tag(2*i+1,2*j+1); 
    end
  end
end;

procedure examen3;
var x1,x2,y1,y2: integer;
begin
  TaskText('Задание examen3. Закрасить помеченные клетки, составив процедуру Perimeter(n,m)');
  Field(13,11);   
  RobotBeginEnd(3,2,9,3);
  x1:=3; x2:=7; y1:=2; y2:=5;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=9; x2:=12; y1:=3; y2:=7;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=2; x2:=5; y1:=8; y2:=10;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
  x1:=3; x2:=7; y1:=2; y2:=5;   TagRect(x1,y1,x1,y2);   TagRect(x2,y1,x2,y2);   TagRect(x1,y1,x2,y1);   TagRect(x1,y2,x2,y2);
end;

procedure examen4;
var n1,n2,len1,len2: integer;
begin
  TaskText('Задание examen4. Закрасить все клетки вокруг прямоугольника');
  n1:=random(3)+3;
  len1:=random(5)+4;
  n2:=random(3)+3;
  len2:=random(5)+4;
  Field(14,14);  
  RobotBeginEnd(n1-1,n2,n1-1,n2);
  HorizontalWall(n1,n2,len1);
  HorizontalWall(n1,n2+len2,len1);
  VerticalWall(n1-1,n2+1,len2);
  VerticalWall(n1-1+len1,n2+1,len2);
  TagRect(n1-1,n2,n1+len1,n2);
  TagRect(n1-1,n2+len2+1,n1+len1,n2+len2+1);
  TagRect(n1-1,n2,n1-1,n2+len2);
  TagRect(n1+len1,n2,n1+len1,n2+len2);
end;

procedure examen5;
var i,n: integer;
begin
  TaskText('Задание examen5. Закрасить все клетки в коридоре переменной длины');
  n:=random(4)+10;
  Field(n,1);
  RobotBeginEnd(1,1,n,1);
  for i:=1 to n div 2 do
    Tag(i*2,1);
end;

procedure examen6;
var i,n,r1,r2: integer;
begin
  TaskText('Задание examen6. Закрасить помеченные клетки, используя 2 условных оператора в цикле');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2);
    if (r1=1) and (r2=0) then
    begin    
      HorizontalWall(i,1,1); 
      Tag(i,3); 
    end;
    if (r2=1) and (r1=0) then
    begin    
      HorizontalWall(i,2,1); 
      Tag(i,1); 
    end;
  end;
end;

procedure examen7;
var i,n,r1,r2,r3: integer;
begin
  TaskText('Задание examen7. Закрасить помеченные клетки, используя в условии "and" и "or"');
  n:=25;
  Field(n,3);
  RobotBeginEnd(1,2,n-1,2);
  VerticalWall(n-1,2,1);
  for i:=2 to n-1 do
  begin  
    r1:=random(2); 
    r2:=random(2); 
    r3:=random(2);
    if r1=1 then
      HorizontalWall(i,1,1); 
    if r2=1 then
      HorizontalWall(i,2,1); 
    if r3=1 then
      MarkPainted(i,2); 
    if (r1=0) and ((r3=1) or (r2=0)) then
      Tag(i,1); 
  end;
end;

procedure examen8;
var n,r: integer;
begin
  TaskText('Задание examen8. Закрасить клетку в противоположном углу');
  n:=11;
  Field(n,n);
  RobotBegin(1,1);
  r:=random(4);
  if r=0 then
  begin  
    RobotBegin(1,1); 
    Tag(n,n); 
    RobotEnd(n,n);
  end;
  if r=1 then
  begin  
    RobotBegin(1,n); 
    Tag(n,1); 
    RobotEnd(n,1);
  end;
  if r=2 then
  begin  
    RobotBegin(n,1); 
    Tag(1,n); 
    RobotEnd(1,n);
  end;
  if r=3 then
  begin  
    RobotBegin(n,n); 
    Tag(1,1); 
    RobotEnd(1,1);
  end;
end;

procedure examen9;
var m,n1,n2: integer;
begin
  TaskText('Задание examen9. Закрасить угол и вернуться. Использовать два счетчика');
  m:=8;
  Field(m,m);
  n1:=random(6)+2;
  n2:=random(6)+2;
  RobotBeginEnd(n1,n2,n1,n2); 
  Tag(m,m);
end;

procedure examen10;
var n1,n2: integer;
begin
  TaskText('Задание examen10. Обойти стену');
  Field(10,4);
  n1:=random(5)+4;
  n2:=random(4)+1;
  HorizontalWall(1,2,n1);
  RobotBeginEnd(n2,2,n2,3);
end;

procedure zzz;
begin
  Field(10,2);
  TaskText('Задание c1. Закрасить помеченные клетки');
  CorrectFieldBounds;
  Stop;
end;

var __initialized := false;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    RobotTaskMaker.__InitModule__;
  end;
end;

initialization
  
finalization
end.