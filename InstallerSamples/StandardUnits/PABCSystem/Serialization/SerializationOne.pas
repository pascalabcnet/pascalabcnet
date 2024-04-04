// Сериализация объектов
// Можно сериализовать только объекты, помеченные атрибутом [Serializable]
// Внешние Serialize, Deserialize позволяют сохранить в файле - восстановить 
//   один объект (один граф объектов с данным корнем)  
type 
  [Serializable]
  My = auto class
    x,y: integer;
  end;

const fname = 'a.dat';

begin
  var m := new My(2,3);
  Serialize(fname,m); // Сериализуем объект в файл
  var m1 := Deserialize(fname) as My; // Десериализуем из файла
  Print(m1);
end.