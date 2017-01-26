// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Исполнитель Робот действует на прямоугольном клеточном поле. Между некоторыми клетками, а также по периметру поля находятся стены. 
///Основная цель Робота – закрасить указанные клетки и переместиться в конечную клетку.
unit Robot;

interface

/// Установить скорость Робота равной n (n=0..10)
procedure Speed(n: integer);
/// Переместить Робота вправо
procedure Right;
/// Переместить Робота влево
procedure Left;
/// Переместить Робота вверх
procedure Up;
/// Переместить Робота вниз
procedure Down;
/// Закрасить текущую ячейку
procedure Paint;
/// Возвращает True, если справа от Робота - свободно, и False в противном случае
function FreeFromRight: boolean;
/// Возвращает True, если слева от Робота - свободно, и False в противном случае
function FreeFromLeft: boolean;
/// Возвращает True, если сверху от Робота - свободно, и False в противном случае
function FreeFromUp: boolean;
/// Возвращает True, если снизу от Робота - свободно, и False в противном случае
function FreeFromDown: boolean;
/// Возвращает True, если справа от Робота - стена, и False в противном случае
function WallFromRight: boolean;
/// Возвращает True, если слева от Робота - стена, и False в противном случае
function WallFromLeft: boolean;
/// Возвращает True, если сверху от Робота - стена, и False в противном случае
function WallFromUp: boolean;
/// Возвращает True, если снизу от Робота - стена, и False в противном случае
function WallFromDown: boolean;
/// Возвращает True, если текущая ячейка не закрашена, и False в противном случае
function CellIsFree: boolean;
/// Возвращает True, если текущая ячейка закрашена, и False в противном случае
function CellIsPainted: boolean;
/// Вызывать задание с именем name
procedure Task(name: string);
/// Создать пустое поле размера 9 на 11 клеток
procedure StandardField;
/// Создать пустое поле размера n на m клеток
procedure Field(n,m: integer);
/// Запустить Робота
procedure Start;
/// Остановить Робота
procedure Stop;

// Русские подпрограммы
/// Переместить Робота вправо
procedure Вправо;
/// Переместить Робота влево
procedure Влево;
/// Переместить Робота вверх
procedure Вверх;
/// Переместить Робота вниз
procedure Вниз;
/// Закрасить текущую ячейку
procedure Закрасить;
/// Возвращает True, если справа от Робота - свободно, и False в противном случае
function СправаСвободно: boolean;
/// Возвращает True, если слева от Робота - свободно, и False в противном случае
function СлеваСвободно: boolean;
/// Возвращает True, если сверху от Робота - свободно, и False в противном случае
function СверхуСвободно: boolean;
/// Возвращает True, если снизу от Робота - свободно, и False в противном случае
function СнизуСвободно: boolean;
/// Возвращает True, если справа от Робота - стена, и False в противном случае
function СправаСтена: boolean;
/// Возвращает True, если слева от Робота - стена, и False в противном случае
function СлеваСтена: boolean;
/// Возвращает True, если сверху от Робота - стена, и False в противном случае
function СверхуСтена: boolean;
/// Возвращает True, если снизу от Робота - стена, и False в противном случае
function СнизуСтена: boolean;
/// Возвращает True, если текущая ячейка не закрашена, и False в противном случае
function ЯчейкаНеЗакрашена: boolean;
/// Возвращает True, если текущая ячейка закрашена, и False в противном случае
function ЯчейкаЗакрашена: boolean;
/// Вызывать задание с данным именем
procedure Задание(имя: string);
/// Создать пустое поле размера 9 на 11 клеток
procedure СтандартноеПоле;
/// Создать пустое поле размера n на m клеток
procedure Поле(n,m: integer);


var 
  __IS_ROBOT_UNIT: boolean;

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses RobotTaskMaker,RobotZadan,RobotField;

procedure Start;
begin
  RobField.Start;  
end;

procedure Stop;
begin
  RobField.Stop;  
end;

procedure Speed(n: integer);
begin
  RobField.SetSpeed(n);
end;

procedure Right;
begin
  RobField.Right;
  if RobField.StepState then
    Stop
  else RobField.Pause;
end;

procedure Left;
begin
  RobField.Left;
  if RobField.StepState then
    Stop
  else RobField.Pause;
end;

procedure Up;
begin
  RobField.Up;
  if RobField.StepState then
    Stop
  else RobField.Pause;
end;

procedure Down;
begin
  RobField.Down;
  if RobField.StepState then
    Stop
  else RobField.Pause;
