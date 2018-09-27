uses BlockFileOfT;

type
  StructLayout = System.Runtime.InteropServices.StructLayoutAttribute;
  LayoutKind = System.Runtime.InteropServices.LayoutKind;
  FieldOffset = System.Runtime.InteropServices.FieldOffsetAttribute;
  
  GCHandle = System.Runtime.InteropServices.GCHandle;
  GCHandleType = System.Runtime.InteropServices.GCHandleType;
  
  
  
  //Эти 2 типа взят из справки, и по рекомендациям от туда переделан под наш случай
  //имеется в виду справка, которая html файл, лежащий в папке с этим примером
  [StructLayout(LayoutKind.&Explicit)]	
  CharArr255Body = record	
    private const MaxLength = 255;	
    private const TSize = 2;	
    private const Size = MaxLength * TSize;	
    [FieldOffset(Size-1)] last: byte;	
  end;
  [StructLayout(LayoutKind.&Explicit)]	
  CharArr255 = record	
    public [FieldOffset(0)] length: byte;	
    public [FieldOffset(1)] body: CharArr255Body;	
    
    public class function operator explicit(a: array of char): CharArr255;
    begin
      Result.length := Min(CharArr255Body.MaxLength, a.Length);//если a.Length>MaxLength - length присвоит MaxLength
      
      if a.Length < CharArr255Body.MaxLength then 
        a := a + new char[CharArr255Body.MaxLength - a.Length];//если a.Length<MaxLength - надо удлинить
                                                              //иначе можем иногда получать ошибки чтения защищённой памяти
                                                              //если откажет в доступе к памяти, которая находиться полсе массива
      
      //надо заблокировать массив на 1 месте в памяти, чтоб сборщик мусора не двигал пока мы копируем
      var hnd := GCHandle.Alloc(a, GCHandleType.Pinned);	
//      try
        var ptr: ^CharArr255Body := pointer(hnd.AddrOfPinnedObject);	
        Result.body := ptr^;
//      finally
        hnd.Free;//в finally, чтоб если возникнет какая то ошибка - блокировку всё равно сняло
                 //иначе получим утечку памяти, потому что блокировка запрещает сборщику мусора освобождать память
                 //Но пока что я убрал, потому что github.com/pascalabcnet/pascalabcnet/issues/1267
//      end;	
    end;
    
    public class function operator explicit(s: string): CharArr255 :=
    CharArr255(s.ToCharArray);
    
    public class function operator explicit(a: CharArr255): array of char;
    begin
      //Если кол-во сохранённых байт < MaxLength - a.body будет занимать бОльший объём в памяти, чем поместится в Result	
      //Поэтому надо сначала прочитать всё, включая пустые символы на конце, во временную переменную	
      //А затем скопировать нужное кол-во символов в Result	
      var temp := new char[CharArr255Body.MaxLength];	
      
      var hnd := GCHandle.Alloc(temp, GCHandleType.Pinned);	
//      try
        var ptr: ^CharArr255Body := pointer(hnd.AddrOfPinnedObject);	
        ptr^ := a.body;
//      finally
        hnd.Free;	
//      end;
      
      if a.length = CharArr255Body.MaxLength then
      begin
        Result := temp;//всё же, копирование ссылки - это быстрее, чем копировать всё содержимое массива
        exit;
      end;
      
      Result := new char[a.length];	
      System.Array.Copy(temp, Result, a.length);
    end;
    
    public function ToRefArr: array of char;
    type
      CharArr = array of char;
    begin
      Result := CharArr(self);	
    end;
    
    public function ToString: string; override :=
    new string(ToRefArr);
    
    public class function operator explicit(s: CharArr255): string :=
    s.ToString;
    
  end;
  
  ///Это будет сохранять в file of T
  AR = record
    s: string[255];
    //dt: System.DateTime;//Не даёт ¯\_(ツ)_/¯
    dt: int64;//Ну ландо, в System.DateTime всё хранится в 1 поле типа int64
    i: integer;
    ch: char;
    b: byte;
  end;
  ///Это будет сохранять в BlockFileOfT
  BR = record
    s: CharArr255;
    //dt: System.DateTime;//И тут тоже - DateTime это "особый тип", подробнее в справке
    dt: int64;//Ну а мы - знаем 1 версию, ту что сейчас, и в ней всё хранится как внутренне поле типа int64
    i: integer;
    //ch: char;//И тут, опять, особый тип. Ну, word хранит информацию так же как char
    ch: word;//В типе IOR описаны все преобразования, как превратить эти типы в System.DateTime и char
    b: byte;
  end;
  
  ///Этот тип для ввода/вывода
  ///Чтоб и string[255] и CharArr255 преобразовывало в string
  ///Так честнее
  ///Хотя при преобразовании string[255] к string почти ничего не происходит, только копируется одна ссылка
  IOR = record
    s: string;
    dt: System.DateTime;
    i: integer;
    ch: char;
    b: byte;
    
    class function operator explicit(a: IOR): AR;
    begin
      Result.s := a.s;
      Result.dt := a.dt.Ticks;
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    class function operator explicit(a: IOR): BR;
    begin
      Result.s := CharArr255(a.s);
      Result.dt := a.dt.Ticks;
      Result.i := a.i;
      Result.ch := word(a.ch);
      Result.b := a.b;
    end;
    
    class function operator explicit(a: AR): IOR;
    begin
      Result.s := a.s;
      Result.dt := new System.DateTime(a.dt);
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    class function operator explicit(a: BR): IOR;
    begin
      Result.s := new string(a.s.ToRefArr);
      Result.dt := new System.DateTime(a.dt);
      Result.i := a.i;
      Result.ch := char(a.ch);
      Result.b := a.b;
    end;
    
    class function GetRandom: IOR;
    begin
      Result.s := new string(ArrGen(Random(256), i -> ChrAnsi(Random(byte.MaxValue))));
      Result.dt := System.DateTime.Now.AddTicks(System.Convert.ToInt64(((Random * 2 - 1) * 1024 * 1024 * 1024 * 1024)));
      Result.i := (Random(word.MaxValue) shl 16) + Random(word.MaxValue);
      Result.ch := ChrAnsi(Random(byte.MaxValue));
      Result.b := Random(256);
    end;
    
    public function ToString: string; override :=
    $'IOR( s.Length={s.Length}, dt={dt}, i={i}, ch="{ch}", b={b} )';
  
  end;

