unit LibForHaskell;
interface
{$reference 'IronMath.dll'}
uses System,System.Collections,System.Collections.Generic,IronMath; 
  
type datatype=class
  public ob:object;
  public type_name:string;
  
  public type_list_name:string; 
  public first:datatype; 
  public second:datatype;
  public last:datatype;
  public list:ArrayList;
  public delta:object;
  
  public constructor create();
  begin
  end;
  public constructor create(o:object; s:string);
  begin
    ob:=o;
    type_name:=s;
  end;
  public class function operator+ (op1,op2: datatype):datatype;
  public class function operator- (op1,op2: datatype):datatype;
  public class function operator- (op1: datatype):datatype;
  public class function operator* (op1,op2: datatype):datatype;
  public class function operator div(op1,op2: datatype):datatype;
  public class function operator and(op1,op2: datatype):datatype;
  public class function operator or(op1,op2: datatype):datatype;
  public class function operator not(op1: datatype):datatype;
  public class function operator< (op1,op2: datatype):datatype;
  public class function operator> (op1,op2: datatype):datatype;
  public class function operator<= (op1,op2: datatype):datatype;
  public class function operator>= (op1,op2: datatype):datatype;
  public class function operator<> (op1,op2: datatype):datatype;
  public class function operator= (op1,op2: datatype):datatype;
  
  public function colon(op2: datatype):datatype;
  public function count:integer;
  public function index(i:integer):datatype;
  public procedure add(o:datatype);
  public function head():datatype;
  public function tail():datatype;
  public function getChar():datatype;
  public procedure print();
  public procedure print1();
end;

arr=array of datatype;

datatype_list=class(datatype)
  public constructor create();
  begin
  end;
  public constructor create(n:integer;type_name:string;params ar:arr);
  public constructor create(n:integer;type_name:string);
  public constructor create(first:datatype;second:datatype;last:datatype;type_name:string);
  public constructor create(first:datatype;last:datatype;type_name:string);
  public procedure print();
  public procedure print1();
end;
function print(a:datatype):datatype;

implementation

