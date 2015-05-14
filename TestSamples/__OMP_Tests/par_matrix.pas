type  
  Matrix = array [,] of real;

type 
  My = class
  A,B,C: Matrix;
  _m,_n,_k:integer;
  
  constructor (m,n,k: integer);
  begin
    CreateRandomMatrix(A,m,k);    
    CreateRandomMatrix(B,k,n);    
    CreateRandomMatrix(C,m,n);    
    _m := m;   
    _n := n;
    _k := k;    
  end;
  
  procedure CreateRandomMatrix(var A: Matrix; m,n: integer);
  begin
    SetLength(A,m,n);
    for var i:=0 to m-1 do
    for var j:=0 to n-1 do
      A[i,j] := Random;
  end;
  
  procedure MultParallel;
  begin
    {$omp parallel for}
    for var i:=0 to _m-1 do
      for var j:=0 to _n-1 do
        for var l:=0 to _k-1 do
           c[i,j] := a[i,l]*b[l,j];
  end;
  
  procedure Mult;
  begin
    for var i:=0 to _m-1 do
      for var j:=0 to _n-1 do
        for var l:=0 to _k-1 do
           c[i,j] := a[i,l]*b[l,j];
  end;
end;

const n = 500;

begin
  
  var m := new My(n,n,n);
  var m0 := Milliseconds;
  m.Mult;
  writeln(Milliseconds-m0, 'ms');
  
  m := new My(n,n,n);
  m0 := Milliseconds;
  m.MultParallel;
  writeln(Milliseconds-m0, 'ms');
  
end.
