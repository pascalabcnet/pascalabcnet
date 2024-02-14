unit TasksStr;

uses LightPT;

function Chr(i: integer) := PABCSystem.Chr(i);

var AllTaskNames: array of string;

procedure CheckTaskT(name: string);
begin
  FlattenOutput; 
  ClearOutputListFromSpaces; 

  case name of
  'Char1': begin 
    CheckData(Input := Empty);
    var a := Arr(word(48), word(100), word(68), word(1102), word(8594));
    CheckOutputSeq(a);
  end;
  'Char2': begin 
    CheckData(Input := Empty);
    var a := Arr(Chr(169), Chr(931), Chr(960), Chr(1105), Chr(8551), Chr(8734), Chr(8776), Chr(8986), Chr(9327), Chr(9775), Chr(9917), Chr(10054));
    CheckOutputSeq(a);
  end;
  'CharTable': begin 
    CheckData(Input := Empty);
    var obj := new ObjectList;
    for var i:=32 to 1200 do
    begin  
      Obj.Add(i); obj.Add(Chr(i));
    end;  
    CheckOutputSeq(obj);
  end;
  'CharPredSucc': begin 
    CheckData(Input := |cChar|);
    GenerateTests('b','c','2','y');
    var c := LightPT.Chr(0);
    CheckOutput(Pred(c),Succ(c));
  end;
  'CharIs': begin 
    CheckData(Input := |cChar|);
    GenerateTests('b','X','2','ю');
    var c := LightPT.Chr(0);
    CheckOutputSeq(Arr(c.IsLetter, c.IsDigit, c.IsLower, c.IsUpper));
  end;
  'CharRange1': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('0'..'9')+Arr('a'..'z')+Arr('а'..'я'));
  end;
  'CharRange2': begin 
    CheckData(Input := |cChar|);
    GenerateTests('a','V','z','Ю','д','1');
    var c := LightPT.Chr(0);
    CheckOutput(c in 'a'..'z');
  end;
  'CharRange3': begin 
    CheckData(Input := |cChar|);
    GenerateTests('a','V','ю','A','1','Z');
    var c := LightPT.Chr(0);
    CheckOutput(c in 'A'..'Z');
  end;
  'CharRange4': begin 
    CheckData(Input := |cChar|);
    GenerateTests('a','V','z','A','1','Ю');
    var c := LightPT.Chr(0);
    CheckOutput(c.ToLower in 'a'..'z');
  end;
  'CharTransform': begin 
    CheckData(Input := |cChar|);
    var c := LightPT.Chr(0);
    GenerateTests('a','V','z','A','1','Ю');
    if c.IsLower then
      c := c.ToUpper
    else if c.IsUpper then
      c := c.ToLower;
    CheckOutput(c);
  end;
  'Strings1': begin 
    CheckData(Input := Empty);
    CheckOutputSeq('трос'.ToCharArray);
  end;
  'Strings2': begin 
    CheckData(Input := Empty);
    CheckOutputSeq('сорт'.ToCharArray);
  end;
  'Strings2a': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('грозы','осы','ложки','кошки'));
  end;
  'Strings3': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('миг', 'мир', 'пир', 'пар'));
  end;
  'Strings4': begin 
    CheckData(Input := Empty);
    var hs := HSet('кошка','мышка','комик','комок','ножка','ножик','коржик','камыш');
    var hs1 := OutputListAsStrings.ToHashSet;
    if (hs*hs1).Count >= 5 then
      TaskResult := Solved
    else begin
      TaskResult := PartialSolution;
      ColoredMessage($'Выведите еще по крайней мере {5-(hs*hs1).Count} правильных слов',MessageColorT.MsgColorGray)
    end;
  end;
  'Strings5': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('********************', '010101010101010101010101010101'));
  end;
  'Strings6': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('кросс', 'атолл', 'Гримм', 'быстрее'));
  end;
  'String13': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(9);
  end;
  'String15': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := |cStr|);
    var s := Str(0);
    CheckOutput(s.Count(c -> c.IsLetter and c.IsLower));
  end;
  'String16': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := |cStr|);
    GenerateTests('HeLLo','привет','ИНФОРМАТИКА');
    var s := Str(0);
    CheckOutput(s.ToLower);
  end;
  'StrCode': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('рсйгжу', 'сткдзф', 'тулеих'));
  end;
  'StrDeCode': begin 
    CheckData(Input := Empty);
    CheckOutput('мы великие взломщики шифров');
  end;
  'StrCountAlg': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr(3,5,2));
  end;
  'StrGenerate1': begin 
    CheckData(Input := Empty);
    CheckOutput('abcdefghijklmnopqrstuvwxyz');
  end;
  'StrGenerate2': begin 
    CheckData(Input := Empty);
    CheckOutput('каждый охотник желает знать где сидит фазан');
  end;
  'StrCount': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(14,15);
  end;
  'StrCountOf': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(3,5,2);
  end;
  'StrCreate1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('abcdefghijklmnopqrstuvwxyz');
  end;
  'StrCreate2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('каждый охотник желает знать где сидит фазан');
  end;
  'StrWhere1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('каждый охотник желает знать где сидит фазан','буря мглою небо кроет вихри снежные крутя'));
  end;
  'StrWhere2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('1223113221113','12112571191460'));
  end;
  'StrSelect1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('вфсѐ!ндмпя!ожвп!лспжу!гйцсй!тожзоьж!лсфуѐ'));
  end;
  'StrSelect2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    var a := Arr(1082,1072,1078,1076,1099,1081,32,1086,1093,1086,1090,1085,1080,1082,32,1078,1077,1083,1072,1077,1090,32,1079,1085,1072,1090,1100,32,1075,1076,1077,32,1089,1080,1076,1080,1090,32,1092,1072,1079,1072,1085 );
    var a1 := a.Select(x->word(x));
    CheckOutputSeq(a1);
  end;
  'StrMinMax': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('а','ё'));
  end;
  'StrWhereSelect': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(23);
  end;
  'StrAllAny1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(True,False);
  end;
  'StrAllAny2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(True,False);
  end;
  'StrEachCount1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    var res := 'The grass is always greener on the other side of the fence'.EachCount;
    var ob := new ObjectList;
    foreach var x in res.ToArray do
      ob.Add(x);
    CheckOutputSeq(ob);
  end;
  'StrEachCount2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    var res := 'L1o24st tim437364e is nev567e9r fo089874u89nd041342 ag8432ai6897'.EachCount;
    var ob := new ObjectList;
    foreach var x in res.ToArray do
      if x.Key.IsDigit then
        ob.Add(x);
    CheckOutputSeq(ob);
  end;
  'StrOrder1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(' adefgilmnorstuv');
  end;
  'StrOrder2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('0','1','3','5','8');
  end;
  'ToWords1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('головой жак звонарь как однажды сломал фонарь');
  end;
  'ToWords2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('как жак сломал фонарь однажды звонарь головой');
  end;
  'ToWordsWhere1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput('косой кривоногий косой косой косой косой');
  end;
  'Pos1': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr(2,9));
  end;
  'Pos2': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutputSeq(Arr(7));
  end;
  'Delete1': begin 
    CheckData(Input := Empty);
    CheckOutput('информатика');
  end;
  'Delete2': begin
    CheckData(Input := Empty); 
    CheckOutput('программирование');
  end;
  'Delete3': begin 
    CheckData(Input := Empty);
    CheckOutput('алгоритм');
  end;
  'Insert1': begin 
    CheckData(Input := Empty);
    CheckOutput('Петр Первый был первым русским императором');
  end;
  'Insert2': begin 
    CheckData(Input := Empty);
    CheckOutput('Петр Первый был великим русским императором');
  end;
  'StrChange1': begin 
    CheckData(Input := Empty);
    CheckOutput('красный коричневый желтый зеленый бирюзовый синий фиолетовый');
  end;
  'StrChange2': begin 
    CheckData(Input := Empty);
    CheckOutput('Алекс у Мелиссы украл кораллы, а Мелисса у Алекса украла кларнет');
  end;
  'StrBorder0': begin 
    CheckData(Input := Empty);
    CheckOutput('наклонный <b>жирный</b> подчеркнутый <b>жирный</b> <b>жирный</b> подчеркнутый');
  end;
  'StrBorder1': begin 
    CheckData(Input := Empty);
    CheckOutput('наклонный <b>жирный</b> подчеркнутый жирный жирный подчеркнутый');
  end;
  'StrSlice1': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('Pascal','ABC','NET'));
  end;
  'StrSlice2': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('клад зарыт на юго западе', 'зашифрованное сообщение'));
  end;
  'StrSlice4': begin 
    CheckData(Input := Empty);
    CheckOutputSeq(Arr('mydocument', 'docx', 'picture', 'jpg'));
  end;
  'Str_Number0': begin 
    CheckData(Input := Empty);
    CheckOutput('оценка1', 'балл2', 'окно3');
  end;
  'Str_Number03': begin 
    CheckData(Input := Empty);
    ClearOutputListFromSpaces;
    CheckOutput(386);
  end;
  'Str_Number04': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := |cInt|);
    GenerateTests(2,34,567,67890);
    var n := Int(0);
    CheckOutput(n.ToString.Select(s->s.ToDigit).Sum);
  end;
  'Str_Number05': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(31.6666666666667,27.0,42.0);
  end;
  'Str_Number06': begin 
    ClearOutputListFromSpaces;
    CheckData(Input := Empty);
    CheckOutput(420);
  end;
  end;
end;

initialization
  CheckTask := CheckTaskT;
  AllTaskNames := Arr('Char1','Char2','CharTable','CharPredSucc','CharIs',
  'CharRange1','CharRange2','CharRange3','CharRange4','CharTransform',
  'Strings1','Strings2','Strings2a','Strings3','Strings4','Strings5','Strings6','String13','String15','String16',
  'StrCode','StrDeCode','StrCountAlg','StrGenerate1','StrGenerate2','StrCount','StrCountOf',
  'StrCreate1','StrCreate2','StrWhere1','StrWhere2','StrSelect1','StrSelect2',
  'StrMinMax','StrWhereSelect','StrAllAny1','StrAllAny2','StrEachCount1','StrEachCount2',
  'StrOrder1','StrOrder2','ToWords1','ToWords2','ToWordsWhere1',
  'Pos1','Pos2','Delete1','Delete2','Delete3','Insert1','Insert2',
  'StrChange1','StrChange2','StrBorder0','StrBorder1',
  'StrSlice1','StrSlice2','StrSlice4',
  'Str_Number0','Str_Number03','Str_Number04','Str_Number05','Str_Number06' );
finalization
end.