{$reference System.Windows.Forms.dll}

/// Конструктор учебных заданий для задачника Programming Taskbook.
/// Версия 1.9 от 26.03.2023 (С) М. Э. Абрамян, 2016-2023.
/// Все компоненты конструктора могут вызываться либо как классовые методы класса pt, 
/// либо как обычные константы и процедуры.
unit xPT4MakerNetX;

interface

const
  OptionAllLanguages = 1;       // группа доступна для всех языков
  OptionPascalLanguages = 2;    // группа доступна для всех реализаций Паскаля
  OptionNETLanguages = 4;       // группа доступна для всех NET-языков
  OptionUseAddition = 8;        // группа доступна только при наличии файла дополнений
  OptionHideExamples = 16;      // не отображать раздел с примером верного решения

/// Процедура, с которой должно начинаться определение нового задания.
/// Должна вызываться в процедуре с именем, начинающемся с текста Task.
/// Параметр topic определяет имя подгруппы и является необязательным.
/// Параметр tasktext содержит формулировку задания; отдельные строки
/// формулировки должны разделяться символами #13, #10 или #13#10.
procedure NewTask(topic, tasktext: string);


/// Процедура, с которой должно начинаться определение нового задания.
/// Должна вызываться в процедуре с именем, начинающемся с текста Task.
/// Параметр tasktext содержит формулировку задания; отдельные строки
/// формулировки должны разделяться символами #13, #10 или #13#10.
procedure NewTask(tasktext: string);


/// Добавляет комментарий в новой строке раздела исходных даннных.
procedure DataComm(comm: string);

/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
procedure Data(comm: string; a: object);

/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
procedure Data(comm1: string; a1: object; comm2: string);

/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
procedure Data(comm1: string; a1: object; comm2: string; a2: object);

/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);

/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);

/// Добавляет последовательность логических данных в раздел исходных данных.
procedure Data(seq: sequence of boolean);

/// Добавляет последовательность целых чисел в раздел исходных данных.
procedure Data(seq: sequence of integer);

/// Добавляет последовательность вещественных чисел в раздел исходных данных.
procedure Data(seq: sequence of real);

/// Добавляет последовательность символов в раздел исходных данных.
procedure Data(seq: sequence of char);

/// Добавляет последовательность строк в раздел исходных данных.
procedure Data(seq: sequence of string);

/// Добавляет комментарий в новой строке раздела результатов.
procedure ResComm(comm: string);

/// Добавляет данные и комментарии в новой строке раздела результатов.
procedure Res(comm: string; a: object);

/// Добавляет данные и комментарии в новой строке раздела результатов.
procedure Res(comm1: string; a1: object; comm2: string);

/// Добавляет данные и комментарии в новой строке раздела результатов.
procedure Res(comm1: string; a1: object; comm2: string; a2: object);

/// Добавляет данные и комментарии в новой строке раздела результатов.
procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);

/// Добавляет данные и комментарии в новой строке раздела результатов.
procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);

/// Добавляет последовательность логических данных в раздел результатов.
procedure Res(seq: sequence of boolean);

/// Добавляет последовательность целых чисел в раздел результатов.
procedure Res(seq: sequence of integer);

/// Добавляет последовательность вещественных чисел в раздел результатов.
procedure Res(seq: sequence of real);

/// Добавляет последовательность символов в раздел результатов.
procedure Res(seq: sequence of char);

/// Добавляет последовательность строк в раздел результатов.
procedure Res(seq: sequence of string);

/// Задает минимальную ширину поля вывода для числовых данных
/// (числа выравниваются по правому краю поля вывода, 
/// т.е. при необходимости дополняются слева пробелами).
/// Если n не лежит в диапазоне 0..10, то вызов процедуры игнорируется.
/// По умолчанию минимальная ширина поля вывода полагается равной 0.
procedure SetWidth(n: integer);

/// Задает формат отображения вещественных чисел: 
/// с фиксированной точкой и n дробными знаками при n > 0,
/// с плавающей точкой и n дробными знаками при n < 0,
/// с плавающей точкой и 6 дробными знаками при n = 0.
/// Если n по модулю превосходит 10, то вызов процедуры игнорируется.
/// По умолчанию устанавливается формат с фиксированной точкой
/// и 2 дробными знаками.
procedure SetPrecision(n: integer);

/// Задает количество тестовых запусков, выполняемых
/// для проверки правильности программы (от 2 до 10). 
/// По умолчанию число тестовых запусков полагается равным 5.
procedure SetTestCount(n: integer);

/// Задает минимально необходимое количество элементов 
/// исходных данных, которое требуется ввести для правильного
/// решения задачи в случае текущего набора исходных данных. 
/// При отсутствии процедуры предполагается,
/// что для правильного решения надо ввести все исходные данные.
procedure SetRequiredDataCount(n: integer);

/// Возвращает номер текущего тестового запуска 
/// (запуски нумеруются от 1).
function CurrentTest: integer;

/// Генерирует случайное целое число в диапазоне от M до N включительно.
/// Если M >= N, то возвращает M.
function Random(M, N: integer): integer;

/// Генерирует случайное вещественное число на промежутке [A, B).
/// Если промежуток [A, B) пуст, то возвращает A.
function Random(A, B: real): real;

/// Генерирует случайное вещественное число на промежутке [A, B)
/// с одним дробным знаком и случайной добавкой порядка 1e-7.
/// Если промежуток [A, B) пуст, то возвращает A, округленное
/// до одного дробного знака и снабженное добавкой порядка 1e-7.
function Random1(A, B: real): real;