end;

procedure Paint;
begin
  RobField.Paint;
  if RobField.StepState then
    Stop
  else RobField.Pause;
end;

function FreeFromRight: boolean;
begin
  Result := not RobField.WallFromRight;
end;

function FreeFromLeft: boolean;
begin
  Result := not RobField.WallFromLeft;
end;

function FreeFromUp: boolean;
begin
  Result := not RobField.WallFromUp;
end;

function FreeFromDown: boolean;
begin
  Result := not RobField.WallFromDown;
end;

function WallFromRight: boolean;
begin
  Result := RobField.WallFromRight;
end;

function WallFromLeft: boolean;
begin
  Result := RobField.WallFromLeft;
end;

function WallFromUp: boolean;
begin
  Result := RobField.WallFromUp;
end;

function WallFromDown: boolean;
begin
  Result := RobField.WallFromDown;
end;

function CellIsFree: boolean;
begin
  Result := RobField.CellIsFree;
end;

function CellIsPainted: boolean;
begin
  Result := RobField.CellIsPainted;
end;

procedure StandardField;
var x,y: integer;
begin
  TaskText('Стандартное поле');
  x := 9; y := 7;
  RobotTaskMaker.Field(x,y);
  RobotBeginEnd(x div 2 + 1,y div 2 + 1,0,0);
  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

procedure Field(n,m: integer);
begin
  TaskText('Поле '+IntToStr(n)+'x'+IntToStr(m));
  RobotTaskMaker.Field(n,m);
  RobotBeginEnd(n div 2 + 1,m div 2 + 1,0,0);
  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

// Русские

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
function ЯчейкаНеЗакрашена := CellIsFree;
function ЯчейкаЗакрашена := CellIsPainted;
procedure Задание(имя: string) := Task(имя);
procedure СтандартноеПоле := StandardField;
procedure Поле(n,m: integer) := Field(n,m);


