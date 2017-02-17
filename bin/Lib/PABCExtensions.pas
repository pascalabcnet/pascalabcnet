// Copyright (c) Ivan Bondarev, Stanislav Mihalkovich (for details please see \doc\copyright.txt)
// This code is distributed under the GNU LGPL (for details please see \doc\license.txt)
///--
unit PABCExtensions;

uses PABCSystem;

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenBinary<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateBinary<T>(fname: string): file of T;
begin
  PABCSystem.Rewrite(Result, fname);
end;

/// Открывает типизированный файл и возвращает значение для инициализации файловой переменной
function OpenFile<T>(fname: string): file of T;
begin
  PABCSystem.Reset(Result, fname);
end;

/// Создаёт или обнуляет типизированный файл и возвращает значение для инициализации файловой переменной
function CreateFile<T>(fname: string): file of T;
begin
  PABCSystem.Rewrite(Result, fname);
end;

/// Открывает типизированный файл целых и возвращает значение для инициализации файловой переменной
function OpenFileInteger(fname: string): file of integer;
begin
  Result := OpenFile&<integer>(fname);
end;

/// Открывает типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function OpenFileReal(fname: string): file of real;
begin
  Result := OpenFile&<real>(fname);
end;

/// Создаёт или обнуляет типизированный файл целых и возвращает значение для инициализации файловой переменной
function CreateFileInteger(fname: string): file of integer;
begin
  Result := CreateFile&<integer>(fname);
end;

/// Создаёт или обнуляет типизированный файл вещественных и возвращает значение для инициализации файловой переменной
function CreateFileReal(fname: string): file of real;
begin
  Result := CreateFile&<real>(fname);
end;

/// Возвращает последовательность элементов открытого типизированного файла
function Elements<T>(Self: file of T): sequence of T; extensionmethod;
begin
  while not Self.Eof do
  begin
    var x: T;
    read(Self,x);
    yield x;
  end;
end;

/// Считывает и возвращает следующий элемент типизированного файла
function ReadElement<T>(Self: file of T): T; extensionmethod;
begin
  Read(Self, Result);
end;

/// Открывает типизированный файл, возвращает последовательность элементов и закрывает его
function ReadElements<T>(fname: string): sequence of T;
begin
  var f := OpenBinary&<T>(fname);
  while not f.Eof do
  begin
    var x := f.ReadElement;
    yield x;
  end;
  f.Close
end;

{procedure Write<T>(Self: file of T; params vals: array of T); extensionmethod;
begin
  foreach var x in vals do
    PABCSystem.Write(Self, x);
end;}

var __initialized: boolean;

procedure __InitModule;
begin

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