// Вложенные циклы for. Сетка из квадртов
uses GraphABC;

const 
  sz = 35;
  zz = 5;

begin
  var h := sz+zz;
  for var nx:=0 to Window.Width div h do
  for var ny:=0 to Window.Height div h do
  begin
    Brush.Color := clRandom;
    Rectangle(zz+nx*h,zz+ny*h,zz+nx*h+sz,zz+ny*h+sz);
  end;  
end.