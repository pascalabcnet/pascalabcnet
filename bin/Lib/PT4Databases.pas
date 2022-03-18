unit PT4Databases;
//uses PrintAttributeUnit;

function PrintAttributeString(a: object): string; forward;
function PrintAttributeString(a: object; fProvider: System.IFormatProvider): string; forward;

type 
  Country = auto class
  private
    _name, _capital: string;
    _population: integer;
    _continent: string;
  public
    [PrintAttribute(0, -32)]
    property Name: string read _name;
    [PrintAttribute(' ', 1, -19)]
    property Capital: string read _capital;
    [PrintAttribute(3, 13, 'd')]
    property Population: integer read _population;
    [PrintAttribute(' ', 2, -9)]
    property Continent: string read _continent;
  end;

  Страна = auto class
  private
    _name, _capital: string;
    _population: integer;
    _continent: string;
  public
    [PrintAttribute(0, -32)]
    property Название: string read _name;
    [PrintAttribute(' ', 1, -19)]
    property Столица: string read _capital;
    [PrintAttribute(3, 13, 'd')]
    property Население: integer read _population;
    [PrintAttribute(' ', 2, -9)]
    property Континент: string read _continent;
  end;


  ТипПола = (Муж, Жен);

  Pupil = auto class
  private
    _name: string;
    _gender: ТипПола; 
    _height: integer;
    _cls: integer;
    _inSunSchool: boolean; 
    function getGender: ТипПола := _gender;
  public
    [PrintAttribute(0, -16)]
    property Name: string read _name;
    [PrintAttribute(' ', 1, -3)]
    property Gender: ТипПола read getGender;
    [PrintAttribute(' ', 2, 3, 'd')]
    property Height: integer read _height;
    [PrintAttribute(' ',3, 2, 'd')]
    property Cls: integer read _cls;
    [PrintAttribute(' ', 4, -5)]
    property InSunSchool: boolean read _inSunSchool;
  end;

  Ученик = auto class
  private
    _name: string;
    _gender: ТипПола; 
    _height: integer;
    _cls: integer;
    _inSunSchool: boolean; 
    function getGender: ТипПола := _gender;
  public
    [PrintAttribute(0, -16)]
    property Фамилия: string read _name;
    [PrintAttribute(' ', 1, -3)]
    property Пол: ТипПола read getGender;
    [PrintAttribute(' ', 2, 3, 'd')]
    property Рост: integer read _height;
    [PrintAttribute(' ',3, 2, 'd')]
    property Класс: integer read _cls;
    [PrintAttribute(' ', 4, -5)]
    property УчитсяВКШ: boolean read _inSunSchool;
  end;

function GenderToТипПола(a: string): ТипПола := a = 'Муж' ? Муж : Жен;
function InSunSchoolToBoolean(a: string): boolean := a = 'Да';

type 
  Fitness = auto class
  private
    _code, _year, _month, _time: integer;
  public
//    [PrintAttribute('( Code = ', 0, 2)]
//    property Код: integer read _code;
    [PrintAttribute('( Code = ', 0, 2)]
    property Code: integer read _code;
//    [PrintAttribute(' , Year = ', 1, 4)]
//    property Год: integer read _year;
    [PrintAttribute(' , Year = ', 1, 4)]
    property Year: integer read _year;
//    [PrintAttribute(' , Month = ', 2, 2)]
//    property Месяц: integer read _month;
    [PrintAttribute(' , Month = ', 2, 2)]
    property Month: integer read _month;
//    [PrintAttribute(' , Time = ', 3, 2)]
//    property Время: integer read _time;
    [PrintAttribute(' , Time = ', 3, 2)]
    property Time: integer read _time;
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;
  
  Abitur = auto class
  private
    _name: string;
    _year: integer;
    _school: integer;
  public
//    [PrintAttribute('( Name = ', 0, -11)]
//    property Фамилия: string read _name;
    [PrintAttribute('( Name = ', 0, -11)]
    property Name: string read _name;
//    [PrintAttribute(', Year = ', 1, 4)]
//    property Год: integer read _year;
    [PrintAttribute(', Year = ', 1, 4)]
    property Year: integer read _year;
