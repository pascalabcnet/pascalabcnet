uses NumLibABC;

// Линейная алгебра (матрицы)
begin
  // M = det ((A-BxC)^T)xA
  var A:=new Matrix(2,4,-3,0,4,-1,2,-7,5,6);
  var B:=new Matrix(2,3,8,1,-5,6,7,2);
  var C:=new Matrix(3,4,1,-1,7,0,3,2,9,4,5,0,-2,-4);
  var M:=(((A-B*C).Transpose)*A).Det;
  Writeln(M)
end.
