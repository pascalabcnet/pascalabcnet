unit PT4Demo;

interface

implementation

uses PT4TaskMaker;

procedure Demo3;
var
  a, b: real;
begin
  CreateTask('Ввод и вывод данных, оператор присваивания');
  TaskText('Даны стороны прямоугольника~{a} и~{b}.', 0, 2);
  TaskText('Найти его площадь {S}~=~{a}\*{b} и периметр ' + 
    '{P}~=~2\*({a}\;+\;{b}).', 0, 4);
  a := (1 + Random(100)) / 10;
  b := (1 + Random(100)) / 10;
  DataR('a = ', a, xLeft, 3, 4);
  DataR('b = ', b, xRight, 3, 4);
  ResultR('S = ', a * b, 0, 2, 4);
  ResultR('P = ', 2 * (a + b), 0, 4, 4);
  SetTestCount(3);
end;

procedure Demo4;
var
  m, n, i, j, k: integer;
  a: array [1..5, 1..8] of real;
begin
  CreateTask('Двумерные массивы (матрицы): вывод элементов');
  TaskText('Дана матрица размера~{M}\;\x\;{N} ' +
    'и целое число~{K} (1~\l~{K}~\l~{M}).', 0, 2);
  TaskText('Вывести элементы {K}-й строки данной матрицы.', 0, 4);
  m := 2 + Random(4);
  n := 4 + Random(5);
  k := 1;
  if m = 5 then k := 0;
  DataN('M = ', m, 3, 1, 1);
  DataN('N = ', n, 10, 1, 1);
  for i := 1 to M do
    for j := 1 to N do
    begin
      a[i, j] := 9.98 * Random;
      DataR('', a[i,j], Center(j, n, 4, 2), i + k, 4);
    end;
  k := 1 + Random(m);
  dataN('K = ', k, 68, 5, 1);
  for j := 1 to n do
    ResultR('', a[k, j], Center(j, n, 4, 2), 3, 4);
end;

procedure Demo5;
var s: string;
begin
  CreateTask('Символы и строки: основные операции');
  TaskText('Дана строка~{S}.', 0, 2);
  TaskText('Вывести ее первый и последний символ.', 0, 4);
  s := WordSample(Random(WordCount));
  DataS('S = ', s, 0, 3);
  ResultC('Первый символ: ', s[1], xLeft, 3);
  ResultC('Последний символ: ', s[length(s)], xRight, 3);
  SetTestCount(4);
end;

function FileName(Len: integer): string;
const
  c = '0123456789abcdefghijklmnopqrstuvwxyz';
var
  i: integer;
begin
  result := '';
  for i := 1 to Len do
    result := result + c[Random(Length(c))+1];
end;

procedure Demo6;
var
  k, i, j, jmax: integer;
  s1, s2, s3: string;
  fs1: file of ShortString;
  fs2: file of ShortString;
  fc3: file of char;
  s: ShortString;
  c: char;
begin
  CreateTask('Символьные и строковые файлы');
  TaskText('Дано целое число~{K} (>\,0) и строковый файл.', 0, 1);
  TaskText('Создать два новых файла: строковый, содержащий ' +
    'первые {K}~символов', 0, 2);
  TaskText('каждой строки исходного файла, и символьный, ' +
    'содержащий {K}-й символ', 0, 3);
  TaskText('каждой строки (если длина строки меньше~{K}, ' +
    'то в строковый файл', 0, 4);
  TaskText('записывается вся строка, а в символьный файл ' +
    'записывается пробел).', 0, 5);
  s1 := '1' + FileName(5) + '.tst';
  s2 := '2' + FileName(5) + '.tst';
  s3 := '3' + FileName(5) + '.tst';
  Assign(fs1, s1);
  Rewrite(fs1);
  Assign(fs2, s2);
  Rewrite(fs2);
  Assign(fc3, s3);
  Rewrite(fc3);
  k := 2 + Random(10);
  jmax := 0;
  for i := 1 to 10 + Random(20) do
  begin
    j := 2 + Random(15);
    if jmax < j then
      jmax := j;
    s := FileName(j);
    write(fs1, s);
    if j >= k then
      c := s[k]
    else
      c := ' ';
    write(fc3, c);
    s := copy(s, 1, k);
    write(fs2,s);
  end;
  Close(fs1);
  Close(fs2);
  Close(fc3);
  DataN('K =', k, 0, 1, 2);
  DataS('Имя исходного файла: ', s1, 3, 2);
  DataS('Имя результирующего строкового файла:  ', s2, 3, 4);
  DataS('Имя результирующего символьного файла: ', s3, 3, 5);
  DataComment('Содержимое исходного файла:', xRight, 2);
  DataFileS(s1, 3, jmax + 3);
  ResultComment('Содержимое результирующего строкового файла:', 0, 2);
  ResultComment('Содержимое результирующего символьного файла:', 0, 4);
  ResultFileS(s2, 3, k + 3);
  ResultFileC(s3, 5, 4);
end;

procedure Demo7;
var
  p: integer;
  s, s1, s2: string;
  t1, t2: text;