////////////////////////////datatype
  function datatype.colon(op2: datatype):datatype;
  begin
    var list2:ArrayList;
    if (op2.type_list_name='enum') or ((op2.ob is ArrayList) and (ArrayList(op2.ob).Count>0)) then
    begin
      list2:=ArrayList(ArrayList(op2.ob).Clone());
      var rez:=new datatype();
      var i:integer;
      rez.ob:=new ArrayList();
      rez.list:=new ArrayList();
      ArrayList(rez.ob).Add(self);
      for i:=0 to list2.Count-1 do
        ArrayList(rez.ob).Add(list2[i]);
      rez.type_name:='list';
      rez.type_list_name:='enum';
      rez.list:=ArrayList(rez.ob);
      Result:= rez;
    end
    else
    if (op2.type_list_name='delta_limited') then
    begin
      var rez:=new datatype();
      var i:integer;
      rez.ob:=new ArrayList();
      rez.first:=datatype(self.ob);
      rez.second:=op2.first;
      rez.last:=op2.last;
      rez.delta:=op2.delta;
      ArrayList(rez.ob).Add(self.ob);
      for i:=0 to list2.Count-1 do
        ArrayList(rez.ob).Add(list2[i]);
      rez.type_list_name:='delta_limited';
      rez.type_name:='list';
      Result:= rez;
    end
    else
    if (op2.type_list_name ='one_limited') then
    begin
      var rez:=new datatype();
      var i:integer;
      rez.ob:=new ArrayList();
      rez.first:=datatype(self.ob);
      rez.last:=op2.last;
      rez.delta:=op2.delta;
      ArrayList(rez.ob).Add(self.ob);
      for i:=0 to list2.Count-1 do
        ArrayList(rez.ob).Add(list2[i]);
      rez.list:=ArrayList(rez.ob);
      rez.type_name:='list';
      rez.type_list_name:='one_limited';
      Result:= rez;
    end
    else
    if (op2.type_list_name='infinity') then
    begin
      var rez:=new datatype();
      var i:integer;
      rez.ob:=new ArrayList();
      rez.first:=datatype(self.ob);
      rez.second:=op2.second;
      rez.delta:=op2.delta;
      rez.type_list_name:='infinity';
      rez.type_name:='list';
      Result:= rez;
    end
    else
    if (op2.type_list_name='empty') then
    begin
      var rez:=new datatype();
      rez.ob:=new ArrayList();
      ArrayList(rez.ob).Add(self);
      rez.list:=ArrayList(rez.ob);
      rez.type_list_name:='enum';
      Result:= rez;
    end;
  end;
  
  function datatype.count:integer;
  begin
    var list:ArrayList;
    list:=ArrayList(ArrayList(ob).Clone());
    Result:=list.Count-1;
  end;
  
  function datatype.index(i:integer):datatype;
  begin
    var list:ArrayList;
    list:=ArrayList(ArrayList(ob).Clone());
    if (list[i] is integer) then
       Result:=new datatype(list[i],'integer')
    else   
    if (list[i] is Biginteger) then
       Result:=new datatype(list[i],'integer')
    else 
    if (list[i] is real) then
       Result:=new datatype(list[i],'real')
    else 
    if (list[i] is char) then
       Result:=new datatype(list[i],'char')
    else 
    if (list[i] is string) then
       Result:=new datatype(list[i],'string')
    else 
       begin
        var dt:datatype:= new datatype(list[i],'list');
        Result:=dt.ob as datatype;
       end;
  end;
  
  procedure datatype.add(o:datatype);
  begin
    var list:ArrayList;
    list:=ArrayList(ArrayList(ob).Clone());
    if (o.type_name='corteg') or ((o.type_name='list') and (o.type_list_name<>'')) then
        list.Add(o)
    else    
        list.Add(o.ob);
    ob:=list.Clone();
    type_name:='list';
  end;
  
  function datatype.head():datatype;
  begin
    if (self.type_list_name<>'infinity') then
    begin
        var list:ArrayList;
        list:=ArrayList(ArrayList(ob).Clone());
        if (list.Count>0) then
        begin
          if (list[0] is integer) then
            Result:=new datatype(integer(list[0]),'integer');
          if (list[0] is Biginteger) then
            Result:=new datatype(Biginteger(list[0]),'integer');
          if (list[0] is real) then
            Result:=new datatype(real(list[0]),'real');
          if (list[0] is char) then
            Result:=new datatype(char(list[0]),'char');
          if (list[0] is string) then
            Result:=new datatype(string(list[0]),'string');
          if (list[0] is boolean) then
            Result:=new datatype(boolean(list[0]),'boolean');
          if (list[0] is datatype) then
            Result:=datatype(list[0]);
        end
        else
          Result:=new datatype(0,'empty_list');
    end
    else
    begin
      Result:=self.first;
    end;
  end;
  
  function datatype.tail():datatype;
  begin
    if (self.type_list_name<>'infinity') then
    begin
      var list:ArrayList;
      list:=ArrayList(ArrayList(ob).Clone());
      if (list.Count>0) then
        list.RemoveAt(0);
      var a:datatype;
      a:=new datatype();
      a.ob:=list.Clone();
      a.type_list_name:=self.type_list_name;
      if (list.Count=0) then
        a.type_list_name:='empty';
      Result:=a;
    end
    else
    begin
      var a:datatype;
      a:=new datatype();
      a.first:=new datatype();
      if (self.delta is integer) then
        a.first.ob:=integer(self.first.ob)+integer(self.delta)
      else
        a.first.ob:=real(self.first.ob)+real(self.delta);
      a.delta:=self.delta;
      a.type_list_name:=self.type_list_name;
      Result:=a;
    end;
  end;
  
  function datatype.getChar():datatype;
  begin
    var a:string;
    readln(a);
    var d:datatype:=new datatype(a,'string');
    Result:=d;
  end;
  
  procedure datatype.print();
  begin
    print1();
    writeln();
  end; 
  
  procedure datatype.print1();
  begin
    if (type_name='integer') then
    begin
      var b1: BigInteger;
      if (ob is integer) then
        write(integer(ob))
      else
        write(BigInteger(ob));
    end
    else
    if (type_name='real') then
      write(real(ob))
    else
    if (type_name='string') then
      write(string(ob))
    else
    if (type_name='char') then
      write(char(ob))
      else
    if (type_name='boolean') then
      write(boolean(ob))
    else
    if (self.type_name='corteg') then  
      begin
        var list:ArrayList;
        list:=ArrayList(ob);
        var i:integer;
        write('(');
        for i:= 0 to list.Count-1 do
        begin
              if (list[i] is integer or list[i] is BigInteger) then
                begin
                  if (list[i] is integer) then
                    write(integer(list[i]))
                  else
                    write(BigInteger(list[i]));
                end;
              if (list[i] is real) then
                write(real(list[i]));
              if (list[i] is char) then
                write('`'+char(list[i])+'`');
              if (list[i] is string) then
                write('"'+string(list[i])+'"');
              if (list[i] is boolean) then
                write(boolean(list[i])); 
              if (list[i] is datatype) then
                  datatype(list[i]).print1();
          if (i<list.Count-1) then
            write(','); 
        end;
          write(')')
      end
      else
      if (ob<>nil) then
      begin
        var list:ArrayList;
        list:=ArrayList(ob);
        var i:integer;
        write('[');
        for i:= 0 to list.Count-1 do
        begin
              if (list[i] is integer) then
                    write(integer(list[i]));
              if (list[i] is Biginteger) then
                    write(BigInteger(list[i]));
              if (list[i] is real) then
                write(real(list[i]));
              if (list[i] is char) then
                write(char(list[i]));
              if (list[i] is string) then
                write(string(list[i]));
              if (list[i] is boolean) then
                write(boolean(list[i])); 
              if (list[i] is datatype) then
                datatype(list[i]).print1();
          if (i<list.Count-1) then
            write(',');
        end;
        write(']');
      end;
  end;
  
