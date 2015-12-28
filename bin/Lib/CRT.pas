// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)

/// <summary>
/// Модуль для работы с консолью
/// </summary>
unit CRT;

{$apptype console}
{$gendoc true}

interface

uses
  System;

const
  Black        = 0; 
  Blue         = 1; 
  Green        = 2;
  Cyan         = 3;
  Red          = 4;
  Magenta      = 5; 
  Brown        = 6; 
  LightGray    = 7;
  DarkGray     = 8; 
  LightBlue    = 9; 
  LightGreen   = 10; 
  LightCyan    = 11; 
  LightRed     = 12; 
  LightMagenta = 13; 
  Yellow       = 14; 
  White        = 15;

/// <summary>
/// Задает заголовок консольного окна
/// </summary>
/// <param name="s">Заголовок консольного окна</param>
procedure SetWindowTitle(s: string);
/// <summary>
/// Задает заголовок консольного окна
/// </summary>
/// <param name="s">Заголовок консольного окна</param>
procedure SetWindowCaption(s: string);
/// <summary>
/// Показывает курсор если он скрыт
/// </summary>
procedure ShowCursor;
/// <summary>
/// Скрывает курсор
/// </summary>
procedure HideCursor;
/// <summary>
/// Считыват нажатую клавишу
/// </summary>
function ReadKey: char;
/// <summary>
/// Возвращает true если была нажата клавиша. Считать символ можно спомощью функции ReadKey
/// </summary>
function KeyPressed: boolean;
/// <summary>
/// Возвращает высоту экрана
/// </summary>
function WindowWidth: integer;
/// <summary>
/// Возвращает ширину экрана
/// </summary>
/// <returns></returns>
function WindowHeight: integer;
/// <summary>
/// Возвращает Х-координату курсора
/// </summary>
function WhereX: integer;
/// <summary>
/// Возвращает Y-координату курсора
/// </summary>
function WhereY: integer;
/// <summary>
/// Переводит курсор в координаты (x,y)
/// </summary>
procedure GotoXY(x, y: integer);
procedure Window(x, y, w, h: integer);
/// <summary>
/// Задает размеры консольного окна
/// </summary>
/// <param name="w">Ширина</param>
/// <param name="h">Высота</param>
procedure SetWindowSize(w, h: integer);
procedure SetBufferSize(w, h: integer);
/// <summary>
/// Очищает экран, заполняя его текущим цветом фона
/// </summary>
procedure ClrScr;
/// <summary>
/// Задает цвет фона выводимого текста
/// </summary>
/// <param name="c">Цвет фона</param>
procedure TextBackground(c: integer);
/// <summary>
/// Задает цвет выводимого текста
/// </summary>
/// <param name="c">Цвет текста</param>
procedure TextColor(c: integer);
/// <summary>
/// Очищает линию на которой установлен курсор
/// </summary>
procedure ClearLine;
/// <summary>
/// Делает паузу на ms миллисекунд
/// </summary>
procedure Delay(ms: integer);

///--
procedure __InitModule__;

implementation

var
  nextkey: char;
  BlankString: string;

procedure ClearLine;
begin
  Console.CursorLeft := 0;
  Console.Write(BlankString);
  Console.CursorLeft := 0;
end;

procedure SetWindowTitle(s: string);
begin
  Console.Title := s;
end;

procedure SetWindowCaption(s: string);
begin
  Console.Title := s;
end;

procedure SetWindowSize(w, h: integer);
begin
  Console.SetWindowSize(w, h);
end;

procedure ShowCursor;
begin
  Console.CursorVisible := True;
end;

procedure HideCursor;
begin
  Console.CursorVisible := False;
end;

function ReadKey: char;// TODO продумать это
var
  KeyInfo: ConsoleKeyInfo;
