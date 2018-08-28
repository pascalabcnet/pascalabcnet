uses BlockFileOfT;

type
  r1=record
    b1:byte;
    i:integer;
    b2:byte;
    
    class function GetRandom:r1;
    begin
      Result.b1 := Random(256);
      Result.i := Random(256) shl 24 + Random(256) shl 16 + Random(256) shl 8 + Random(256);
      Result.b2 := Random(256);
    end;
  end;

begin//Уберите флажок "Debug версия" в сервис>>настройки>>опции компиляции и запускайте по Shift+F9, иначе не честно
  
  var sw := new System.Diagnostics.Stopwatch;//точнее чем этим замерить невозможно
  var lc := 1000;
  var ec := 100;
  var t1,t2:int64;
  
  var f1: file of r1;
  var f2 := new BlockFileOf<r1>;
  
  while true do
  begin
    
    var test_arr := ArrGen(ec,i->r1.GetRandom);
    
    sw.Restart;
    loop lc do
    begin
      Rewrite(f1,'temp.bin');
      f1.Write(test_arr);
      f1.Close;
      
      Reset(f1);
      foreach var el in f1.ReadElements do;
      f1.Close;
    end;
    sw.Stop;
    t1 += sw.ElapsedTicks;
    
    sw.Restart;
    loop lc do
    begin
      f2.Rewrite('temp.bin');
      f2.Write(test_arr);
      f2.Close;
      
      f2.Reset;
      f2.Read(f2.FileSize);
      f2.Close;
    end;
    sw.Stop;
    t2 += sw.ElapsedTicks;
    
    System.Console.Clear;
    writeln($'file of T: {t1}');
    writeln($'BlockFile: {t2}');
  end;
  
end.