procedure TestIntegrity1(c: integer);
begin
  
  var f: file of AR;
  var test_arr := ArrGen(c, i -> IOR.GetRandom);
  
  Rewrite(f, 'temp.bin');
  foreach var a in test_arr.Select(a -> AR(a)) do
    f.Write(a);
  
  f.Close;
  
  Reset(f);
  var n_test_arr := f.ReadElements.Select(a -> IOR(a)).ToArray;
  f.Close;
  
  if not test_arr.SequenceEqual(n_test_arr) then
  begin
    writeln('тест 1 (file of T) не пройден');
    if test_arr.Length <> n_test_arr.Length then
      writeln($'len: {test_arr.Length} <> {n_test_arr.Length}') else
      for var i := 0 to test_arr.Length - 1 do
        if test_arr[i] <> n_test_arr[i] then
        begin
          writeln($'el:{#10}{test_arr[i]}{#10}{n_test_arr[i]}{#10}');
        end;
    readln;
    exit;
  end;
  
end;

procedure TestIntegrity2(c: integer);
begin
  
  var f := new BlockFileOf<BR>('temp.bin');
  var test_arr := ArrGen(c, i -> IOR.GetRandom);
  
  f.Rewrite;
  f.Write(test_arr.ConvertAll(a -> BR(a)));
  f.Close;
  
  f.Reset;
  var n_test_arr := f.Read(f.Size).ConvertAll(a -> IOR(a));
  f.Close;
  
  if not test_arr.SequenceEqual(n_test_arr) then
  begin
    writeln('тест 2 (BlockFileOf<T>) не пройден');
    if test_arr.Length <> n_test_arr.Length then
      writeln($'len: {test_arr.Length} <> {n_test_arr.Length}') else
      for var i := 0 to test_arr.Length - 1 do
        if test_arr[i] <> n_test_arr[i] then
        begin
          writeln($'el:{#10}{test_arr[i]}{#10}{n_test_arr[i]}{#10}');
        end;
    
    readln;
    exit;
  end;
  
end;

procedure TestIntegrity;
begin
  TestIntegrity1(1);
  writeln('тест 1 ok');
  TestIntegrity2(1024);
  writeln('тест 2 ok');
  writeln('тесты ok');
end;

procedure TestSpeed;
begin
  
  var sw := new System.Diagnostics.Stopwatch;//точнее чем этим замерить невозможно
  var lc := 10;
  var ec := 10000;//Чем больше элементов - тем больше преимущество BlockFileOf<T>, потому что он сохраняет их всех сразу.
                  //Но он быстрее даже если сохранять по 1 элементу
  var t1, t2: int64;
  
  var f1: file of AR;
  Assign(f1, 'temp.bin');
  var f2 := new BlockFileOf<BR>('temp.bin');
  
  while true do
  begin
    
    var test_arr := ArrGen(ec, i -> IOR.GetRandom);
    
    sw.Restart;
    loop lc do
    begin
      Rewrite(f1);
      var AR_arr := test_arr.ConvertAll(a -> AR(a));
      foreach var a in AR_arr do f1.Write(a);
      f1.Close;
      
      Reset(f1);
      ArrGen(ec, i -> IOR(f1.Read));
      f1.Close;
    end;
    sw.Stop;
    t1 += sw.ElapsedTicks;
    
    sw.Restart;
    loop lc do
    begin
      f2.Rewrite;
      f2.Write(test_arr.ConvertAll(a -> BR(a)));
      f2.Close;
      
      f2.Reset;
      f2.Read(ec).ConvertAll(a -> IOR(a));
      f2.Close;
    end;
    sw.Stop;
    t2 += sw.ElapsedTicks;
    
    System.GC.Collect;
    System.Console.Clear;
    writeln($'file of T: {t1}');
    writeln($'BlockFile: {t2}');
  end;
  
end;

begin//Уберите флажок "Debug версия" в сервис>>настройки>>опции компиляции и запускайте по Shift+F9, иначе не честно
  
  TestIntegrity;//можно закомментировать
  
  TestSpeed;
  
end.