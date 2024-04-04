{$reference System.Windows.Forms.dll}

/// Конструктор для электронного задачника Programming Taskbook 4.23

unit xPT4TaskMakerNET;

//------------------------------------------------------------------------------
// Конструктор для электронного задачника Programming Taskbook 4.23
//------------------------------------------------------------------------------
// Модуль для создания NET-библиотек с группами заданий в системе PascalABC.NET
//
// Copyright (c) 2013-2023 М.Э.Абрамян
// Электронный задачник Programming Taskbook Copyright (c) М.Э.Абрамян,1998-2023
//------------------------------------------------------------------------------


interface

const

  xCenter = 0;
  xLeft   = 100;
  xRight  = 200;

  SampleError = '#ERROR?';
  MaxLineCount = 50;

  lgPascal       = $0000001; 
  lgPascalABCNET = $0000401; 
  lgPascalNET    = $0000400;
  lgPascalABCNET_flag 
                 = $0000400; // добавлено в версии 4.14
  lgVB           = $0000002;
  lgCPP          = $0000004;
  lg1C           = $0000040;
  lgPython       = $0000080; 
  lgPython3      = $1000080; // добавлено в версии 4.14
  lgPython3_flag = $1000000; // добавлено в версии 4.14
  lgCS           = $0000100;
  lgVBNET        = $0000200;
  lgJava         = $0010000; // добавлено в версии 4.11
  lgRuby         = $0020000; // добавлено в версии 4.12
  lgWithPointers = $000003D;
  lgWithObjects  = $0FFFF80; // изменено в версии 4.22
  lgNET          = $000FF00;
  lgAll          = $0FFFFFF; // изменено в версии 4.10
  lgFS           = $0000800; // добавлено в версии 4.19
  lgJulia        = $0040000; // добавлено в версии 4.22
  lgC            = $0000008; // добавлено в версии 4.23

type

/// Процедурный тип, используемый при создании групп заданий
  TInitTaskProc = procedure(N: integer);

/// Указатель на узел динамической структуры
  PNode = ^TNode;

/// Узел динамической структуры
  TNode = record
    Data: integer;
    Next: PNode;
    Prev: PNode;
    Left: PNode;
    Right: PNode;
    Parent: PNode;
  end;

/// Добавляет к задачнику новую группу заданий с указанными характеристиками
procedure CreateGroup(GroupName, GroupDescription, GroupAuthor, GroupKey: string;
  TaskCount: integer; InitTaskProc: TInitTaskProc);

/// Добавляет к создаваемой группе задание из другой группы
procedure UseTask(GroupName: string; TaskNumber: integer);

/// Добавляет к создаваемой группе задание из другой группы
procedure UseTask(GroupName: string; TaskNumber: integer; TopicDescription: string); // добавлено в версии 4.19

/// Должна указываться первой при определении нового задания
procedure CreateTask(SubgroupName: string); overload;


/// Должна указываться первой при определении нового задания
procedure CreateTask; overload;

/// Должна указываться первой при определении нового задания
/// (вариант для параллельного режима задачника)
procedure CreateTask(SubgroupName: string; var ProcessCount: integer); overload;

/// Должна указываться первой при определении нового задания
/// (вариант для параллельного режима задачника)
procedure CreateTask(var ProcessCount: integer); overload;

/// Позволяет определить текущий язык программирования, выбранный для задачника
/// Возвращает значения, связанные с константами lgXXX
function CurrentLanguage: integer;

/// Возвращает двухбуквенную строку с описанием текущей локали
/// (в даной версии возвращается либо 'ru', либо 'en')
function CurrentLocale: string;

/// Возвращает номер текущей версии задачника в формате 'd.dd'
/// (для версий, меньших 4.10, возвращает '4.00')
function CurrentVersion: string;  // добавлено в версии 4.10

//-----------------------------------------------------------------------------

/// Добавляет к формулировке задания строку
procedure TaskText(S: string; X, Y: integer);

/// Определяет все строки формулировки задания
/// (в параметре S отдельные строки формулировки
/// должны разделяться символами #13, #10 или парой #13#10;
/// начальные и конечные пробелы в строках удаляются, 
/// пустые строки в формулировку не включаются)
procedure TaskText(S: string); // добавлено в версии 4.11

//-----------------------------------------------------------------------------

/// Добавляет к исходным данным элемент логического типа с комментарием
procedure DataB(Cmt: string; B: boolean; X, Y: integer);