/// Генерирует случайное вещественное число на промежутке [A, B)
/// с двумя дробными знаками и случайной добавкой порядка 1e-7.
/// Если промежуток [A, B) пуст, то возвращает A, округленное
/// до двух дробных знаков и снабженное добавкой порядка 1e-7.
function Random2(A, B: real): real;

/// Генерирует случайную строку длины len, состоящую
/// из цифр и строчных (т.е. маленьких) латинских букв.
function RandomName(len: integer): string;

/// Создает новую группу с кратким описанием GroupDescription,
/// информацией об авторе GroupAuthor и набором необязательных опций, объединяемых операцией or.
/// Имя группы определяется по имени библиотеки (путем отбрасывания префикса xPT4
/// и возможных суффиксов _ru или _en). 
/// В группу включаются задания, определенные в процедурах, имена которых начинаются с текста Task.
/// Процедура NewGroup должна быть вызвана в процедуре inittaskgroup без параметров, которую
/// необходимо описать в библиотеке с группой заданий (все буквы в имени inittaskgroup - строчные).
procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer := 0);

/// Создает новую группу с кратким описанием GroupDescription, английским описанием GroupEnDescription,
/// информацией об авторе GroupAuthor и набором необязательных опций, объединяемых операцией or.
/// Имя группы определяется по имени библиотеки (путем отбрасывания префикса PT4
/// и возможных суффиксов _ru или _en). 
/// В группу включаются задания, определенные в процедурах, имена которых начинаются с текста Task.
/// Процедура NewGroup должна быть вызвана в процедуре inittaskgroup без параметров, которую
/// необходимо описать в библиотеке с группой заданий (все буквы в имени inittaskgroup - строчные).
procedure NewGroup(GroupDescription, GroupEnDescription, GroupAuthor: string; Options: integer := 0);

/// Обеспечивает регистрацию созданной группы в электронном задачнике.
/// Процедура ActivateNET(S) должна быть вызвана в процедуре activate(S: string),
/// которую необходимо описать в библиотеке с группой заданий (все буквы в имени activate - строчные).
procedure ActivateNET(S: string);

/// Импортирует в создаваемую группу существующее задание
/// из группы GroupName с номером TaskNumber. Должна вызываться
/// в процедуре с именем, начинающемся с текста Task.
procedure UseTask(GroupName: string; TaskNumber: integer);

/// Импортирует в создаваемую группу существующее задание
/// из группы GroupName с номером TaskNumber. Должна вызываться
/// в процедуре с именем, начинающемся с текста Task.
procedure UseTask(GroupName: string; TaskNumber: integer; TopicDescription: string);

/// Возвращает массив из 116 русских слов.
function GetWords: array of string;
/// Возвращает массив из 116 английских слов.
function GetEnWords: array of string;
/// Возвращает массив из 61 русского предложения.
function GetSentences: array of string;
/// Возвращает массив из 61 английского предложения.
function GetEnSentences: array of string;
/// Возвращает массив из 85 русских многострочных текстов.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
function GetTexts: array of string;
/// Возвращает массив из 85 английских многострочных текстов.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
function GetEnTexts: array of string;

/// Возвращает случайное русское слово из массива, 
/// входящего в конструктор учебных заданий.
function RandomWord: string;
/// Возвращает случайное английское слово из массива, 
/// входящего в конструктор учебных заданий.
function RandomEnWord: string;
/// Возвращает случайное русское предложение из массива, 
/// входящего в конструктор учебных заданий.
function RandomSentence: string;
/// Возвращает случайное английское предложение из массива, 
/// входящего в конструктор учебных заданий.
function RandomEnSentence: string;
/// Возвращает случайный русский многострочный текст из массива, 
/// входящего в конструктор учебных заданий.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
function RandomText: string;
/// Возвращает случайный английский многострочный текст из массива, 
/// входящего в конструктор учебных заданий.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
function RandomEnText: string;


/// Добавляет в задание исходный файл целых чисел
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных.
procedure DataFileInteger(FileName: string);
/// Добавляет в задание исходный файл вещественных чисел
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных.
procedure DataFileReal(FileName: string);
/// Добавляет в задание исходный символьный файл
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных. Символы должны
/// храниться в файле в однобайтной кодировке.
procedure DataFileChar(FileName: string);
/// Добавляет в задание исходный строковый файл
/// с элементами типа ShortString и именем FileName 
/// и отображает его содержимое в разделе исходных данных.
/// Длина элементов файла не должна превосходить 70 символов.
/// Строки должны храниться в файле в однобайтной кодировке.
procedure DataFileString(FileName: string);
/// Добавляет в задание исходный текстовый файл
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных. Длина каждой строки
/// текстового файла не должна превосходить 70 символов.
/// Текст должен храниться в файле в однобайтной кодировке.
procedure DataText(FileName: string; LineCount: integer := 4);

/// Добавляет в задание результирующий файл целых чисел
/// с именем FileName и отображает его содержимое в разделе результатов.
procedure ResFileInteger(FileName: string);
/// Добавляет в задание результирующий файл вещественных чисел
/// с именем FileName и отображает его содержимое в разделе результатов.
procedure ResFileReal(FileName: string);
/// Добавляет в задание результирующий символьный файл
/// с именем FileName и отображает его содержимое в разделе результатов.
/// Символы должны храниться в файле в однобайтной кодировке.
procedure ResFileChar(FileName: string);
/// Добавляет в задание результирующий строковый файл
/// с элементами типа ShortString и именем FileName 
/// и отображает его содержимое в разделе результатов.
/// Длина элементов файла не должна превосходить 70 символов.
/// Строки должны храниться в файле в однобайтной кодировке.
procedure ResFileString(FileName: string);
/// Добавляет в задание результирующий текстовый файл
/// с именем FileName и отображает его содержимое в разделе результатов. 
/// Длина каждой строки текстового файла не должна превосходить 70 символов.
/// Текст должен храниться в файле в однобайтной кодировке.
procedure ResText(FileName: string; LineCount: integer := 5);

