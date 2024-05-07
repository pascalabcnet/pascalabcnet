type
  r1 = record
    public a := 123;
    public b := 'abc';
    internal c := 'err';
    
    public property p: byte read byte(a);
    internal property e: string read 'err';
    
    private function raise_getter: byte;
    begin
      Result := 0;
      raise new Exception('Expected Exception');
    end;
    public property f: byte read raise_getter;
    
  end;
  
  Base<T1,T2,T3> = class end;
  Derived<T> = class(Base<T, array of T, word>) end;
  
var res := new StringBuilder;
procedure TestO(o: object);
begin
  TypeName(o, res);
  res.Append('{ ');
  _ObjectToString(o, res);
  res.Append(' }'#10);
end;
procedure TestT(t: System.Type);
begin
  TypeToTypeName(t, res);
  res.Append(#10);
end;

begin
  
  TestO(1);
  TestO('abc'); // typeof(string).IsPrimitive=false, но Type.GetTypeCode работает
  TestO(new System.UIntPtr(123)); // Выводим только UIntPtr, без пространства имён
  TestO(System.ConsoleColor.Cyan); // Type.GetTypeCode возвращает integer - это надо явно обходить
  TestO(Lst(0).GetEnumerator); // Вложенный тип
  
  // Картежи
  TestO(System.Tuple.Create(1,2.3));
  TestO(System.ValueTuple.Create(1,2.3)); // Сейчас ValueTuple не обрабатывает
  
  // Последовательности
  TestO(new byte[2,1,1,1,1,1,1,1,2]);
  TestO(new byte[2,1,1,1,0,1,1,1,2]); // 0 элементов
  TestO(|1.2,3|.ToList); // Элементы real, потому что у 1 из них есть вещественная часть
  TestO(|1.2,3|.ToHashSet); // Мноежства выводятся в {}
  TestO(|1,2,3,4,5|.Skip(1).Take(3)); // C# yield последовательность
  TestO(|1,2,3,4,5|.AdjacentGroup); // Паскалевская yield последовательность
  TestO(new System.Collections.ArrayList(|6,7,8|)); // Вывод содержимого НЕ типизированной последовательности
  
  // Тип не реализует последовательность - а является ею
  TestO(|Seq(0)|);
  TestO(|Seq(0) as System.Collections.IEnumerable|);
  
  // Сложная структура со свойствами
  TestO(new r1);
  
  // Содержимое ссылок на функции
  var d1 := procedure(b: byte)->exit();
  var d2: Action<byte> := d1;
  var d3 := function: object->d1;
  var d4: Func0<object> := d3;
  TestO(d1);
  TestO(d2); // Преобразование к содержимому типу делегата
  TestO(Delegate(d3)); // Захват переменной
  TestO(Delegate(d4)); // Два предыдущих вместе
  TestO(Arr(d1)); // Внутри массива
  TestO(Arr&<System.MulticastDelegate>(d1)); // Тип элементов массива абстрактный, поэтому у него нету .Invoke
  
  // Генерики
  TestT(typeof(Dictionary<,>)); // Не подставленные параметры в [], т.е. [TKey]
  TestT(typeof(Derived<byte>).BaseType); // Сложная подстановка типов
  TestT(typeof(Derived<>).BaseType); // Только часть типов подставлена
  TestT(Lst(0).GetType.GetGenericTypeDefinition); 
  TestT(Lst(0).GetEnumerator.GetType.GetGenericTypeDefinition); // Вложенный тип без подстановки
  
  // Особый генерик, подобный не существует нигде в стандартной библиотеке .Net
  begin
    {$reference ObjectToStringExt.dll}
    var t := typeof(OTS.CS.Parent<>).GetNestedTypes.Single;
    TestT(t);
    TestT(t.MakeGenericType(typeof(byte), typeof(word)));
    TestT(t.MakeGenericType(typeof(byte), t.GetGenericArguments[1]));
    TestT(t.MakeGenericType(t.GetGenericArguments[0], typeof(word)));
  end;
  
  var otp_fname := 'ObjectToString.txt';
  var otp_new_fname := 'ObjectToString[new].txt';
  var enc := new System.Text.UTF8Encoding(true);
  
  var dir := GetCurrentDir;
  if System.IO.Path.GetFileName(dir)='exe' then
    dir := System.IO.Path.GetDirectoryName(dir);
  
  var expected := ReadAllText(dir+'\'+otp_fname, enc).Remove(#13);
  var curr := res.ToString.RegexReplace('(?<=Exception:[^\n]*)\r?\n(?:(?!(?<!\()\)).)*', '', RegexOptions.Singleline);
  var tr := expected = curr;
  if not tr then WriteAllText(dir+'\'+otp_new_fname, curr, enc);
  Assert(tr);
end.