uses NumLibABC;

procedure TestApproxCheb(s:string; a,b:array of real; eps:real);
// a - массив вычисленных значений
// b - массив ожидаемых значений
// eps - допустимая абсолютная погрешность решений
begin
  for var i:=0 to a.Length-1 do begin
    var Msg:=s+': найдено: '+a[i]+', ожидалось '+b[i];
    Assert(Abs(Abs(a[i])-Abs(b[i]))<=eps,Msg);
    end
end;

procedure TestDecomp(s:string; a:array[,] of real; b:array of real;
    roots:array of real; eps:real);
// a - матрица системы;
// b - вектор правых частей;
// roots - вектор эталонных решений;
// eps - максимальная абсолютная погрешность решений
begin
  var oD:=new Decomp(a);
  var Msg:=s+': cond='+oD.cond+' обнаружена вырожденность матрицы';
  var Flag:=oD.cond+1=oD.cond;
  Assert(not Flag,Msg);
  if not Flag then begin
    oD.Solve(b);
    var sum:=b.Zip(roots,(p,q)->Abs(Abs(p)-Abs(q))).Sum;
    Msg:=s+': недопустимая погрешность '+sum+' > '+eps+NewLine+
        'Получено : '+b.JoinIntoString(' ')+NewLine+
        'Ожидалось: '+roots.JoinIntoString(' ');
    Assert(sum<=eps,Msg);
    end
end;

procedure TestFactors(s:string; a:array of integer; roots:array of integer);
// a - коэффициенты полинома;
// aroots - вектор эталонных решений;
begin
  var oL:=new Factors(a);
  var r:=oL.Factorize;
  var r1:=r.Rows.SelectMany(x->x);
  var Msg:=s+': ошибка.'+Newline+r1.JoinIntoString+': получено'+NewLine+
      roots.JoinIntoString+': ожидалось';
  if r1.Count=roots.Count then begin
    var s1:=r1.Zip(roots,(i,j)->i-j).Sum;
    Assert(s1=0,Msg)
    end
  else
    Assert(false,Msg)
end;

{$region FMinTest}
procedure TestFMin(s:string; f:real->real; a,b,etx,ety,epsx,epsy:real);
// etx - ожидаемое значение аргумента
// etx - ожидаемое значение функции
// epsx - допустимая абсолютная погрешность по аргументу
// epsy - допустимая абсолютная погрешность по функции
begin
  var oL:=new FMin(f,a,b);
  var (x,y):=(oL.x,oL.Value);
  var Msg:=s+': найден аргумент: '+x+', ожидался '+etx;
  Assert(Abs(x-etx)<=epsx,Msg);
  Msg:=s+': значение функции: '+y+', ожидалось '+ety;
  Assert(Abs(y-ety)<=epsy,Msg);
end;

procedure TestFMinN1(s:string; f:function(x:array of real):real;
    xb:array of real; etx:array of real; ety,epsx,epsy:real);
// xb - массив аргументов
// etx - массив ожидаемых значений аргументов
// etx - ожидаемое значение функции
// epsx - допустимая абсолютная погрешность по аргументу
// epsy - допустимая абсолютная погрешность по функции
begin
  var oL:=new FMinN(xb,f);
  var x:=oL.HJ;
  var y:=f(x);
  var Msg:=s+':'+NewLine+'найдены аргументы  : '+
      x.Select(t->Format('{0}',t)).JoinIntoString+NewLine+
      'ожидались аргументы: '+
      etx.Select(t->Format('{0}',t)).JoinIntoString;
  for var i:=0 to x.Length-1 do
    if Abs(x[i]-etx[i])>epsx then begin
      Assert(false,Msg);
      break
    end;
  Msg:=s+': значение функции: '+y+', ожидалось '+ety;
  Assert(Abs(y-ety)<=epsy,Msg);
end;

procedure TestFMinN2(s:string; f:function(x:array of real):real;
    a,b:array of real; k,m:integer; etx:array of real; ety,epsx,epsy:real);
// a,b - массивы нижних и верхних границ аргументов
// k - количество случайных точек в BPHS
// m - количество вызовов MKSearch
// etx - массив ожидаемых значений аргументов
// etx - ожидаемое значение функции
// epsx - допустимая абсолютная погрешность по аргументу
// epsy - допустимая абсолютная погрешность по функции
begin
  var oL:=new FMinN(etx,f);
  var y:real;
  oL.BPHS(a,b,y,k,m);
  var Msg:=s+':'+NewLine+'найдены аргументы  : '+
      oL.x.Select(t->Format('{0}',t)).JoinIntoString+NewLine+
      'ожидались аргументы: '+
      etx.Select(t->Format('{0}',t)).JoinIntoString;
  for var i:=0 to oL.x.Length-1 do
    if Abs(oL.x[i]-etx[i])>epsx then begin
      Assert(false,Msg);
      break
    end;
  Msg:=s+': значение функции: '+y+', ожидалось '+ety;
  Assert(Abs(y-ety)<=epsy,Msg);
end;

procedure TestFMinN3(s:string; f:function(x:array of real):real;
    a,b:array of real; k,m:integer; etx:array of real; ety,epsx,epsy:real);
