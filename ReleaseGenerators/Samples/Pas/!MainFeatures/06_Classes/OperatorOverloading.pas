// Перегрузка операций
type 
  Frac = record
  private
    num,denom: integer;
  public  
    constructor (n,d: integer);
    begin
      num := n;
      denom := d;
    end;
    class function operator+(a,b: Frac): Frac; 
    begin
      Result := new Frac(a.num*b.denom+b.num*a.denom,a.denom*b.denom); 
    end; 
    class function operator-(a,b: Frac): Frac; 
    begin
      Result := new Frac(a.num*b.denom-b.num*a.denom,a.denom*b.denom); 
    end; 
    class function operator*(a,b: Frac): Frac; 
    begin
      Result := new Frac(a.num*b.num,a.denom*b.denom); 
    end;
    class function operator/(a,b: Frac): Frac; 
    begin
      Result := new Frac(a.num*b.denom,a.denom*b.num); 
    end;
    class function operator=(a,b: Frac): boolean; 
    begin
      Result := (a.num = b.num) and (a.denom = b.denom);
    end; 
    class function operator<>(a,b: Frac): boolean; 
    begin
      Result := not (a=b);
    end; 
    class function operator<(a,b: Frac): boolean; 
    begin
      Result := a.num/real(a.denom)<b.num/real(b.denom);
    end; 
    class function operator<=(a,b: Frac): boolean; 
    begin
      Result := a.num/real(a.denom)<=b.num/real(b.denom);
    end; 
    class function operator>(a,b: Frac): boolean; 
    begin
      Result := a.num/real(a.denom)>b.num/real(b.denom);
    end; 
    class function operator>=(a,b: Frac): boolean; 
    begin
      Result := a.num/real(a.denom)>=b.num/real(b.denom);
    end; 
    class procedure operator+=(var a: Frac; b: Frac); 
    begin
      a := a + b;
    end; 
    class function operator-(a: Frac): Frac; 
    begin
      Result := new Frac(-a.num,a.denom);
    end; 
    class function operator+(a: Frac): Frac; 
    begin
      Result := a;
    end; 
    function ToString: string; override;
    begin
      Result := Format('{0}/{1}',num,denom);
    end;
  end;

var 
  f := new Frac(1,2);
  f1 := new Frac(3,5);

begin
  writelnFormat('{0} + {1} = {2}',f,f1,f+f1);
  writelnFormat('{0} - {1} = {2}',f,f1,f-f1);
  writelnFormat('{0} * {1} = {2}',f,f1,f*f1);
  writelnFormat('{0} / {1} = {2}',f,f1,f/f1);
  writeln(f1=f);
  f += f1;
  writeln(-f);
end.