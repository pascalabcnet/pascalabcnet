uses FormsABC;

type 
  View = class
  private
    a,b,sum,prod: IntegerField;
    procedure MyClick;
    begin
      sum.Value := a.Value + b.Value;
      prod.Value := a.Value * b.Value;
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