begin
  CreateTask('Текстовые файлы: основные операции');
  TaskText('Дан текстовый файл.', 0, 2);
  TaskText('Удалить из него все пустые строки.', 0, 4);
  s1 := FileName(6) + '.tst';
  s2 := '#' + FileName(6) + '.tst';
  s := TextSample(Random(TextCount));
  Assign(t2, s2);
  Rewrite(t2);
  Assign(t1, s1);
  Rewrite(t1);
  writeln(t2, s);
  Close(t2);
  p := Pos(#13#10#13#10, s);
  while p <> 0 do
  begin
    Delete(s, p, 2);
    p := Pos(#13#10#13#10, s);
  end;
  writeln(t1, s);
  Close(t1);
  ResultFileT(s1, 1, 5);
  Rename(t2, s1);
  DataFileT(s1, 2, 5);
  DataS('Имя файла: ', s1, 0, 1);
  SetTestCount(3);
end;

var WrongNode: TNode;

procedure Demo8Data;
var
  i, n: integer;
  p, p1, p2: PNode;
begin
  if Random(4) = 0 then
    n := 1
  else
    n := 2 + Random(8);
  new(p1);
  p1^.Data := Random(90) + 10;
  p1^.Prev := nil;
  p2 := p1;
  for i := 2 to n do
  begin
    new(p);
    p^.Data := Random(90) + 10;
    p^.Prev := p2;
    p2^.Next := p;
    p2 := p;
  end;
  p2^.Next := nil;
  SetPointer(1, p1);
  SetPointer(2, p2);
  ResultP('Последний элемент: ', 2, 0, 2);
  ResultList(1, 0, 3);
  ShowPointer(2);
  DataP('', 1, 0, 2);
  p := p1;
  for i := 1 to n do
  begin
    p^.prev := @WrongNode;
    p := p^.Next;
  end;
  DataList(1, 0, 3);
  ShowPointer(1);
end;

procedure Demo8;
begin
  CreateTask('Динамические структуры данных: двусвязный список');
  TaskText('Дан указатель~{P}_1 на начало непустой цепочки ' +
    'элементов-записей типа TNode,', 0, 1);
  TaskText('связанных между собой с помощью поля Next. Используя ' +
    'поле Prev записи TNode,', 0, 2);
  TaskText('преобразовать исходную (\Iодносвязную\i) цепочку ' +
    'в  \Iдвусвязную\i, в которой каждый', 0, 3);
  TaskText('элемент связан не только с последующим элементом ' +
    '(с помощью поля Next),', 0, 4);
  TaskText('но и с предыдущим (с помощью поля Prev). Поле Prev ' +
    'первого элемента положить', 0, 5);
  TaskText('равным \N. Вывести указатель на последний элемент ' +
    'преобразованной цепочки.', 0, 0);
  Demo8Data;
end;

procedure Demo8Net;
begin
  CreateTask('Динамические структуры данных: двусвязный список');
  TaskText('Дана ссылка~{A}_1 на начало непустой цепочки ' +
    'элементов-объектов типа Node,', 0, 1);
  TaskText('связанных между собой с помощью своих свойств Next. ' +
    'Используя свойства Prev', 0, 2);
  TaskText('данных объектов, преобразовать исходную ' +
    '(\Iодносвязную\i) цепочку в \Iдвусвязную\i,', 0, 3);
  TaskText('в которой каждый элемент связан не только ' +
    'с последующим элементом (с помощью', 0, 4);
  TaskText('свойства Next), но и с предыдущим (с помощью ' +
    'свойства Prev). Свойство Prev', 0, 5);
  TaskText('первого элемента положить равным \O. Вывести ' +
    'ссылку~{A}_2 на последний', 0, 0);
  TaskText('элемент преобразованной цепочки.', 0, 0);
  SetObjectStyle;
  Demo8Data;
end;

procedure InitTask(num: integer);
begin
  case num of
  1..2: UseTask('Begin', num);
  3: Demo3;
  4: Demo4;
  5: Demo5;
  6: Demo6;
  7: Demo7;
  8: Demo8;
  9: Demo8Net;
  end;
end;

begin
  CreateGroup('Demo', 'Примеры различных задач', 'М. Э. Абрамян, 2009',
    'df6yui13dffdsttd', 9, InitTask);
  CommentText('Данная группа демонстрирует различные возможности');
  CommentText('\Iконструктора учебных заданий\i \MPT4TaskMaker\m.');

  Subgroup('Ввод и вывод данных, оператор присваивания');
  CommentText('В этой подгруппе содержатся задания, импортированные');
  CommentText('из группы Begin.\PПриводимый ниже абзац преамбулы');
  CommentText('также импортирован из данной группы.\P');
  UseComment('Begin');

  Subgroup('Двумерные массивы (матрицы): вывод элементов');
  CommentText('Данное задание дублирует задание Matrix7.');

  Subgroup('Символьные и строковые файлы');
  CommentText('Данное задание дублирует задание File63. Оно демонстрирует');
  CommentText('особенности, связанные с двоичными \Iстроковыми\i файлами.');

  Subgroup('Текстовые файлы: основные операции');
  CommentText('Данное задание дублирует задание Text16.');

  Subgroup('Динамические структуры данных: двусвязный список');
  CommentText('Первое задание дублирует задание Dynamic30.');
  CommentText('Второе задание дублирует задание ObjDyn30 и представляет');
  CommentText('собой \<объектный\> вариант первого задания.');

  RegisterGroup('PT4Demo');
end.