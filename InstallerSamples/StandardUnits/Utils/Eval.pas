uses Utils;

begin
  Println(Eval('2 + 3 * 4'));
  Println(Eval('(2.4 + 3 % 2) * 6 - 1'));
  Println(Eval('(5 > 3) and (2 < 4)'));
  Println(Eval('IIF(17 % 2 = 0, ''even'', ''odd'')'));
  Println(Eval('Len(''PascalABC.NET'')'));
  
  var i := EvalInt('2 + 3');
  var x := EvalReal('10 / 4.0');
  var b := EvalBool('(10 >= 5) and (3 <> 4)');
  var s := EvalStr('''PascalABC'' + ''.NET''');
  
  Println(i, x, b, s)
end.