type pt = class
  private
   constructor Create;
   begin
   end;
  public
/// Дополнительная опция для процедуры NewGroup: 
/// группа доступна для всех языков.  
  class function OptionAllLanguages: integer;    
  begin
    result := xPT4MakerNetX.OptionAllLanguages;
  end;  
/// Дополнительная опция для процедуры NewGroup: 
/// группа доступна для всех реализаций языка Pascal.
  class function OptionPascalLanguages: integer;    
  begin
    result := xPT4MakerNetX.OptionPascalLanguages;
  end;  
/// Дополнительная опция для процедуры NewGroup: 
/// группа доступна для всех NET-языков.
  class function OptionNETLanguages: integer;    
  begin
    result := xPT4MakerNetX.OptionNETLanguages;
  end;  
/// Дополнительная опция для процедуры NewGroup: 
/// группа доступна только при наличии связанного с ней файла дополнений.
  class function OptionUseAddition: integer;    
  begin
    result := xPT4MakerNetX.OptionUseAddition;
  end;  
/// Дополнительная опция для процедуры NewGroup: 
/// в заданиях данной группы не будет отображаться пример верного решения.
  class function OptionHideExamples: integer;    
  begin
    result := xPT4MakerNetX.OptionHideExamples;
  end;  
/// Процедура, с которой должно начинаться определение нового задания.
/// Должна вызываться в процедуре с именем, начинающемся с текста Task.
/// Параметр topic определяет имя подгруппы (является необязательным).
/// Параметр tasktext содержит формулировку задания; отдельные строки
/// формулировки должны разделяться символами #13, #10 или #13#10.
class procedure NewTask(topic, tasktext: string);
begin
  xPT4MakerNetX.NewTask(topic, tasktext);
end;

/// Процедура, с которой должно начинаться определение нового задания.
/// Должна вызываться в процедуре с именем, начинающемся с текста Task.
/// Параметр tasktext содержит формулировку задания; отдельные строки
/// формулировки должны разделяться символами #13, #10 или #13#10.
class procedure NewTask(tasktext: string);
begin
  xPT4MakerNetX.NewTask(tasktext);
end;

/// Добавляет комментарий в новой строке раздела исходных даннных.
class procedure DataComm(comm: string);
begin
  xPT4MakerNetX.DataComm(comm);
end;
/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
class procedure Data(comm: string; a: object);
begin
  xPT4MakerNetX.Data(comm, a);
end;
/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
class procedure Data(comm1: string; a1: object; comm2: string);
begin
  xPT4MakerNetX.Data(comm1, a1, comm2);
end;
/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object);
begin
  xPT4MakerNetX.Data(comm1, a1, comm2, a2);
end;
/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  xPT4MakerNetX.Data(comm1, a1, comm2, a2, comm3);
end;
/// Добавляет данные и комментарии в новой строке раздела исходных даннных.
class procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  xPT4MakerNetX.Data(comm1, a1, comm2, a2, comm3, a3);
end;
/// Добавляет последовательность логических данных в раздел исходных данных.
class procedure Data(seq: sequence of boolean);
begin
  xPT4MakerNetX.Data(seq);
end;
/// Добавляет последовательность целых чисел в раздел исходных данных.
class procedure Data(seq: sequence of integer);
begin
  xPT4MakerNetX.Data(seq);
end;
/// Добавляет последовательность вещественных чисел в раздел исходных данных.
class procedure Data(seq: sequence of real);
begin
  xPT4MakerNetX.Data(seq);
end;
/// Добавляет последовательность символов в раздел исходных данных.
class procedure Data(seq: sequence of char);
begin
  xPT4MakerNetX.Data(seq);
end;
/// Добавляет последовательность строк в раздел исходных данных.
class procedure Data(seq: sequence of string);
begin
  xPT4MakerNetX.Data(seq);
end;

/// Добавляет комментарий в новой строке раздела результатов.
class procedure ResComm(comm: string);
begin
  xPT4MakerNetX.ResComm(comm);
end;
/// Добавляет данные и комментарии в новой строке раздела результатов.
class procedure Res(comm: string; a: object);
begin
  xPT4MakerNetX.Res(comm, a);
end;
/// Добавляет данные и комментарии в новой строке раздела результатов.
class procedure Res(comm1: string; a1: object; comm2: string);
begin
  xPT4MakerNetX.Res(comm1, a1, comm2);
end;
/// Добавляет данные и комментарии в новой строке раздела результатов.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object);
begin
  xPT4MakerNetX.Res(comm1, a1, comm2, a2);
end;
/// Добавляет данные и комментарии в новой строке раздела результатов.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  xPT4MakerNetX.Res(comm1, a1, comm2, a2, comm3);
end;
/// Добавляет данные и комментарии в новой строке раздела результатов.
class procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  xPT4MakerNetX.Res(comm1, a1, comm2, a2, comm3, a3);
end;
/// Добавляет последовательность логических данных в раздел результатов.
class procedure Res(seq: sequence of boolean);
begin
  xPT4MakerNetX.Res(seq);
end;
/// Добавляет последовательность целых чисел в раздел результатов.
class procedure Res(seq: sequence of integer);
begin
  xPT4MakerNetX.Res(seq);