//    [PrintAttribute(' , School = ', 2, 2)]
//    property Школа: integer read _school;
    [PrintAttribute(' , School = ', 2, 2)]
    property School: integer read _school;
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;

  Debtor = auto class
  private
    _name: string;
    _flat: integer;
    _debt: real;
  public
//    [PrintAttribute('( Name = ', 0, -11)]
//    property Фамилия: string read _name;
    [PrintAttribute('( Name = ', 0, -11)]
    property Name: string read _name;
//    [PrintAttribute(', Flat = ', 1, 3)]
//    property Квартира: integer read _flat;
    [PrintAttribute(', Flat = ', 1, 3)]
    property Flat: integer read _flat;
//    [PrintAttribute(' , Debt = ', 2, 7, 'f2')]
//    property Долг: real read _debt;
    [PrintAttribute(' , Debt = ', 2, 7, 'f2')]
    property Debt: real read _debt;
//    [PrintAttribute(' , Entrance = ', 3, 1)]
//    property Подъезд: integer read (_flat - 1) div 36 + 1;
    [PrintAttribute(' , Entrance = ', 3, 1)]
    property Entrance: integer read (_flat - 1) div 36 + 1;
//    [PrintAttribute(' , Floor = ', 4, 1)]
//    property Этаж: integer read (_flat - 1) mod 36 div 4 + 1;
    [PrintAttribute(' , Floor = ', 4, 1)]
    property Floor: integer read (_flat - 1) mod 36 div 4 + 1;
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;

FuelStation = auto class
  private
    _brand, _price: integer;
    _company, _street: string;
  public
//    [PrintAttribute('( Brand = ', 0, 2)]
//    property Марка: integer read _brand;
    [PrintAttribute('( Brand = ', 0, 2)]
    property Brand: integer read _brand;
//    [PrintAttribute(', Price = ', 1, 4)]
//    property Цена: integer read _price;
    [PrintAttribute(', Price = ', 1, 4)]
    property Price: integer read _price;
//    [PrintAttribute(', Company = ', 2, -12)]
//    property Компания: string read _company;
    [PrintAttribute(', Company = ', 2, -12)]
    property Company: string read _company;
//    [PrintAttribute(', Street = ', 3, -15)]
//    property Улица: string read _street;
    [PrintAttribute(', Street = ', 3, -15)]
    property Street: string read _street;
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;

PupilExam = auto class
  private
    _name: string;
    _school, _point0, _point1, _point2: integer;
//    [PrintAttribute(' , Point : ', 2, 3)]
//    property Балл0: integer read _point0;
    [PrintAttribute(' , Point : ', 2, 3)]
    property Point0: integer read _point0;
//    [PrintAttribute(' , ', 3, 3)]
//    property Балл1: integer read _point1;
    [PrintAttribute(' , ', 3, 3)]
    property Point1: integer read _point1;
//    [PrintAttribute(' , ', 4, 3)]
//    property Балл2: integer read _point2;
    [PrintAttribute(' , ', 4, 3)]
    property Point2: integer read _point2;
  public
//    [PrintAttribute('( Name = ', 0, -16)]
//    property ФИО: string read _name;
    [PrintAttribute('( Name = ', 0, -16)]
    property Name: string read _name;
//    [PrintAttribute(', School = ', 1, 2)]
//    property Школа: integer read _school;
    [PrintAttribute(', School = ', 1, 2)]
    property School: integer read _school;
//    property Балл: array of integer read Arr(_point0, _point1, _point2);
    property Point: array of integer read Arr(_point0, _point1, _point2);
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;

PupilMark = auto class
  private
    _name: string;
    _cls: integer;
    _subject: string;
    _mark: integer;
  public
//    [PrintAttribute('( Name = ', 0, -16)]
//    property ФИО: string read _name;
    [PrintAttribute('( Name = ', 0, -16)]
    property Name: string read _name;
//    [PrintAttribute(', Cls = ', 1, 2)]
//    property Класс: integer read _cls;
    [PrintAttribute(', Cls = ', 1, 2)]
    property Cls: integer read _cls;
