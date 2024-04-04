unit Tasks;

{$savepcu false}

uses LightPT;
uses TasksArr;

procedure CheckTaskT(name: string);
begin
  if name in TasksArr.AllTaskNames then
    TasksArr.CheckTaskT(name)
  else  
  case name of
  'Введение': begin 
    ColoredMessage('Данный урок посвящён массивам в PascalABC.NET');
    ColoredMessage('Ознакомьтесь предварительно с теорией: https://pascalabcnet.github.io/school_arrays.html',MsgColorGray);
  end;
  'HW2': begin 
    ClearOutputListFromSpaces;
    ConvertStringsToNumbersInOutputList;
    var n := 12;
    CheckData(Input := |cInt|*n);
    GenerateTests(10, tInt(1,10) * n);
    
    var m := MatrByRow(3,4,IntArr(n));
    CheckOutputSeq(m.ElementsByRow + |m.Row(1).Max,m.Col(2).Min,m.Col(3).Sum|); 
  end;
  end;
end;

initialization
  CheckTask := CheckTaskT;
finalization
end.