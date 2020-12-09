// Сериализация объектов
// Можно сериализовать только объекты, помеченные атрибутом [Serializable]
// Внешние Serialize, Deserialize позволяют сохранить в файле - восстановить 
//   один объект (один граф объектов с данным корнем)  
type 
  [Serializable]
  My = auto class
    x,y: integer;
  end;
  IntArray = array of integer;

const fname = 'a.dat';

begin
  var f := CreateBinary(fname);
  f.Serialize(new My(444,555));
  f.Serialize(|1,2,3|);
  f.Serialize(Lst(1..9));
  f.Close;
  
  f := OpenBinary(fname);
  var m: My := f.Deserialize as My;
  var a: array of integer := f.Deserialize as IntArray;
  var l: List<integer> := f.Deserialize as List<integer>;
  f.Close;
  
  Print(m,a,l);
end.