// a,b - массивы нижних и верхних границ аргументов
// k - количество случайных точек в BPHS
// m - количество вызовов MKSearch
// etx - массив ожидаемых значений аргументов
// etx - ожидаемое значение функции
// epsx - допустимая абсолютная погрешность по аргументу
// epsy - допустимая абсолютная погрешность по функции
begin
  var oL:=new FMinN(etx,f);
  var y:real;
  var r:=oL.BestP(a,b,epsx,10,k,m);
  var x:array of real;
  foreach var t in r do begin
    (y,x):=(t[0],t[1]);
    var Msg:=s+':'+NewLine+'найдены аргументы  : '+
        x.Select(t->Format('{0}',t)).JoinIntoString+NewLine+
        'ожидались аргументы: '+
        etx.Select(t->Format('{0}',t)).JoinIntoString;
      for var i:=0 to x.Length-1 do
      if Abs(x[i]-etx[i])>epsx then begin
        Assert(false,Msg);
        break
        end;
    Msg:=s+': значение функции: '+y+', ожидалось '+ety;
    Assert(Abs(y-ety)<=epsy,Msg)
    end
end;

procedure TestFMin4(s:string; f:function(x:array of real):real;
    x:array of real; R:real; var t:real; v:array of real; eps,epsf:real);
// f - функция
// x - вектор координат
// R - максимально допустимая неопределенность
// t - полученный радиус сферы неопределенности
// v - вектор эталонного решения
// eps - оценочная точность решения по координатам
// epsf - оценочная точность по функции
begin
  var oL:=new FMinN(x,f);
  oL.ARS(R,t);
  var Msg:=s+': Требуемая точность не достигнута';
  Assert(t<=R,Msg);
  Msg:=s+': не найдено решение, ожидалось ['+v.Skip(1).JoinIntoString(',')+
    '], получено ['+oL.x.JoinIntoString(',')+']';
  var p:=true;
  for var i:=0 to oL.x.Length-1 do p:=p and (Abs(oL.x[i])-Abs(v[i+1])<=eps);
  Assert(p,Msg);
  Msg:=s+': найден радиус сферы '+t+', ожидаемый '+v[0];
  Assert(Abs(t-v[0])<=epsf,Msg)
end;
{$endregion}

procedure TestFraction(s:string; res,ans:Fraction);
// результат сравнения res=ans
begin
  Assert(res=ans,s+': получено '+res.ToString+', ожидалось '+ans.ToString)
end;

{$region MatrixTest}
procedure TestMatrixS(s:string; a,r,eps:real);
// a - найденное значение;
// r - ожидаемое значение;
// eps - максимальная допустимая абсолютная погрешность
begin
  var Msg:=s+': погрешность выше допустимой.'+Newline+'Получено значение '+
      a+', ожидалось значение '+r;
  Assert(Abs(a-r)<=eps,Msg)
end;

procedure TestMatrixV(s:string; a,r:Vector; eps:real);
// a - вектор найденных значений;
// r - вектор ожидаемых значений;
// eps - максимальная допустимая абсолютная погрешность
begin
  for var i:=0 to a.Length-1 do
    Assert(Abs(a.Value[i]-r.Value[i])<=eps,
        s+': погрешность выше допустимой.'+Newline+'Получено значение '+
        a.Value[i]+', ожидалось значение '+r.Value[i])
end;

procedure TestMatrixM(s:string; a,r:Matrix; eps:real);
// a - вычисленная матрица;
// r - эталонная матрица;
// eps - максимальная допустимая абсолютная погрешность
begin
  for var i:=0 to a.RowCount-1 do
    for var j:=0 to a.ColCount-1 do
      Assert(Abs(a.Value[i,j]-r.Value[i,j])<=eps,
          s+': погрешность выше допустимой.'+Newline+'Получено значение a['+
          i+','+j+']='+a.Value[i,j]+', ожидалось значение '+r.Value[i,j])
end;
{$endregion}

procedure TestPolRt(s:string; a:Polynom; roots:array of complex; eps:real);
// roots -массив ожидаемых значений корней
// eps - максимальная абсолютная погрешность решений
begin
  var oP:=new PolRt(a);
  if oP.ier>0 then begin
    var Msg:=s+': тест не прошел, ошибка с кодом '+oP.ier;
    Assert(false,Msg)
    end
  else begin
    var r:=oP.Value;
    for var i:=0 to r.Length-1 do begin
      var Msg:=s+': найдено: ('+r[i].Real+','+r[i].Imaginary+'), ожидалось ('+
        roots[i].Real+','+roots[i].Imaginary+')';
      Assert(Complex.Abs(r[i]-roots[i])<=eps,Msg);
      end
    end  
end;

procedure TestPolynomD(s:string; res,lim:real; n1,n2:integer);
// res - вычисленное значение
// lim - предельно допустимая величина res
// проверяется также условие n2<n1
begin
  Assert(n1>n2,s+': экономизации не произошло');
  Assert(res<=lim,s+': отклонение '+res+' превышает '+lim);
end;

procedure TestPolynomV(s:string; p:Polynom; x,r,eps:real);
// х - аргумент, для которого вычисляется полином
// r - ожидаемое значение
// eps - максимальная абсолютная погрешность решения
begin
  var a:=p.Value(x);
  var Msg:=s+': найдено: '+a+', ожидалось '+r;
  Assert(Abs(a-r)<=eps,Msg)
end;

procedure TestQuanc8(s:string; a,b:real; F:real->real; ae,re,r2,eps:real);
// cres - аналитически найденное значение интеграла
// eps - допустимая абсолютная погрешность решения
begin
  var Msg:string;
  var oQ:=new Quanc8(f,a,b,ae,re);
  var r1:=oQ.Value;
  if r1[2]=0 then begin
    Msg:=s+': вычислено '+r1[0]+', ожидалось '+r2;
    Assert(Abs(r1[0]-r2)<=eps,Msg)
    end
  else begin
    Msg:=s+': вычислено '+r1[0]+', ожидалось '+r2+', errest='+r1[1]+', flag='+r1[2];
    Assert(Abs(r1[0]-r2)<=eps,Msg)
    end