/// Добавляет к исходным данным элемент логического типа
procedure DataB(B: boolean; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным целочисленный элемент с комментарием
procedure DataN(Cmt: string; N: integer; X, Y, W: integer);

/// Добавляет к исходным данным целочисленный элемент
procedure DataN(N: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным два целочисленных элемента с общим комментарием
procedure DataN2(Cmt: string; N1, N2: integer; X, Y, W: integer);

/// Добавляет к исходным данным два целочисленных элемента
procedure DataN2(N1, N2: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным три целочисленных элемента с общим комментарием
procedure DataN3(Cmt: string; N1, N2, N3: integer; X, Y, W: integer);

/// Добавляет к исходным данным три целочисленных элемента
procedure DataN3(N1, N2, N3: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным вещественный элемент с комментарием
procedure DataR(Cmt: string; R: real; X, Y, W: integer);

/// Добавляет к исходным данным вещественный элемент
procedure DataR(R: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным два вещественных элемента с общим комментарием
procedure DataR2(Cmt: string; R1, R2: real; X, Y, W: integer);

/// Добавляет к исходным данным два вещественных элемента
procedure DataR2(R1, R2: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным три вещественных элемента с общим комментарием
procedure DataR3(Cmt: string; R1, R2, R3: real; X, Y, W: integer);

/// Добавляет к исходным данным три вещественных элемента
procedure DataR3(R1, R2, R3: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным символьный элемент с комментарием
procedure DataC(Cmt: string; C: char; X, Y: integer);

/// Добавляет к исходным данным символьный элемент
procedure DataC(C: char; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным строковый элемент с комментарием
procedure DataS(Cmt: string; S: string; X, Y: integer);

/// Добавляет к исходным данным строковый элемент
procedure DataS(S: string; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к исходным данным элемент типа PNode с комментарием
procedure DataP(Cmt: string; NP: integer; X, Y: integer);

/// Добавляет к исходным данным элемент типа PNode
procedure DataP(NP: integer; X, Y: integer); // добавлено в версии 4.11

/// Добавляет комментарий в область исходных данных
procedure DataComment(Cmt: string; X, Y: integer);

//-----------------------------------------------------------------------------

/// Добавляет к результирующим данным элемент логического типа с комментарием
procedure ResultB(Cmt: string; B: boolean; X, Y: integer);

/// Добавляет к результирующим данным элемент логического типа
procedure ResultB(B: boolean; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным целочисленный элемент с комментарием
procedure ResultN(Cmt: string; N: integer; X, Y, W: integer);

/// Добавляет к результирующим данным целочисленный элемент
procedure ResultN(N: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным два целочисленных элемента с общим комментарием
procedure ResultN2(Cmt: string; N1, N2: integer; X, Y, W: integer);

/// Добавляет к результирующим данным два целочисленных элемента
procedure ResultN2(N1, N2: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным три целочисленных элемента с общим комментарием
procedure ResultN3(Cmt: string; N1, N2, N3: integer; X, Y, W: integer);

/// Добавляет к результирующим данным три целочисленных элемента
procedure ResultN3(N1, N2, N3: integer; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным вещественный элемент с комментарием
procedure ResultR(Cmt: string; R: real; X, Y, W: integer);

/// Добавляет к результирующим данным вещественный элемент
procedure ResultR(R: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным два вещественных элемента с общим комментарием
procedure ResultR2(Cmt: string; R1, R2: real; X, Y, W: integer);

/// Добавляет к результирующим данным два вещественных элемента
procedure ResultR2(R1, R2: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным три вещественных элемента с общим комментарием
procedure ResultR3(Cmt: string; R1, R2, R3: real; X, Y, W: integer);

/// Добавляет к результирующим данным три вещественных элемента
procedure ResultR3(R1, R2, R3: real; X, Y, W: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным символьный элемент с комментарием
procedure ResultC(Cmt: string; C: char; X, Y: integer);

/// Добавляет к результирующим данным символьный элемент
procedure ResultC(C: char; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным строковый элемент с комментарием
procedure ResultS(Cmt: string; S: string; X, Y: integer);

/// Добавляет к результирующим данным строковый элемент
procedure ResultS(S: string; X, Y: integer); // добавлено в версии 4.11

/// Добавляет к результирующим данным элемент типа PNode с комментарием
procedure ResultP(Cmt: string; NP: integer; X, Y: integer);

/// Добавляет к результирующим данным элемент типа PNode
procedure ResultP(NP: integer; X, Y: integer); // добавлено в версии 4.11

/// Добавляет комментарий в область результирующих данных
procedure ResultComment(Cmt: string; X, Y: integer);

//-----------------------------------------------------------------------------

/// Задает число дробных знаков при отображении вещественных чисел
procedure SetPrecision(N: integer);

/// Задает число исходных данных, минимально необходимое
/// для нахождения правильных результирующих данных
procedure SetRequiredDataCount(N: integer);

/// Задает число тестовых испытаний (от 2 до 9), при успешном
/// прохождении которых задание будет считаться выполненным
procedure SetTestCount(N: integer);

/// Возвращает горизонтальную координату, начиная с которой следует выводить I-й элемент из набора,
/// содержащего N элементов, при условии, что ширина каждого элемента равна W, а между элементами
/// надо указывать B пробелов (элементы нумеруются от 1)
function Center(I, N, W, B: integer): integer;

/// Возвращает случайное целое число, лежащее
/// в диапазоне от M до N включительно. Если указанный
/// диапазон пуст, то возвращает M.
function RandomN(M, N: integer): integer; // добавлено в версии 4.11

/// Возвращает случайное вещественное число, лежащее
/// на полуинтервале [A, B). Если указанный полуинтервал
/// пуст, то возвращает A.
function RandomR(A, B: real): real; // добавлено в версии 4.11

/// Возвращает порядковый номер текущего тестового запуска
/// (учитываются только успешные тестовые запуски). Если ранее успешных
/// запусков не было, то возвращает 1. Если задание уже выполнено
/// или запущено в демонстрационном режиме, то возвращает 0.
/// При использовании предыдущих версий задачника (до 4.10 включительно)
/// всегда возвращает 0.
function CurrentTest: integer; // добавлено в версии 4.11

//-----------------------------------------------------------------------------

/// Добавляет к исходным данным двоичный файл с целочисленными элементами (file of integer)
procedure DataFileN(FileName: string; Y, W: integer);

/// Добавляет к исходным данным двоичный файл с вещественными элементами (file of real)
procedure DataFileR(FileName: string; Y, W: integer);

/// Добавляет к исходным данным двоичный файл с символьными элементами (file of char)
procedure DataFileC(FileName: string; Y, W: integer);

/// Добавляет к исходным данным двоичный файл со строковыми элементами (file of ShortString)
procedure DataFileS(FileName: string; Y, W: integer);

/// Добавляет к исходным данным текстовый файл
procedure DataFileT(FileName: string; Y1, Y2: integer);

//-----------------------------------------------------------------------------

/// Добавляет к результирующим данным двоичный файл с целочисленными элементами (file of integer)
procedure ResultFileN(FileName: string; Y, W: integer);

/// Добавляет к результирующим данным двоичный файл с вещественными элементами (file of real)
procedure ResultFileR(FileName: string; Y, W: integer);

/// Добавляет к результирующим данным двоичный файл с символьными элементами (file of char)
procedure ResultFileC(FileName: string; Y, W: integer);

/// Добавляет к результирующим данным двоичный файл со строковыми элементами (file of ShortString)
procedure ResultFileS(FileName: string; Y, W: integer);

/// Добавляет к результирующим данным текстовый файл
procedure ResultFileT(FileName: string; Y1, Y2: integer);

//-----------------------------------------------------------------------------

/// Связывает номер NP с указателем P
procedure SetPointer(NP: integer; P: PNode);

/// Добавляет к исходным данным линейную динамическую структуру
procedure DataList(NP: integer; X, Y: integer);

/// Добавляет к результирующим данным линейную динамическую структуру
procedure ResultList(NP: integer; X, Y: integer);

/// Добавляет к исходным данным бинарное дерево
procedure DataBinTree(NP: integer; X, Y1, Y2: integer);

/// Добавляет к результирующим данным бинарное дерево
procedure ResultBinTree(NP: integer; X, Y1, Y2: integer);

/// Добавляет к исходным данным дерево общего вида
procedure DataTree(NP: integer; X, Y1, Y2: integer);

/// Добавляет к результирующим данным дерево общего вида
procedure ResultTree(NP: integer; X, Y1, Y2: integer);

/// Отображает в текущей динамической структуре указатель с номером NP
procedure ShowPointer(NP: integer);

/// Помечает в текущей результирующей динамической структуре указатель, требующий создания
procedure SetNewNode(NNode: integer);

/// Помечает в текущей исходной динамической структуре указатель, требующий удаления
procedure SetDisposedNode(NNode: integer);

//-----------------------------------------------------------------------------

/// Возвращает количество слов-образцов для языка, соответствующего текущей локали
function WordCount: integer;     //116

/// Возвращает количество предложений-образцов для языка, соответствующего текущей локали
function SentenceCount: integer; //61

/// Возвращает количество текстов-образцов для языка, соответствующего текущей локали
function TextCount: integer;     //85

/// Возвращает слово-образец с номером N (нумерация от 0) для языка, соответствующего текущей локали
function WordSample(N: integer): string;

/// Возвращает предложение-образец с номером N (нумерация от 0) для языка, соответствующего текущей локали
function SentenceSample(N: integer): string;

/// Возвращает текст-образец с номером N (нумерация от 0) для языка, соответствующего текущей локали,
/// между строками текста располагаются символы #13#10, в конце текста эти символы отсутствуют,
/// число строк не превышает MaxLineCount, между абзацами текста помещается одна пустая строка
function TextSample(N: integer): string;

//-----------------------------------------------------------------------------

/// Возвращает количество английских слов-образцов
function EnWordCount: integer;     //116

/// Возвращает количество английских предложений-образцов
function EnSentenceCount: integer; //61

/// Возвращает количество английских текстов-образцов
function EnTextCount: integer;     //85

/// Возвращает английское слово-образец с номером N (нумерация от 0)
function EnWordSample(N: integer): string;

/// Возвращает английское предложение-образец с номером N (нумерация от 0)
function EnSentenceSample(N: integer): string;

/// Возвращает английский текст-образец с номером N (нумерация от 0),
/// между строками текста располагаются символы #13#10, в конце текста эти символы отсутствуют,
/// число строк не превышает MaxLineCount, между абзацами текста помещается одна пустая строка
function EnTextSample(N: integer): string;

//-----------------------------------------------------------------------------

/// Добавляет строку комментария к текущей группе или подгруппе заданий
procedure CommentText(S: string);

/// Добавляет комментарий из другой группы заданий (или ее подгруппы,
/// если ее второй параметр не является пустой строкой)
procedure UseComment(GroupName, SubgroupName: string); overload;

/// Добавляет комментарий из другой группы заданий
procedure UseComment(GroupName: string); overload;

/// Устанавливает режим добавления комментария к подгруппе заданий
procedure Subgroup(SubgroupName: string);

//-----------------------------------------------------------------------------

/// Процедура, обеспечивающая отображение динамических структур данных
/// в "объектном стиле" при выполнении заданий в среде PascalABC.NET
/// (при использовании других сред не выполняет никаких действий)
procedure SetObjectStyle;

//-----------------------------------------------------------------------------

/// Процедура для внутреннего использования; она должна быть вызвана
/// из процедуры с именем activate (с маленькой буквы) в любой 
/// библиотеке NET с группой заданий (процедура activate имеет
/// тот же строковый параметр S, что и процедура ActivateNET)
procedure ActivateNET(S: string);

//-----------------------------------------------------------------------------

/// Устанавливает текущий процесс для последующей передачи ему данных 
/// числовых типов (при выполнении задания в параллельном режиме)
procedure SetProcess(ProcessRank: integer);

//-----------------------------------------------------------------------------

implementation

uses System.Runtime.InteropServices, System.Text;

function LoadLibrary(FileName: string): integer;
  external 'kernel32.dll' Name 'LoadLibrary';

function GetProcAddress(handle: integer; ProcName: string): System.IntPtr;
  external 'kernel32.dll' Name 'GetProcAddress';
 
function FreeLibrary(handle: integer): boolean;
  external 'kernel32.dll' Name 'FreeLibrary';

function MessageBox(hWnd: integer; lpText, lpCaption: string; uType: integer): Integer;
  external 'user32.dll' Name 'MessageBoxA';

type TTaskText = procedure (s: string);
     TCreateGroup = procedure (GroupName: string; InitTaskProc: TInitTaskProc);   

type
  TNFunc = function : integer;
  TNFuncN4 = function (N1, N2, N3, N4: integer): integer;
  TProcSb = procedure (S: StringBuilder);
  TProcNSb = procedure (N: integer; S: StringBuilder);
  TProc = procedure;
  TProcS = procedure (S: array of byte);
  TProcSCN2 = procedure (S: array of byte; C: Char; N1, N2: integer);
  TProcSN = procedure (S: array of byte; N: integer); 
  TProcSN2 = procedure (S: array of byte; N1, N2: integer);
  TProcSN3 = procedure (S: array of byte; N1, N2, N3: integer);
  TProcSN4 = procedure (S: array of byte; N1, N2, N3, N4: integer);
  TProcSN5 = procedure (S: array of byte; N1, N2, N3, N4, N5: integer);
  TProcSN6 = procedure (S: array of byte; N1, N2, N3, N4, N5, N6: integer);
  TProcSRN3 = procedure (S: array of byte; R: real; N1, N2, N3: integer);
  TProcSR2N3 = procedure (S: array of byte; R1, R2: real; N1, N2, N3: integer);
  TProcSR3N3 = procedure (S: array of byte; R1, R2, R3: real; N1, N2, N3: integer);
  TProcS2 = procedure (S1, S2: array of byte);
  TProcS2N2 = procedure (S1, S2: array of byte; N1, N2: integer);
  TProcS4NP = procedure (S1, S2, S3, S4: array of byte; N: integer; P: TInitTaskProc);
  TProcN = procedure (N: integer);
  TProcNP = procedure (N: integer; P: pointer);
  TProcN3 = procedure (N1, N2, N3: integer);
  TProcN4 = procedure (N1, N2, N3, N4: integer);
  TProcSvN = procedure (S: array of byte; var N: integer);
  TProcSNS = procedure (S1: array of byte; N: integer; S2: array of byte);

var
  creategroup_: TProcS4NP;
  usetask_: TProcSN;
  createtask_: TProcS;
  currentlanguage_: TNFunc;
  currentlocale_: TProcSb;
  tasktext_: TProcSN2;
  datab_, resultb_: TProcSN3;
  datan_, resultn_: TProcSN4;
  datan2_, resultn2_: TProcSN5;
  datan3_, resultn3_: TProcSN6;
  datar_, resultr_: TProcSRN3;
  datar2_, resultr2_: TProcSR2N3;
  datar3_, resultr3_: TProcSR3N3;
  datac_, resultc_: TProcSCN2;
  datas_, results_: TProcS2N2;
  datap_, resultp_: TProcSN3;
  datacomment_, resultcomment_: TProcSN2;
  setprecision_, settestcount_, setrequireddatacount_: TProcN;
  center_: TNFuncN4;
  datafilen_, datafiler_, datafilec_, datafiles_, datafilet_: TProcSN2;
  resultfilen_, resultfiler_, resultfilec_, resultfiles_, resultfilet_: TProcSN2;
  setpointer_: TProcNP;
  datalist_, resultlist_: TProcN3;
  databintree_, datatree_, resultbintree_, resulttree_: TProcN4;
  showpointer_, setnewnode_, setdisposednode_: TProcN;
  wordcount_, sentencecount_, textcount_: TNFunc;
  enwordcount_, ensentencecount_, entextcount_: TNFunc;
  wordsample_, sentencesample_, textsample_: TProcNSb;
  enwordsample_, ensentencesample_, entextsample_: TProcNSb;
  commenttext_: TProcS;
  usecomment_: TProcS2;
  subgroup_{, registergroup_}: TProcS; // удалено в версии 4.11
  setobjectstyle_: TProc;
  createtask2_: TProcSvN;
  setprocess_: TProcN;
  currentversion_: TProcSb; // добавлено в версии 4.10
  currenttest_: TNFunc; // добавлено в версии 4.11
  usetaskex_: TProcSNS; // добавлено в версии 4.19

  FHandle: integer;

function ToBytes(s: string): array of byte;
begin
  var utf8 := Encoding.Unicode;
  var ansi := Encoding.GetEncoding(1251);
  result := Encoding.Convert(utf8, ansi, utf8.GetBytes(S));
end;

procedure ActivateNET(S: string);
begin
  FHandle := LoadLibrary(S);
  creategroup_ := TProcS4NP(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'creategroup'), typeof(TProcS4NP)));
  usetask_ := TProcSN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'usetask'), typeof(TProcSN)));
  createtask_ := TProcS(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'createtask'), typeof(TProcS)));
  currentlanguage_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'currentlanguage'), typeof(TNFunc)));
  currentlocale_ := TProcSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'currentlocale2'), typeof(TProcSb)));
  tasktext_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'tasktext'), typeof(TProcSN2)));
  datab_ := TProcSN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datab'), typeof(TProcSN3)));
  datan_ := TProcSN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datan'), typeof(TProcSN4)));
  datan2_ := TProcSN5(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datan2'), typeof(TProcSN5)));
  datan3_ := TProcSN6(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datan3'), typeof(TProcSN6)));
  datar_ := TProcSRN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datar'), typeof(TProcSRN3)));
  datar2_ := TProcSR2N3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datar2'), typeof(TProcSR2N3)));
  datar3_ := TProcSR3N3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datar3'), typeof(TProcSR3N3)));
  datac_ := TProcSCN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datac'), typeof(TProcSCN2)));
  datas_ := TProcS2N2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datas'), typeof(TProcS2N2)));
  datap_ := TProcSN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datap'), typeof(TProcSN3)));
  datacomment_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datacomment'), typeof(TProcSN2)));
  resultb_ := TProcSN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultb'), typeof(TProcSN3)));
  resultn_ := TProcSN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultn'), typeof(TProcSN4)));
  resultn2_ := TProcSN5(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultn2'), typeof(TProcSN5)));
  resultn3_ := TProcSN6(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultn3'), typeof(TProcSN6)));
  resultr_ := TProcSRN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultr'), typeof(TProcSRN3)));
  resultr2_ := TProcSR2N3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultr2'), typeof(TProcSR2N3)));
  resultr3_ := TProcSR3N3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultr3'), typeof(TProcSR3N3)));
  resultc_ := TProcSCN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultc'), typeof(TProcSCN2)));
  results_ := TProcS2N2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'results'), typeof(TProcS2N2)));
  resultp_ := TProcSN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultp'), typeof(TProcSN3)));
  resultcomment_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultcomment'), typeof(TProcSN2)));
  setprecision_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setprecision'), typeof(TProcN)));
  settestcount_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'settestcount'), typeof(TProcN)));
  setrequireddatacount_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setrequireddatacount'), typeof(TProcN)));
  center_ := TNFuncN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'center'), typeof(TNFuncN4)));
  datafilen_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datafilen'), typeof(TProcSN2)));
  datafiler_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datafiler'), typeof(TProcSN2)));
  datafilec_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datafilec'), typeof(TProcSN2)));
  datafiles_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datafiles'), typeof(TProcSN2)));
  datafilet_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datafilet'), typeof(TProcSN2)));
  resultfilen_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultfilen'), typeof(TProcSN2)));
  resultfiler_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultfiler'), typeof(TProcSN2)));
  resultfilec_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultfilec'), typeof(TProcSN2)));
  resultfiles_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultfiles'), typeof(TProcSN2)));
  resultfilet_ := TProcSN2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultfilet'), typeof(TProcSN2)));
  setpointer_ := TProcNP(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setpointer'), typeof(TProcNP)));
  datalist_ := TProcN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datalist'), typeof(TProcN3)));
  resultlist_ := TProcN3(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultlist'), typeof(TProcN3)));
  databintree_ := TProcN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'databintree'), typeof(TProcN4)));
  datatree_ := TProcN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'datatree'), typeof(TProcN4)));
  resultbintree_ := TProcN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resultbintree'), typeof(TProcN4)));
  resulttree_ := TProcN4(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'resulttree'), typeof(TProcN4)));
  showpointer_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'showpointer'), typeof(TProcN)));
  setnewnode_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setnewnode'), typeof(TProcN)));
  setdisposednode_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setdisposednode'), typeof(TProcN)));
  wordcount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'wordcount'), typeof(TNFunc)));
  sentencecount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'sentencecount'), typeof(TNFunc)));
  textcount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'textcount'), typeof(TNFunc)));
  wordsample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'wordsample2'), typeof(TProcNSb)));
  sentencesample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'sentencesample2'), typeof(TProcNSb)));
  textsample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'textsample2'), typeof(TProcNSb)));
  enwordcount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'enwordcount'), typeof(TNFunc)));
  ensentencecount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'ensentencecount'), typeof(TNFunc)));
  entextcount_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'entextcount'), typeof(TNFunc)));
  enwordsample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'enwordsample2'), typeof(TProcNSb)));
  ensentencesample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'ensentencesample2'), typeof(TProcNSb)));
  entextsample_ := TProcNSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'entextsample2'), typeof(TProcNSb)));
  commenttext_ := TProcS(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'commenttext'), typeof(TProcS)));
  usecomment_ := TProcS2(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'usecomment'), typeof(TProcS2)));
  subgroup_ := TProcS(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'subgroup'), typeof(TProcS)));
