unit u_nested1;
type Examinations = record
algebra, calculus, programming : integer;

procedure InitRan;
end;

procedure Examinations.InitRan;
  function RandomMark : integer;
  begin
    result := 10;
  end;
begin
  algebra := RandomMark;
  calculus := RandomMark;
  programming := RandomMark;
  assert(algebra=10);
  assert(calculus=10);
  assert(programming=10);
end;

type CExaminations = class
algebra, calculus, programming : integer;

procedure InitRan;
end;

procedure CExaminations.InitRan;
  function RandomMark : integer;
  begin
    result := 10;
  end;
begin
  algebra := RandomMark;
  calculus := RandomMark;
  programming := RandomMark;
  assert(algebra=10);
  assert(calculus=10);
  assert(programming=10);
end;

var rec : Examinations;
    cls : CExaminations;
    
begin
  rec.InitRan;
  cls := new CExaminations;
  cls.InitRan;
end.