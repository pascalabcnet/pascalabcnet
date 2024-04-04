unit Tasks1BoolIfCase;

uses LightPT;

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  ClearOutputListFromSpaces; 

  case name of
  'Types1': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(cInt,cRe,cStr);
  end;
  'Types2': begin 
    CheckData(Input := |cInt,cRe,cStr|);
    CheckOutput(Int(0),Re(1),Str(2));
  end;
  'Types3': begin 
    CheckData(Empty);
    CheckOutput(cInt,cRe,cStr);
  end;
  'Boo1': begin 
  end;
  'Boo2': begin 
    CheckOutput(False,True);
    if TaskResult = BadSolution then
      TaskResult := InitialTask
  end;
  'Boo3': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    CheckOutput(54,54 mod 3 = 0,54,54 mod 3 = 5)
  end;
  'Boo4': begin 
    // Задачник
  end;
  'Boo5_And_Or': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    CheckOutput(cBool,cBool);
  end;
  'Boo7': begin 
    FilterOnlyNumbersAndBools;
    CheckOutput(True);
    //if TaskResult = BadSolution then
    //  TaskResult := InitialTask
  end;
  'If1Пароль': begin 
    CheckData(InitialInput := |cInt|);
    //if int(0)=777 then
      CheckOutput('Пароль правильный');
  end;
  'If2Пароль': begin 
    CheckData(Input := |cInt|);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var a := Random(1,1000);
      if tnum = 5 then
        a := 777;
      InputList.AddTestData(|a|);
    end;
    if int(0)=777 then
      CheckOutput('Пароль правильный')
    else CheckOutput('Попытка взлома системы!');
  end;
  'If3ПроверкаИмени': begin 
    CheckData(InitialInput := |cStr|);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var a := Arr('Саша','Петя','Вася','Оля').RandomElement;
      if tnum = 5 then
        a := 'Саша';
      InputList.AddTestData(|a|);
    end;
    if Str(0)='Саша' then
      CheckOutput('Привет Саша! Я тебя знаю')
    else CheckOutput('Я тебя не знаю');
  end;
  'If4Четное': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(20,|tInt(1,100)|);
    if Int(0) mod 2 = 0 then
      CheckOutput(Int(0),'Четное')
    else CheckOutput(Int(0),'Нечетное');
  end;
  'If5ЛогинПарольAnd': begin 
    CancelMessagesIfInitial := False;
    CheckData(InitialInput := |cStr,cInt|);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var a := Arr('Angel','Demon','Ogr').RandomElement;
      var p := Arr(777,666,555).RandomElement;
      if tnum = 5 then
        (a,p) := ('Angel',777);
      InputList.AddTestData(|object(a),object(p)|);
    end;
    if (Str(0) = 'Angel') and (Int(1) = 777) then
      CheckOutput('Вход разрешен')
    else CheckOutput('Попытка взлома системы!');
  end;
  'If6ЛогинOr': begin 
    CheckData(InitialInput := |cStr|);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var a := Arr('Angel','Demon','Alex','John','Olga','Iris').RandomElement;
      if tnum = 5 then
        a := 'Angel'
      else if tnum = 6 then
        a := 'Demon';
      InputList.AddTestData(a);
    end;
    if (Str(0) = 'Angel') or (Str(0) = 'Devil') then
      CheckOutput('Вход разрешен')
    else CheckOutput('Попытка взлома системы!');
  end;
  'If7Not': begin 
    FilterOnlyNumbersAndBools;
    CheckOutput(True,False);
    if TaskResult = BadSolution then
      TaskResult := PartialSolution;
  end;
  'If8Зима': begin 
    CheckData(Input := |cInt|);
    var m := Int(0);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var a := Random(1,12);
      case tnum of
        5: a := 12;
        6: a := 1; 
        7: a := 2;
      end;
      InputList.AddTestData(a);
    end;
    CheckOutput(m in |12,1,2|? 'Зима':'Не зима')
  end;
  'If8ЧетДвузнач': begin 
    CheckData(Input := |cInt|);
    var x := Int(0);
    TestCount := 20;
    GenerateTestData := tnum -> begin
      var x := Random(5,15);
      case tnum of
        5: x := 9;
        6: x := 10; 
        7: x := 99;
        8: x := 100;
      end;
      InputList.AddTestData(x);
    end;
    var b := x.IsEven and (x in 10..99);
    if b then
      CheckOutput('Это чётное двузначное')
    else CheckOutput();
  end;
  'If9ВДиапазоне': begin 
    CheckData(Input := |cInt|);
    var x := Int(0);
    var b :=  x in 3..5;
    TestCount := 20;
    GenerateTestData := tnum -> begin
      var x := Random(1,5);
      case tnum of
        5: x := 2;
        6: x := 3; 
        7: x := 5;
        8: x := 6;
      end;
      InputList.AddTestData(x);
    end;
    if b then
      CheckOutput('экзамен сдан')
    else CheckOutput('экзамен не сдан')
  end;
  'If9НеВДиапазоне': begin 
    CheckData(Input := |cInt|);
    var H := Int(0);
    TestCount := 20;
    GenerateTestData := tnum -> begin
      var x := Random(100,200);
      case tnum of
        5: x := 149;
        6: x := 150; 
        7: x := 190;
        8: x := 191;
      end;
      InputList.AddTestData(x);
    end;

    var b := H not in 150..190;
    if b then
      CheckOutput('Необычный рост')
    else CheckOutput('Норма')
  end;
  'Minmax_2': begin 
    CheckData(InitialOutput := |cInt|);
    CheckOutputSeq(Arr(3,5,3,5,6,6));
  end;
  'ВложенныеIf2': begin 
    CheckData(Input := |cStr,cInt|);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var name := Arr('Alex','Bob','John').RandomElement;
      var pass := Arr(555,666,777).RandomElement;
      case tnum of
        1: (name,pass) := ('Alex',666);
        2: (name,pass) := ('Alex',777);
        3: (name,pass) := ('Bob',666);
        4: (name,pass) := ('Bob',444);
      end;
      InputList.AddTestData(name);
      InputList.AddTestData(pass);
    end;
    if (Str(0) = 'Alex') then
      if (Int(1) = 666) then
        CheckOutput('Вход разрешен')
      else CheckOutput('Пароль неверен')
    else CheckOutput('Логин неверен');
  end;
  'ЦепочечныеIf1': begin 
    CheckData(Input := |cInt|);
    GenerateTests(20,|tInt(1,4)|);
    case Int(0) of
      1: CheckOutput('Зима');
      2: CheckOutput('Весна');
      3: CheckOutput('Лето');
      4: CheckOutput('Осень');
    end;
  end;
  'ЦепочечныеIf2': begin 
    CheckData(Input := |cInt|);
    GenerateTests(20,|tInt(2,5)|);
    case Int(0) of
      2: CheckOutput('неудовлетворительно');
      3: CheckOutput('удовлетворительно');
      4: CheckOutput('хорошо');
      5: CheckOutput('отлично');
    end;
  end;
  //'ЦепочечныеIf3': begin end;
  'ЦепочечныеIf4': begin 
    CheckData(Input := |cStr|);
    TestCount := 4;
    GenerateTestData := tnum -> begin
      var word: string;
      case tnum of
        1: word := 'черное';
        2: word := 'высокий';
        3: word := 'худой';
        4: word := 'хорошо';
      end;
      // жуткое исключение если в InputList ничего не добавить!!!
      InputList.AddTestData(word);
    end;
    case Str(0) of
      'черное' : CheckOutput('белое');
      'высокий': begin
        CheckOutputSilent('низкий');
        if TaskResult = TaskStatus.BadSolution then
          CheckOutput('невысокий');
      end;  
      'худой'  : begin
        CheckOutputSilent('толстый');
        if TaskResult = TaskStatus.BadSolution then
          CheckOutputSilent('полный');
        if TaskResult = TaskStatus.BadSolution then
          CheckOutput('жирный');
      end;  
      'хорошо' : CheckOutput('плохо');
      else CheckOutput('');
    end;
  end;
  'ЦепочечныеIf5': begin 
    CheckData(Input := |cInt|);
    //GenerateTests(20,tInt(1,6)*1);
    GenerateTests(1,2,3,4,5,6);
    // Хотелось бы генерировать неслучайные тесты из некоторого диапазона
    // Непонятно, как совмещать несколько параметров
    case Int(0) of
      1,5: CheckOutput(1);
      3,4: CheckOutput(2);
      2,6: CheckOutput(3);
    end;
  end;
  'If0Situations1': begin 
    CheckData(Input := |cInt|);
    GenerateTests(10,tInt(-1,1)*1);
    if Int(0)<0 then
      CheckOutput('отрицательное')
    else if Int(0)=0 then
      CheckOutput('равно 0')
    else CheckOutput('положительное')
  end;
  'Holidays': begin 
    CheckData(Input := |cInt,cInt|);
    var (d,m) := (Int(0),Int(1));
    //GenerateTests(список кортежей)
    //GenerateTests(список значений)
    GenerateTests((1,1),(23,2),(8,3),(1,5),(9,5),(12,6),(4,11),(12,12),(7,13));
    if (d,m) = (1,1) then
      CheckOutput('Новый год')
    else if (d,m) = (23,2) then
      CheckOutput('День защитника Отечества')
    else if (d,m) = (8,3) then
      CheckOutput('Международный женский день')
    else if (d,m) = (1,5) then
      CheckOutput('Первомай')
    else if (d,m) = (9,5) then
      CheckOutput('День Победы')
    else if (d,m) = (12,6) then
      CheckOutput('День России')
    else if (d,m) = (4,11) then
      CheckOutput('День народного единства')
    else CheckOutput('');
  end;
  'IfXSituations2': begin 
    CheckData(Input := |cInt|);
    GenerateTests(-2,-1,0,1,2);
    if Int(0) < 3 then
      CheckOutput('x меньше 3')
    else if Int(0) in 3..5 then
      CheckOutput('x от 3 до 5')
    else if Int(0) > 5 then 
      CheckOutput('x больше 5')
  end;
  'IfDiscriminant': begin 
    CheckData(Input := |cRe,cRe,cRe|);
    GenerateTests(20,tRe(-3,3,digits := 1)*3);
    var (a,b,c) := (Re(0),Re(1),Re(2));
    var d := B*B-4*A*C;
    if d<0 then
      CheckOutput('Корней нет')
    else if d=0 then
      CheckOutput('Один корень')
    else CheckOutput('Два корня')
  end;
  'Case1': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,2,3,4,5,6,7);
    case Int(0) of
    1: CheckOutput('Понедельник');
    2: CheckOutput('Вторник');
    3: CheckOutput('Среда');
    4: CheckOutput('Четверг');
    5: CheckOutput('Пятница');
    6: CheckOutput('Суббота');
    7: CheckOutput('Воскресенье');
    end;
  end;
  'Case2': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,2,3,4,5,6);
    case Int(0) of
    2: CheckOutput('Двойка');
    3: CheckOutput('Тройка');
    4: CheckOutput('Четверка');
    5: CheckOutput('Пятерка');
    else CheckOutput('Не оценка');
    end;
  end;
  'Case3': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,2,3,4,5,6,7,8,9,10,11,12);
    case Int(0) of
    12,1,2: CheckOutput('Зима');
    3..5: CheckOutput('Весна');
    6..8: CheckOutput('Лето');
    9..11: CheckOutput('Осень');
    end;
  end;
  'Case4': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,2,3,4,5,6,7,8,9,10,11,12);
    case Int(0) of
    4,6,9,11: CheckOutput(30);
    2: CheckOutput(28);
    else CheckOutput(31);
    end;
  end;
  'Diapason1_Case': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(20,tInt(80,230)*1);
    case Int(0) of
    80..119: CheckOutput('Карлик');
    120..149: CheckOutput('Низкий');
    150..179: CheckOutput('Средний');
    180..230: CheckOutput('Высокий');
    end;
  end;
  'TranslateRobot': begin 
    CheckData(InitialInput := |cStr|);
    GenerateTests('Left','Right','Up','Down');
    case Str(0) of
    'Left': CheckOutput('Влево');
    'Right': CheckOutput('Вправо');
    'Up': CheckOutput('Вверх');
    'Down': CheckOutput('Вниз');
    end;
  end;
  'Комнаты': begin 
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,2,3,4,5,6);
    case Int(0) of
    1,5: CheckOutput(1);
    3,4: CheckOutput(2);
    2,6: CheckOutput(3);
    end;
  end;
  'Спецагенты': begin 
    CheckData(InitialInput := |cStr|);
    GenerateTests('Икс','Омега','Игрек','Кси','Бета','Зед','Пси','Вася','Петя','Оля');
    case Str(0) of
    'Икс', 'Омега': CheckOutput(1);
    'Игрек', 'Кси', 'Бета': CheckOutput(2);
    'Зед', 'Пси': CheckOutput(3);
    else CheckOutput(999);
    end;
  end;  
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('Types1','Types2','Types3',
  'Boo1','Boo2','Boo3','Boo4','Boo5_And_Or','Boo7',
  'If1Пароль','If2Пароль','If3ПроверкаИмени','If4Четное','If5ЛогинПарольAnd','If6ЛогинOr','If7Not',
  'If8Зима','Зима','If8ЧетДвузнач','If9ВДиапазоне','If9НеВДиапазоне','Minmax_2','ВложенныеIf2',
  'ЦепочечныеIf1','ЦепочечныеIf2','ЦепочечныеIf3','ЦепочечныеIf4','ЦепочечныеIf5',
  'If0Situations1','Holidays','IfXSituations2','IfDiscriminant',
  'Case1','Case2','Case3','Case4','Diapason1_Case','TranslateRobot','Комнаты','Спецагенты');
finalization
end.