//    [PrintAttribute(' , Subject = ', 2, -11)]
//    property Предмет: string read _subject;
    [PrintAttribute(' , Subject = ', 2, -11)]
    property Subject: string read _subject;
//    [PrintAttribute(' , Mark = ', 3, 1)]
//    property Оценка: integer read _mark;
    [PrintAttribute(' , Mark = ', 3, 1)]
    property Mark: integer read _mark;
    function ToString: string; override;
    begin
      result := PrintAttributeString(self);
    end;
  end;

function ЗаполнитьМассивСтран: array of Страна;
begin
  var fname := 'c:\Program files (x86)\PascalABC.NET\Files\Databases\Страны.csv';
  if fname = '' then
    raise new System.ApplicationException('Не найден массив стран Databases\Страны.csv');
  Result := ReadLines(fname)
    .Select(s->s.ToWords(';'))
    .Select(w->new Страна(w[0],w[1],w[2].ToInteger,w[3])).ToArray;
end; 

function GetCountries: array of Country;
begin
  var fname := 'c:\Program files (x86)\PascalABC.NET\Files\Databases\Страны.csv';
  if fname = '' then
    raise new System.ApplicationException('Не найден массив стран Databases\Страны.csv');
  Result := ReadLines(fname)
    .Select(s->s.ToWords(';'))
    .Select(w->new Country(w[0],w[1],w[2].ToInteger,w[3])).ToArray;
end; 

function ЗаполнитьМассивУчеников: array of Ученик;
begin
  var fname := 'c:\Program files (x86)\PascalABC.NET\Files\Databases\Ученики.csv';
  if fname = '' then
    raise new System.ApplicationException('Не найден массив учеников Databases\Ученики.csv');
  Result := ReadLines(fname)
    .Select(s->s.ToWords(';'))
    .Select(w->new Ученик(w[0],GenderToТипПола(w[2]),w[4].ToInteger,w[1].ToInteger,
      InSunschoolToBoolean(w[3]))).ToArray;
end; 

function GetPupils: array of Pupil;
begin
  var fname := 'c:\Program files (x86)\PascalABC.NET\Files\Databases\Ученики.csv';
  if fname = '' then
    raise new System.ApplicationException('Не найден массив учеников Databases\Ученики.csv');
  Result := ReadLines(fname)
    .Select(s->s.ToWords(';'))
    .Select(w->new Pupil(w[0],GenderToТипПола(w[2]),w[4].ToInteger,w[1].ToInteger,
      InSunschoolToBoolean(w[3]))).ToArray;
end; 

function GetFitness: array of Fitness;
begin
  var fname := 'Fitness.tst';
  if not FileExists(fname) then
  begin
    fname := 'Fitness.txt';
    if not FileExists(fname) then
    raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов Fitness');
  end;  
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new Fitness(w[0].ToInteger,w[1].ToInteger,w[2].ToInteger,w[3].ToInteger)).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов Fitness содержит неверные данные');
  end;
end; 

function GetAbiturs: array of Abitur;
begin
  var fname := 'Abitur.tst';
  if not FileExists(fname) then
  begin
    fname := 'Abitur.txt';
    if not FileExists(fname) then
    raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов Abitur');
  end;  
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new Abitur(w[0],w[1].ToInteger,w[2].ToInteger)).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов Abitur содержит неверные данные');
  end;
end; 

function GetDebtors: array of Debtor;
begin
  var fname := 'Debtor.tst';
  if not FileExists(fname) then
  begin
    fname := 'Debtor.txt';
    if not FileExists(fname) then
    raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов Debtor');
  end;  
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new Debtor(w[0],w[1].ToInteger,w[2].ToReal)).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов Debtor содержит неверные данные');
  end;
end; 

function GetFuelStations: array of FuelStation;
begin
  var fname := 'FuelStation.tst';
  if not FileExists(fname) then
  begin
    fname := 'FuelStation.txt';
    if not FileExists(fname) then
      raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов FuelStation');
  end;    
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new FuelStation(w[0].ToInteger,w[1].ToInteger,w[2],w[3])).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов FuelStation содержит неверные данные');
  end;
