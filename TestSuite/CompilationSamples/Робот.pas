// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Исполнитель Робот действует на прямоугольном клеточном поле. Между некоторыми клетками, а также по периметру поля находятся стены. 
///Основная цель Робота – закрасить указанные клетки и переместиться в конечную клетку.
unit Робот;

uses Robot,RobotField;

procedure Вправо := Right;
procedure Влево := Left;
procedure Вверх := Up;
procedure Вниз := Down;
procedure Закрасить := Paint;
function СправаСвободно := FreeFromRight;
function СлеваСвободно := FreeFromLeft;
function СверхуСвободно := FreeFromUp;
function СнизуСвободно := FreeFromDown;
function СправаСтена := WallFromRight;
function СлеваСтена := WallFromLeft;
function СверхуСтена := WallFromUp;
function СнизуСтена := WallFromDown;
function КлеткаНеЗакрашена := CellIsFree;
function КлеткаЗакрашена := CellIsPainted;
procedure Задание(имя: string) := Task(имя);
procedure СтандартноеПоле := StandardField;
procedure Поле(n,m: integer) := Field(n,m);

begin
  HelpStr := 
'Разработчик  исполнителя  Робот:  Михалкович С.С., 2002-17  '#10#13#10#13+
    'Команды  исполнителя  Робот:'#10#13+
    '    Вправо'#10#13+
    '    Влево'#10#13
    '    Вверх'#10#13+
    '    Вниз'#10#13
    '    Закрасить - закрасить текущую клетку'#10#13
    '    Задание(имя) - вызвать задание с указанным именем'#10#13
    '    СтандартноеПоле - вызвать стандартное поле'#10#13
    '    Поле(n,m) - вызвать поле размера n на m'#10#13
    'Условия, проверяемые  исполнителем  Робот:'#10#13
    '    СправаСтена'#10#13
    '    СлеваСтена'#10#13
    '    СверхуСтена'#10#13
    '    СнизуСтена'#10#13
    '    СправаСвободно'#10#13
    '    СлеваСвободно'#10#13
    '    СверхуСвободно'#10#13
    '    СнизуСвободно'#10#13
    '    КлеткаЗакрашена'#10#13
    '    КлеткаНеЗакрашена'#10#13;
end.