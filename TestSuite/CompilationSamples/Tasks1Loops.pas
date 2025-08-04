unit Tasks1Loops;

uses LightPT;

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  ClearOutputListFromSpaces; 

  case name of
  'LoopErr2': begin
    TaskResult := ErrFix; // Это задача на исправление ошибок. Откомпилировалась 1 раз - и внеслась в базу
    if OutputString.ToString = '*' then 
      ColoredMessage('Сотри ; после do!',MsgColorGray);
    if OutputString.ToString = '*'*50 then 
      ColoredMessage('Молодец! И помни: нельзя ставить ; после do!',MsgColorGray);
  end;
  'LoopErr3': begin
    TaskResult := ErrFix;
    if OutputList.Count = 11 then 
      ColoredMessage('Используй begin end для объединения нескольких операторов в один!',MsgColorGray);
    if OutputList.Count = 20 then 
      ColoredMessage('Молодец! Если в цикле - несколько действий, окайми их операторными скобками begin-end',MsgColorGray);
  end;
  'Loop_Arithm1': begin
    var n := 10; 
    CheckData(InitialOutput := |cInt|*n);
    CheckOutputAfterInitial(ArrGen(n,1,i->i+2));
  end;
  'Loop_Arithm2': begin 
    CheckData(Input := Empty);
    var sq := ObjectList.New.AddArithm(10,19,-2).AddArithm(9,1.1,0.1);
    CheckOutput(sq);
  end;
  'For1': begin
    CheckData(Empty);
    TaskResult := InitialTask;
    ColoredMessage('Что лучше в данном случае: цикл loop или цикл for?',msgColorGray);
  end;
  'For1a': begin
    var n := 10; 
    CheckData(InitialOutput := |cInt|*n);
    var sq := ObjectList.New.AddArithm(n,2,2).AddArithm(n,2,2);
    CheckOutput(sq);
  end;
  'For1b': begin
    var n := 10; 
    CheckData(InitialOutput := |cInt|*n);
    var sq := ObjectList.New.AddArithm(n,1,2).AddArithm(n,1,2);
    CheckOutput(sq);
  end;
  'For1b_step': begin
    ClearOutputListFromSpaces; 
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(10,2,2).AddArithm(10,1,2);
    CheckOutput(sq);
  end;
  'For1c': begin
    ClearOutputListFromSpaces; 
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(11,15,-1);
    CheckOutput(sq);
  end;
  'For1d': begin
    ClearOutputListFromSpaces; 
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(10,20,-2);
    CheckOutputSeq(sq);
  end;
  'For1e': begin
    ClearOutputListFromSpaces; 
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(10,5,5).AddArithm(7,40,-4);
    CheckOutput(sq);
  end;
  'For1f': begin
    ClearOutputListFromSpaces; 
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(9,1.9,-0.1).AddArithm(9,1.9,-0.1);
    CheckOutput(sq);
  end;
  'ForErr1': begin
    TaskResult := ErrFix; // Задание на исправление ошибок
    ColoredMessage('Всегда описывайте переменную i в заголовке цикла for',msgColorGray);
  end;
  'ForErr2': begin
    TaskResult := ErrFix;
    var a := OutputString.ToString.ToWords(NewLine);
    ColoredMessage('Описывайте переменную i в заголовке цикла for: for var i',msgColorGray);
    if a.Length < 2 then
      ColoredMessage('Между циклами необходимо вызвать команду Println перехода на новую строку',msgColorRed);
  end;
  'ForErr3': begin
    TaskResult := ErrFix;
    ColoredMessage('Запомните: операторы не могут писаться до begin основной программы',msgColorGray);
  end;
  'ForErr4': begin
    CheckData(Empty);
    var sq := ObjectList.New.AddArithm(11,15,-1);
    CheckOutputSeq(sq);
  end;
  'ForErr5': begin
    CheckData(Empty);
    //CheckInitialOutputValues('*');
    CheckOutputSeq(ArrFill(50,'*'));
  end;
  'ForErr6': begin
    TaskResult := ErrFix;
    ColoredMessage('Если в цикле выполняется два и более действий, окаймите их begin end',msgColorGray);
  end;
  'ForErr7': begin
    TaskResult := ErrFix;
    ColoredMessage('В данном случае лучше использовать цикл loop',msgColorGray);
  end;
  'Tabl1': begin
    CheckData(InitialOutput := |cInt|*20);
    var sq := (1..10).SelectMany(x -> |x,x*x,x*x*x|);
    CheckOutput(sq);
  end;
  'Tabl2': begin
    CheckData(Empty);
    var sq := (1..10).SelectMany(x -> |object(x),object(sqrt(x))|); // приводим к object поскольку типы разные
    CheckOutput(sq);
  end;
  'Tabl4': begin
    CheckData(Empty);
    var sq := PartitionPoints(1.1,1.9,8).SelectMany(x -> |x,x*x|);
    CheckOutput(sq);
  end;
  'Sum1': begin
    FilterOnlyNumbers;
    // Частичные решения не проработаны. Они крайне редки конечно
    TaskResult := PartialSolution; // Решение состоит в последовательном решении нескольких родственных задач
    if CompareValuesWithOutput(4905) then // CompareValuesWithOutput используется именно для частичного решения
      ColoredMessage('Отлично! Теперь найдите сумму квадратов всех двузначных чисел', MsgColorGray)
    else if CompareValuesWithOutput(328065) then
      TaskResult := Solved;
  end;
  'Sum1a': begin
    FilterOnlyNumbers;
    CheckData(Input := cInt*2);
    var (a,b) := (Int(0),Int(1));
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var (a,b) := Random2(1..20);
      if a>b then Swap(a,b);
      InputList.AddTestData(|a,b|);
    end;
    CheckOutput((a..b).Sum);
  end;
  'Sum2': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput((11..99).Step(2).Sum);
  end;
  'Sum3': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput((1..9).Select(x->x*0.1+1.0).Sum);
  end;
  'Sum4': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput((1..9).Select(x->1/x).Sum);
  end;
  'Prod1': begin
    TaskResult := PartialSolution;
    FilterOnlyNumbers;
    // CompareValuesWithOutput - более низкоуровневая чем CheckOutput 
    // Она просто сравнивает значения с выводом и возвращает True/False
    if CompareValuesWithOutput(3*3*3*3*3*3*3) then
      ColoredMessage('Вычислите 4 в степени 6', MsgColorGray)
    else if CompareValuesWithOutput(4*4*4*4*4*4) then
      ColoredMessage('Вычислите 5 в степени 5', MsgColorGray)
    else if CompareValuesWithOutput(5*5*5*5*5) then
      TaskResult := Solved
    else if not CompareValuesWithOutput(2*2*2*2*2) then
      TaskResult := BadSolution
  end;
  'Prod2_Power': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(2**3,3**7,4**6,5**5);
  end;
  'Prod3_Fact': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(2*3*4*5*6*7,2*3*4*5*6*7*8,2*3*4*5*6*7*8*9*10);
  end;
  'Geom1': begin
    FilterOnlyNumbers;
    CheckData(InitialOutput := |cInt|*11);
    var sq := ObjectList.New.AddGeom(15,1,3);
    CheckOutputAfterInitial(sq);
  end;
  'Geom2': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    var sq := ObjectList.New.AddGeom(11,1,10);
    sq.Add(integer.MaxValue);
    if OutputList.Count = 11 then
      ColoredMessage('Выведите integer.MaxValue - самое большое целое типа integer',MsgColorMagenta);
    CheckOutput(sq);
  end;
  'Geom3': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    var sq := ObjectList.New.AddGeom(20,1.0,10.0);
    sq.Add(real.MaxValue);
    if OutputList.Count = 20 then
      ColoredMessage('Выведите real.MaxValue - самое большое целое типа real',MsgColorMagenta);
    CheckOutput(sq);
  end;
  'Geom4': begin
    FilterOnlyNumbers;
    CheckData(Empty);
    var sq := ObjectList.New.AddGeom(8,1.0,0.999);
    CheckOutput(sq);
  end;
  'Loop_If_1': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(InitialInput := |cInt|*n);
    GenerateTests(10,tInt(1,100)*n);
    
    var a := IntArr(n);
    CheckOutput(a.Where(x->x.IsEven));  
  end;
  'Loop_If_2': begin 
    var n := 10;
    CheckData(Input := |cInt|*n);
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var x := ArrRandomInteger(n,1,100);
      x[0] := 1;
      x[2] := -1;
      x[3] := 9;
      x[4] := 10;
      x[5] := 99;
      x[6] := 100;
      InputList.AddTestData(x);
    end;
    var lst := new ObjectList;
    for var i:=0 to n-1 do
      if Int(i) in 10..99 then
        lst.Add(Int(i));
    CheckOutput(lst);  
  end;
  'Loop_If_2a': begin 
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(20,tInt(1,100)*n);
    var lst := new ObjectList;
    for var i:=0 to n-1 do
      if Int(i).Divs(3) and Int(i).NotDivs(5) then
        lst.Add(Int(i));
    CheckOutput(lst);  
  end;
  'Loop_If_3': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    var lst := new ObjectList;
    GenerateTests(10,tInt(1,100)*n); 
    for var i:=1 to n do
      if i.IsEven then
        lst.Add(Int(i-1));
    CheckOutput(lst);  
  end;
  'Loop_If_4': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,100)*n); 
    var lst := new ObjectList;
    for var i:=0 to n-1 do
      if Int(i).IsEven then
        lst.Add(i+1);
    CheckOutput(lst);  
  end;
  'Loop_If_5_Count': begin 
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,100)*n); 
    var c := 0;
    for var i:=0 to n-1 do
      if Int(i).IsEven then
        c += 1;
    CheckOutput(c);
  end;  
  'Loop_If_7_Count': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,6)*n); 
    var c := 0;
    for var i:=0 to n-1 do
      if Int(i) in 2..5 then
        c += 1;
    CheckOutput(c);
  end;
  'Loop_If_8_Count': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,10)*n); 
    var c := 0;
    for var i:=0 to n-1 do
      if Int(i) not in 2..5 then
        c += 1;
    CheckOutput(c);
  end;
  'Loop_If_9_Count': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,100)*n); 
    var c := 0;
    for var i:=0 to n-1 do
      if (Int(i) in 10..99) and Int(i).IsOdd then
        c += 1;
    CheckOutput(c);
  end;
  'Loop_If_1_Sum': begin 
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,10)*n); 
    var s := 0;
    for var i:=0 to n-1 do
      if Int(i)<5 then
        s += Int(i);
    CheckOutput(s);
  end;
  'Loop_If_2_Sum': begin 
    FilterOnlyNumbersAndBools;
    var N := Int(0);
    CheckData(Input := |cInt|*(N+1));

    TestCount := 5;
    GenerateTestData := tnum -> begin
      var N := Random(5, 10);
      var a := ArrRandomInteger(N,1,10);
      InputList.AddTestData(|N|+a);
    end;

    var s := 0;
    for var i:=1 to N do
      if Int(i).IsEven then
        s += Int(i);
    CheckOutput(s);
  end; 
  'Зацикливание2': begin 
    FilterOnlyNumbers;
    CheckOutputSeq(1..9);
    if TaskResult = Solved then
      ColoredMessage('Не ставьте ; после do!');
  end;
  'Wh1': begin 
    FilterOnlyNumbers;
    CheckData(InitialOutput := |cInt|*18); 
    
    CheckOutput(Arr(1..9)*3);
  end;
  'Wh2': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutputSeq(ArrGen(10,1,x->x+2));
  end;
  'Wh3': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(ArrGen(45,99,x->x-2));
  end;
  'Дюймы1': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutputSeq(ArrGen(7,17.49,x->x-2.51));
  end;
  'Дюймы2': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(7);
  end;
  'Дюймы3': begin 
    FilterOnlyNumbers;
    CheckData(Input := cRe * 2);
    GenerateTests(10,tRe(2,10,1)*2);
    CheckOutput(Trunc(Re(0)/Re(1)));
  end;
  'Дюймы4': begin 
    FilterOnlyNumbers;
    CheckData(Input := cRe * 2);
    CheckOutput(Re(0) - Trunc(Re(0)/Re(1)));
  end;
  'Sum1W': begin 
    FilterOnlyNumbers;
    CheckData(InitialOutput := |cInt|);
    CheckOutput(165,165);
  end;
  'Sum2W': begin 
    FilterOnlyNumbers;
    CheckData(InitialOutput := |cInt|);
    CheckOutput(385,385);
  end;
  'Sum3W': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(1035);
  end;
  'Умножение1': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(ArrGen(14,1,x->x*2));
  end;
  'Умножение2': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(11);
  end;
  'Деление1': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(ArrGen(6,26.5,x->x/2));
  end;
  'Деление2': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(7);
  end;
  'Digits': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(4,3,5,2,6,1,7);
  end;
  'Bank_ForLeaders': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    CheckOutput(ArrGen(9,109000.0,x->x*1.09));
  end;
  'Digits1': begin 
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(2,23,345,4567,56789,678901,7890123);
    var c := Abs(Int(0));
    var res := c.ToString.Select(cc->cc.ToDigit).Count;
    CheckOutput(res);
  end;
  'Digits2': begin 
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(2,23,345,4567,56789,678901,7890123);
    var c := Abs(Int(0));
    var res := c.ToString.Select(cc->cc.ToDigit).Sum;
    CheckOutput(res);
  end;
  'Digits3': begin 
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(2,23,345,4567,56789,678901,7890123);
    var c := Abs(Int(0));
    var res := c.ToString.Select(cc->cc.ToDigit).Count(x->x.IsEven);
    CheckOutput(res);
  end;
  'Digits4': begin 
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(2,23,345,4567,56789,678901,7890123);
    var c := Abs(Int(0));
    var res := c.ToString.Select(cc->cc.ToDigit).Where(x->x.IsOdd).Sum;
    CheckOutput(res);
  end;
  'Digits5': begin 
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(1,23,345,4567,56789,678901,7890123);
    var c := Abs(Int(0));
    var res := c.ToString.First.ToDigit;
    CheckOutput(res);
  end;
  'BinNumSystem': begin
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(23,14,481);
    var x := Int(0);
    var L := new List<integer>;
    while x<>0 do
    begin
      L.Add(x mod 2);
      x := x div 2;
    end;
    CheckOutput(L);
  end;
  'BinNumSystem2': begin
    FilterOnlyNumbers;
    CheckData(InitialInput := |cInt|);
    GenerateTests(23,14,255);
    var x := Int(0);
    var L := new List<integer>;
    while x<>0 do
    begin
      L.Add(x mod 2);
      x := x div 2;
    end;
    CheckOutput(L.Count(x->x=1));
  end;
  'Difficult_Power2': begin 
    FilterOnlyNumbersAndBools;
    CheckData(InitialInput := |cInt|);
    GenerateTests(23,14,256,16);
    var x := Int(0);
    while x mod 2 = 0 do
      x := x div 2;
    CheckOutput(x = 1);
  end;
  'Min1': begin 
    CheckData(InitialInput := |cRe|*2);
    GenerateTests(10,tRe(1,100)*2);
    CheckOutput(Min(Re(0),Re(1)),Max(Re(0),Re(1)));
  end;
  'Min2': begin 
    CheckData(InitialInput := |cRe|*2);
    GenerateTests(10,tRe(1,100)*2);
    CheckOutput(Min(Re(0),Re(1)));
  end;
  'Min3': begin 
    CheckData(InitialInput := |cRe|*3);
    GenerateTests(10,tRe(1,100)*3);
    CheckOutput(Min(Re(0),Re(1),Re(2)));
  end;
  'SeriesMin2': begin 
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt(1,100)*n);
    // Лучше var a := IntArr(n)
    CheckOutput(InputListAsIntegers.Max - InputListAsIntegers.Min)
  end;
  'Break1': begin 
    var n := 10;
    CheckData(Input := |cInt|*n);
    GenerateTests(20,tInt(1,10)*n);
    CheckOutput(3 in InputListAsIntegers);
  end;
  'Break2': begin 
    TestCount := 3;
    GenerateTestData := tnum -> begin
      case tnum of
        1: InputList.AddTestData(|1,2,8,4,5,6,7,1,2,9|);
        2: InputList.AddTestData(|1,1,3|);
        3: InputList.AddTestData(|7,6,4,3|);
      end;  
    end;
    var n := 10;
    var f := false;
    for var i:=0 to n-1 do
    begin
      var x := Int(i);
      if x = 3 then
      begin
        f := True;
        break;
      end;
    end;
    CheckOutput(f)
  end;
  'Break3': begin 
    var n := 10;
    TestCount := 3;
    GenerateTestData := tnum -> begin
      case tnum of
        1: InputList.AddTestData(|1,2,3,4,5,6,7,1,2,3|);
        2: InputList.AddTestData(|1,1,9|);
        3: InputList.AddTestData(|7,6,8|);
      end;  
    end;
    var f := 0;
    for var i:=0 to n-1 do
    begin
      var x := Int(i);
      if x >= 8 then
      begin
        f := x;
        break;
      end;
    end;
    CheckOutput(f)
  end;
  'Break4': begin 
    CheckData(Input := |cInt|);
    GenerateTests(123,1023,134,11110);
    var x := Int(0);
    var h := False;
    while x<>0 do
    begin
      var c := x mod 10;
      if c = 0 then
      begin
        h := True;
        break
      end;
      x := x div 10;
    end;
    CheckOutput(h)
  end;
  'BreakIntheMiddle1': begin 
    var i := 0;
    var c := 0;
    repeat
      var x := Int(i);
      i += 1;
      if x = 0 then
        break;
      if x < 5 then
        c += 1;
    until False;
    TestCount := 10; // Это немного неудобно - надо не забыть присвоить!
    GenerateTestData := tnum -> begin
      var n := Random(7,12); // Разобраться, как работает в Test mode
      var a := ArrRandomInteger(n,1,10);
      a[^1] := 0;
      InputList.AddTestData(a);
    end;
    CheckOutput(c)
  end;
  'BreakIntheMiddle2': begin 
    var i := 0;
    var c := 0;
    repeat
      var x := Int(i);
      i += 1;
      if x = 0 then
        break;
      c += x;
    until False;
    TestCount := 10;
    //CheckData(Input := |cInt|*i); 
    GenerateTestData := tnum -> begin
      var n := Random(7,12);
      var a := ArrRandomInteger(n,1,10);
      a[^1] := 0;
      InputList.AddTestData(a);
    end;
    CheckOutput(c)
  end;
  'Divisors': begin 
    CheckData(Input := |cInt|);
    GenerateTests(72,234,17,2);
    var x := Int(0);
    var L := new List<integer>;
    for var i:=2 to x-1 do
    if x mod i = 0 then
      L.Add(i);
    CheckOutputSeq(L);
  end;
  'SimpleNumbers': begin 
    CheckData(Input := |cInt|);
    GenerateTests(72,234,17,2);
    var x := Int(0);
    var L := new List<integer>;
    for var i:=2 to x-1 do
    if x mod i = 0 then
      L.Add(i);
    CheckOutput(L.Count = 0);
  end;
  'сс1': begin 
    CheckOutputString((9*'*'+#13#10) * 5);
  end;
  'сс2': begin 
    // Уже можно использовать многострочную строку! Но лень переписывать. Работает.
    CheckOutputString(9*'1'+#13#10 + 9*'2'+#13#10 + 9*'3'+#13#10 + 9*'4'+#13#10 + 9*'5'+#13#10)
  end;
  'сс3': begin 
    CheckOutputString(5*('123456789'+#13#10));
  end;
  'сс4': begin 
    CheckOutputString('012345'+#13#10 + '123456'+#13#10 + '234567'+#13#10 + '345678'+#13#10  + '456789'+#13#10 );
  end;
  'сс5': begin 
    CheckOutputString('12345'+#13#10 + '246810'+#13#10 + '3691215'+#13#10 + '48121620'+#13#10  + '510152025'+#13#10 );
  end;
  'сс6': begin 
    CheckOutputString('  1  2  3  4  5'+#13#10 + '  2  4  6  8 10'+#13#10 + '  3  6  9 12 15'+#13#10 + '  4  8 12 16 20'+#13#10  + '  5 10 15 20 25'+#13#10 );
  end;
  'сс7': begin 
    var sb := new StringBuilder;
    for var i:=1 to 19 do
    begin
      for var j:=1 to 19 do
        sb.Append($'{(i*j),4}');
      sb.Append(#13#10);
    end;
    CheckOutputString(sb.ToString);
  end;
  'сс8': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to 5 do
        sb.Append($'({i},{j}) ');
      sb.Append(#13#10);
    end;
    CheckOutputString(sb.ToString);
  end;
  'ссi1': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to i do
        sb.Append('*');
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);
  end;
  'ссi2': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to 2*i do
        sb.Append('*');
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);
  end;
  'ссi3': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to 2*i-1 do
        sb.Append('*');
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);
  end;
  'ссi4': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to 6-i do
        sb.Append('*');
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);  
  end;
  'ссi5': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to i do
        sb.Append(i);
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);  
  end;
  'ссi6': begin 
    var sb := new StringBuilder;
    for var i:=1 to 5 do
    begin
      for var j:=1 to i do
        sb.Append(j);
      sb.Append(#13#10);
    end;  
    CheckOutputString(sb.ToString);  
  end;
  'Combinations': begin 
    CheckData(Empty);
    var lst := ObjectList.New;
    for var c := 'a' to 'z' do
    for var i := 0 to 9 do
    begin  
      lst.Add(c);
      lst.Add(i);
    end;
    CheckOutput(lst);
  end;
  'Primes': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var n := 2 to 1000 do
    begin  
      var IsPrime := True;
      for var i:=2 to Round(Sqrt(n)) do
        if n mod i = 0 then
        begin
          IsPrime := False;
          break;
        end;
      if IsPrime then
        lst.Add(n);
    end;  
    CheckOutput(lst);
  end;
  'Счастливые билеты': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    CheckOutput(55252);
  end;
  '1_ПрямоугольныеТреугольники': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var a:=1 to 20 do
    for var b:=1 to 20 do
    for var c:=1 to 20 do
      if (a*a + b*b = c*c) and (a<b) then
        lst.AddRange(|a,b,c|);
    CheckOutput(lst);  
  end;
  '2_Перебор вариантов': begin 
    CheckData(Empty);
    CheckOutput(4,6,12,3); 
  end;
  'ДесятичныеЧисла': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var d1:=1 to 9 do
    for var d2:=0 to 9 do
    for var d3:=0 to 9 do
    begin
      var a := d1*100 + d2*10 + d3;
      if (d1 + d2 + d3).Divs(3) and (d1<d2) and (d2<d3) then
        lst.Add(a);
    end;
    CheckOutput(lst);
  end;
  'ДвоичныеЧисла': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var d1:=0 to 1 do
    for var d2:=0 to 1 do
    for var d3:=0 to 1 do
    begin
      var a := d1*4 + d2*2 + d3;
      Lst.AddRange(|d1,d2,d3,a|);
    end;
    CheckOutput(lst);
  end; 
  'Product': begin 
    CheckData(Empty);
    var lst := ObjectList.New;
    for var c := 'a' to 'z' do
    for var i := 0 to 9 do
    begin  
      lst.Add(c);
      lst.Add(i);
    end;
    CheckOutput(lst);
  end;
  'Перебор вариантов': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    CheckOutput(4,6,12,3); 
  end;
  'ProductPartial': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var i:=1 to 4 do
    for var j:=1 to 4 do
    for var k:=1 to 4 do
      if (j < k) and (j < i) then
      lst.AddRange(|i,j,k|);
    CheckOutput(lst); 
  end;
  'Combinations1': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var i := 1 to 5 do
    for var j := i+1 to 5 do
    for var k := j+1 to 5 do
      lst.AddRange(|i,j,k|);
    CheckOutput(lst); 
  end;
  'Permutations1': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var i := 1 to 3 do
    for var j := 1 to 3 do
    for var k := 1 to 3 do
      if (i<>j) and (j<>k) and (i<>k) then
        lst.AddRange(|i,j,k|);
    CheckOutput(lst); 
  end;
  'Permutations2': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var i := 1 to 4 do
    for var j := 1 to 4 do
    for var k := 1 to 4 do
    for var m := 1 to 4 do
      if (i<>j) and (i<>k) and (i<>m) and (j<>k) and (j<>m) and (k<>m)
      then
        lst.AddRange(|i,j,k,m|);
    CheckOutput(lst); 
  end;
  'ПрямоугольныеТреугольники': begin 
    FilterOnlyNumbers;
    CheckData(Empty);
    var lst := ObjectList.New;
    for var a:=1 to 20 do
    for var b:=1 to 20 do
    for var c:=1 to 20 do
      if (a*a + b*b = c*c) and (a<b) then
        lst.AddRange(|a,b,c|);
    CheckOutput(lst);  
  end;
  
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('LoopErr2','LoopErr3','Loop_Arithm1','Loop_Arithm2',
  'For1','For1a','For1b','For1b_step','For1c','For1d','For1e','For1f',
  'ForErr1','ForErr2','ForErr3','ForErr4','ForErr5','ForErr6','ForErr7',
  'Tabl1','Tabl2','Tabl4','Sum1','Sum1a','Sum2','Sum3','Sum4','Prod1','Prod2_Power','Prod3_Fact',
  'Geom1','Geom2','Geom3','Geom4',
  'Loop_If_1','Loop_If_2','Loop_If_2a','Loop_If_3','Loop_If_4',
  'Loop_If_5_Count','Loop_If_7_Count','Loop_If_8_Count','Loop_If_9_Count','Loop_If_1_Sum','Loop_If_2_Sum',
  'Зацикливание2','Wh1','Wh2','Wh3','Дюймы1','Дюймы2','Дюймы3','Дюймы4','Sum1W','Sum2W','Sum3W',
  'Умножение1','Умножение2','Деление1','Деление2','Digits','Bank_ForLeaders',
  'Digits1','Digits2','Digits3','Digits4','Digits5',
  'BinNumSystem','BinNumSystem2','Difficult_Power2',
  'Min1','Min2','Min3','SeriesMin2',
  'Break1','Break2','Break3','Break4','BreakIntheMiddle1','BreakIntheMiddle2',
  'Divisors','SimpleNumbers',
  'сс1','сс2','сс3','сс4','сс5','сс6','сс7','сс8',
  'ссi1','ссi2','ссi3','ссi4','ссi5','ссi6',
  'Combinations','Primes','1_ПрямоугольныеТреугольники','ДесятичныеЧисла','ДвоичныеЧисла',
  'Product','Счастливые билеты','Перебор вариантов','ProductPartial','Combinations1','Permutations1','Permutations2','ПрямоугольныеТреугольники'
  );
finalization
end.