end;
/// Добавляет последовательность вещественных чисел в раздел результатов.
class procedure Res(seq: sequence of real);
begin
  xPT4MakerNetX.Res(seq);
end;
/// Добавляет последовательность символов в раздел результатов.
class procedure Res(seq: sequence of char);
begin
  xPT4MakerNetX.Res(seq);
end;
/// Добавляет последовательность строк в раздел результатов.
class procedure Res(seq: sequence of string);
begin
  xPT4MakerNetX.Res(seq);
end;

/// Задает минимальную ширину поля вывода для числовых данных
/// (числа выравниваются по правому краю поля вывода, 
/// т.е. при необходимости дополняются слева пробелами).
/// Если n не лежит в диапазоне 0..10, то вызов процедуры игнорируется.
/// По умолчанию минимальная ширина поля вывода полагается равной 0.
class procedure SetWidth(n: integer);
begin
  xPT4MakerNetX.SetWidth(n);
end;
/// Задает формат отображения вещественных чисел: 
/// с фиксированной точкой и n дробными знаками при n > 0,
/// с плавающей точкой и n дробными знаками при n < 0,
/// с плавающей точкой и 6 дробными знаками при n = 0.
/// Если n по модулю превосходит 10, то вызов процедуры игнорируется.
/// По умолчанию устанавливается формат с фиксированной точкой
/// и 2 дробными знаками.
class procedure SetPrecision(n: integer);
begin
  xPT4MakerNetX.SetPrecision(n);
end;
/// Задает количество тестовых запусков, выполняемых
/// для проверки правильности программы (от 2 до 10). 
/// По умолчанию число тестовых запусков полагается равным 5.
class procedure SetTestCount(n: integer);
begin
  xPT4MakerNetX.SetTestCount(n);
end;
/// Задает минимально необходимое количество элементов 
/// исходных данных, которое требуется ввести для правильного
/// решения задачи в случае текущего набора исходных данных. 
/// При отсутствии процедуры предполагается,
/// что для правильного решения надо ввести все исходные данные.
class procedure SetRequiredDataCount(n: integer);
begin
  xPT4MakerNetX.SetRequiredDataCount(n);
end;
/// Возвращает номер текущего тестового запуска 
/// (запуски нумеруются от 1).
class function CurrentTest: integer;
begin
  result := xPT4MakerNetX.CurrentTest;
end;

/// Генерирует случайное целое число в диапазоне от M до N включительно.
/// Если M >= N, то возвращает M.
class function Random(M, N: integer): integer;
begin
  result := xPT4MakerNetX.Random(M, N);
end;
/// Генерирует случайное вещественное число на промежутке [A, B).
/// Если промежуток [A, B) пуст, то возвращает A.
class function Random(A, B: real): real;
begin
  result := xPT4MakerNetX.Random(A, B);
end;
/// Генерирует случайное вещественное число на промежутке [A, B)
/// с одним дробным знаком и случайной добавкой порядка 1e-7.
/// Если промежуток [A, B) пуст, то возвращает A, округленное
/// до одного дробного знака и снабженное добавкой порядка 1e-7.
class function Random1(A, B: real): real;
begin
  result := xPT4MakerNetX.Random1(A, B);
end;
/// Генерирует случайное вещественное число на промежутке [A, B)
/// с двумя дробными знаками и случайной добавкой порядка 1e-7.
/// Если промежуток [A, B) пуст, то возвращает A, округленное
/// до двух дробных знаков и снабженное добавкой порядка 1e-7.
class function Random2(A, B: real): real;
begin
  result := xPT4MakerNetX.Random2(A, B);
end;
/// Генерирует случайную строку длины len, состоящую
/// из цифр и строчных (т.е. маленьких) латинских букв.
class function RandomName(len: integer): string;
begin
  result := xPT4MakerNetX.RandomName(len);
end;

/// Создает новую группу с кратким описанием GroupDescription,
/// информацией об авторе GroupAuthor и набором необязательных опций, объединяемых операцией or.
/// Имя группы определяется по имени библиотеки (путем отбрасывания префикса xPT4 
/// и возможных суффиксов _ru или _en). 
/// В группу включаются задания, определенные в процедурах, имена которых начинаются с текста Task.
/// Процедура NewGroup должна быть вызвана в процедуре inittaskgroup без параметров, которую
/// необходимо описать в библиотеке с группой заданий (все буквы в имени inittaskgroup - строчные).
class procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer := 0);
begin
  xPT4MakerNetX.NewGroup(GroupDescription, GroupAuthor, Options);
end;
/// Создает новую группу с кратким описанием GroupDescription, английским описанием GroupEnDescription,
/// информацией об авторе GroupAuthor и набором необязательных опций, объединяемых операцией or.
/// Имя группы определяется по имени библиотеки (путем отбрасывания префикса PT4
/// и возможных суффиксов _ru или _en). 
/// В группу включаются задания, определенные в процедурах, имена которых начинаются с текста Task.
/// Процедура NewGroup должна быть вызвана в процедуре inittaskgroup без параметров, которую
/// необходимо описать в библиотеке с группой заданий (все буквы в имени inittaskgroup - строчные).
class procedure NewGroup(GroupDescription, GroupEnDescription, GroupAuthor: string; Options: integer := 0);
begin
  xPT4MakerNetX.NewGroup(GroupDescription, GroupEnDescription, GroupAuthor, Options);
end;
/// Обеспечивает регистрацию созданной группы в электронном задачнике.
/// Процедура ActivateNET(S) должна быть вызвана в процедуре activate(S: string),
/// которую необходимо описать в библиотеке с группой заданий (все буквы в имени activate - строчные).
class procedure ActivateNET(S: string);
begin
  xPT4MakerNetX.ActivateNET(S);
