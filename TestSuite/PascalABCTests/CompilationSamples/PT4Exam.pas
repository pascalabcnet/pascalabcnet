/// Модуль электронного задачника PT for Exam
unit PT4Exam;


//------------------------------------------------------------------------------
// Модуль для подключения задачника PT for Exam
// Версия 2.4
// Новое в версии: используется только стандартный вывод,
//   для ввода требуется организовать чтение из файла в самой учебной программе
// Copyright © 2014-2021 М.Э.Абрамян
//------------------------------------------------------------------------------


interface

/// Обеспечивает инициализацию задания
procedure Task(name: string);

procedure FinExam;

/// Возвращает введенное значение типа string
function ReadString: string;

/// Выводит строку S в разделе отладки окна задачника
procedure Show(S: string);

/// Выводит набор данных в разделе отладки окна задачника.
/// Вещественные числа выводятся в формате, настроенном
/// с помощью функции SetPrecision (по умолчанию 2 дробных знака).
/// Если аргументом является последовательность, то после вывода
/// ее элементов выполняется автоматический переход на новую строку.
procedure Show(params args: array of object);

/// Выполняет переход на новую экранную строку
/// в разделе отладки окна задачника
procedure ShowLine;

/// Выводит набор данных в разделе отладки окна задачника,
/// после чего выполняет переход на новую экранную строку.
/// Вещественные числа выводятся в формате, настроенном
/// с помощью функции SetPrecision (по умолчанию 2 дробных знака).
/// Если аргументом является последовательность, то после вывода
/// ее элементов выполняется автоматический переход на новую строку.
procedure ShowLine(params args: array of object);

/// Задает ширину W области вывода для числовых и строковых данных
/// в разделе отладки. Влияет на последующие вызовы функций
/// Show и ShowLine. 
procedure SetWidth(W: Integer);

/// Настраивает формат вывода вещественных чисел в разделе отладки
/// окна задачника. Если N > 0, то число выводится в формате
/// с фиксированной точкой и N дробными знаками. Если N = 0,
/// то число выводится в экспоненциальном формате, число дробных
/// знаков определяется шириной поля вывода
procedure SetPrecision(N: integer);

/// Обеспечивает автоматическое скрытие всех разделов
/// окна задачника, кроме раздела отладки
procedure HideTask;


implementation

uses 
  PT4, System.IO, System.Text;

procedure ToWin(s: string);
begin
  var fs: StreamReader := new StreamReader(s);
  var fs1: StreamWriter := new StreamWriter('$$.tmp', false, 
    Encoding.Default); 
  fs1.Write(fs.ReadToEnd);
  fs.Close;
  fs1.Close;    
  System.IO.File.Delete(s);
  System.IO.File.Move('$$.tmp',s);
end;

var 
  NextTask: boolean := False;
  s2: string;

procedure Task(name: string);
var 
  f: textfile;
begin
  PT4.Task(name);
  if NextTask then
    exit;
  NextTask := True;  
  GetS(s2);
  if s2 = '' then
  begin
    s2 := 'null2.tst';
  end;  
  Assign(output, s2);
  Rewrite(output);
end;

function ReadString: string;
begin
  result := PT4.ReadString;
end;

procedure FinExam;
begin
  Sleep(100);
  Close(output);
  if s2 = 'null2.tst' then
    Erase(output)
  else  
    ToWin(s2);
  NextTask := false;  
  Sleep(100);
end;

// == Версия 1.3. Дополнения ==

procedure Show(S: string);
begin
  PT4.Show(S);
end;

procedure Show(params args: array of object);
begin
  PT4.Show(args);
end;

procedure ShowLine;
begin
  PT4.ShowLine;
end;

procedure ShowLine(params args: array of object);
begin
  PT4.ShowLine(args);
end;

procedure SetWidth(W: Integer);
begin
  PT4.SetWidth(W);
end;

procedure HideTask;
begin
  PT4.HideTask;
end;

procedure SetPrecision(N: Integer);
begin
  PT4.SetPrecision(N);
end;


initialization
  PrintDelimDefault := '';
finalization

  FinExam;

end.
