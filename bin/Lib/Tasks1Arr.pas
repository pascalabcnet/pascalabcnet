﻿unit Tasks1Arr;

uses LightPT;

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  ClearOutputListFromSpaces;
  case name of
  'Arr0': begin 
    CheckData(Empty);
    CheckOutput(5,4,3,2,1,777,5,4,3,2,1,777);
  end;
  'Arr1': begin 
    CheckData(Empty);
    CheckOutput(777,1,1,1,777);
  end;
  'Arr2': begin 
    var n := 5;
    CheckData(Input := cInt*n);
    GenerateTests(10,tInt*n);
    var il := IntArr(n);
    CheckOutput(il.Select(x->x+1));
  end;
  'Arr3': begin 
    var n := 5;
    CheckData(Input := |cInt|*n);
    GenerateTests(10,tInt*n);
    var il := IntArr(n);
    CheckOutput(il.Select(x->x*2));
  end;
  'Arr4': begin 
    var n := 10;
    CheckData(Input := cInt*n);
    GenerateTests(10,tInt*n);
    var il := IntArr(n);
    CheckOutput(il+il.Select(x->x*x));
  end;
  'Arr5': begin 
    var n := 10;
    CheckData(Input := cInt*n);
    GenerateTests(10, tInt*n);
    var il := IntArr(n);
    CheckOutput(il, il.Reverse);
  end;
  'ArrErr1': begin 
    TaskResult := TaskStatus.ErrFix
  end;
  'ArrErr2': begin 
    TaskResult := TaskStatus.ErrFix
  end;
  'Arr1Sum': begin 
    var n := 10;
    CheckData(Input := cInt*n);
    GenerateTests(10, tInt*n);
    var il := InputListAsIntegers.Select(x->x*0.1);
    CheckOutput(il, il.Sum);
  end;
  'Arr2Prod': begin 
    CancelMessagesIfInitial := False; // !
    var n := 8;
    CheckData(cRe*n, cRe*n);
    GenerateTests(10, tRe(1,10,1)*n);
    var il := ReArr(n).Select(x->x);
    CheckOutputAfterInitial(il.Product);
  end;
  'Arr1_If': begin 
    CancelMessagesIfInitial := False;
    var n := 10;
    CheckData(cInt*n, cInt*n);
    GenerateTests(10, tInt*n);
    var il := IntArr(n);
    CheckOutput(il, il.Where(x->x.IsEven));
  end;
  'Arr2_If': begin 
    CancelMessagesIfInitial := False;
    var n := 10;
    CheckData(cInt*n, cInt*n);
    GenerateTests(10,tInt*n);
    var il := IntArr(n);
    CheckOutput(il, il.Where((x,i)->i.IsOdd));
  end;
  'Arr0_Count': begin 
    CancelMessagesIfInitial := False;
    var n := 10;
    CheckData(|cInt|*n, |cInt|*n);
    GenerateTests(10,tInt*n);
    var il := IntArr(n);
    CheckOutput(il, il.Count(x->x.IsOdd));
  end;
  'Arr1_Sum': begin 
    var n := InputList.Count;
    GenerateTests(10, tInt*n);
    var a := IntArr(n);
    var il := a.Where(x->x in 3..5).ToArray;
    CheckOutput(a, il, il.sum);
  end;
  'Arr2_Count': begin 
    var n := InputList.Count;
    GenerateTests(10, tInt*n);
    var a := IntArr(n);
    CheckOutput(a, a.CountOf(2), a.CountOf(3), a.CountOf(4), a.CountOf(5));
  end;
  'Arr1_MinMax': begin 
    var n := InputList.Count;
    GenerateTests(10, tInt*n);
    var a := IntArr(n);
    CheckOutput(a, a.Min);
  end;
  'Arr2_MinMax': begin 
    var n := InputList.Count;
    GenerateTests(10,tInt*n);
    var a := IntArr(n);
    CheckOutput(a, a.Max);
  end;
  'Arr3_MinMax': begin 
    var n := InputList.Count;
    GenerateTests(10,tInt*n);
    var a := IntArr(n);
    CheckOutput(a, a.IndexMin);
  end;
  'ПоискВМассиве': begin 
    var n := 10;
    CheckData(cInt*n, cInt*n);
    GenerateTests(10, tInt*n);
    var a := IntArr(n);
    CheckOutputAfterInitial(5 in a);
  end;
  'ПоискВМассивеСтрок': begin 
    CheckData(InitialInput := |cStr|, InitialOutput := |cStr|*8);
    GenerateTests('Козлов','Серов','Иванова','Петров','Умнова','Хулиганкин','Белова','Ослов');
    var s := Str(0);
    var a := Arr('Козлов','Серов','Иванова','Петров','Умнова','Хулиганкин','Белова','Ослов');
    CheckOutputAfterInitial(s in a);
  end;
  'Заполнение1': begin 
    CheckData(Empty);
    CheckOutput(0,1,4,9,16,25,36,49,64,81);
  end;
  'Заполнение2Арифм': begin 
    CheckData(Empty);
    CheckOutput(ArrGen(10,1,x->x+2));
  end;
  'Заполнение3Арифм': begin 
    CheckData(Empty);
    CheckOutput(ArrGen(10,1,x->x+2));
  end;
  'Заполнение4Арифм': begin 
    CheckData(Empty);
    CheckOutput(ArrGen(10,1.0,x->x+0.5));
  end;
  'Заполнение1Геом': begin 
    CheckData(Empty);
    CheckOutput(ArrGen(10,1,x->x*2));
  end;
  'ЗаполнениеЧисламиФибоначчи': begin 
    CheckData(Empty);
    CheckOutput(ArrGen(10,1,1,(x,y)->x+y));
  end;
  'СоседниеЭлементы1': begin 
    var n := 10;
    CheckData(cInt*n, cInt*n);
    GenerateTests(10,tInt*n);
    var a := IntArr(n);
    var ol := new ObjectList;
    ol.AddRange(a.Pairwise.Where(\(x,y)->x<y).Select(t->t[1]));
    CheckOutputAfterInitial(ol);
  end;
  'СоседниеЭлементы2': begin 
    var n := 10;
    CheckData(cInt*n, cInt*n);
    GenerateTests(10, tInt*n);
    var a := IntArr(n);
    var ol := new ObjectList;
    ol.AddRange(a);
    ol.Add(a.Pairwise.Where(\(x,y)->x<y).Count);
    CheckOutput(ol);
  end;
  'ИнвертированиеМассива1': begin 
    var n := 7;
    CheckData(InitialOutput := |cStr|*n);
    var a := OutSliceStrArr(0,n-1);
    CheckOutput(a + a.Reverse);
  end;
  'ИнвертированиеМассива2': begin 
    CancelMessagesIfInitial := False;
    // Введено целое, потом массив целых(n). Выведен этот массив
    var n := Int;
    CheckData(InitialInput := |cInt|*(n+1), InitialOutput := |cInt|*n);
    // Тут сгенерировать тесты сложнее потому что длина массива переменная. В дело вступает лямбда
    TestCount := 10;
    GenerateTestData := tnum -> begin
      var n := Random(5,10);
      var a := ArrRandomInteger(n);
      InputList.AddTestData(|n|+a);
    end;
    var a := IntArr(n);
    var ares := a.Reverse.ToArray;
    CheckOutputAfterInitial(ares); 
  end;
  'СтрокаКакМассив1': begin 
    CancelMessagesIfInitial := False;
    CheckData(InitialOutput := |cStr|*2);
    CheckOutput('бочка','ночка','дочка','кочка','почка','точка'); 
  end;
  'СтрокаКакМассив2': begin 
    CheckData(Empty);
    CheckOutput('миг','мир','пир','пар'); 
  end;
  'СтрокаКакМассив3': begin 
    CancelMessagesIfInitial := False;
    var s := 'на паркете написана абракадабра';
    CheckData(InitialOutput := |cStr|*1);
    
    var ol := new ObjectList;
    ol.Add(s);
    ol.Add(s.CountOf('а'));
    CheckOutputSeq(ol);
  end;
  'СтрокаКакМассив4': begin 
    CheckData(Empty);
    CheckOutput(True);
  end;
  'СтрокаКакМассив5': begin 
    CheckData(Empty);
    CheckOutput('абракадабра','эбрэкэдэбрэ');
  end;
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('Arr0','Arr1','Arr2','Arr3','Arr4','Arr5',
  'ArrErr1','ArrErr2','Arr1Sum','Arr2Prod','Arr1_If','Arr2_If','Arr0_Count','Arr1_Sum','Arr2_Count',
  'Arr1_MinMax','Arr2_MinMax','Arr3_MinMax',
  'ПоискВМассиве','ПоискВМассивеСтрок',
  'Заполнение1','Заполнение2Арифм','Заполнение3Арифм','Заполнение4Арифм','Заполнение1Геом','ЗаполнениеЧисламиФибоначчи',
  'СоседниеЭлементы1','СоседниеЭлементы2',
  'ИнвертированиеМассива1','ИнвертированиеМассива2',
  'СтрокаКакМассив1','СтрокаКакМассив2','СтрокаКакМассив3','СтрокаКакМассив4','СтрокаКакМассив5' );
finalization
end.