end;
/// Импортирует в создаваемую группу существующее задание
/// из группы GroupName с номером TaskNumber. Должна вызываться
/// в процедуре с именем, начинающемся с текста Task.
class procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  xPT4MakerNetX.UseTask(GroupName, TaskNumber);
end;
/// Импортирует в создаваемую группу существующее задание
/// из группы GroupName с номером TaskNumber. Должна вызываться
/// в процедуре с именем, начинающемся с текста Task.
class procedure UseTask(GroupName: string; TaskNumber: integer; TopicDescription: string);
begin
  xPT4MakerNetX.UseTask(GroupName, TaskNumber, TopicDescription);
end;
/// Возвращает массив из 116 русских слов.
class function GetWords: array of string;
begin
  result := xPT4MakerNetX.GetWords;
end;
/// Возвращает массив из 116 английских слов.
class function GetEnWords: array of string;
begin
  result := xPT4MakerNetX.GetEnWords;
end;
/// Возвращает массив из 61 русского предложения.
class function GetSentences: array of string;
begin
  result := xPT4MakerNetX.GetSentences;
end;
/// Возвращает массив из 61 английского предложения.
class function GetEnSentences: array of string;
begin
  result := xPT4MakerNetX.GetEnSentences;
end;
/// Возвращает массив из 85 русских многострочных текстов.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
class function GetTexts: array of string;
begin
  result := xPT4MakerNetX.GetTexts;
end;
/// Возвращает массив из 85 английских многострочных текстов.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
class function GetEnTexts: array of string;
begin
  result := xPT4MakerNetX.GetEnTexts;
end;
/// Возвращает случайное русское слово из массива, 
/// входящего в конструктор учебных заданий.
class function RandomWord: string;
begin
  result := xPT4MakerNetX.RandomWord;
end;
/// Возвращает случайное английское слово из массива, 
/// входящего в конструктор учебных заданий.
class function RandomEnWord: string;
begin
  result := xPT4MakerNetX.RandomEnWord;
end;
/// Возвращает случайное русское предложение из массива, 
/// входящего в конструктор учебных заданий.
class function RandomSentence: string;
begin
  result := xPT4MakerNetX.RandomSentence;
end;
/// Возвращает случайное английское предложение из массива, 
/// входящего в конструктор учебных заданий.
class function RandomEnSentence: string;
begin
  result := xPT4MakerNetX.RandomEnSentence;
end;
/// Возвращает случайный русский многострочный текст из массива, 
/// входящего в конструктор учебных заданий.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
class function RandomText: string;
begin
  result := xPT4MakerNetX.RandomText;
end;
/// Возвращает случайный английский многострочный текст из массива, 
/// входящего в конструктор учебных заданий.
/// Строки текста разделяются символами #13#10.
/// В конце текста символы #13#10 отсутствуют.
class function RandomEnText: string;
begin
  result := xPT4MakerNetX.RandomEnText;
end;

/// Добавляет в задание исходный файл целых чисел
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных.
class procedure DataFileInteger(FileName: string);
begin
  xPT4MakerNetX.DataFileInteger(FileName);
end;
/// Добавляет в задание исходный файл вещественных чисел
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных.
class procedure DataFileReal(FileName: string);
begin
  xPT4MakerNetX.DataFileReal(FileName);
end;
/// Добавляет в задание исходный символьный файл
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных. Символы должны
/// храниться в файле в однобайтной кодировке.
class procedure DataFileChar(FileName: string);
begin
  xPT4MakerNetX.DataFileChar(FileName);
end;
/// Добавляет в задание исходный строковый файл
/// с элементами типа ShortString и именем FileName 
/// и отображает его содержимое в разделе исходных данных.
/// Длина элементов файла не должна превосходить 70 символов.
/// Строки должны храниться в файле в однобайтной кодировке.
class procedure DataFileString(FileName: string);
begin
  xPT4MakerNetX.DataFileString(FileName);
end;
/// Добавляет в задание исходный текстовый файл
/// с именем FileName и отображает его содержимое
/// в разделе исходных данных. Длина каждой строки
/// текстового файла не должна превосходить 70 символов.
/// Текст должен храниться в файле в однобайтной кодировке.
class procedure DataText(FileName: string; LineCount: integer := 4);
begin
  xPT4MakerNetX.DataText(FileName);
end;

/// Добавляет в задание результирующий файл целых чисел
/// с именем FileName и отображает его содержимое в разделе результатов.
class procedure ResFileInteger(FileName: string);
begin
  xPT4MakerNetX.ResFileInteger(FileName);
end;
/// Добавляет в задание результирующий файл вещественных чисел
/// с именем FileName и отображает его содержимое в разделе результатов.
class procedure ResFileReal(FileName: string);
begin
  xPT4MakerNetX.ResFileReal(FileName);
end;
/// Добавляет в задание результирующий символьный файл
/// с именем FileName и отображает его содержимое в разделе результатов.
/// Символы должны храниться в файле в однобайтной кодировке.
class procedure ResFileChar(FileName: string);
begin
  xPT4MakerNetX.ResFileChar(FileName);
end;
/// Добавляет в задание результирующий строковый файл
/// с элементами типа ShortString и именем FileName 
/// и отображает его содержимое в разделе результатов.
/// Длина элементов файла не должна превосходить 70 символов.
/// Строки должны храниться в файле в однобайтной кодировке.
class procedure ResFileString(FileName: string);
begin
  xPT4MakerNetX.ResFileString(FileName);
