// Перегрузка операторов
type 
  Student = auto class
    Name: string;
    Height: integer;
  public
    // Сравнение по росту
    class function operator<(left,right: Student): boolean := left.Height < right.Height;
    class function operator>(left,right: Student): boolean := left.Height > right.Height;
    function ToString: string; override := string.Format('{0} ({1})', Name, Height);
  end;

begin
  var s1 := new Student('Stepa Morkovkin',188);
  var s2 := new Student('Petya Pomidorov',180);
  Writeln('s1: ',s1);
  Writeln('s2: ',s2);
  Writeln;
  Writeln('s1<s2: ',s1<s2);
  //
  Writeln('Student.operator>(s1,s2): ',Student.operator>(s1,s2));
end.