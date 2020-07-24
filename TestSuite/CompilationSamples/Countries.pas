unit Countries;

type 
  Att = class(System.Attribute) 
    Num: integer;
    constructor (n: integer) := Num := n;
  end;
  
  ///!#
  Country = auto class
  private  
    nm,cap: string;
    inh: integer;
    cont: string;
  public
    [Att(1)]
    property Название: string read nm;
    [Att(2)]
    property Столица: string read cap;
    [Att(4)]
    property Население: integer read inh;
    [Att(3)]
    property Континент: string read cont;
  end;
  
function ЗаполнитьМассивСтран: array of Country;
begin
  var fname := __FindFile('Databases\Страны.csv');
  if fname = '' then
    raise new System.ApplicationException('Не найден массив стран Databases\Страны.csv');
  Result := ReadLines(fname)
    .Select(s->s.ToWords(';'))
    .Select(w->new Country(w[0],w[1],w[2].ToInteger,w[3])).ToArray;
end; 

begin  
  
end.