end;
/// Добавляет в задание результирующий текстовый файл
/// с именем FileName и отображает его содержимое в разделе результатов. 
/// Длина каждой строки текстового файла не должна превосходить 70 символов.
/// Текст должен храниться в файле в однобайтной кодировке.
class procedure ResText(FileName: string; LineCount: integer := 5);
begin
  xPT4MakerNetX.ResText(FileName);
end;
end;


implementation
{$reference system.windows.forms.dll}
uses System.Reflection, xPT4TaskMakerNET, System.Windows.Forms, System;

const
  alphabet = '0123456789abcdefghijklmnopqrstuvwxyz';
  ErrMes1 = 'Error: Раздел размером более 5 строк не может содержать файловые данные.';
  ErrMes2 = 'Error: При наличии файловых данных раздел не может содержать более 5 строк.';
  ErrMes3 = 'Error: Количество исходных данных превысило 200.';
  ErrMes4 = 'Error: Количество результирующих данных превысило 200.';
  ErrMes5 = 'Error: При определении задания первой должна вызываться процедура NewTask.';
  ErrMes6 = 'Error: При определении задания не указаны исходные данные.';
  ErrMes7 = 'Error: При определении задания не указаны результирующие данные.';

var
  yd, yr, ye, nd, nr, pr, wd: integer;
  nt, ut, fd, fr: boolean;
  fmt: string;
  tasks := new List<MethodInfo>(100);

procedure Show(s: string);
begin
  MessageBox.Show(s, 'Error', MessageBoxButtons.OK, MessageBoxIcon.Error);
end;

function ErrorMessage(s: string): string;
begin
  result := Copy(s + new string(' ', 100), 1, 78);
end;

procedure ErrorInfo(s: string);
begin
  xPT4TaskMakerNET.TaskText('\B'+ErrorMessage(s)+'\b', 0, ye);
  ye := ye + 1;
  if ye > 5 then ye := 0;
end;

function CheckTT: boolean;
begin
  result := ut;
  if not nt then
  begin
    NewTask('');
    ErrorInfo(ErrMes5);
  end;  
end;

function RandomName(len: integer): string;
begin
  result := ArrRandom(len, 1, alphabet.Length)
    .Select(e -> alphabet[e]).JoinIntoString('');
end;

procedure SetPrecision(n: integer);
begin
  if CheckTT then exit;
  if abs(n) > 10 then exit;
  pr := n;
  if n < 0 then
  begin
    fmt := 'e' + IntToStr(-n);
    n := 0;
  end
  else if n = 0 then
    fmt := 'e6'
  else
    fmt := 'f' + IntToStr(n); 
  xPT4TaskMakerNET.SetPrecision(n);
end;

procedure SetWidth(n: integer);
begin
  if (n >= 0) and (n <= 20) then
    wd := n;
end;

procedure NewTask(topic, tasktext: string);
begin
  if nt then exit;
  xPT4TaskMakerNET.CreateTask(topic);
  xPT4TaskMakerNET.TaskText(tasktext);
  nt := true;  // вызвана процедура NewTask
  ut := false; // было подключено существующее задание (процедурой UseTask)
  ye := 1;     // текущий номер строки для вывода сообщения об ошибке
  yd := 0;     // текущий номер строки в разделе исходных данных
  yr := 0;     // текущий номер строки в разделе результатов
  nd := 0;     // количество элементов исходных данных
  nr := 0;     // количество элементов результирующих данных
  fd := false; // наличие файловых данных в разделе исходных данных
  fr := false; // наличие файловых данных в разделе результатов
  pr := 2;     // текущая точность вывода вещественных данных
  fmt := 'f2'; // текущий формат вывода вещественных данных
  wd := 0;     // текущая ширина поля вывода для чисел
end;

function wreal(w: integer; x: real): integer;
begin
  result := w;
  if w = 0 then
  begin
    result := Format('{0,0:' + fmt + '}', x).Length;
    if (pr <= 0) and (x >= 0) then
      result := result + 1;
  end;
end;

function winteger(w: integer; x: integer): integer;
begin
  result := w;
  if w = 0 then
    result := IntToStr(x).Length;
end;

procedure NewTask(tasktext: string);
begin
  NewTask('', tasktext);
end;

procedure Data(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fd then
  begin
    ErrorInfo(ErrMes2);
    exit;  
  end;
  inc(nd);
  if nd > 200 then
  begin
    ErrorInfo(ErrMes3);
    exit;  
  end;
  if a is boolean then
    DataB(s, boolean(a), x, y)
  else if a is integer then
    DataN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    DataR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    DataC(s, char(a), x, y)
  else if a is string then
    DataS(s, string(a), x, y)
  else
    DataComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure Res(s: string; a: object; x, y, w: integer);
begin
  if (y > 5) and fr then
  begin
    ErrorInfo(ErrMes2);
    exit;  
  end;
  inc(nr);
  if nr > 200 then
  begin
    ErrorInfo(ErrMes4);
    exit;  
  end;
  if a is boolean then
    ResultB(s, boolean(a), x, y)
  else if a is integer then
    ResultN(s, integer(a), x, y, winteger(w, integer(a)))
  else if a is real then
    ResultR(s, real(a), x, y, wreal(w, real(a)))
  else if a is char then
    ResultC(s, char(a), x, y)
  else if a is string then
    ResultS(s, string(a), x, y)  
  else
    ResultComment(Copy(s + '!WrongType:' + a.GetType.Name, 1, 38), x, y);
end;

procedure DataComm(comm: string);
begin
  if CheckTT then exit;
  inc(yd);
  DataComment(comm, 0, yd);
end;

procedure Data(comm: string; a: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm, a, 0, yd, wd);
end;

procedure Data(comm1: string; a1: object; comm2: string);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  DataComment(comm2, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, xRight, yd, wd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, 0, yd, wd);
  DataComment(comm3, xRight, yd);
end;

procedure Data(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  if CheckTT then exit;
  inc(yd);
  Data(comm1, a1, xLeft, yd, wd);
  Data(comm2, a2, 0, yd, wd);
  Data(comm3, a3, xRight, yd, wd);
end;

procedure Data(seq: sequence of boolean);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of integer);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of real);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> wreal(0, e)(*Format('{0,0:'+fmt+'}', e)*)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of char);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure Data(seq: sequence of string);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yd);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yd);
      i := 1;
    end;
    Data('', e, Center(i, n, w, 2), yd, w);
  end;  