begin
  if NextKey <> Chr(0) then 
  begin
    ReadKey := nextkey;
    NextKey := #0;
  end 
  else 
  begin
    KeyInfo := Console.ReadKey(true);
    ReadKey := Convert.ToChar(KeyInfo.KeyChar);
    if KeyInfo.KeyChar = #0 then 
      NextKey := Convert.ToChar(KeyInfo.Key);
  end;
  //ReadKey := Convert.ToChar(Console.ReadKey(true).Key);
end;

function KeyPressed: boolean;
begin
  KeyPressed := Console.KeyAvailable;
end;

function WindowWidth: integer;
begin
  WindowWidth := Console.WindowWidth;
end;

function WindowHeight: integer;
begin
  WindowHeight := Console.WindowHeight;
end;

function WhereX: integer;
begin
  WhereX := Console.CursorLeft + 1;
end;

function WhereY: integer;
begin
  WhereY := Console.CursorTop + 1;
end;

procedure GotoXY(x, y: integer);
begin
  if (x <= Console.WindowWidth) and (y <= Console.WindowHeight) and (x > 0) and (y > 0) then
    Console.SetCursorPosition(x - 1, y - 1);
end;

procedure Window(x, y, w, h: integer);
begin
  writeln('Функция CRT.Window не реализована');
  {Console.WindowLeft:=x;
  Console.WindowTop:=y;
  Console.WindowWidth:=w;
  Console.WindowHeight:=h;}
end;

procedure SetBufferSize(w, h: integer);
begin
  Console.SetBufferSize(w, h);
end;

procedure ClrScr;
begin
  Console.Clear;
end;

function IntToConsoleColor(c: integer): ConsoleColor;
begin
  case c of
    Black: IntToConsoleColor := ConsoleColor.Black;
    Blue: IntToConsoleColor := ConsoleColor.DarkBlue;
    Green: IntToConsoleColor := ConsoleColor.DarkGreen;
    Cyan: IntToConsoleColor := ConsoleColor.DarkCyan;
    Red: IntToConsoleColor := ConsoleColor.DarkRed;
    Magenta: IntToConsoleColor := ConsoleColor.DarkMagenta;
    Brown: IntToConsoleColor := ConsoleColor.DarkYellow;
    LightGray: IntToConsoleColor := ConsoleColor.Gray;
    DarkGray: IntToConsoleColor := ConsoleColor.DarkGray;
    LightBlue: IntToConsoleColor := ConsoleColor.Blue;
    LightGreen: IntToConsoleColor := ConsoleColor.Green;
    LightCyan: IntToConsoleColor := ConsoleColor.Cyan;
    LightRed: IntToConsoleColor := ConsoleColor.Red;
    LightMagenta: IntToConsoleColor := ConsoleColor.Magenta;
    Yellow: IntToConsoleColor := ConsoleColor.Yellow;
    White: IntToConsoleColor := ConsoleColor.White;
  else 
    raise new System.ArgumentOutOfRangeException('c');
  end;
end;

procedure TextBackground(c: integer);
begin
  Console.BackgroundColor := IntToConsoleColor(c);
end;

procedure TextColor(c: integer);
begin
  Console.ForegroundColor := IntToConsoleColor(c);
end;

procedure Delay(ms: integer);
begin
  Sleep(ms);
end;

function RedirectIOUnitUsed: boolean;
var
  t: &Type;
begin
  t := System.Reflection.Assembly.GetExecutingAssembly.GetType('__RedirectIOMode.__RedirectIOMode');
  Result := t <> nil;
end;

var
  i: integer;

var __initialized := false;

procedure __InitModule;
begin
  if (not RedirectIOUnitUsed) {and IsConsoleApplication} then 
  begin
    Console.CursorSize := 15;    
    BlankString := new string(' ', Console.BufferWidth - 1);
  end
  else
  begin
    Console.WriteLine('Программу с подключенным модулем CRT нельзя запускать по F9.');
    Console.WriteLine('Запустите программу, используя Shift-F9');
    Halt;
  end;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    PABCSystem.__InitModule__;
    __InitModule;
  end;
end;

begin
  //zdes oshibka ISConsoleApplication u nas nikogda ne inicializiruetsja
  __InitModule;
end.