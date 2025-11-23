uses FormsABC;

type 
  Model = class
    class procedure Calc(x,y: integer; var sum,prod: integer);
    begin
      sum := x + y;
      prod := x * y;
    end;
  end;
  
  View = class
  private
    a,b,sum,prod: IntegerField;
    procedure MyClick;
    begin
      var s,p: integer;
      Model.Calc(a.Value,b.Value,s,p);
      sum.Value := s;
      prod.Value := p;
    end;
  public
    constructor Create;
    begin
      a := new IntegerField('a:');
      b := new IntegerField('b:');
      LineBreak;
      sum := new IntegerField('Сумма:',220);
      LineBreak;
      prod := new IntegerField('Произведение:',220);
      LineBreak;
      EmptyLine(20);
      var d := new Button('Вычислить');
      d.Click += MyClick;
    end;
  end;

begin
  var v := new View;
end.