end;

procedure ResComm(comm: string);
begin
  if CheckTT then exit;
  inc(yr);
  ResultComment(comm, 0, yr);
end;

procedure Res(comm: string; a: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm, a, 0, yr, wd);
end;

procedure Res(comm1: string; a1: object; comm2: string);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  ResultComment(comm2, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, xRight, yr, wd);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, 0, yr, wd);
  ResultComment(comm3, xRight, yr);
end;

procedure Res(comm1: string; a1: object; comm2: string; a2: object; comm3: string; a3: object);
begin
  if CheckTT then exit;
  inc(yr);
  Res(comm1, a1, xLeft, yr, wd);
  Res(comm2, a2, 0, yr, wd);
  Res(comm3, a3, xRight, yr, wd);
end;

procedure Res(seq: sequence of boolean);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 5;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yr);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of integer);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> IntToStr(e)).Max(e -> e.Length);
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yr);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of real);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := wd;
  if w = 0 then
    w := seq.Select(e -> wreal(0, e)).Max;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yr);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of char);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := 3;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yr);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;

procedure Res(seq: sequence of string);
begin
  if CheckTT then exit;
  var n := seq.Count;
  if n = 0 then exit;
  inc(yr);
  var w := seq.Max(e -> e.Length) + 2;
  var wmax := 80 div (w + 2);
  if n > wmax then
    n := wmax;
  var i := 0;
  foreach var e in seq do
  begin
    inc(i);
    if i > wmax then
    begin
      inc(yr);
      i := 1;
    end;
    Res('', e, Center(i, n, w, 2), yr, w);
  end;  
end;


procedure ActivateNET(S: string);
begin
  xPT4TaskMakerNET.ActivateNET(S);
end;

procedure RunTask(num: integer);
var ut0: boolean;
begin
  try
    try
    if (num > 0) and (num <= tasks.Count) then
      tasks[num - 1].Invoke(nil, nil);
    finally
      nt := false;
      ut0 := ut;
      ut := false;
    end;
  except
    on e: TargetInvocationException do
    begin
      ErrorInfo('Error ' 
          + e.InnerException.GetType.Name + ': '
          + e.InnerException.message);
    end;
  end;
  if ut0 then exit;
  if (nd = 0) and not fd then 
    DataS('\B'+Copy(ErrorMessage(ErrMes6),1,76)+'\b', '', 1, 1);
  if (nr = 0) and not fr then 
    ResultS('\B'+Copy(ErrorMessage(ErrMes7),1,76)+'\b', '', 1, 1);
end;

function GetGroupName(assname: string; var libname: string): string;
begin
  libname := copy(assname, 1, pos(',', assname)-1);
  result := libname;
  var p := pos('_', libname);
  if p > 0 then
    delete(result, p, 100); // удаление суффикса, определяющего язык интерфейса
  delete(result, 1, 4);     // удаление префикса xPT4
end;

function AcceptedLanguage(opt: integer): boolean;
begin
  var lang := CurrentLanguage;
  result := (lang = lgPascalABCNET) 
    or (opt and OptionAllLanguages = OptionAllLanguages) 
    or (lang and lgNET <> 0) and (opt and OptionNETLanguages = OptionNETLanguages)
    or (lang and lgPascal = lgPascal) and (opt and OptionPascalLanguages = OptionPascalLanguages);
end;

procedure NewGroup(GroupDescription, GroupAuthor: string; Options: integer);
begin
  NewGroup(GroupDescription, '', GroupAuthor, Options);
end;

procedure NewGroup(GroupDescription, GroupEnDescription, GroupAuthor: string; Options: integer);
begin
  if not AcceptedLanguage(Options) then 
    exit; // недопустимый текущий язык
  var ass := Assembly.GetCallingAssembly;
  var LibName := '';
  var GroupName := GetGroupName(ass.FullName, LibName);
  tasks.Clear;
  var GroupKey := 'GK';
  foreach var e in ass.GetType(LibName + '.' + LibName).GetMethods.OrderBy(e -> e.Name.ToUpper) do  //2018.08
    if e.Name.ToUpper.StartsWith('TASK') then
    begin
      tasks.Add(e);
      GroupKey := GroupKey + Copy(e.Name, 5, 100);
    end; 
  if tasks.Count = 0 then
  begin
    Show('Группа ' + GroupName + ' не содержит заданий'#13#10+
    '(имена процедур с заданиями должны начинаться с текста "Task").');
    exit;
  end;
  if tasks.Count > 999 then
  begin
    Show('Группа ' + GroupName + ' содержит более 999 заданий.');
    exit;
  end;
  GroupKey := Copy(GroupKey, 1, 30);
  if Options and OptionUseAddition = OptionUseAddition then
    GroupKey := GroupKey + '#UseAddition#';
  if Options and OptionHideExamples = OptionHideExamples then
    GroupKey := GroupKey + '#HideExamples#';
  if GroupEnDescription.Trim <> '' then
    GroupKey := GroupKey + '#EnTopic<' + GroupEnDescription.Trim + '>#';
  CreateGroup(GroupName, GroupDescription, GroupAuthor, 
      GroupKey, tasks.Count, RunTask);