procedure RegisterTasks;
begin
  RegisterTask('a1',a1);
  RegisterTask('a2',a2);
  RegisterTask('a3',a3);
  RegisterTask('a4',a4);

  RegisterTask('c1',c1);
  RegisterTask('c2',c2);
  RegisterTask('c3',c3);
  RegisterTask('c4',c4);
  RegisterTask('c5',c5);
  RegisterTask('c6',c6);
  RegisterTask('c7',c7);
  RegisterTask('c8',c8);
  RegisterTask('c9',c9);
  RegisterTask('c10',c10);
  RegisterTask('c11',c11);
  RegisterTask('c12',c12);
  RegisterTask('c13',c13);
  RegisterTask('c14',c14);
  RegisterTask('c15',c15);
  RegisterTask('c16',c16);
  
  RegisterTask('if1',if1);
  RegisterTask('if2',if2);
  RegisterTask('if3',if3);
  RegisterTask('if4',if4);
  RegisterTask('if5',if5);
  RegisterTask('if6',if6);
  RegisterTask('if7',if7);
  RegisterTask('if8',if8);
  RegisterTask('if9',if9);
  RegisterTask('if10',if10);
  RegisterTask('if11',if11);

  RegisterTask('w1',w1);
  RegisterTask('w2',w2);
  RegisterTask('w3',w3);
  RegisterTask('w4',w4);
  RegisterTask('w5',w5);
  RegisterTask('w6',w6);
  RegisterTask('w7',w7);
  RegisterTask('w8',w8);
  RegisterTask('w9',w9);
  RegisterTask('w10',w10);
  RegisterTask('w11',w11);
  RegisterTask('w12',w12);
  RegisterTask('w13',w13);
  RegisterTask('w14',w14);
  RegisterTask('w15',w15);
  RegisterTask('w16',w16);
  RegisterTask('w17',w17);
 
  RegisterTask('cif1',cif1);
  RegisterTask('cif2',cif2);
  RegisterTask('cif3',cif3);
  RegisterTask('cif4',cif4);
  RegisterTask('cif5',cif5);
  RegisterTask('cif6',cif6);
  RegisterTask('cif7',cif7);
  RegisterTask('cif8',cif8);
  RegisterTask('cif9',cif9);
  RegisterTask('cif10',cif10);
  RegisterTask('cif11',cif11);
  RegisterTask('cif12',cif12);
  RegisterTask('cif13',cif13);
  RegisterTask('cif14',cif14);
  RegisterTask('cif15',cif15);
  RegisterTask('cif16',cif16);
  RegisterTask('cif17',cif17);
  RegisterTask('cif18',cif18);
  RegisterTask('cif19',cif19);
  RegisterTask('cif20',cif20);
  RegisterTask('cif21',cif21);
  RegisterTask('cif22',cif22);

  RegisterTask('count1',count1);
  RegisterTask('count2',count2);
  RegisterTask('count3',count3);
  RegisterTask('count4',count4);
  RegisterTask('count5',count5);
  RegisterTask('count6',count6);
  RegisterTask('count7',count7);
  RegisterTask('count8',count8);
  RegisterTask('count9',count9);
  RegisterTask('count10',count10);
  RegisterTask('count11',count11);
  RegisterTask('count12',count12);
  RegisterTask('count13',count13);
  RegisterTask('count14',count14);
  RegisterTask('count15',count15);
  RegisterTask('count16',count16);
  RegisterTask('count17',count17);

  RegisterTask('cc1',cc1);
  RegisterTask('cc2',cc2);
  RegisterTask('cc3',cc3);
  RegisterTask('cc4',cc4);
  RegisterTask('cc5',cc5);
  RegisterTask('cc6',cc6);
  RegisterTask('cc7',cc7);
  RegisterTask('cc8',cc8);
  RegisterTask('cc9',cc9);
  RegisterTask('cc10',cc10);
  RegisterTask('cc11',cc11);
  RegisterTask('cc12',cc12);
  RegisterTask('cc13',cc13);
  RegisterTask('cc14',cc14);
  RegisterTask('cc15',cc15);
  RegisterTask('cc16',cc16);
  RegisterTask('cc17',cc17);
  RegisterTask('cc18',cc18);
  RegisterTask('cc19',cc19);
  
  RegisterTask('mix1',mix1);
  RegisterTask('mix2',mix2);
  RegisterTask('mix3',mix3);
  RegisterTask('mix4',mix4);
  RegisterTask('mix5',mix5);
  RegisterTask('mix6',mix6);
  RegisterTask('mix7',mix7);
  RegisterTask('mix8',mix8);
  RegisterTask('mix9',mix9);
  RegisterTask('mix10',mix10);

  RegisterTask('p1',p1);
  RegisterTask('p2',p2);
  RegisterTask('p3',p3);
  RegisterTask('p4',p4);
  RegisterTask('p5',p5);
  RegisterTask('p6',p6);
  RegisterTask('p7',p7);
  RegisterTask('p8',p8);
  RegisterTask('p9',p9);
  RegisterTask('p10',p10);
  RegisterTask('p11',p11);
  RegisterTask('p12',p12);
  RegisterTask('p13',p13);
  RegisterTask('p14',p14);
  RegisterTask('p15',p15);

  RegisterTask('pp1',pp1);
  RegisterTask('pp2',pp2);
  RegisterTask('pp3',pp3);
  RegisterTask('pp4',pp4);
  RegisterTask('pp5',pp5);
  RegisterTask('pp6',pp6);
  RegisterTask('pp7',pp7);
  RegisterTask('pp8',pp8);

  RegisterTask('examen1',examen1);
  RegisterTask('examen2',examen2);
  RegisterTask('examen3',examen3);
  RegisterTask('examen4',examen4);
  RegisterTask('examen5',examen5);
  RegisterTask('examen6',examen6);
  RegisterTask('examen7',examen7); 
  RegisterTask('examen8',examen8);
  RegisterTask('examen9',examen9);
  RegisterTask('examen10',examen10);
end;

procedure Task(name: string);
begin
  name := Trim(LowerCase(name));
  if name.StartsWith('rb') then
    Delete(name,1,2);

  if TasksDictionary.ContainsKey(name) then 
    TasksDictionary[name]()
  else RobotError(name+' - нет такого задания для Робота','Задание отсутствует');
  
  // Добавил - МА
  RobField.TaskName := name; 
  //

  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

var __initialized := false;
var __finalized := false;

procedure __InitModule;
begin
  __IS_ROBOT_UNIT := true;
  RegisterTasks;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    RobotField.__InitModule__;
    RobotZadan.__InitModule__;
    RobotTaskMaker.__InitModule__;
    __InitModule;
  end;
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    RobotField.__FinalizeModule__;
    //RobotZadan.__FinalizeModule__;
    //RobotTaskMaker.__FinalizeModule__;
    
  end;
end;

initialization
  //необходимо инициализировать иначе она не восстановится
  __InitModule;
finalization
  __FinalizeModule__
end.