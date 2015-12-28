//Перегрузка операторов
type 
  Student = class
    Name: string;
    Height: integer;
  public
    constructor(Name: string; Height: integer);
    begin
      Self.Name := Name;
      Self.Height := Height;
    end;
    class function operator<(left,right: Student): boolean; 
    // Сравнение по росту
    begin
      Result := left.Height < right.Height;
    end;
    class function operator>(left,right: Student): boolean; 
    begin
      Result := left.Height > right.Height;
    end;
    function ToString: string; override;
    begin
      Result := string.Format('{0} ({1})', Name, Height);
    end;
  end;

var
  s1,s2: Student;

begin
  s1 := new Student('Stepa Morkovkin',188);
  s2 := new Student('Petya Pomidorov',180);
  Writeln('s1: ',s1);
  Writeln('s2: ',s2);
  Writeln;
  Writeln('s1<s2: ',s1<s2);
  //
  Writeln('Student.operator>(s1,s2): ',Student.operator>(s1,s2));
end.