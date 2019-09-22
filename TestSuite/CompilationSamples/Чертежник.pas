// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Исполнитель Чертежник предназначен для построения рисунков и чертежей на плоскости с координатами. 
///Чертежник имеет перо, которое он может поднимать, опускать и перемещать. При перемещении опущенного пера за ним остается след. 
unit Чертежник;

uses Drawman,DrawManField;

procedure ОпуститьПеро := PenDown;
procedure ПоднятьПеро := PenUp;
procedure КТочке(x,y: integer) := ToPoint(x,y);
procedure НаВектор(a,b: integer) := OnVector(a,b);
procedure Задание(имя: string) := Task(имя);
procedure СтандартноеПоле := StandardField;
procedure Поле(n,m: integer) := Field(n,m);

begin
    HelpStr := 'Разработчик  исполнителя  Чертежник:  Михалкович С.С., 2002-17  '#10#13#10#13+
    'Команды  исполнителя  Чертежник:'#10#13+
    '    ОпуститьПеро'#10#13+
    '    ПоднятьПеро'#10#13
    '    КТочке(x,y) - переместиться в точку с координатами (x,y)'#10#13+
    '    НаВектор(a,b) - переместиться из текущей точки на вектор (a,b)'#10#13
    '       a>0 - вправо, a<0 - влево, b>0 - вверх, b<0 - вниз'#10#13
    '    Задание(name) - вызвать задание с указанным именем'#10#13
    '    СтандартноеПоле - вызвать стандартное поле'#10#13
    '    Поле(n,m) - вызвать поле размера n на m'#10#13#10#13;
end.