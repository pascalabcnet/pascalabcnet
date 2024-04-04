// Сериализация объектов
// Можно сериализовать только объекты, помеченные атрибутом [Serializable]
// Внешние Serialize, Deserialize позволяют сохранить в файле - восстановить 
//   один объект (один граф объектов с данным корнем)  
type 
  [Serializable]
  Node = auto class
    x: integer;
    next: Node;
  end;

const fname = 'a.dat';

begin
  var m := new Node(5,new Node(3,new Node(4,nil))); // Связный  список
  Serialize(fname,m); // Сериализуем объект в файл
  var m1 := Deserialize(fname) as Node; // Десериализуем из файла
  Print(m1);
end.