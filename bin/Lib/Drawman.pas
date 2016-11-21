// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

///Исполнитель Чертежник предназначен для построения рисунков и чертежей на плоскости с координатами. 
///Чертежник имеет перо, которое он может поднимать, опускать и перемещать. При перемещении опущенного пера за ним остается след. 
unit Drawman;

interface

/// Устанавливает скорость Чертежника равной n (n=0..10)
procedure Speed(n: integer);
/// Поднять перо
procedure PenUp;
/// Опустить перо
procedure PenDown;
/// Переместиться к точке (x,y)
procedure ToPoint(x,y: integer);
/// Переместиться на вектор (a,b)
procedure OnVector(a,b: integer);
/// Вызывать задание с именем name
procedure Task(name: string);
/// Создать пустое поле размера 9 на 11 клеток
procedure StandardField;
/// Создать пустое поле размера n на m клеток
procedure Field(n,m: integer);

/// Запустить Чертежника
procedure Start;
/// Остановить Чертежника
procedure Stop;

/// Поднять перо
procedure ОпуститьПеро;
/// Опустить перо
procedure ПоднятьПеро;
/// Переместиться к точке (x,y)
procedure КТочке(x,y: integer);
/// Переместиться на вектор (a,b)
procedure НаВектор(a,b: integer);
/// Вызывать задание с данным именем
procedure Задание(имя: string);
/// Создать пустое поле размера 9 на 11 клеток
procedure СтандартноеПоле;
/// Создать пустое поле размера n на m клеток
procedure Поле(n,m: integer);


/// Установить ширину пера Чертежника
procedure SetDrawmanWidth(w: integer);

///--
procedure __InitModule__;
///--
procedure __FinalizeModule__;

implementation

uses DMTaskMaker,DMZadan,DrawmanField;

procedure SetDrawmanWidth(w: integer);
begin
  DMField.dmwidth := w
end;

procedure Start;
begin
  DMField.Start;  
end;

procedure Stop;
begin
  DMField.Stop;  
end;

procedure Speed(n: integer);
begin
  DMField.SetSpeed(n);
end;

procedure PenUp;
begin
  DMField.PenUp;
  if DMField.StepState then
    Stop
  else DMField.Pause;
end;

procedure PenDown;
begin
  DMField.PenDown;
  if DMField.StepState then
    Stop
  else DMField.Pause;
end;

procedure ToPoint(x,y: integer);
begin
  DMField.ToPoint(x,y);
  if DMField.StepState then
    Stop
  else DMField.Pause;
end;

procedure OnVector(a,b: integer);
begin
  DMField.OnVector(a,b);
  if DMField.StepState then
    Stop
  else DMField.Pause;
end;

procedure StandardField;
var x,y: integer;
begin
  TaskText('Стандартное поле');
  x := 9; y := 7;
  DMTaskMaker.Field(x,y);
  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

procedure Field(n,m: integer);
begin
  TaskText('Поле '+IntToStr(n)+'x'+IntToStr(m));
  DMTaskMaker.Field(n,m);
  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

procedure ОпуститьПеро := PenDown;
procedure ПоднятьПеро := PenUp;
procedure КТочке(x,y: integer) := ToPoint(x,y);
procedure НаВектор(a,b: integer) := OnVector(a,b);
procedure Задание(имя: string) := Task(имя);
procedure СтандартноеПоле := StandardField;
procedure Поле(n,m: integer) := Field(n,m);


procedure RegisterTasks;
begin
  RegisterTask('a1',a1);
  RegisterTask('a2',a2);
  RegisterTask('a3',a3);
  RegisterTask('a4',a4);
  RegisterTask('a5',a5);
  RegisterTask('a6',a6);
  
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
  RegisterTask('c17',c17);
  RegisterTask('c18',c18);
  RegisterTask('c19',c19);
  RegisterTask('c20',c20);
  RegisterTask('c21',c21);
  RegisterTask('c22',c22);
  RegisterTask('c23',c23);
  RegisterTask('c24',c24);
  RegisterTask('c25',c25);
  RegisterTask('c26',c26);
  
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

  RegisterTask('p1',p1);
  RegisterTask('p2',p2);
  RegisterTask('p3',p3);
  RegisterTask('p4',p4);
  
  RegisterTask('pp1',pp1);
  RegisterTask('pp2',pp2);
  RegisterTask('pp3',pp3);
  RegisterTask('pp4',pp4);
  RegisterTask('pp5',pp5);
  RegisterTask('pp6',pp6);
  RegisterTask('pp7',pp7);
  RegisterTask('pp8',pp8);
  RegisterTask('pp9',pp9);
  RegisterTask('pp10',pp10);
  RegisterTask('pp11',pp11);
  RegisterTask('pp12',pp12);
  RegisterTask('pp13',pp13);
  RegisterTask('pp14',pp14);
  RegisterTask('pp15',pp15);
  RegisterTask('pp16',pp16);
  RegisterTask('pp17',pp17);
  RegisterTask('pp18',pp18);
  RegisterTask('pp19',pp19);

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
  name:=Trim(LowerCase(name));
  if name.StartsWith('dm') then
    Delete(name,1,2);

  if TasksDictionary.ContainsKey(name) then 
    TasksDictionary[name]()
  else DrawManError(name+' - нет такого задания для Чертежника','Задание отсутствует');
  
  // Добавил - МА
  DMField.TaskName := name; 
  //

  CorrectFieldBounds;
  SetTaskCall;
  Stop;
end;

var __initialized := false;
var __finalized := false;

procedure __InitModule;
begin
  RegisterTasks;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    DrawManField.__InitModule__;
    DMZadan.__InitModule__;
    DMTaskMaker.__InitModule__;
    __InitModule;
  end;
end;

procedure __FinalizeModule__;
begin
  if not __finalized then
  begin
    __finalized := true;
    DrawManField.__FinalizeModule__;
    DMZadan.__FinalizeModule__;
    DMTaskMaker.__FinalizeModule__;
  end;
end;

initialization
  __InitModule;
end.