class function datatype.operator+ (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
        var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1+b2, 'integer');
        end;
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob)+string(op2.ob), 'string');
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a+b, 'real');
  end;
end;

class function datatype.operator- (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
        var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1-b2, 'integer');
  end;
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a-b, 'real');
  end;
end;

class function datatype.operator- (op1: datatype):datatype;
begin
  if (op1.type_name = 'integer') then
  begin
  var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        Result:= new datatype((-1)*b1, 'integer');
        end;
  if (op1.type_name = 'real') then
        Result:= new datatype(-real(op1.ob), 'real');
end;

class function datatype.operator* (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
        var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1*b2, 'integer');
  end;
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a*b, 'real');
  end;
end;

class function datatype.operator div (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
  var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1/b2+0.0, 'real');
  end;
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a/b+0.0, 'real');
  end;
end;

class function datatype.operator and (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'boolean') and (op2.type_name = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) and boolean(op2.ob), 'boolean');
end;

class function datatype.operator or (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'boolean') and (op2.type_name = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) or boolean(op2.ob), 'boolean');
end;

class function datatype.operator not (op1: datatype):datatype;
begin
  if (op1.type_name = 'boolean') then
        Result:= new datatype(not boolean(op1.ob), 'boolean');
end;

class function datatype.operator= (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
        var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1 = b2,'boolean')
        end
        else
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob) = string(op2.ob),'boolean')
        else
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
        Result:= new datatype(real(op1.ob) = real(op2.ob),'boolean')
        else
  if ((op1.type_name = 'char') and (op2.type_name = 'char')) then
        Result:= new datatype(char(op1.ob) = char(op2.ob),'boolean')
        else
  if ((op1.type_name = 'boolean') and (op2.type_name = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) = boolean(op2.ob),'boolean')
        else
  if (((op2.type_name = 'empty_list') and (op1.type_name = '') and (op1.type_list_name='empty')){ or 
       ((ArrayList(op1.ob).Count=0) and (ArrayList(op2.ob).Count=0))}) then
        Result:= new datatype(true, 'boolean')
        else
        Result:= new datatype(false, 'boolean');
