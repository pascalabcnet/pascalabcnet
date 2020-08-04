// Случайные шары
uses Graph3D,Controls;

function R := Random(-7,7);

begin
  Window.Title := 'Случайные шары';
  LeftPanel(150,Colors.Orange);
  var b := new ButtonWPF('Создать шар');
  b.Click := () → begin
    Sphere(P3D(R,R,R),1);
  end; 
end.