uses BlockFileOfT;

// Эта процедура позволяет копировать память между содержимым массива и другим массивом/стеком
// При этом не волнуясь о блокировках массивов в памяти
// Если элемент массива передан как var-параметр, то сборщик мусора не трогает этот массив
procedure CopyMem<T1,T2>(var o1: T1; var o2: T2; count: integer) :=
System.Buffer.MemoryCopy(
  @o1, @o2,
  count, count
);

type
  StructLayout = System.Runtime.InteropServices.StructLayoutAttribute;
  LayoutKind = System.Runtime.InteropServices.LayoutKind;
  FieldOffset = System.Runtime.InteropServices.FieldOffsetAttribute;
  
  
  
  // Size контролирует размер записи. Его можно не указывать, тогда его выберет автоматически
  // В нашем случае нужен один байт для поля length
  // А затем место под 255 символов (масимум, который может хранить эта строка)
  // При этом каждый символ занимает два байта в памяти
  [StructLayout(LayoutKind.&Explicit, Size= 1 + 255*2 )]
  ValueString255 = record	
    public [FieldOffset(0)] length: byte;	
    public [FieldOffset(1)] body: char;	
    
    public const MaxLength = 255;
    
    // operator explicit, принимающий array of char и возвращающий ValueString255 выглядит в коде как:
    // var s := ValueString255(a);
    // Где тип у "a" — array of char
    public static function operator explicit(a: array of char): ValueString255;
    begin
      Result.length := Min(MaxLength, a.Length); // если a.Length>MaxLength, то length надо обрезать до MaxLength
      if Result.length=0 then exit; // иначе упадёт a[0]
      CopyMem(a[0], Result.body, Result.length*2); // char занимает два байта, поэтому копируем length*2
    end;
    
    public static function operator explicit(s: string): ValueString255;
    begin
      Result.length := Min(MaxLength, s.Length);
      if Result.length=0 then exit;
      CopyMem(s[1], Result.body, Result.length*2);
    end;
    
    public static function operator explicit(s: ValueString255): array of char;
    begin
      Result := new char[s.length];
      if s.length=0 then exit;
      CopyMem(s.body, Result[0], s.length*2);
    end;
    
    // объявлять тип CharArr может быть неудобно
    public function ToCharArray: array of char;
    type CharArr = array of char;
    begin
      Result := CharArr(self);	
    end;
    
    public static function operator explicit(s: ValueString255): string :=
    new string(@s.body, 0, s.length);
    
    public function ToString: string; override :=
    string(self);
    
  end;
  
  ///Это будет сохранять в file of T
  AR = record
    s: string[255];
    
    //dt: DateTime; // Не даёт ¯\_(ツ)_/¯
    dt: int64; // Ну и ладно, в System.DateTime всё хранится в одном поле типа int64
    
    i: integer;
    ch: char;
    b: byte;
  end;
  ///Это будет сохранять в BlockFileOf<T>
  BR = record
    s: ValueString255;
    dt: DateTime; // А BlockFileOf<T> принимает любые размерные типы без ограничений
    i: integer;
    ch: char;
    b: byte;
  end;
  
  ///Это тип для ввода/вывода
  ///В нём хранятся входные данные, общие и для file of T, и для BlockFileOf<T>
  ///Тут же описаны и преобразования всех особых типов
  ///Заметьте, преобразование string[255] к string и назад ничего не копирует и не преобразовывает, если изначальная строка уже была <= 255 символов
  ///Поэтому преобразования между ValueString255 и string работают медленнее
  ///И даже при этом BlockFileOf<T> всё равно быстрее
  IOR = record
    s: string;
    dt: System.DateTime;
    i: integer;
    ch: char;
    b: byte;
    
    static function operator explicit(a: IOR): AR;
    begin
      Result.s := a.s;
      Result.dt := a.dt.Ticks; // Ticks возвращает никак не преобразованное значение единственного внутреннего поля DateTime
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    static function operator explicit(a: IOR): BR;
    begin
      Result.s := ValueString255(a.s); // вызываем наш operator explicit
      Result.dt := a.dt;
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    static function operator explicit(a: AR): IOR;
    begin
      Result.s := a.s;
      Result.dt := new DateTime(a.dt); // Единственный конструктор DateTime принимающий 1 параметр - принимает кол-во тиков. И напрямую присваивает это значение внутреннему полю
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    static function operator explicit(a: BR): IOR;
    begin
      Result.s := string(a.s);
      Result.dt := a.dt;
      Result.i := a.i;
      Result.ch := a.ch;
      Result.b := a.b;
    end;
    
    static function GetRandom: IOR;
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
  TestIntegrity1(10000);
  writeln('тест 1 ok');
  TestIntegrity2(10000);
  writeln('тест 2 ok');
  writeln('тесты ok');
end;

procedure TestSpeed;
begin
  
  var sw := new System.Diagnostics.Stopwatch; // Точнее, чем этим, замерить невозможно
  var lc := 10;
  var ec := 10000;//Чем больше элементов, тем больше преимущество BlockFileOf<T>, потому что он сохраняет их всех сразу.
                  //Но он быстрее даже если сохранять по одному элементу
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

begin
  // Уберите флажок "Debug версия" в "Сервис>>Настройки>>Опции компиляции" и запускайте по Shift+F9, иначе отладка может неравномерно влиять на результаты
  
  // Тест на отсутствие ошибок. Можно убрать
  TestIntegrity;
  
  // Тест скорости
  // Он бесконечный, чем дольше тестируется - тем более усреднённые, а значит и более точные результаты
  TestSpeed;
  
end.