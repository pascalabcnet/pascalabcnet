unit TasksArr;

uses LightPT;

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  //FlattenOutput; // Это теперь делается автоматически до вызова CheckTask
  ClearOutputListFromSpaces; // Это чтобы a.Print работал. По идее, надо писать всегда. Я не знаю задач, где пробелы в ответе

  case name of
  'Arr1','Arr2': begin 
     FilterOnlyNumbersAndBools;
     CheckData(InitialOutput := cInt*5);
     CheckOutput(|cInt|*10); // Это еще и типы проверяет. Бывает, что только типы и надо проверить
     // Надо проверить, что Output[9] - целое ненулевое
     if OutputList.Count<10 then // чтобы не получить исключение. Ошибки в CheckOutput кидают исключения уже после
       exit;
     if OutIsInt(9) then // Это - редкая техника
       if OutInt(9) = 0 then
       begin
         ColoredMessage('Массив надо заполнить ненулевыми значениями');
         TaskResult := BadSolution;
       end;
  end;
  'Arr3','Arr4': begin
    FilterOnlyNumbersAndBools;
    TaskResult := PartialSolution; // То есть, уже начали проверять, уже под контролем
    CheckData(|cInt|*5, |cInt|*5, |cInt|*10);
    if TaskResult <> PartialSolution then // Не знаю, зачем это
      exit;
    
    CheckOutput(InputList); // Что ввели, то и вывели
  end;
  'Arr5','Arr6': begin 
    // Проверка того что введено и выведено 10 значений, что ввод совпадает с выводом и что вывод в диапазоне от 2 до 5
    // Что плохо - все Check неявно меняют TaskResult, а здесь надо еще и явно
    FilterOnlyNumbersAndBools;
    var n := 10;
    CheckData(InitialInput := |cInt|*n, InitialOutput := |cInt|*n);

    TaskResult := PartialSolution; // потому что если просто запустить - будет InitialSolution
    
    CheckOutput(InputList); // Что ввели, то и вывели
    // Дополнительная проверка  
    if TaskResult = Solved then // То есть, после проверки решения мы проверяем еще входные данные
    begin
      // проверим на диапазон от 2 до 5
      var a := IntArr(n);
      if a.Any(x->x not in 2..5) then
      begin  
        TaskResult := BadSolution;
        ColoredMessage('Результирующие данные должны быть от 2 до 5',MsgColorGray);        
        exit;
      end;  
    end;
  end;
  'Arr_Sum0': begin // Этой задачи нет
     // Это паттерн: вводится n, а потом вводится n элементов массива
     FilterOnlyNumbersAndBools;
     var n := Int;
     CheckData(Input := |cInt| + n*cInt);
     var a := IntArr(n);
     CheckOutput(a.Sum)
  end;
  'Arr_Sum1','Arr_Sum2': begin 
     FilterOnlyNumbersAndBools;
     // Это всё надо. Начальный ввод - 10 целых, весь ввод - тоже
     var n := 10;
     CheckData(cInt*n, cInt*(n+1), cInt*n);
     
     var a := IntArr(n);
     var p := integer(a.Product); // потому что ученик будет вычислять в вещественной переменной
     CheckOutputAfterInitial(p)
  end;
  'Arr_Sum3': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*(n+1), cInt*n);
          
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Product)
  end;
  'Arr_Sum4': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n, cInt*n);
     
     var a := IntArr(n);
     var ave := a.Average;
     CheckOutputAfterInitial(ave,ave)
  end;
  'Arr_CountA1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     GenerateTests(10,tInt * n);
     var a := IntArr(n);
     CheckOutputAfterInitial(a.CountOf(3));
  end;
  'Arr_CountA2': begin 
     // Кроме чисел тут строки
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     var cnt4 := a.CountOf(4);
     var cnt5 := a.CountOf(5);
     
     var lst := ObjectList.New; // Можно без списка. Наверное, думал выводить что то еще
     case cnt4.CompareTo(cnt5) of
    1: lst.Add('четвёрок больше');
    0: lst.Add('одинаково');
    -1: lst.Add('пятёрок больше');
     end;
     CheckOutputAfterInitial(lst);
  end;
  'Arr_Replace1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);
     
     var arr := IntArr(n);
     arr.Replace(2,3);
     CheckOutputAfterInitial(arr);
  end;
  'Arr_Replace2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);

     var arr := IntArr(n);
     arr.Replace(2,22);
     arr.Replace(5,55);
     arr.Replace(22,5);
     arr.Replace(55,2);
     CheckOutputAfterInitial(arr);
  end;
  'Arr_Find1': begin 
     FilterOnlyNumbersAndBools;
     var n := 5;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);

     var a := IntArr(n);
     var has2 := 2 in a;
     CheckOutputAfterInitial(has2);
  end;
  'Arr_Find2': begin 
     FilterOnlyNumbersAndBools;
     var n := 5;
     CheckData(|cInt|*n, |cInt|*n, |cInt|*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,2,5);
       if tnum = 2 then
         a := ArrFill(n,1);
       InputList.AddTestData(a);
     end;
     
     var a := IntArr(n);
     var haseven := a.FindIndex(x -> x.IsEven) <> -1;
     CheckOutputAfterInitial(haseven);
  end;
  'Arr_Find3': begin 
     FilterOnlyNumbersAndBools;
     var n := 5;
     CheckData(cInt*n, cInt*n, cInt*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,1,99);
       if tnum = 2 then
         a := ArrRandomInteger(n,1,30)
       else if tnum = 3 then
         a := ArrRandomInteger(n,51,99);
       InputList.AddTestData(a);
     end;
     
     var a := IntArr(n);
     var haseven := a.ToArray.FindIndex(x -> x in 40..50) <> -1;
     CheckOutputAfterInitial(haseven);
  end;

  'Arr_Find4': begin 
     FilterOnlyNumbersAndBools;
     var n := 3;
     CheckData(|cInt|*n, |cInt|*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,2,5);
       if tnum = 2 then
         a := ArrFill(n,11);
       InputList.AddTestData(a);
     end;
     
     var a := IntArr(n);
     var allodd := a.All(x -> x.IsOdd);
     CheckOutputAfterInitial(allodd);
  end;
  'Arr_Index1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,2,5);
       if tnum = 2 then
         a := ArrFill(n,5);
       InputList.AddTestData(a);
     end;
     
     var a := IntArr(n);
     var ind := a.FindIndex(x -> x = 2);
     CheckOutputAfterInitial(ind);
  end;
  'Arr_Index2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,2,5);
       if tnum = 2 then
         a := ArrFill(n,4);
       InputList.AddTestData(a);
     end;
     
     var a := IntArr(n);
     var ind := a.FindLastIndex(x -> x = 5);
     CheckOutputAfterInitial(ind);
  end;
  'Arr_Index2a': begin
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10,tInt * n);
     var a := IntArr(n);
     var ind2 := a.IndexOf(2);
     var ind5 := a.IndexOf(5);
     CheckOutputAfterInitial(ind2,ind5);
  end;
  'Arr_Index3': begin 
     FilterOnlyNumbersAndBools;
     var n := 20;
     CheckData(cInt*n, cInt*n);
     TestCount := 10;
     GenerateTestData := tnum -> begin
       var a := ArrRandomInteger(n,2,5);
       a[^2] := 2;
       a[^1] := 5;
       InputList.AddTestData(a);
     end;

     var arr := IntArr(n);
     var ind2 := arr.IndexOf(2);
     var ind5 := arr.IndexOf(5);
     Swap(arr[ind2],arr[ind5]);
     CheckOutputAfterInitial(arr);
  end;
  'Arr_Index4': begin 
     FilterOnlyNumbersAndBools;
     var n := 30;
     CheckData(|cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);
     var arr := IntArr(n);
     var ind := arr.IndexOf(2);
     ind := arr.IndexOf(2,ind + 1);
     CheckOutputAfterInitial(ind);
  end;

  'Arr_MinMax1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(InitialOutput := cInt * n);

     var a := OutSliceIntArr(0,n-1); // Ввода нет, поэтому пользуемся начальным выводом
     // Вот здесь если ученик его сотрёт, то будет плохо
     var min := a.Min;
     CheckOutputAfterInitial(min);
  end;
  'Arr_MinMax2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10,tInt * n);
     
     var a := IntArr(n);
     var max := a.Max;
     CheckOutputAfterInitial(max);
  end;
  'Arr_MinMax3': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10,tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Min);
  end;
  'Arr_MinMax4': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10,tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Min,a.Max);
  end;
  
  'Arr_Neighbors1': begin 
     FilterOnlyNumbersAndBools;
     var n := 20;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt*n);
     
     var a := IntArr(n);
     var cnt := 0;
     for var i := 0 to n-2 do
       if (a[i] = 2) and (a[i+1] = 5) then 
         cnt += 1;
     CheckOutputAfterInitial(cnt);
  end;
  'Arr_MinMaxInd1': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateAutoTests(10);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.IndexMax,a.IndexMax);
  end;
  'Arr_MinMaxInd2': begin 
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n);
     GenerateAutoTests(10);
     
     var a := IntArr(n);
     var acopy := Copy(a);
     var indmax := a.IndexMax;
     var indmin := a.IndexMin;
     Swap(a[indmax],a[indmin]);
     CheckOutput(acopy, a);  
  end;

  'Arr_Fill1': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Input := Empty);
    
    CheckOutput(ArrFill(10,1));
  end;
  'Arr_Fill2': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Input := Empty);
    
    CheckOutput(ArrFill(10,1));
  end;
  'Заполнение1','ЗаполнениеЛямбда1': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,i->i*i));
  end;
  'Заполнение2','ЗаполнениеЛямбда2': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,1,i->i+2));
  end;
  'ЗаполнениеПоПред1','ЗаполнениеПоПред2': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,5,i->i+5));
  end;
  'ЗаполнениеГеом1','ЗаполнениеГеом2': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,1,i->i*2));
  end;
  'ЗаполнениеСумма1': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,5,i->i+5).Sum);
  end;
  'ЗаполнениеСумма2': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(ArrGen(10,1,i->i*2).Sum);
  end;
  'ЗаполнениеСумма3_add': begin 
     CheckData(Input := |cInt|*1);
     var n := Int;
     CheckOutput(ArrGen(n,i -> (i+1)*(i+1)).Sum);
    end;
  // 'Arr_Transf1' 'Arr_Transf1a' 'Arr_Transf2' 'Arr_Transf3' 'Arr_Transf4' 'Arr_Transf5' 'Arr_Transf7' 'Arr_Transf8' 'Arr_Transf9'- задачник
  'Arr_Transf6': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     foreach var i in a.Indices do
       if i.IsOdd then
         a[i] := 0;
     CheckOutputAfterInitial(a);  
  end;

  'Arr_Reverse1','Arr_Reverse2': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Reverse);
  end;
  'Arr_Reverse3': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     for var i:= 0 to n div 2 - 1 do
       Swap(a[i],a[i + n div 2]);
     CheckOutputAfterInitial(a);
  end;

  'Arr_Reverse3a': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);

     var out := IntArr(n);
     var out0 := Copy(out);
     Reverse(out, 0, 5);
     Reverse(out, 5, 5);
     CheckOutput(out0, out);
  end;

  'Arr_Count1','Arr_Count2': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);

     var a := IntArr(n);
     CheckOutputAfterInitial(a.Count(x -> x.IsEven));
  end;
  'Arr_Count3': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.CountOf(3),a.CountOf(4),a.CountOf(5));
  end;
  'Arr_Count4': begin 
     var n := 30;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.CountOf(a.Min),a.CountOf(a.Max));
  end;

  'Arr_FindIndex1': begin 
     var n := 10;
     CheckData(|cInt|*n, |cInt|*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.FindIndex(x-> x.IsOdd),a.FindLastIndex(x-> x.IsOdd));
  end;
  'Arr_FindIndex2': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt * n);
     
     var a := IntArr(n);
     var i1 := a.FindIndex(x -> x.IsEven);
     var i2 := a.FindLastIndex(x -> x.IsOdd);
     Swap(a[i1],a[i2]);
     CheckOutputAfterInitial(a);
  end;

  'Arr_Filter1': begin 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateTests(10, tInt(1,100) * n);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Where(x -> x < 50));
  end;
  'Arr_GenFilter1': begin 
     CheckData(Input := Empty); 
     var a := ArrGen(50, 1, x -> x+2);
     CheckOutput(a.Where(x -> x mod 7 = 0));
  end;
  'Arr_GenFilter2': begin 
     var n := 40;
     CheckData(InitialOutput := |cInt|*n);
     var a := ArrGen(40,1,1,(x,y)->x+y);
     CheckOutputAfterInitial(a.Where(x->x mod 5 = 0));
  end;

  'Arr_Op1': begin 
    CheckData(Empty);
    var a := |1,2,3|*5;
    var b := |0| * 6 + |1| * 6;
    var c := |1,2,3|*3 + |4,5,6|*3;
    var d := |1| * 5 + |2| * 5 + |1| * 5 + |2| * 5;
    CheckOutput(a, b, c, d);
  end;

  'Arr_Slice1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(InitialOutput := |cInt| * n);
     var a := Arr(0..9);
     CheckOutput(a, a[2:6], a[:4], a[2:]);
  end;
  'Arr_Slice2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(InitialOutput := |cInt| * n);
     var a := Arr(0..9);
     CheckOutput(a, a[:2], a[5:], a[1:^1]);
  end;
  'Arr_Slice3': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(InitialOutput := cInt * n + cInt * 2);
     var a := Arr(0..9);
     CheckOutput(a, 9, 8, 7, a[^2:], a[1:^1])
  end;
  'Arr_Slice7': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     CheckData(InitialOutput := cInt * n);
     var a := Arr(0..9);
     CheckOutput(a, a[4::-1], a[9::-2])
  end;
  'Arr_Slice10': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Input := Empty); 
    var a := |6,2,4,3,6,1,7,8,2,4|;
    CheckOutput(a[:5].Sum);
  end;
  'Arr_Slice11': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Input := Empty); 
    var a := |6,2,4,3,6,1,7,8,2,4|;
    CheckOutput(a[5:].Min);
  end;
  'Arr_Slice12': begin 
    FilterOnlyNumbersAndBools;
    CheckData(Input := Empty); 
    var a := |6,2,4,3,6,1,7,8,2,4|;
    CheckOutput(a[::2].Average);
  end;

  'Arr_InsDel1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     var a := Arr(0..9);
     CheckData(InitialOutput := cInt * n);
     CheckOutput(a, a[:5], a[6:]);
  end;
  'Arr_InsDel2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     var a := Arr(0..9);
     CheckData(InitialOutput := cInt * n);
     CheckOutput(a, a[:5], 333, a[5:]);
  end;
  'Arr_InsDel3': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     var a := Arr(0..9);
     CheckData(InitialOutput := cInt * n);
     CheckOutput(a, a[:1], 555, a[1:^1], 555, a[^1:]);
  end;

  'Arr_Shift1': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     var a := Arr(0..9);
     CheckData(InitialOutput := cInt * n);
     CheckOutput(a, a[1:], 0);
  end;
  'Arr_Shift2': begin 
     FilterOnlyNumbersAndBools;
     var n := 10;
     var a := Arr(0..9);
     CheckData(InitialOutput := |cInt| * n);
     CheckOutput(a, 0, a[:^1]);
  end;

  'List1': begin 
     ClearOutputListFromSpaces; 
     CheckData(InitialOutput := |cInt|);
     CheckOutput('Иванов','Петров','Сидоров');
  end;
  'List2': begin 
     FilterOnlyNumbersAndBools; 
     CheckData(Input := Empty);
     CheckOutput(11,2,13,4,15);
  end;
  'List3': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(1,3,5,7);
  end;
  'List4': begin 
     FilterOnlyNumbersAndBools; 
     CheckData(Input := Empty);
     CheckOutputSeq((1..7).Select(x->x*x));
  end;
  'List5': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateAutoTests(10);
     
     var a := IntArr(n);
     CheckOutputAfterInitial(a.Where(x->x.IsEven));
  end;
  'List6': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(|cInt|*n, cInt*n);
     GenerateAutoTests(10);

     var a := IntArr(n);
     CheckOutputAfterInitial(a.Where(x->x.IsEven), a.Where(x->x.IsOdd));
  end;
  'List6a': begin 
     FilterOnlyNumbersAndBools; 
     CheckData(InitialInput := cInt * 3);
     GenerateAutoTests(10);
     
     var a := IntArr(3);
     CheckOutput(1,3,5,a);
  end;
  'List6b': begin 
     FilterOnlyNumbersAndBools;
     CheckData(Input := Empty);
     CheckOutput(1,3,5,2,4,6,9,8,7);
  end;
  'List7': begin 
     FilterOnlyNumbersAndBools; 
     var n := 20;
     CheckData(cInt*n, cInt*n);
     GenerateAutoTests(5);
     var a := IntArr(10);
     var b := IntArr(10);
     CheckOutputAfterInitial(a.Interleave(b));
  end;
  'List8': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(InitialOutput := cInt*n); 
     var a := OutSliceIntArr(0,n-1);
     var b := a.ToList;
     b.Insert(2,555);
     CheckOutput(a, b);
  end;
  'List9': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(InitialOutput := cInt*n); 
     var a := OutSliceIntArr(0,n-1);
     var b := a.ToList;
     if b.Count>0 then
       b.RemoveAt(2);
     CheckOutput(a, b);
  end;
  'List9a': begin 
     FilterOnlyNumbersAndBools; 
     CheckData(Input := Empty);
     CheckOutputSeq((1..10));
  end;
  'List9b': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(InitialInput := |cInt|*n);
     var a := IntArr(n);
     CheckOutput(a);
  end;
  
  'Sort1': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateAutoTests(5);
     var a := IntArr(n);
     CheckOutput(a, a.Sorted);
  end;
  'Sort2': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(cInt*n, cInt*n);
     GenerateAutoTests(5);
     var a := IntArr(n);
     CheckOutput(a, a.SortedDescending);
  end;
  'Sort3': begin 
     FilterOnlyNumbersAndBools; 
     var n := 9;
     CheckData(InitialOutput := cInt*n);
     
     var a := OutSliceIntArr(0,n-1);
     CheckOutput(a, a.Sorted);
  end;
  'BubbleSort': begin 
     FilterOnlyNumbersAndBools; 
     var n := 10;
     CheckData(InitialOutput := cInt*n);
     
     var a := OutSliceIntArr(0,n-1);
     var b := Copy(a);
     for var j:=n-1 downto 1 do
       if b[j-1] > b[j] then
         Swap(b[j-1], b[j]);
     CheckOutput(a, b);
  end;
  'BubbleSort2': begin 
     FilterOnlyNumbersAndBools; 
     var n := 9;
     CheckData(InitialOutput := cInt*n);
     
     var a := OutSliceIntArr(0,n-1);
     CheckOutput(a, a.Sorted);
  end;
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('Arr1','Arr2','Arr3','Arr4','Arr5','Arr6',
  'Arr_Sum0','Arr_Sum1','Arr_Sum2','Arr_Sum3','Arr_Sum4','Arr_CountA1','Arr_CountA2',
  'Arr_Replace1','Arr_Replace2','Arr_Find1','Arr_Find2','Arr_Find3','Arr_Find4',
  'Arr_Index1','Arr_Index2','Arr_Index2a','Arr_Index3','Arr_Index4',
  'Arr_MinMax1','Arr_MinMax2','Arr_MinMax3','Arr_MinMax4','Arr_Neighbors1','Arr_MinMaxInd1','Arr_MinMaxInd2',
  'Arr_Fill1','Arr_Fill2','Заполнение1','ЗаполнениеЛямбда1','Заполнение2','ЗаполнениеЛямбда2',
  'ЗаполнениеПоПред1','ЗаполнениеПоПред2','ЗаполнениеГеом1','ЗаполнениеГеом2','ЗаполнениеСумма1','ЗаполнениеСумма2',
  'Arr_Transf1','Arr_Transf1a','Arr_Transf2','Arr_Transf3','Arr_Transf4','Arr_Transf5','Arr_Transf7',
  'Arr_Transf8','Arr_Transf9','Arr_Transf6',
  'Arr_Reverse1','Arr_Reverse2','Arr_Reverse3','Arr_Reverse3a',
  'Arr_Count1','Arr_Count2','Arr_Count3','Arr_Count4',
  'Arr_FindIndex1','Arr_FindIndex2',
  'Arr_Filter1','Arr_GenFilter1','Arr_GenFilter2',
  'Arr_Op1',
  'Arr_Slice1','Arr_Slice2','Arr_Slice3','Arr_Slice7','Arr_Slice10','Arr_Slice11','Arr_Slice12',
  'Arr_InsDel1','Arr_InsDel2','Arr_InsDel3',
  'Arr_Shift1','Arr_Shift2',
  'List1','List2','List3','List4','List5','List6','List6a','List6b','List7','List8','List9','List9a','List9b',
  'Sort1','Sort2','Sort3',
  'BubbleSort','BubbleSort2');
finalization
end.