end;


procedure SetTestCount(n: integer);
begin
  if CheckTT then exit;
  xPT4TaskMakerNET.SetTestCount(n);
end;

procedure SetRequiredDataCount(n: integer);
begin
  if CheckTT then exit;
  xPT4TaskMakerNET.SetRequiredDataCount(n);
end;

function Random(M, N: integer): integer;
begin
  result := xPT4TaskMakerNET.RandomN(M, N);
end;

function Random(A, B: real): real;
begin
  result := xPT4TaskMakerNET.RandomR(A, B);
end;

function Random1(A, B: real): real;
begin
  result := Random(Round(a*10), Round(b*10)) / 10
     + Random * 1.0e-7;
end;

function Random2(A, B: real): real;
begin
  result := Random(Round(a*100), Round(b*100)) / 100
     + Random * 1.0e-7;
end;

function CurrentTest: integer;
begin
  if CheckTT then exit;
  result := xPT4TaskMakerNET.CurrentTest;
end;

procedure UseTask(GroupName: string; TaskNumber: integer);
begin
  if ut then exit;
  xPT4TaskMakerNET.UseTask(GroupName, TaskNumber);
  ut := true;
end;

procedure UseTask(GroupName: string; TaskNumber: integer; TopicDescription: string);
begin
  if ut then exit;
  xPT4TaskMakerNET.UseTask(GroupName, TaskNumber, TopicDescription);
  ut := true;
end;

function GetWords: array of string;
begin
  Result := ArrGen(WordCount, i -> WordSample(i));
end;

function RandomWord: string;
begin
  Result := WordSample(RandomN(0, WordCount - 1));
end;

function GetEnWords: array of string;
begin
  Result := ArrGen(EnWordCount, i -> EnWordSample(i));
end;

function RandomEnWord: string;
begin
  Result := EnWordSample(RandomN(0, EnWordCount - 1));
end;

function GetSentences: array of string;
begin
  Result := ArrGen(SentenceCount, i -> SentenceSample(i));
end;

function RandomSentence: string;
begin
  Result := SentenceSample(RandomN(0, SentenceCount - 1));
end;

function GetEnSentences: array of string;
begin
  Result := ArrGen(EnSentenceCount, i -> EnSentenceSample(i));
end;

function RandomEnSentence: string;
begin
  Result := EnSentenceSample(RandomN(0, EnSentenceCount - 1));
end;

function GetTexts: array of string;
begin
  Result := ArrGen(TextCount, i -> TextSample(i));
end;

function RandomText: string;
begin
  Result := TextSample(RandomN(0, TextCount - 1));
end;

function GetEnTexts: array of string;
begin
  Result := ArrGen(EnTextCount, i -> EnTextSample(i));
end;

function RandomEnText: string;
begin
  Result := EnTextSample(RandomN(0, EnTextCount - 1));
end;

procedure DataFileInteger(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileN(FileName, yd, w + 2);
end;

procedure DataFileReal(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileR(FileName, yd, w + 2);
end;

procedure DataFileChar(FileName: string);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fd := true;
  DataFileC(FileName, yd, 5);
end;

procedure DataFileString(FileName: string);
begin

  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if Length(a) > w then
        w := Length(a);
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      DataComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fd := true;
  DataFileS(FileName, yd, w + 4);
  
end;

procedure DataText(FileName: string; LineCount: integer);
begin
  if CheckTT then exit;
  inc(yd);
  if yd > 5 then
  begin
    DataComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fd := true;
  var yd2 := yd + LineCount - 1;
  if yd2 > 5 then yd2 := 5;
  DataFileT(FileName, yd, yd2);
  yd := yd2;
end;

procedure ResFileInteger(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of integer;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: integer;
      read(f, a);
      var s := IntToStr(a);
      if s.Length > w then
        w := s.Length;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileN(FileName, yr, w + 2);
end;

procedure ResFileReal(FileName: string);
begin
  if CheckTT then exit;
  ResComm(fmt);
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of real;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: real;
      read(f, a);
      var s := wreal(0, a);
      if s > w then
        w := s;
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileR(FileName, yr, w + 2);
end;

procedure ResFileChar(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fr := true;
  ResultFileC(FileName, yr, 5);
end;

procedure ResFileString(FileName: string);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  var w := 0;
  try
    var f: file of ShortString;
    Reset(f, FileName);
    while not EOF(f) do
    begin
      var a: ShortString;
      read(f, a);
      if Length(a) > w then
        w := Length(a);
    end;
    Close(f);
  except
    on ex: Exception do
    begin
      ResComm('\B'+ErrorMessage('FileError(' + FileName + '): ' + ex.Message)+'\b');
      exit;
    end;  
  end;
  fr := true;
  ResultFileS(FileName, yr, w + 4);
end;

procedure ResText(FileName: string; LineCount: integer);
begin
  if CheckTT then exit;
  inc(yr);
  if yr > 5 then
  begin
    ResComm('\B'+ErrorMessage(ErrMes1)+'\b');
    exit;
  end;
  fr := true;
  var yr2 := yr + LineCount - 1;
  if yr2 > 5 then yr2 := 5;
  ResultFileT(FileName, yr, yr2);
  yr := yr2;
end;

end.