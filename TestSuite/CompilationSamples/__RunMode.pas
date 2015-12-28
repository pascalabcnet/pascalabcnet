// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit __RunMode;

//------------------------------------------------------------------------------
// Модуль, подключаемый в режиме запуска без связи с оболочкой
// (с) DarkStar 2007
// Функции:
//    1. Перехват исключений по AppDomain.CurrentDomain.UnhandledException и 
//       вывод их в консоль
//    2. Если это - консольное окно, то устанавливает имя заголовка на имя exe-файла
//------------------------------------------------------------------------------

interface

///--
procedure __InitModule__;

implementation

uses PABCSystem, System;

procedure DbgExceptionHandler(sender: object; args: UnhandledExceptionEventArgs);
begin
  var e := Exception(args.ExceptionObject);
  if args.IsTerminating and (ExecuteBeforeProcessTerminateIn__Mode<>nil) then 
    ExecuteBeforeProcessTerminateIn__Mode(e);
  if IsConsoleApplication then begin
    Console.Error.Write('Unhandled Exception: ' + e.ToString);
    if args.IsTerminating then
      Halt;
  end else
    raise e;
end;

var __initialized := false;

procedure __InitModule;
begin
  if (CommandLineArgs.Length>0) and (CommandLineArgs[0]='[RUNMODE]') and not ExecuteAssemlyIsDll then begin
    AppDomain.CurrentDomain.UnhandledException += DbgExceptionHandler;
    if IsConsoleApplication then
        Console.Title := ExtractFileName(GetEXEFileName);
    var _a := new string[CommandLineArgs.Length-1];
    for var i:=1 to CommandLineArgs.Length-1 do
      _a[i-1] := CommandLineArgs[i];
    CommandLineArgs := _a;
  end;
end;

procedure __InitModule__;
begin
  if not __initialized then
  begin
    __initialized := true;
    __InitPABCSystem;
    __InitModule;
  end;
end;

begin
  __InitModule;
end.