//  registergroup_ := TProcS(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'registergroup'), typeof(TProcS)));
  setobjectstyle_ := TProc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setobjectstyle'), typeof(TProc)));
  createtask2_ := TProcSvN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'createtask2'), typeof(TProcSvN)));
  setprocess_ := TProcN(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'setprocess'), typeof(TProcN)));
  currentversion_ := TProcSb(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'currentversion2'), typeof(TProcSb))); // добавлено в версии 4.10
  currenttest_ := TNFunc(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'curt'), typeof(TNFunc))); // добавлено в версии 4.11
  usetaskex_ := TProcSNS(Marshal.GetDelegateForFunctionPointer(GetProcAddress(FHandle, 'usetaskex'), typeof(TProcSNS))); // добавлено в версии 4.19

end;

//=============================================================================

var p: TInitTaskProc := nil;

procedure CreateGroup(GroupName, GroupDescription, GroupAuthor, GroupKey: string;
  TaskCount: integer; InitTaskProc: TInitTaskProc);
begin
  p := InitTaskProc;
  creategroup_(ToBytes(GroupName), ToBytes(GroupDescription), ToBytes(GroupAuthor),
    ToBytes(GroupKey), TaskCount, p);
end;

procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  usetask_(ToBytes(GroupName), TaskNumber);
end;