end;

procedure TestRKF45(s:string; res,ans,eps:real);
begin
  Assert(Abs(res-ans)<=eps,s+': контрольная сумма '+res+', ожидалась '+ans)
end;

procedure TestRootsIsolation(s:string; f:real->real; a,b,h:real;
    t:array of real);
// f - функция
// a,b - границы интервала поиска
// h - шаг поиска
// t - эталонный массив нулей функции
begin
  var oRI:=new RootsIsolation(f,a,b,h);
  var r:=oRI.Value;
  for var i:=0 to t.Length-1 do begin
    var Msg:=s+': точка нуля '+t[i]+' не в интервале ['+r[i][0]+';'+r[i][1]+']';
    Assert(t[i].Between(r[i][0],r[i][1]),Msg)
    end;
end;

procedure TestSpline(st:string; x:real; F:real->real; eps:real; S:Spline);
// eps - относительная погрешность в процентах
begin
  var r1:=F(x);
  var r2:=S.Value(x);
  var Msg:=st+': F('+x+')='+r1+', получено '+r2;
  Assert(Abs((r1-r2)/r1)<=eps/100,Msg);
end;

procedure TestSvenn(s:string; f:real->real; x0,t,a,b:real);
// f - функция
// x0 - начальная точка поиска
// t - шаг поиска
// a,b - ожидаемый интервал (результат должен ему принадлежать)
begin
  var oS:=new Svenn(f,x0,t);
  var r:=oS.Value;
  var Msg:=s+': не найдено решение, ожидалось ['+a+';'+b+']';
  Assert(r[2]=0,Msg);
  Msg:=s+': интервал ['+a+';'+b+'] не входит в ['+r[0]+';'+r[1]+']';
  Assert(a.Between(r[0],r[1]) and b.Between(r[0],r[1]),Msg)
end;

{$region VectorTest}
procedure TestVector1(s:string; a,r,eps:real);
// a - найденное значение;
// r - ожидаемое значение;
// eps - максимальная допустимая абсолютная погрешность
begin
  var Msg:=s+': погрешность выше допустимой.'+Newline+'Получено значение '+
      a+', ожидалось значение '+r;
  Assert(Abs(a-r)<=eps,Msg)
end;

procedure TestVectorN(s:string; a,r:Vector; eps:real);
// a - вектор найденных значений;
// r - вектор ожидаемых значений;
// eps - максимальная допустимая абсолютная погрешность
begin
  for var i:=0 to a.Length-1 do
    Assert(Abs(a.Value[i]-r.Value[i])<=eps,
        s+': погрешность выше допустимой.'+Newline+'Получено значение '+
        a.Value[i]+', ожидалось значение '+r.Value[i])
end;
{$endregion}

procedure TestZeroin(s:string; a,b:real; F:real->real; root,eps:real);
// root - точное значение корня
// eps - абсолютная погрешность значения корня
begin
  var oZ:=new Zeroin(F,eps);
  var x:=oZ.Value(a,b);
  var Msg:=s+': корень: '+x+', ожидалось '+root;
  Assert(Abs(x-root)<=eps,Msg);
end;

