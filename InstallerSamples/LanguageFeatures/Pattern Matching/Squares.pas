type
  Line = class
  end;
  Rectangle = auto class
    X,Y,Width,Height: real;
  end;
  Circle = auto class
    X,Y,Radius: real;
  end;
  
begin
  var l := new List<Object>;
  l.Add(new Line);
  l.Add(new Circle(10,10,5));
  l.Add(new Rectangle(10,10,20,10));
  foreach var x in l do
    match x with
  Line(var ll): Println('Line S =',0);
  Circle(var c): Println('Circle S =',c.Radius*c.Radius*Pi);
  Rectangle(var r): Println('Rectangle S =',r.Width*r.Height);
    end;
end.  