end;

class function datatype.operator<> (op1,op2: datatype):datatype;
begin
  if (op2.ob = nil) and (op1.ob = nil) then
        Result:=new datatype(true, 'boolean') 
  else
        if (((op2.ob = nil) and (op1.ob <> nil)) or ((op2.ob <> nil) and (op1.ob = nil))) then
              Result:=new datatype(false, 'boolean')
        else
        begin
          if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
          begin
                var b1, b2: BigInteger;
                if (op1.ob is integer) then
                  b1:=new BigInteger(1,integer(op1.ob))
                else
                  b1:=BigInteger(op1.ob);
                if (op2.ob is integer) then
                  b2:=new BigInteger(1,integer(op2.ob))
                else
                  b2:=BigInteger(op2.ob);
                Result:= new datatype(b1 <> b2,'boolean');
          end;
          if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
                Result:= new datatype(string(op1.ob) <> string(op2.ob),'boolean');
          if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
                Result:= new datatype(real(op1.ob) <> real(op2.ob),'boolean');
          if ((op1.type_name = 'char') and (op2.type_name = 'char')) then
                Result:= new datatype(char(op1.ob) <> char(op2.ob),'boolean');
          if ((op1.type_name = 'boolean') and (op2.type_name = 'boolean')) then
                Result:= new datatype(boolean(op1.ob) <> boolean(op2.ob),'boolean');
        end;
end;

class function datatype.operator< (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
        var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1 < b2, 'boolean');
  end;
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob)<string(op2.ob), 'boolean');
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a<b, 'boolean');
  end;
end;

class function datatype.operator> (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
  var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1>b2, 'boolean');
        end;
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob)>string(op2.ob), 'boolean');
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a>b, 'boolean');
  end;
end;

class function datatype.operator<= (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
  var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1<=b2, 'boolean');
        end;
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob)<=string(op2.ob), 'boolean');
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a<=b, 'boolean');
  end;
end;

class function datatype.operator>= (op1,op2: datatype):datatype;
begin
  if ((op1.type_name = 'integer') and (op2.type_name = 'integer')) then
  begin
       var b1, b2: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
        Result:= new datatype(b1>=b2, 'boolean');
        end;
  if ((op1.type_name = 'string') and (op2.type_name = 'string')) then
        Result:= new datatype(string(op1.ob)>=string(op2.ob), 'boolean');
  if ((op1.type_name = 'real') or (op2.type_name = 'real')) then
  begin
  var a,b:real;
    if (op1.type_name = 'integer') then
    begin
        var b1: BigInteger;
        if (op1.ob is integer) then
          b1:=new BigInteger(1,integer(op1.ob))
        else
          b1:=BigInteger(op1.ob);
      a:=b1;
    end
    else
      a:=real(op1.ob);
    if (op2.type_name = 'integer') then
    begin
    var b2: BigInteger;
        if (op2.ob is integer) then
          b2:=new BigInteger(1,integer(op2.ob))
        else
          b2:=BigInteger(op2.ob);
      b:=b2;
      end
    else
      b:=real(op2.ob);
    Result:= new datatype(a>=b, 'boolean');
  end;
end;

