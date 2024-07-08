unit Tasks1Begin;

uses LightPT;

var AllTaskNames: array of string;
  
procedure CheckTaskT(name: string);
begin
  ClearOutputListFromSpaces; 

  case name of
  'Var_1': begin 
    CheckData(InitialOutput := |cInt|);
    CheckOutput(3,32); 
  end;
  'Var_2': begin 
    CheckData(InitialOutput := cInt*2);
    CheckOutput(3,4,20,45);
  end;
  'Var_4': begin 
    CheckData(Input := Empty);
    CheckOutput(25,3.14,100,0.75);
  end;  
  'Var_5': begin 
    TaskResult := Solved;
  end;  
  'Var_6': begin 
    CheckData(Input := Empty);
    CompareTypeWithOutput(cStr,cStr,cStr,cStr,cInt,cInt);
  end;  
  'Calculations1': begin 
    CheckData(InitialOutput := |cInt|*3);
    CheckOutput(35,65,35+65,35*65);
  end;
  'Calculations2': begin 
    CheckData(InitialOutput := |cInt,cInt,cStr,cInt|);
    CheckOutput(35,65,cStr,35+65,cStr,35*65); // Это круто - можно перемежать типы и значения!
  end;
  'Calculations3': begin 
    CheckData(InitialOutput := |cStr,cInt,cInt,cInt|);
    CheckOutput(cStr,3,4,5,cStr,3+4+5);
  end;
  'Calculations4': begin 
    CheckData(InitialOutput := |cStr,cInt|);
    CheckOutput(cStr,253,cStr,253/10,cStr,253/100);
  end;
  'Read1': begin 
    CheckData(InitialInput := |cInt,cInt|);
    TestCount := 5;
    GenerateTestData := tnum -> begin
      var (a,b) := Random2(1,100);
      InputList.AddTestData(|a,b|);
    end;
    CheckOutput(Int(0)+Int(1));
  end;
  'Read2': begin
    FilterOnlyNumbersAndBools;
    CheckData(Input := |cRe,cRe|);
    var S := Re(0)*Re(1);
    var P := 2*(Re(0)+Re(1));
    TestCount := 10;
    GenerateTests(10, tRe(1,10,digits := 0)*2);
    CheckOutputSilent(S,P); // Все сообщения ColoredMessage гасятся
    if TaskResult <> Solved then // Это прикольный способ - проверить, что вывел то, но не в том порядке!
      CheckOutput(P,S);
  end;
  'Task1': begin
    FilterOnlyNumbersAndBools;
    CheckData(Input := |cRe,cRe|);
    var S := Re(0)+Re(1);
    var P := Re(0)*Re(1);
    GenerateTests(10,tRe(1,10,0)*2); // Это когда нет особых случаев
    CheckOutputSilent(S,P); // Все сообщения ColoredMessage гасятся
    if TaskResult <> Solved then
      CheckOutput(P,S);
  end;
  'Task2': begin
    FilterOnlyNumbersAndBools;
    CheckData(Input := |cRe|);
    var x := Re(0);
    TestCount := 10;
    GenerateTestData := tnum -> begin // это когда есть особый случай
      var a: real;
      if tnum = 5 then
        a := RandomReal(1,100,digits := 0)
      else a := RandomReal(1,10,digits := 0);
      InputList.AddTestData(|a|);
    end;
    CheckOutput(x*x,x*x*x,x*x*x*x);
  end;
  'Intermediate1': begin 
    CheckData(Input := Empty);
    CheckOutput(27);
  end;
  'Intermediate2': begin 
    CheckData(Input := Empty);
    CheckOutput(2.5);
  end;
  'Swap1':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      CheckOutput(666.666, 555.555);
    end;
  'Swap2': CheckOutput('Иванов', 'Петров', 'Петров', 'Иванов');
  'AssignAdd1':
    begin
      FilterOnlyNumbers;
      CheckData(InitialOutput := |cInt, cInt|);
      CheckOutput(Arr(1..9));  
    end;
  'AssignAdd2':
    begin
      FilterOnlyNumbers;
      CheckInitialOutput(cInt, cInt, cInt);
      CheckOutput(1, 2, 4, 7, 11, 16, 22, 29, 37, 46);
    end;  
  'AssignMult':
    begin
      FilterOnlyNumbers;
      CheckInitialOutput(cInt, cInt);
      CheckOutput(1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024);
    end;  
  'AssignMultAddGame':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      if OutputList.Count <> 1 then
      begin  
        ColoredMessage('Вы должны вывести ровно одно значение');
        exit;
      end;
      TaskResult := TaskStatus.PartialSolution;
      if CompareValues(OutputList[0],18) then
        exit;
      if CompareValues(OutputList[0],33) then
        ColoredMessage('Молодец! Теперь получи в переменной a значение 67',MsgColorGray)
      else CheckOutput(67);
    end; 
  'AssignMultAddКруги':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      CheckOutput(38);
    end; 
  'Sqrt1':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      if OutputList.Count <> 1 then
      begin  
        ColoredMessage('Вы должны вывести ровно одно значение');
        exit;
      end;
      TaskResult := TaskStatus.PartialSolution;
      if CompareValues(OutputList[0],Sqrt(2)) then
        exit;
      if CompareValues(OutputList[0],Sqrt(3)) then
        ColoredMessage('Молодец! Теперь получи значение Sqrt(5)',MsgColorGray)
      else if CompareValues(OutputList[0],Sqrt(5)) then
        ColoredMessage('Молодец! Теперь получи значение Sqrt(7)',MsgColorGray)
      else if CompareValues(OutputList[0],Sqrt(7)) then
        CheckOutput(Sqrt(7)) // Верно
      else CheckOutput(Sqrt(7)) // Неверно
    end; 
  'Abs_1':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      // Просто поясняющее сообщение. Ничего не проверяется
      ColoredMessage('Abs - функция, убирающая знак числа. В математике известна как модуль числа', MsgColorGray);
    end; 
  'Abs_2_AB':
    begin
      FilterOnlyNumbers;
      CheckData(InitialOutput := cInt * 6); 
      CheckOutput(5,3,2,-1,5,6,7,4,3,9,18,9);
    end; 
  'SwapProc1':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      CheckOutput(3,1,7,5,1,3,5,7);
    end; 
  'SwapProc2':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      CheckOutput(1,2,3,4,2,1,3,4,2,3,1,4,2,3,4,1);
    end; 
  'DivMod01':
    begin
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      // Просто поясняющее сообщение
      ColoredMessage('Перепишите результат в тетрадь', MsgColorGray);
    end; 
  'DivMod02':
    begin
      FilterOnlyNumbers;
      CheckData(InitialOutput := |cInt|*6);
      CheckOutputAfterInitial(3,1,5,1,7,1);
      //ColoredMessage('Если x - четно, то x mod 2 = 0', MsgColorGray);
    end; 
  'DivMod03':
    begin
      TaskResult := PartialSolution;
      FilterOnlyNumbers;
      CheckData(Input := Empty);
      if OutputList.Count = 6 then
      begin
        if CompareValues(OutputList[0],37) then // Здесь используется CompareValues, а не CompareWithOutput т к логика сложнее
          ColoredMessage('Измените значение a на 123', MsgColorGray)
        else if CompareValues(OutputList[0],123) then
          ColoredMessage('Верно. Проверьте еще значение a = 12345', MsgColorGray)
        else if CompareValues(OutputList[0],12345) then
        begin  
          ColoredMessage('Верно!', MsgColorGreen);
          TaskResult := Solved;
        end;  
      end;
    end; 
  
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('Var_1','Var_2','Var_4','Var_5','Var_6',
  'Calculations1','Calculations2','Calculations3','Calculations4',
  'Read1','Read2','Task1','Task2','Intermediate1','Intermediate2','Swap1','Swap2',
  'AssignAdd1','AssignAdd2','AssignMult','AssignMultAddGame','AssignMultAddКруги',
  'Sqrt1','Abs_1','Abs_2_AB','SwapProc1','SwapProc2','DivMod01','DivMod02','DivMod03' );
finalization
end.