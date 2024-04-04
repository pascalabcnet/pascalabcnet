unit TasksMatr;

uses LightPT;

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  FlattenOutput; // Во всех задачах на массивы
  ClearOutputListFromSpaces; // Это чтобы a.Print работал. По идее, надо писать всегда. Яне знаю задач, где пробелы в ответе

  case name of
  'ДвумерныеМассивы1': begin 
    ConvertStringsToNumbersInOutputList;
    //FilterOnlyNumbersAndBools; 
    CheckOutputSeq(|5,4,5,5,2,4,3,3,5,3,5,4|);
  end;
  'ДвумерныеМассивы2': begin 
    // Преобразовать в 6 целых по строкам
    ConvertStringsToNumbersInOutputList;
    CheckOutputSeq(|5,4,5,5,2,4,3,3,5,3,5,4|);
  end;
  'ДвумерныеМассивы3': begin 
    ConvertStringsToNumbersInOutputList;
    CheckData(Input := |cInt|*12);
    CheckOutputSeq(InputList);
  end;
  'Заполн1_Формула','Заполн2_Matr': begin 
    ConvertStringsToNumbersInOutputList;
    CheckData(Input := Empty);
    
    var ob := new ObjectList;
    for var i:=0 to 8 do
    for var j:=0 to 8 do
      ob.Add((i+1)*(j+1));
    CheckOutputSeq(ob);
  end;
  'Заполн3_Шахм': begin 
    ConvertStringsToNumbersInOutputList;
    CheckData(Input := Empty);
    
    var ob := new ObjectList;
    for var i:=0 to 8 do
    for var j:=0 to 8 do
      if (i + j).IsEven then
        ob.Add(1)
      else ob.Add(0);
    CheckOutputSeq(ob);
  end;
  'SumInMatr': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 12;
    CheckData(InitialInput := |cInt|*n, InitialOutput := |cInt|*n);
    var res := IntArr(n).Sum;
    CheckOutputAfterInitial(res);
  end;
  'MaxInMatr': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 12;
    CheckData(|cInt|*n, |cInt|*n);
    var res := IntArr(n).Max;
    CheckOutputAfterInitial(res);
  end;
  'СтрокаСтолбец1','СтрокаСтолбец3': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 54;
    CheckData(|cInt|*n, |cInt|*n);

    var m := MatrByRow(6,9,IntArr(n));
    CheckOutputAfterInitialSeq(m.Row(2)); 
  end;
  'СтрокаСтолбец2','СтрокаСтолбец4': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 54;
    CheckData(|cInt|*n, |cInt|*n);

    var m := MatrByRow(6,9,IntArr(n));
    CheckOutputAfterInitialSeq(m.Col(8)); 
  end;
  'MaxInRow': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 54;
    CheckData(|cInt|*n, |cInt|*n);

    var m := MatrByRow(6,9,IntArr(n));
    CheckOutputAfterInitial(m.Row(0).Max,m.Row(5).Min,m.Col(4).Average);    
  end;
  'CountInRowsCols': begin 
    ConvertStringsToNumbersInOutputList;
    var n := 54;
    CheckData(|cInt|*n, |cInt|*n);
    GenerateTests(10, tInt(1,5) * n);
    
    var m := MatrByRow(6,9,IntArr(n));
    CheckOutputAfterInitial(m.Row(1).CountOf(2), m.Col(2).Count(x->x.IsOdd));
  end;  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('ДвумерныеМассивы1','ДвумерныеМассивы2','ДвумерныеМассивы3',
  'Заполн1_Формула','Заполн2_Matr','Заполн3_Шахм','SumInMatr','MaxInMatr',
  'СтрокаСтолбец1','СтрокаСтолбец3','СтрокаСтолбец2','СтрокаСтолбец4','MaxInRow','CountInRowsCols' );
finalization
end.