procedure UseTask(GroupName: string; TaskNumber: integer; TopicDescription: string);
begin
  if usetaskex_ <> nil then
    usetaskex_(ToBytes(GroupName), TaskNumber, ToBytes(TopicDescription))
  else  
    usetask_(ToBytes(GroupName), TaskNumber);
end;

procedure CreateTask(SubgroupName: string);
begin
  createtask_(ToBytes(SubgroupName));
end;

procedure CreateTask;
begin
  CreateTask('');
end;










function CurrentLanguage: integer;
begin
  result := currentlanguage_;
end;

function CurrentLocale: string;
begin
  var S := new StringBuilder(100);
  currentlocale_(S);
  result := S.ToString;
end;

procedure TaskText(S: string; X, Y: integer);
begin
  tasktext_(ToBytes(S), X, Y);
end;

procedure TaskText(S: string);
var 
  p1, p2: array[1..205] of integer;
  n, i, k, l: integer;
  m: set of char;  
begin
  n := 0;
  m := [#13, #10];
  l := Length(S);
  i := 1;
  while i <= l do
  begin
    if not (S[i] in m) and ((i = 1) or (S[i-1] in m)) 
      and (n < 205) then
    begin
      while (i <= l) and (S[i] = ' ') do
        Inc(i);
      if (i <= l) and not (S[i] in m) then
      begin
        Inc(n);  
        p1[n] := i;
      end;
    end;
    if i > l then break;
    if not (S[i] in m) and ((i = l) or (S[i+1] in m)) then
    begin
      k := i;
      if S[k] = ' ' then
      begin
        while S[k] = ' ' do
          Dec(k);
        if (S[k] = '\') and ((k = 1) or (S[k-1] <> '\')) then
          Inc(k);
      end;
      p2[n] := k - p1[n] + 1;
      if n = 205 then break;
    end;
    Inc(i);
  end;
  case n of
  0: ;
  1: tasktext(Copy(S, p1[1], p2[1]), 0, 3);
  2: for i := 1 to n do
       tasktext(Copy(S, p1[i], p2[i]), 0, 2*i);
  3, 4:
     for i := 1 to n do
       tasktext(Copy(S, p1[i], p2[i]), 0, i+1);
  else
     begin
       for i := 1 to 5 do
         tasktext(Copy(S, p1[i], p2[i]), 0, i);
       for i := 6 to n do
         tasktext(Copy(S, p1[i], p2[i]), 0, 0);
     end;
  end;
end;

function BtoN(B: boolean): integer;
begin
  if B then
    result := 1
  else
    result := 0;
end;

procedure DataB (Cmt: string; B: boolean; X, Y: integer);
begin
  datab_(ToBytes(Cmt), BtoN(B), X, Y);
end;

procedure DataB (B: boolean; X, Y: integer);
begin
  datab_(ToBytes(''), BtoN(B), X, Y);
end;

procedure DataN (Cmt: string; N: integer; X, Y, W: integer);
begin
  datan_(ToBytes(Cmt), N, X, Y, W);
end;

procedure DataN (N: integer; X, Y, W: integer);
begin
  datan_(ToBytes(''), N, X, Y, W);
end;

procedure DataN2(Cmt: string; N1, N2: integer; X, Y, W: integer);
begin
  datan2_(ToBytes(Cmt), N1, N2,  X, Y, W);
end;

procedure DataN2(N1, N2: integer; X, Y, W: integer);
begin
  datan2_(ToBytes(''), N1, N2,  X, Y, W);
end;

procedure DataN3(Cmt: string; N1, N2, N3: integer; X, Y, W: integer);
begin
  datan3_(ToBytes(Cmt), N1, N2, N3, X, Y, W);
end;

procedure DataN3(N1, N2, N3: integer; X, Y, W: integer);
begin
  datan3_(ToBytes(''), N1, N2, N3, X, Y, W);
end;

procedure DataR(Cmt: string; R: real; X, Y, W: integer);
begin
  datar_(ToBytes(Cmt), R, X, Y, W);
end;

procedure DataR(R: real; X, Y, W: integer);
begin
  datar_(ToBytes(''), R, X, Y, W);
end;

procedure DataR2(Cmt: string; R1, R2: real; X, Y, W: integer);
begin
  datar2_(ToBytes(Cmt), R1, R2, X, Y, W);
end;

procedure DataR2(R1, R2: real; X, Y, W: integer);
begin
  datar2_(ToBytes(''), R1, R2, X, Y, W);
end;

procedure DataR3(Cmt: string; R1, R2, R3: real; X, Y, W: integer);
begin
  datar3_(ToBytes(Cmt), R1, R2, R3, X, Y, W);
end;

procedure DataR3(R1, R2, R3: real; X, Y, W: integer);
begin
  datar3_(ToBytes(''), R1, R2, R3, X, Y, W);
end;

procedure DataC(Cmt: string; C: char; X, Y: integer);
begin
  datac_(ToBytes(Cmt), C, X, Y);
end;

procedure DataC(C: char; X, Y: integer);
begin
  datac_(ToBytes(''), C, X, Y);
end;

procedure DataS(Cmt: string; S: string; X, Y: integer);
begin
  datas_(ToBytes(Cmt), ToBytes(S), X, Y);
end;

procedure DataS(S: string; X, Y: integer);
begin
  datas_(ToBytes(''), ToBytes(S), X, Y);
end;

procedure DataP(Cmt: string; NP: integer; X, Y: integer);
begin
  datap_ (ToBytes(Cmt), NP, X, Y);
end;

procedure DataP(NP: integer; X, Y: integer);
begin
  datap_ (ToBytes(''), NP, X, Y);
end;

procedure DataComment(Cmt: string; X, Y: integer);
begin
  datacomment_(ToBytes(Cmt), X, Y);
end;

procedure ResultB(Cmt: string; B: boolean; X, Y: integer);
begin
  resultb_(ToBytes(Cmt), BtoN(B), X, Y);
end;

procedure ResultB(B: boolean; X, Y: integer);
begin
  resultb_(ToBytes(''), BtoN(B), X, Y);
end;

procedure ResultN(Cmt: string; N: integer; X, Y, W: integer);
begin
  resultn_(ToBytes(Cmt), N, X, Y, W);
end;

procedure ResultN(N: integer; X, Y, W: integer);
begin
  resultn_(ToBytes(''), N, X, Y, W);
end;

procedure ResultN2(Cmt: string; N1, N2: integer; X, Y, W: integer);
begin
  resultn2_(ToBytes(Cmt), N1, N2, X, Y, W);
end;

procedure ResultN2(N1, N2: integer; X, Y, W: integer);
begin
  resultn2_(ToBytes(''), N1, N2, X, Y, W);
end;

procedure ResultN3(Cmt: string; N1, N2, N3: integer; X, Y, W: integer);
begin
  resultn3_(ToBytes(Cmt), N1, N2, N3, X, Y, W);
end;

procedure ResultN3(N1, N2, N3: integer; X, Y, W: integer);
begin
  resultn3_(ToBytes(''), N1, N2, N3, X, Y, W);
end;

procedure ResultR(Cmt: string; R: real; X, Y, W: integer);
begin
  resultr_(ToBytes(Cmt), R, X, Y, W);
end;

procedure ResultR(R: real; X, Y, W: integer);
begin
  resultr_(ToBytes(''), R, X, Y, W);
end;

procedure ResultR2(Cmt: string; R1, R2: real; X, Y, W: integer);
begin
  resultr2_(ToBytes(Cmt), R1, R2, X, Y, W);
end;

procedure ResultR2(R1, R2: real; X, Y, W: integer);
begin
  resultr2_(ToBytes(''), R1, R2, X, Y, W);
end;

procedure ResultR3(Cmt: string; R1, R2, R3: real; X, Y, W: integer);
begin
  resultr3_(ToBytes(Cmt), R1, R2, R3, X, Y, W);
end;

procedure ResultR3(R1, R2, R3: real; X, Y, W: integer);
begin
  resultr3_(ToBytes(''), R1, R2, R3, X, Y, W);
end;

procedure ResultC(Cmt: string; C: char; X, Y: integer);
begin
  resultc_(ToBytes(Cmt), C, X, Y);
end;

procedure ResultC(C: char; X, Y: integer);
begin
  resultc_(ToBytes(''), C, X, Y);
end;

procedure ResultS(Cmt: string; S: string; X, Y: integer);
begin
  results_(ToBytes(Cmt), ToBytes(S), X, Y);
end;

procedure ResultS(S: string; X, Y: integer);
begin
  results_(ToBytes(''), ToBytes(S), X, Y);
end;

procedure ResultP(Cmt: string; NP: integer; X, Y: integer);
begin
  resultp_(ToBytes(Cmt), NP, X, Y);
end;

procedure ResultP(NP: integer; X, Y: integer);
begin
  resultp_(ToBytes(''), NP, X, Y);
end;

procedure ResultComment(Cmt: string; X, Y: integer);
begin
  resultcomment_(ToBytes(Cmt), X, Y);
end;

procedure SetPrecision(N: integer);
begin
  setprecision_(N);
end;

procedure SetTestCount(N: integer);
begin
  settestcount_(N);
end;

procedure SetRequiredDataCount(N: integer);
begin
  setrequireddatacount_(N);
end;

function Center(I, N, W, B: integer): integer;
begin
  result := center_(I, N, W, B);
end;

procedure DataFileN(FileName: string; Y, W: integer);
begin
  datafilen_(ToBytes(FileName), Y, W);
end;

procedure DataFileR(FileName: string; Y, W: integer);
begin
  datafiler_(ToBytes(FileName), Y, W);
end;

procedure DataFileC(FileName: string; Y, W: integer);
begin
  datafilec_(ToBytes(FileName), Y, W);
end;

procedure DataFileS(FileName: string; Y, W: integer);
begin
  datafiles_(ToBytes(FileName), Y, W);
end;

procedure DataFileT(FileName: string; Y1, Y2: integer);
begin
  datafilet_(ToBytes(FileName), Y1, Y2);
end;

procedure ResultFileN(FileName: string; Y, W: integer);
begin
  resultfilen_(ToBytes(FileName), Y, W);
end;

procedure ResultFileR(FileName: string; Y, W: integer);
begin
  resultfiler_(ToBytes(FileName), Y, W);
end;

procedure ResultFileC(FileName: string; Y, W: integer);
begin
  resultfilec_(ToBytes(FileName), Y, W);
end;

procedure ResultFileS(FileName: string; Y, W: integer);
begin
  resultfiles_(ToBytes(FileName), Y, W);
end;

procedure ResultFileT(FileName: string; Y1, Y2: integer);
begin
  resultfilet_(ToBytes(FileName), Y1, Y2);
end;

procedure SetPointer(NP: integer; P: PNode);
begin
  setpointer_(NP, P);
end;

procedure DataList(NP: integer; X, Y: integer);
begin
  datalist_(NP, X, Y);
end;

procedure ResultList(NP: integer; X, Y: integer);
begin
  resultlist_(NP, X, Y);
end;

procedure DataBinTree(NP: integer; X, Y1, Y2: integer);
begin
  databintree_(NP, X, Y1, Y2);
end;

procedure ResultBinTree(NP: integer; X, Y1, Y2: integer);
begin
  resultbintree_(NP, X, Y1, Y2);
end;

procedure DataTree(NP: integer; X, Y1, Y2: integer);
begin
  datatree_(NP, X, Y1, Y2);
end;

procedure ResultTree(NP: integer; X, Y1, Y2: integer);
begin
  resulttree_(NP, X, Y1, Y2);
end;

procedure ShowPointer(NP: integer);
begin
  showpointer_(NP);
end;

procedure SetNewNode(NNode: integer);
begin
  setnewnode_(NNode);
end;

procedure SetDisposedNode(NNode: integer);
begin
  setdisposednode_(NNode);
end;

function WordCount: integer;
begin
  result := wordcount_;
end;

function SentenceCount: integer;
begin
  result := sentencecount_;
end;

function TextCount: integer;
begin
  result := textcount_;
end;

function WordSample(n: integer): string;
begin
  var S := new StringBuilder(20); //max=11
  wordsample_(n, S);
  result := S.ToString;
end;

function SentenceSample(n: integer): string;
begin
  var S := new StringBuilder(100); //max=76
  sentencesample_(n, S);
  result := S.ToString;
end;

function TextSample(n: integer): string;
begin
  var S := new StringBuilder(1100); //max=1076
  textsample_(n, S);
  result := S.ToString;
end;

function EnWordCount: integer;
begin
  result := enwordcount_;
end;

function EnSentenceCount: integer;
begin
  result := ensentencecount_;
end;

function EnTextCount: integer;
begin
  result := entextcount_;
end;

function EnWordSample(n: integer): string;
begin
  var S := new StringBuilder(20); //max=14
  enwordsample_(n, S);
  result := S.ToString;
end;

function EnSentenceSample(n: integer): string;
begin
  var S := new StringBuilder(100); //max=76
  ensentencesample_(n, S);
  result := S.ToString;
end;

function EnTextSample(n: integer): string;
begin
  var S := new StringBuilder(1100); //max=1009
  entextsample_(n, S);
  result := S.ToString;
end;

procedure CommentText(S: string);
begin
  commenttext_(ToBytes(S));
end;

procedure UseComment(GroupName, SubgroupName: string);
begin
  usecomment_(ToBytes(GroupName), ToBytes(SubgroupName));
end;

procedure UseComment(GroupName: string);
begin
  UseComment(GroupName, '');
end;

procedure Subgroup(SubgroupName: string);
begin
  subgroup_(ToBytes(SubgroupName));
end;

procedure SetObjectStyle;
begin
  setobjectstyle_;
end;

{ // удалено в версии 4.11
procedure RegisterGroup(UnitName: string);
begin
  registergroup_(PChar(UnitName));
end;
}

procedure ShowError(S1, S2: string);
begin
  MessageBox(0, S1+#13#10'is not found '+
      'in the PT4 library.'#13#10'You should update '+
      'the Programming Taskbook to '+S2+' version.', 
      'PT4TaskMaker Error', 16);
end;

procedure CreateTask(SubgroupName: string; var ProcessCount: integer);
begin
  if createtask2_ <> nil then
    createtask2_(ToBytes(SubgroupName), ProcessCount)
  else
    ShowError('The CreateTask procedure with ProcessCount parameter', '4.9');
end;

procedure CreateTask(var ProcessCount: integer);
begin
  CreateTask('', ProcessCount);
end;

procedure SetProcess(ProcessRank: integer);
begin
  if setprocess_ <> nil then
    setprocess_(ProcessRank);
end;

function CurrentVersion: string;
begin
  result := '4.00';
  if currentversion_ <> nil then
  begin
    var S := new StringBuilder(100);
    currentversion_(S);
    result := S.ToString;
  end;
end;

function CurrentTest: integer;
begin
  result := 0;
  if currenttest_ <> nil then
    result := currenttest_;
end;

function RandomN(M, N: integer): integer;
begin
  result := M;
  if M < N then
    result := Random(N-M+1) + M;
end;

function RandomR(A, B: real): real;
begin
  result := A;
  if A < B then
    result := Random * (B-A) + A;
end;

initialization

finalization

  FreeLibrary(FHandle);

end.

