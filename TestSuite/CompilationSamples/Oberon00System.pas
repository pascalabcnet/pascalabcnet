// (c) Mikst, Juliet 2010
/// Системный модуль для языка Oberon
unit Oberon00System;

interface

type
  /// Логический тип (TRUE | FALSE)
  BOOLEAN = System.boolean;
  /// Целое число
  INTEGER = System.Int32;
  /// Короткое целое число
  SHORTINT = System.byte;
  /// Длинное целое число
  LONGINT = System.int64;
  /// Вещественное число
  REAL = System.double;
  /// Длинное вещественное число
  LONGREAL = System.double;
  /// Символ
  CHAR = System.char;
  /// Строка
  STRING = System.string;
  /// Множество целых
  iset = array of integer;

procedure Print(o: object);
procedure Println(o: object);
procedure Println(o1,o2: integer);
procedure Println;

implementation

uses System; 

procedure Print(o: object);
begin
  Console.Write(o);
end;

/// Вывести значение
procedure Println(o: object);
begin
  Console.WriteLine(o);
end;

procedure Println(o1,o2: integer);
begin
  Print(o1);
  Println(o2);
end;

procedure Println;
begin
  Console.WriteLine;
end;
 
end.