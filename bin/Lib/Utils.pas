// Copyright (c) Ivan Bondarev, Stanislav Mikhalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
{$reference System.Data.dll}
unit Utils;

interface

uses System;
uses System.Data;

function RecordToByteArray(obj : object): array of byte;
procedure ShowMessage(msg: string);
procedure ShowMessage(msg,capt: string);
function GetFunctionPointer(fnc : Delegate) : integer;

/// Замеряет время работы процедуры p в миллисекундах. Производит n запусков и усредняет время работы
function Benchmark(p: procedure; n: integer := 100): real;

/// Вычисляет значение выражения, представленного в виде строки
function Eval(expr: string): object;

/// Вычисляет целое значение выражения, представленного в виде строки
function EvalInt(expr: string): integer;

/// Вычисляет вещественное значение выражения, представленного в виде строки
function EvalReal(expr: string): real;

/// Вычисляет логическое значение выражения, представленного в виде строки
function EvalBool(expr: string): boolean;

/// Вычисляет строковое значение выражения, представленного в виде строки
function EvalStr(expr: string): string;

implementation

uses System.Runtime.InteropServices;

function _MessageBox(h: integer; m,c: string; t: integer): integer;
external 'User32.dll' name 'MessageBox';

procedure ShowMessage(msg:string);
begin
  ShowMessage(msg,'Сообщение');
end;

procedure ShowMessage(msg,capt:string);
begin
  _MessageBox(0,msg,capt,0);
end;

function RecordToByteArray(obj : object): array of byte;
begin
  var len := Marshal.SizeOf(obj);
  Result := new byte[len];
  var ptr := Marshal.AllocHGlobal(len);
  Marshal.StructureToPtr(obj, ptr, false);
  Marshal.Copy(ptr, Result, 0, len);
  Marshal.FreeHGlobal(ptr); 
end;

function GetFunctionPointer(fnc : Delegate) : integer;
begin
  Result := System.Runtime.InteropServices.Marshal.GetFunctionPointerForDelegate(fnc).ToInt32;  
end;

function Benchmark(p: procedure; n: integer): real;
begin
  var sw := new Stopwatch;
  sw.Start;
  loop n do
    p;
  sw.Stop;
  Result := sw.ElapsedMilliseconds/n;
end;

function Eval(expr: string): object;
begin
  Result := DataTable.Create.Compute(expr, '');
end;

function EvalInt(expr: string): integer;
begin
  Result := Convert.ToInt32(Eval(expr));
end;

function EvalReal(expr: string): real;
begin
  Result := Convert.ToDouble(Eval(expr));
end;

function EvalBool(expr: string): boolean;
begin
  Result := Convert.ToBoolean(Eval(expr));
end;

function EvalStr(expr: string): string;
begin
  Result := Convert.ToString(Eval(expr));
end;


begin
end.