///////////////////////////////datatype_list
  constructor datatype_list.create(n:integer;type_name:string;params ar:arr);
  begin
    var i:integer;
    ///////////////////////////////////check
    if (type_name<>'corteg') then
    begin
        var types:ArrayList := new ArrayList(); 
        for i:=0 to ar.length-1 do
            if (not(types.Contains(ar[i].type_name)) and (ar[i].type_name<>'')) then
               types.Add(ar[i].type_name);
        if ((types.Count=2) and not(types.Contains('integer') and types.Contains('real'))) or 
           (types.Count>2) then
           begin
              writeln('элементы списка имеют разный тип');
              exit;
           end;
    end;   
    ///////////////////////////////////
    list:=new ArrayList();
    for i:=0 to ar.length-1 do
    begin
      if (ar[i].type_name='integer') then
      begin
        if (ar[i].ob is integer) then
          list.Add((integer)(ar[i].ob))
        else
          list.Add(BigInteger(ar[i].ob));
      end;
      if (ar[i].type_name='real') then
        list.Add((real)(ar[i].ob));
      if (ar[i].type_name='char') then
        list.Add((char)(ar[i].ob));
      if (ar[i].type_name='string') then
        list.Add((ar[i].ob).ToString());
      if (ar[i].type_name='boolean') then
        list.Add((ar[i].ob).ToString());
      if (ar[i].type_name='list') then  
        list.Add(ar[i]);
      if (ar[i].type_name='corteg') then  
        list.Add(ar[i]);
    end;
    ob:=list.Clone();
    self.type_name:=type_name;
    type_list_name:='enum';
  end;
  
  constructor datatype_list.create(n:integer;type_name:string);
  begin
    var i:integer;
    list:=new ArrayList();
    ob:=list.Clone();
    type_list_name:='empty';
  end;
  
  constructor datatype_list.create(first:datatype;second:datatype;last:datatype;type_name:string);
  begin
    var first_ob:Biginteger;
    if (first.ob is Biginteger) then
       first_ob:=Biginteger(first.ob);
    if (first.ob is integer) then
       first_ob:=Biginteger.Create(integer(first.ob));
    var second_ob:Biginteger;
    if (second.ob is Biginteger) then
       second_ob:=Biginteger(second.ob);
    if (second.ob is integer) then
       second_ob:=Biginteger.Create(integer(second.ob));   
    var last_ob:Biginteger;
    if (last.ob is Biginteger) then
       last_ob:=Biginteger(last.ob);
    if (last.ob is integer) then
       last_ob:=Biginteger.Create(integer(last.ob));
  ///////////////////////////////////check
    var types:ArrayList := new ArrayList(); 
    if (not(types.Contains(first.type_name)) and (first.type_name<>'')) then
       types.Add(first.type_name);
    if (not(types.Contains(second.type_name)) and (second.type_name<>'')) then
       types.Add(second.type_name);
    if (not(types.Contains(last.type_name)) and (last.type_name<>'nil') and (last.type_name<>'')) then
       types.Add(last.type_name);
    if ((types.Count=2) and not(types.Contains('integer') and types.Contains('real'))) or 
       (types.Count>2) then
       begin
          writeln('элементы списка имеют разный тип');
          exit;
       end;
    ///////////////////////////////////
    if (last.type_name<>'nil') then
    begin
      self.first:=first;
      self.last:=last;
      if ((first.type_name='integer') and (second.type_name='integer') and (last.type_name='integer')) then
      begin
          self.delta:=second_ob-first_ob;
          var i:Biginteger;
          i:=first_ob;
          list:=new ArrayList();
          while (i<=last_ob) do
          begin
            list.Add(i);
            i:=i+Biginteger(self.delta);
          end;
      end;
      if ((first.type_name='real') or (second.type_name='real') or (last.type_name='real')) then
      begin
          if (first.type_name='integer') then
            first.ob:=first_ob + 0.0;
          if (second.type_name='integer') then
            second.ob:=second_ob + 0.0;
          if (last.type_name='integer') then
            last.ob:=last_ob + 0.0;
          self.delta:=real(second.ob)-real(first.ob);
          //self.type_name:='real';
          var i:real;
          i:=real(first.ob);
          list:=new ArrayList();
          while (i<=real(last.ob)) do
          begin
            list.Add(i);
            i:=i+real(self.delta);
          end;
      end;
      ob:=list.Clone();
      type_list_name:='delta_limited';
    end
    else
    begin
      self.first:=first;
      if ((first.type_name='integer') and (second.type_name='integer')) then
      begin
          self.delta:=second_ob-first_ob;
          //self.type_name:='integer';
      end
      else
      begin
        if (first.type_name='integer') then
            first.ob:=first_ob + 0.0;
        if (second.type_name='integer') then
            second.ob:=second_ob + 0.0;
        self.delta:=real(second.ob)-real(first.ob);
        //self.type_name:='real';
      end;
      list:=new ArrayList();
      ob:=list.Clone();
      type_list_name:='infinity';
    end;
    self.type_name:=type_name;
  end;
  
  constructor datatype_list.create(first:datatype;last:datatype;type_name:string);
  begin
  var first_ob:Biginteger;
    if (first.ob is Biginteger) then
       first_ob:=Biginteger(first.ob);
    if (first.ob is integer) then
       first_ob:=Biginteger.Create(integer(first.ob));
    var last_ob:Biginteger;
    if (last.ob is Biginteger) then
       last_ob:=Biginteger(last.ob);
    if (last.ob is integer) then
       last_ob:=Biginteger.Create(integer(last.ob));
  ///////////////////////////////////check
    var types:ArrayList := new ArrayList(); 
    if (not(types.Contains(first.type_name)) and (first.type_name<>'')) then
       types.Add(first.type_name);
    if (not(types.Contains(last.type_name)) and (last.type_name<>'')) then
       types.Add(last.type_name);
    if ((types.Count=2) and not(types.Contains('integer') and types.Contains('real'))) then
       begin
          writeln('элементы списка имеют разный тип');
          exit;
       end;
    ///////////////////////////////////
    self.first:=first;
    self.last:=last;
    self.delta:=Biginteger.Create(1);
    self.type_name:=type_name;
    if ((first.type_name='integer') and (last.type_name='integer')) then
    begin
        var i:Biginteger;
        i:=first_ob;
        list:=new ArrayList();
        while (i<=last_ob) do
        begin
          list.Add(i);
          i:=i+Biginteger(self.delta);
        end;
    end;
    if ((first.type_name='real') or (last.type_name='real')) then
    begin
        if (first.type_name='integer') then
            first.ob:=first_ob + 0.0;
        if (last.type_name='integer') then
            last.ob:=last_ob + 0.0;
        var i:real;
        i:=real(first.ob);
        list:=new ArrayList();
        while (i<=real(last.ob)) do
        begin
          list.Add(i);
          i:=i+Biginteger(self.delta);
        end;
    end;
    ob:=list.Clone();
    //self.type_name:=string(typeof(self.delta));
    type_list_name:='one_limited';
  end;
  
  procedure datatype_list.print();
  begin
     print1();
     writeln();
  end;
  
  procedure datatype_list.print1();
  begin
    if (list<>nil) then
        if (type_name='corteg') then
        begin
            var i:integer;
            write('(');
            for i:= 0 to list.Count-1 do
            begin
              if (list[i] is integer) then
                write(integer(list[i]));
              if (list[i] is real) then
                write(real(list[i]));
              if (list[i] is char) then
                write('`'+char(list[i])+'`');
              if (list[i] is string) then
                write('"'+(list[i]).ToString()+'"');
              if (list[i] is boolean) then
                write((list[i]).ToString()); 
              if (list[i] is datatype_list) then
                        datatype_list(list[i]).print1();
              if (i<list.Count-1) then
                write(',');
            end;
            write(')');
        end
        else
        begin
            var i:integer;
            write('[');
            for i:= 0 to list.Count-1 do
            begin
              if (list[i] is integer) then
                write(integer(list[i]));
              if (list[i] is real) then
                write(real(list[i]));
              if (list[i] is char) then
                write(char(list[i]));
              if (list[i] is string) then
                write((list[i]).ToString());
              if (list[i] is boolean) then
                write((list[i]).ToString()); 
              if (list[i] is datatype_list) then
                        datatype_list(list[i]).print1();
              if (i<list.Count-1) then
                write(',');
            end;
            write(']');
        end;
  end;
  function print(a:datatype):datatype;
  begin
    a.print();
    result:=nil;
  end;
begin
end.