end; 

function GetPupilExams: array of PupilExam;
begin
  var fname := 'PupilExam.tst';
  if not FileExists(fname) then
  begin
    fname := 'PupilExam.txt';
    if not FileExists(fname) then
      raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов PupilExam');
  end;  
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new PupilExam(w[0],w[1].ToInteger,w[2].ToInteger,w[3].ToInteger,w[4].ToInteger)).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов PupilExam содержит неверные данные');
  end;
end; 

function GetPupilMarks: array of PupilMark;
begin
  var fname := 'PupilMark.tst';
  if not FileExists(fname) then
  begin
    fname := 'PupilMark.txt';
    if not FileExists(fname) then
    raise new System.IO.FileNotFoundException('Не найден файл-источник для массива объектов PupilMark');
  end;
  try  
  Result := ReadLines(fname)
    .Select(s->s.ToWords(','))
    .Select(w->new PupilMark(w[0],w[1].ToInteger,w[2],w[3].ToInteger)).ToArray;
  except
    raise new System.FormatException('Файл-источник для массива объектов PupilMark содержит неверные данные');
  end;
end; 

function PrintAttributeString(a: object; fProvider: System.IFormatProvider): string;
begin
  var t := a.GetType();
  var res := new Dictionary<integer, string>();
  foreach var p in t.GetProperties(System.Reflection.BindingFlags.Public 
    or System.Reflection.BindingFlags.NonPublic or System.Reflection.BindingFlags.Instance) do
  begin
    var att := p.GetCustomAttribute(typeof(PrintAttribute), false) as PrintAttribute;
    if att = nil then
      continue;
    var (c, n, w, f) := (att.Comment, att.Num, att.Width, att.Fmt);
    var val := '';
    var o := p.GetValue(a, nil);
    if (f <> '') and (o is System.IFormattable) then
    begin
      val := System.IFormattable(o).ToString(f, fProvider);
    end
    else
    begin
      val := o.ToString;
    end;  
    if w > 0 then
      val := val.PadLeft(w)
    else if w < 0 then
      val := val.PadRight(-w);
    res[n] := c + val;
  end;
  result := nil;
  if res.Keys.Count = 0 then
    exit;
  result := '';  
  foreach var k in res.Keys.OrderBy(e -> e) do
  begin
    result += res[k];
  end;  
  var delim := Copy(result, 2, 1);  
  if result.StartsWith('{') then
    result += (delim = ' ') ? ' }' : '}'
  else if result.StartsWith('[') then
    result += (delim = ' ') ? ' ]' : ']'
  else if result.StartsWith('(') then
    result += (delim = ' ') ? ' )' : ')'
  else if result.StartsWith('''') then
    result += (delim = ' ') ? ' ''' : ''''
  else if result.StartsWith('"') then
    result += (delim = ' ') ? ' "' : '"';
end;

function PrintAttributeString(a: object): string;
begin
  result := PrintAttributeString(a, nil);
end;

function ToStr(params a: array of object): string;
begin
  result := '';
  var b := false;
  foreach var e in a do
  begin
    if b then
      result += ' '
    else
      b := true;
    if e is real then
      result += real(e).ToString(2)
    else if e is string then
      result += e.ToString()
    else if e.GetType.FullName.StartsWith('System.Tuple') then
    begin
       var b1 := false;
       foreach var e1 in e.GetType.GetProperties do
       begin
          if b1 then
            result += ' '
          else
            b1 := true;
          result += ToStr(Arr(e1.GetValue(e,nil)));      
       end;   
    end      
    else if e is System.Collections.IEnumerable then
    begin
       var b1 := false;
       var e1 := (e as System.Collections.IEnumerable).GetEnumerator;
       while e1.MoveNext do
       begin
          if b1 then
            result += ' '
          else
            b1 := true;
          result += ToStr(Arr(e1.Current));
       end;   
    end      
    else 
          result += e.ToString();
  end;
end;

begin  
  
end.