begin
  var nt:=1;
  Writeln('*** ',&NumLibABCVersion,' ***');
  Writeln('     *** Тестирование начато ***');
  
  {$region ApproxCheb}
  begin
    var e:=0.1;
    var x:=ArrGen(12,i->0.25*i-2);
    var y:=x.Select(z->2*z-5*Sqr(z)+8*z*Sqr(z)).ToArray;
    var oC:=new ApproxCheb(x,y,e);
    oC.MakeCoef;
    TestApproxCheb('AppoxCheb 1',oC.c,Arr(0.0,2.0,-5.0,8.0),1e-12);
    
    e:=0.1;
    x:=ArrGen(10,i->i-3.0);
    y:=x.Select(z->4+z*(-2.5+z*(1.752+z*(-9+z*0.28)))).ToArray;
    oC:=new ApproxCheb(x,y,e);
    oC.MakeCoef;
    TestApproxCheb('AppoxCheb 2',oC.c,Arr(4.0,-2.5,1.752,-9.0,0.28),1e-12);
    
    e:=0.5;
    x:=ArrGen(7,i->0.2*i-0.3);
    y:=x.Select(z->3*sin(z)+5.6*Ln(Abs(z))).ToArray;
    oC:=new ApproxCheb(x,y,e);
    TestApproxCheb('AppoxCheb 3',oC.f,y,0.8);
    
    Writeln(nt:2,'. Проверка класса ApproxCheb завершена');
    nt+=1;
  end;
  {$endregion}  
    
  {$region Decomp}
  begin
    // из первоисточника
    var a:=new real[3,3] (
        (10.0,-7.0,0.0),
        (-3.0,2.0,6.0),
        (5.0,-1.0,5.0));
    var b:=Arr(7.0,4.0,6.0);    
    var r:=Arr(0.0,-1.0,1.0);
    TestDecomp('Decomp/Solve 1',a,b,r,1e-15);
    
    // Фаддеев Д.К, Фаддеева В.Н. "Вычислительные методы линейной алгебры"
    // Точное регение получено при помощи пакета Maple15
    a:=new real[4,4] (
        (1.0,0.17,-0.25,0.54),
        (0.47,1.0,0.67,-0.32),
        (-0.11,0.35,1.0,-0.74),
        (0.55,0.43,0.36,1.0));
    b:=Arr(0.3,0.5,0.7,0.9);
    r:=Arr(7039205/15965951,-5796135/15965951,18629045/15965951,6283675/15965951);
    TestDecomp('Decomp/Solve 2',a,b,r,1e-15);
    
    // Свидетельство к алгоритму 135б. В кн. Агеев М.И. и др.
    // "Библиотека алгоритмов 101б-150б"
    var aa:=new real[3,3] (
        (4.0,2.0,2.0),
        (2.0,2.0,2.0),
        (2.0,2.0,3.0));
    a:=Copy(aa); //для теста
    //a:=MatrGen(3,3,(i,j)->aa[3*i+j]);
    b:=Arr(2.0,3.0,4.0);
    r:=Arr(-0.5,1.0,1.0);
    TestDecomp('Decomp/Solve 3-1',a,b,r,1e-15);
    a:=Copy(aa);
    b:=Arr(-1.0,1.0,2.0);
    r:=Arr(-1.0,0.5,1.0);
    TestDecomp('Decomp/Solve 3-2',a,b,r,1e-15);
    a:=Copy(aa);
    b:=Arr(3.0,2.0,3.0);
    r:=Arr(0.5,-0.5,1.0);
    TestDecomp('Decomp/Solve 3-3',a,b,r,1e-15);
    
    Writeln(nt:2,'. Проверка класса Decomp/Solve завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Factors}
  begin
    var a:=Arr(-20,7,73,-42);
    TestFactors('Factors 1',a,Arr(3,-1,2,-1,3,5,7,4));
    
    a:=Arr(45,-12,-52,-1,6);
    TestFactors('Factors 2',a,Arr(2,1,1,3,2,-5));
    
    a:=Arr(6,-13,9,-2);
    TestFactors('Factors 3',a,Arr(3,-1,1,1,2,3,1,2));
    
    a:=Arr(40,10,2);
    TestFactors('Factors 4',a,Arr(0,1));
    
    a:=Arr(-40,78,-29,3);
    TestFactors('Factors 5',a,Arr(3,1,1,4,1,5,3,2));
    
    Writeln(nt:2,'. Проверка класса Factors завершена');
    nt+=1
  end;
  {$endregion}
    
  {$region FMin}
  begin
    var f:real->real:=x->x*(x*x-2)-5;
    TestFMin('FMin 1',f,0,1,Sqrt(2/3),-(Sqrt(32/27)+5),1e-8,1e-18);
    
    TestFMin('FMin 2',f,-4,4,Sqrt(2/3),-(Sqrt(32/27)+5),1e-7,1e-18);
    
    f:=x->x*Sqr(x-1)*(x-3)*Sqr(x-3);
    var etx:=(13-Sqrt(97))/12;
    var ety:=-(232229+4171*Sqrt(97))/93312;
    TestFMin('FMin 3',f,-5,1.5,etx,ety,1e-7,1e-17);
    
    etx:=(13+Sqrt(97))/12;
    ety:=(-232229+4171*Sqrt(97))/93312;
    TestFMin('FMin 4',f,0.5,3.1,etx,ety,1e-7,1e-15);
    
    f:=x->x=0?1e15:(x+2)*Exp(1/x);
    TestFMin('FMin 5',f,-1.5,4,2.0,4*Exp(0.5),1e-7,1e-17);
    
    Writeln(nt:2,'. Проверка класса FMin завершена');
    nt+=1
  end;
  {$endregion}

  {$region FMinN}
  begin
    var Rosenbrock:function(x:array of real):real:=
        x->100*Sqr(x[1]-Sqr(x[0]))+Sqr(1-x[0]);
    var xb:=Arr(-1.2,1.0);
    var xet:=Arr(1.0,1.0);
    TestFminN1('FMinN 1: HJ, функция Розенброка',
        Rosenbrock,xb,xet,0,1e-5,1e-8);
        
    var Woods:function(x:array of real):real:=x->
      begin
        var s1:=x[1]-Sqr(x[0]);
        var s2:=1-x[0];
        var s3:=x[1]-1;
        var t1:=x[3]-Sqr(x[2]);
        var t2:=1-x[2];
        var t3:=x[3]-1;
        var t4:=s3+t3;
        var t5:=s3-t3;
        Result:=100*Sqr(s1)+Sqr(s2)+90*Sqr(t1)+Sqr(t2)+10*Sqr(t4)+0.1*Sqr(t5)
      end;
    xb:=Arr(-3.0,-1.0,-3.0,-1.0);
    xet:=Arr(1.0,1.0,1.0,1.0);
    TestFminN1('FMinN 2: HJ, функция с 4 аргументами',
        Woods,xb,xet,0,1e-4,1e-8);
    
    var f1:function(x:array of real):real:=
        x->x[0]*(Sqr(x[0])-2)-5;
    xb:=Arr(0.0);
    xet:=Arr(Sqrt(2/3));
    TestFminN1('FMinN 3: HJ, функция x^3-2x-5',
        f1,xb,xet,-Sqrt(32/27)-5,1e-4,1e-8);    
    
    var FSimplex:function(x:array of real):real:=x->
      begin
        var x1:=x[0];
        var x2:=x[1];
        var s:=0.0;
        if x1+x2>8 then s:=real.MaxValue
        else if -2*x1+3*x2>9 then s:=real.MaxValue
        else if 2*x1-x2>10 then s:=real.MaxValue
        else if x1<0 then s:=real.MaxValue
        else if x2<0 then s:=real.MaxValue;
        Result:=-4*x1-3*x2+1+s
      end;
    xb:=Arr(0.0,0.0);
    xet:=Arr(6.0,2.0);
    var oL:=new FMinN(xb,FSimplex);
    var a:=Arr(0.0,0.0);
    var b:=Arr(8.0,8.0);
    var y:real;
    oL.MKSearch(a,b,y);
    xb:=oL.x.Select(t->real(Round(t))).ToArray;
    TestFminN1('FMinN 4: MKSearch+HJ, целочисленная функция со штрафом',
        FSimplex,xb,xet,-29,1e-4,1e-8);
        
    a:=Arr(-1.0,-1.0);
    b:=Arr(2.0,2.0);
    xb:=new real[a.Length];
    xet:=Arr(1.0,1.0);
    TestFminN2('FMinN 5: BPHS, функция Розенброка',
        Rosenbrock,a,b,100,1000,xet,0,1e-3,1e-5);
        
    a:=Arr(-1.0,-1.0,-1.0,-1.0);
    b:=Arr(2.0,2.0,2.0,2.0);
    xet:=Arr(1.0,1.0,1.0,1.0);
    TestFminN2('FMinN 6: BPHS, функция с 4 аргументами',
        Woods,a,b,100,5000,xet,0,1e-1,1e-2);
        
    a:=Arr(0.0,0.0);
    b:=Arr(8.0,8.0);
    xet:=Arr(6.0,2.0);
    TestFminN2('FMinN 7: BestP, целочисленная функция со штрафом',
        FSimplex,a,b,100,1000,xet,-29,1e-2,1e-6);
        
    f1:=x->4*Sqr(x[0]-5)+Sqr(x[1]-6);
    xb:=Arr(-8.0,9.0);
    var (t,R):=(1.0,1e-6);
    var v:=Arr(f1(Arr(5.0,6.0)),5.0,6.0);
    TestFMin4('FMinN 8: ARS',f1,xb,R,t,v,2*R,10*R);
    
    f1:=x->2*Sqr(x[0])+x[0]*x[1]+Sqr(x[1]);
    xb:=Arr(-10.0,10.0);
    (t,R):=(1.0,1e-10);
    v:=Arr(f1(Arr(0.0,0.0)),0.0,0.0);
    TestFMin4('FMinN 9: ARS',f1,xb,R,t,v,2*R,10*R);    
    
    Writeln(nt:2,'. Проверка класса FMinN завершена');
    nt+=1
  end;
  {$endregion}
    
  {$region Fraction}
  begin
    var r:=((Frc(5,5,9)-Frc(7,18))/35+(Frc(40,63)-Frc(8,21))/20+
        (Frc(83,90)-Frc(41,50))/2)*50;
    TestFraction('Fraction 1',r,Frc(74,7));
    
    r:=Frc(34,197)+Frc(6,9,91)-Frc(351,95113)*Frc(1,7);
    TestFraction('Fraction 2',r,Frc(10692560566,1705090751));
    
    var m:=Range(1,30,2).Aggregate(BigInteger(1),(i,j)->i*j);
    var n:=Range(2,30,2).Aggregate(BigInteger(1),(i,j)->i*j)+1;
    var a:=Frc(m,n);
    var b:=Frc(6190283353629375,42849873690624001); 
    TestFraction('Fraction 3',a,b);
    
    Writeln(nt:2,'. Проверка класса Fraction завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Matrix}
  begin
    var a:=new Matrix(3,4,-2,4,0,3,6,11,-5,7,0,8,-4,1);
    a.SetRow(new Vector(a.Row(2).Value.Select(x->x-2).ToArray),2);
    a.MultCol(1,3);
    var vb:=new Vector(1,-1,0,2);
    a.InsertRowBefore(vb,3);
    a.SwapRows(2,3);
    a.SwapCols(1,3);
    a:=a.Transpose;
    var Atr:=a.Inv;
    var d:=254.0;
    var x:=new Matrix(4,4,173/d,83/d,27/d,28/d,-426/d,-222/d,-112/d,72/d,
        122/d,60/d,44/d,8/d,-277/d,-105/d,-77/d,-14/d);
    TestMatrixM('Matrix 1',Atr,x,1e-14);
  
    a:=new Matrix(2,4,-3,0,4,-1,2,-7,5,6);
    var b:=new Matrix(2,3,8,1,-5,6,7,2);
    var c:=new Matrix(3,4,1,-1,7,0,3,2,9,4,5,0,-2,-4);
    var r:=(((a-b*c).Transpose)*a).Det;
    TestMatrixS('Matrix 2',r,0.0,1e-15);
    
    a:=new Matrix(3,3,2,3,-1,1,-2,1,1,0,2);
    vb:=new Vector(9,3,2);
    var vr:=a.Inv*vb;
    TestMatrixV('Matrix 3',vr,new Vector(4,0,-1),1e-15);
    
    vr:=new Vector(vb.Length);
    var det:=a.Det;
    for var i:=0 to vb.Length-1 do begin
      var t:=a.Copy;
      t.SetCol(vb,i,0);
      var detx:=t.Det;
      vr.Value[i]:=detx/det;
    end;
    TestMatrixV('Matrix 4',vr,new Vector(4,0,-1),1e-15);
    
    var cond:real;
    vr:=A.SLAU(vb,cond);
    TestMatrixV('Matrix 5.1',vr,new Vector(4,0,-1),1e-15);
    TestMatrixS('Matrix 5.2',cond,1.97935318837932,1e-14);

    Writeln(nt:2,'. Проверка класса Matrix завершена');
    nt+=1
  end;  {$endregion}
  
  {$region Polrt}
  begin
    var p:=new Polynom(-120,34,-4,-1,1);
    var cr:=Arr(cplx(3,0),cplx(-4,0),cplx(1,-3),cplx(1,3));
    TestPolrt('Polrt 1',p,cr,1e-15);
    
    p:=new Polynom(6,-5,-2,1);
    cr:=Arr(cplx(1,0),cplx(-2,0),cplx(3,0));
    TestPolrt('Polrt 2',p,cr,1e-15);
    
    p:=new Polynom(-120,274,-225,85,-15,1);
    cr:=Arr(cplx(1,0),cplx(2,0),cplx(3,0),cplx(4,0),cplx(5,0));
    TestPolrt('Polrt 3',p,cr,1e-12);
    
    p:=new Polynom(8,-6,6,1,2,-2,1);
    cr:=ArrFill(6,cplx(-1,-1));
    cr[0]:=cplx(0.5,-Sqrt(3)/2); cr[1]:=Conjugate(cr[0]);
    cr[2]:=cplx(1.5,-Sqrt(7)/2); cr[3]:=Conjugate(cr[2]);
    cr[5]:=Conjugate(cr[4]);
    TestPolrt('Polrt 4',p,cr,1e-15);
    
    p:=new Polynom(-36,0,49,0,-14,0,1);
    cr:=Arr(cplx(-2,0),cplx(2,0),cplx(-3,0),cplx(3,0),cplx(-1,0),cplx(1,0));
    TestPolrt('Polrt 5',p,cr,1e-15);
    
    p:=new Polynom(-16,0,0,0,1);
    cr:=Arr(cplx(0,2),cplx(0,-2),cplx(-2,0),cplx(2,0));
    TestPolrt('Polrt 6',p,cr,1e-15);
    
    p:=new Polynom(-250,125,45,-32,4);
    cr:=Arr(cplx(2.5,0),cplx(5,0),cplx(-2,0),cplx(2.5,0));
    TestPolrt('Polrt 7',p,cr,1e-8);
    
    p:=new Polynom(-1,0,0,0,0,1);
    cr:=ArrFill(5,cplx(1,0));  // y=x^5-1
    cr[1]:=cplx(-(Sqrt(5)+1)/4,-Sqrt(10-2*Sqrt(5))/4); cr[2]:=Conjugate(cr[1]);
    cr[3]:=cplx((Sqrt(5)-1)/4,-Sqrt(10+2*Sqrt(5))/4); cr[4]:=Conjugate(cr[3]);
    TestPolrt('Polrt 8',p,cr,1e-15);
    
    Writeln(nt:2,'. Проверка класса PolRt завершена');
    nt+=1;
  end;
  {$endregion}
    
  {$region Polynom}
  begin
    var p:=new Polynom(2,6,8,3,1);  // запись 13862(10)
    TestPolynomV('Polynom 1',p,10,13862,1e-15);
    
    p:=new Polynom(0,3,8,6,4,2,5); // запись 0.386425
    TestPolynomV('Polynom 2',p,0.1,0.386425,1e-15);
    
    p:=new Polynom(1,0,1,1,0,1,1,0,1); // 365(10)=101101101(2)
    TestPolynomV('Polynom 3',p,2,365,1e-15);
    
    p:=new Polynom(1,1,1/2,1/6,1/24,1/120,1/720,1/5040,1/40320); // exp(x)
    TestPolynomV('Polynom 4',p,0.36,exp(0.36),1e-9);
    
    // пример -1435+(25*12+917) = -218
    var a:=new Polynom(5,3,4,1);
    var b:=new Polynom(5,2);
    var c:=new Polynom(2,1);
    var d:=new Polynom(7,1,9);
    TestPolynomV('Polynom 5',-a+(b*c+d),10,-218,1e-15);
    
    p:=new Polynom(6,-2,0,-5,0,2,3);
    a:=new Polynom(-1,3,-2,4);
    (b,c):=p/a;
    var x:=pi;
    var x1:=b.Value(x)+c.Value(x)/a.Value(x);
    var x2:=p.Value(x)/a.Value(x);
    var Msg:='Polynom 6: найдено: '+x1+', ожидалось '+x2;
    Assert(Abs(x1-x2)<=1e-15,Msg);
    
    var k:=ArrFill(20,0.0);
    var i:=3;
    k[1]:=2;
    var pr:=1.0;
    while i<=19 do begin
      pr:=-pr*i*(i-1);
      k[i]:=1/pr;
      i:=i+2
      end;
    p:=new Polynom(k);
    var p2:=p.EconomSym(1,1e-10);
    var dm:=-1.0;;
    for var j:=0 to 20 do begin
      x:=-1.0+j/10;
      var y:=p.Value(x);
      var z:=p2.Value(x);
      var d1:=Abs(y-z);
      if dm<d1 then dm:=d1
      end;
    TestPolynomD('Polynom 7',dm,1e-10,p.n,p2.n);
  
    p2:=p.EconomUnsym(1,1e-10);
    dm:=-1.0;;
    for var j:=0 to 10 do begin
      x:=j/10;
      var y:=p.Value(x);
      var z:=p2.Value(x);
      var d1:=Abs(y-z);
      if dm<d1 then dm:=d1;
      end;
    TestPolynomD('Polynom 8',dm,1e-10,p.n,p2.n); 
    
    Writeln(nt:2,'. Проверка класса Polynom завершена');
    nt+=1
  end;
  {$endregion}
    
  {$region Quanc8}
  begin
    var (a,b,relerr,abserr):=(0.0,2.0,1e-12,0.0);
    var f:real->real:=x->x=0?1.0:Sin(x)/x; // интегральный синус
    var s:real; 
    begin  
      s:=2.0;
      var (p2,f1,sgn,i):=(2.0,1.0,1,1);
      var tt:real;
      repeat
        p2*=4; f1*=2*i*(2*i+1); sgn:=-sgn;
        tt:=sgn*p2/f1/(2*i+1);
        s+=tt;
        i+=1;
      until Abs(tt)<relerr;
      end;
    TestQuanc8('Quanc8 1',a,b,f,abserr,relerr,s,1e-12);
    
    f:=x->4/(1+x*x);
    TestQuanc8('Quanc8 2',0,1,f,abserr,relerr,pi,1e-13);
    
    s:=6+20*Sqrt(10);
    TestQuanc8('Quanc8 3',-9,1000,x->1/Sqrt(Abs(x)),abserr,relerr,s,1e-3);
    
    f:=x->x/Sqrt(Sqr(Sqr(x))+16);
    s:=ln(2)/2;
    TestQuanc8('Quanc8 4',0,Sqrt(3),f,abserr,relerr,s,1e-12);
    
    f:=x->ArcCos(2*x);
    s:=pi/2;
    TestQuanc8('Quanc8 5',-0.5,0.5,f,abserr,relerr,s,1e-15);
    
    Writeln(nt:2,'. Проверка класса Quanc8 завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region RKF45}
  begin
    // лямбда-процедура чтобы не выходить за пределы блока  
    var p1:procedure(t:real; y,yp:array of real):=(t,y,yp)->
    begin
      var alpha:=Sqr(ArcTan(1.0));
      var r:=y[0]*y[0]+y[1]*y[1]; r:=r*Sqrt(r)/alpha;
      yp[0]:=y[2]; yp[1]:=y[3]; yp[2]:=-y[0]/r; yp[3]:=-y[1]/r
    end;
    // конец процедуры
    var e:=0.25;
    var y:=Arr(1.0-e,0.0,0.0,ArcTan(1)*Sqrt((1.0+e)/(1.0-e)));
    var (abserr,relerr):=(0.0,0.3e-6);
    var oL:=new RKF45(p1,y,abserr,relerr);
    var (t,tb,th):=(0.0,12.0,0.5);
    var t_out:=t;
    var ss:=0.0;
    repeat
      oL.Solve(t,t_out);
      ss+=t+oL.y[0]+y[1];
      case oL.flag of
      -3,-2,-1,1,8: break;
      2:t_out:=t+th;
        end
    until t>=tb;
    TestRKF45('RKF45 1',ss,140.749980780164,1e-12);
    
    p1:=(t,y,yp)->begin yp[0]:=y[0]/4*(1-y[0]/20) end;
    //
    (abserr,relerr):=(0.0,1e-6);
    (t,tb,th):=(0.0,20.0,5.0); 
    y:=Arr(1.0);
    t_out:=t;
    oL:=new RKF45(p1,y,abserr,relerr);
    ss:=0.0;
    repeat
      OL.Solve(t,t_out);
      ss+=t+oL.y[0]+20/(1+19*Exp(-0.25*t));
      case oL.flag of
      -3,-2,-1,1,8: break;
      2:t_out:=t+th
        end
    until t>=tb;
    TestRKF45('RKF45 2',ss,136.941910731927,1e-12);
    
    (abserr,relerr):=(0.0,1e-6);
    (t,tb,t_out):=(0.0,0.0,0.0);
    var (te,ns):=(20.0,4); 
    y:=Arr(1.0);
    oL:=new RKF45(p1,y,abserr,relerr);
    oL.flag:=-1;
    p1(t,y,oL.yp);
    for var i:=1 to ns do begin
      t:=((ns-i+1)*tb+(i-1)*te)/ns;
      t_out:=((ns-i)*tb+i*te)/ns;
      while oL.flag<0 do begin
        oL.Solve(t,t_out);
        ss+=t+oL.y[0]+20/(1+19*Exp(-0.25*t));
        case oL.flag of
          -3,-1,1,8: break;
          end
        end;
      oL.flag:=-2
    end;
    TestRKF45('RKF45 3',ss,603.231676788451,1e-12);
    
    Writeln(nt:2,'. Проверка класса RKF45 завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region RootsIsolation}
  begin
    var f:real->real:=x->(x+4.5)*(x+3)*(x-2)*(x-3.8);
    var (a,b,h):=(-10.0,8.0,0.5);
    TestRootsIsolation('RootsIsolation 1',f,a,b,h,Arr(-4.5,-3.0,2.0,3.8));
    
    f:=t->sin(t)/(1+Sqr(Exp(t)))-0.1;
    (a,b,h):=(-10,5,0.3);
    var r:=Arr(-9.52495,-6.18307,-3.24191,0.27789,1.00272); // найдено заранее
    TestRootsIsolation('RootsIsolation 2',f,a,b,h,r);
    
    Writeln(nt:2,'. Проверка класса RootsIsolation завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Spline}
  begin
    var f:real->real:=x->x*x*x;
    var pp:=Partition(1.0,10.0,9).Select(x->new Point(x,f(x))).ToArray;
    var Sp:=new Spline(pp); // создаем сплайн с заданными узлами интерполяции.
    TestSpline('Spline1-1',1,f,1e-15,Sp); // левая точка
    TestSpline('Spline1-2',1.25,f,1e-15,Sp); // внутри
    TestSpline('Spline1-3',2.5,f,1e-15,Sp); // внутри
    TestSpline('Spline1-4',7.2,f,1e-15,Sp); // внутри
    TestSpline('Spline1-5',10,f,1e-5,Sp); // правая точка
    
    f:=x->Power(x,4);
    pp:=Partition(1.0,10.0,9).Select(x->new Point(x,f(x))).ToArray;
    Sp:=new Spline(pp);
    TestSpline('Spline2-1',1.28,f,32,Sp); // 32% у края... надо исходный шаг мельче
    TestSpline('Spline2-2',2.5,f,0.8,Sp); // 0.8%
    TestSpline('Spline2-3',5.1,f,0.005,Sp); // 0.005%
    TestSpline('Spline2-4',9.7,f,0.01,Sp); // 0.01%
    
    pp:=Partition(1.0,10.0,36).Select(x->new Point(x,f(x))).ToArray;
    Sp:=new Spline(pp);
    TestSpline('Spline3-1',1.28,f,0.03,Sp); // 0.03%
    TestSpline('Spline3-2',1.1,f,0.24,Sp); // 0.24%
    TestSpline('Spline3-3',1.03,f,0.18,Sp); // 0.18%
    
    f:=x->(3*x-8)/(8*x-4.1);
    pp:=Partition(1.0,10.0,18).Select(x->new Point(x,f(x))).ToArray;
    Sp:=new Spline(pp);
    TestSpline('Spline4-1',1.1,f,4.8,Sp); // 4.8%
    TestSpline('Spline4-2',2.6,f,5.8,Sp); // 5.8%
    TestSpline('Spline4-3',5.9,f,0.001,Sp); // <0.001%
    TestSpline('Spline4-4',9.9,f,0.001,Sp); // <0.001% 
    
    Writeln(nt:2,'. Проверка класса Spline завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Svenn}
  begin
    var f:real->real:=x->Sqr(x-5);
    TestSvenn('Svenn 1',f,0,1,5-1e-10,5+1e-10);
    
    f:=x->Abs((x-4)*(x+7));
    TestSvenn('Svenn 2',f,0,2,4-1e-10,4+1e-10);
    
    TestSvenn('Svenn 3',f,-3,1,-7-1e-10,-7+1e-10);
    
    f:=x->x*(x*x-2)-5;
    TestSvenn('Svenn 4',f,0,1,2.0945514814,2.0945514816);
    
    Writeln(nt:2,'. Проверка класса Svenn завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Vector}
  begin
    var a:=new Vector(3,-4,1);
    var b:=new Vector(-1,0,5);
    var r:=(2*a-b).ModV;
    TestVector1('Vector 1',r,Sqrt(122),1e-15);
    
    a:=new Vector(Arr(3.0,0.0,-4.0));
    var vr:=a.Ort;
    TestVectorN('Vector 2',vr,new Vector(0.6,0,-0.8),1e-15);
    
    var pa:=Arr(2.0,-1.0,2.0);
    var pb:=Arr(1.0,2.0,-1.0);
    var pc:=Arr(3.0,2.0,1.0);
    var BC:=new Vector(pb,pb);
    var CA:=new Vector(pc,pa);
    var CB:=new Vector(pc,pb);
    vr:=(BC-2*CA).VP(CB);
    TestVectorN('Vector 3',vr,new Vector(-12,8,12),1e-15);
    
    a:=new Vector(2,-1,1);
    b:=new Vector(2,3,6);
    r:=a*b/(a.ModV*b.ModV);
    TestVector1('Vector 4',r,1/Sqrt(6),1e-15);
    
    pa:=Arr(7.0,3.0,4.0);
    pb:=Arr(1.0,0.0,6.0);
    pc:=Arr(4.0,5.0,-2.0);
    a:=new Vector(pa,pb);
    b:=new Vector(pA,pc);
    r:=a.VP(b).ModV/2;
    TestVector1('Vector 5',r,24.5,1e-15);
    
    Writeln(nt:2,'. Проверка класса Vector завершена');
    nt+=1
  end;
  {$endregion}
  
  {$region Zeroin}
  begin  
    var f:real->real:=x->x*(x*x-2)-5; // классика
    // Точное решeние по формуле Кардано 2.094551481542326591482386540579...
    var root:=(Power(5+Sqrt(643/27),1/3)+Power(5-Sqrt(643/27),1/3))/Power(2,1/3);
    TestZeroin('Zeroin 1',2,3,f,root,1e-15);
    
    f:=x->Power((12-2*x)/(x-1),1/3)+Power((x-1)/(12-2*x),1/3)-2.5;
    root:=2;
    TestZeroin('Zeroin 2',1.01,3.5,f,root,1e-15); // разрыв при х=1
    
    root:=97/17;
    TestZeroin('Zeroin 3',3,5.99,f,root,1e-15); // разрыв при х=6
    
    f:=x->3*Sin(x)+4*Cos(x)-5;
    root:=2*ArcTan(1/3);
    TestZeroin('Zeroin 4',-1,1,f,root,1e-8);
    
    Writeln(nt:2,'. Проверка класса Zeroin завершена');
    nt+=1
  end;  
  {$endregion}
  
  Writeln('*** Тестирование завершено ***');
  Writeln('*** Если в тесте класса FMinN были ошибки, повторите тест ***');
  
end.