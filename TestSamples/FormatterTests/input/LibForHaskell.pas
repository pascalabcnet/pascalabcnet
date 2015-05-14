unit LibForHaskell;
interface
uses System,System.Collections,System.Collections.Generic;

type datatype=class
  public ob:object;
  public tp:string;
  public type_list:string;
  public constructor create();
  begin
  end;
  public constructor create(o:object; s:string);
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
  public function head():datatype;
  begin
    var list:ArrayList;
    list:=ArrayList(ArrayList(ob).Clone());
    if (list.Count>0) then
    begin
      if (list[0] is integer) then
        Result:=new datatype(integer(list[0]),'integer');
      if (list[0] is real) then
        Result:=new datatype(real(list[0]),'real');
      if (list[0] is char) then
        Result:=new datatype(char(list[0]),'char');
      if (list[0] is string) then
        Result:=new datatype(string(list[0]),'string');
      if (list[0] is boolean) then
        Result:=new datatype(boolean(list[0]),'boolean');
    end
    else
      Result:=new datatype(0,'empty_list');
  end;
  public function tail():datatype;
  begin
    var list:ArrayList;
    list:=ArrayList(ArrayList(ob).Clone());
    if (list.Count>0) then
      list.RemoveAt(0);
    var a:datatype;
    a:=new datatype();
    a.ob:=list.Clone();
    a.tp:='';
    if (list.Count=0) then
      a.type_list:='empty';
    Result:=a;
  end;
  public procedure print();
  begin
    if (tp='integer') then
      writeln(integer(ob))
    else
    if (tp='real') then
      writeln(real(ob))
      else
    if (tp='string') then
      writeln(string(ob))
      else
    if (tp='char') then
      writeln(char(ob))
      else
    if (tp='boolean') then
      writeln(boolean(ob))
      else
      begin
        var list:ArrayList;
        list:=ArrayList(ob);
        var i:=0;
        for i:= 0 to list.Count-1 do
        begin
          if (list[i] is integer) then
            writeln(integer(list[i]));
          if (list[i] is real) then
            writeln(real(list[i]));
          if (list[i] is char) then
            writeln(char(list[i]));
          if (list[i] is string) then
            writeln(string(list[i]));
        end;
      end;
  end;
end;

type arr=array of datatype;

type datatype_list=class(datatype)
  public first:datatype; 
  public second:datatype;
  public last:datatype;
  public list:ArrayList;
  public delta:integer;
  public tp:string;
  //public type_list:string;
  public constructor create();
  begin
  end;
  public constructor create(n:integer;tp:string;params ar:arr);
  begin
    var i:integer;
    list:=new ArrayList();
    for i:=0 to ar.length-1 do
    begin
      if (ar[i].tp='integer') then
        list.Add((integer)(ar[i].ob));
      if (ar[i].tp='real') then
        list.Add((real)(ar[i].ob));
      if (ar[i].tp='char') then
        list.Add((char)(ar[i].ob));
      if (ar[i].tp='string') then
        list.Add((ar[i].ob).ToString());
    end;
    ob:=list.Clone();
    type_list:='enum';
  end;
  public constructor create(n:integer;tp:string);
  begin
    var i:integer;
    list:=new ArrayList();
    ob:=list.Clone();
    type_list:='empty';
  end;
  public constructor create(first:datatype;second:datatype;last:datatype;tp:string);
  begin
    if (last.tp<>'nil') then
    begin
      self.first:=first;
      self.last:=last;
      self.delta:=integer(second.ob)-integer(first.ob);
      var i:integer;
      i:=integer(first.ob);
      list:=new ArrayList();
      while (i<=integer(last.ob)) do
      begin
        list.Add(i);
        i:=i+self.delta;
      end;
      ob:=list.Clone();
      type_list:='delta_limited';
    end
    else
    begin
      self.first:=first;
      self.delta:=integer(second.ob)-integer(first.ob);
      type_list:='infinity';
    end;
  end;
  public constructor create(first:datatype;last:datatype;tp:string);
  begin
    //if (tp='integer') then
    begin
      self.first:=first;
      self.last:=last;
      self.delta:=1;
      var i:integer;
      i:=integer(first.ob);
      list:=new ArrayList();
      while (i<=integer(last.ob)) do
      begin
        list.Add(i);
        i:=i+self.delta;
      end;
    end;
    ob:=list.Clone();
    type_list:='one_limited';
  end;
  public procedure print();
  begin
    var i:integer;
    for i:= 0 to list.Count-1 do
    begin
      if (list[i] is integer) then
        writeln(integer(list[i]));
      if (list[i] is real) then
        writeln(real(list[i]));
      if (list[i] is char) then
        writeln(char(list[i]));
      if (list[i] is string) then
        writeln((list[i]).ToString());
    end;
  end;
  
  class function operator+ (op1,op2: datatype_list):datatype_list;
  begin
    if (op1.type_list<>'infinity') and (op2.type_list<>'infinity') then
    begin
      var rez:=new datatype_list();
      var i:integer;
      rez.list:=new ArrayList();
      for i:=0 to op1.list.Count-1 do
        rez.list.Add(op1.list[i]);
      for i:=0 to op2.list.Count-1 do
        rez.list.Add(op2.list[i]);
      rez.ob:=rez.list.Clone();
      Result:= rez;
    end
    else
    begin
      var rez:=new datatype_list();
      if (integer(op1.first.ob)<integer(op2.first.ob)) then
        rez.first:=new datatype(op1.first.ob,'integer')
      else
        rez.first:=new datatype(op2.first.ob,'integer');
      if (op1.delta<>0) and (op1.delta<op2.delta) then
        rez.delta:=op1.delta;
      if (op2.delta<>0) and (op2.delta<op1.delta) then
        rez.delta:=op2.delta;
    end;
  end;
  public function concat (op1,op2: datatype_list):datatype_list;
  begin
    if (op1.type_list = op2.type_list) and (op1.type_list<>'infinity') then
    begin
      var rez:=new datatype_list();
      var i:integer;
      rez.list:=new ArrayList();
      for i:=0 to op1.list.Count-1 do
        rez.list.Add(op1.list[i]);
      for i:=0 to op2.list.Count-1 do
        rez.list.Add(op2.list[i]);
      rez.ob:=rez.list.Clone();
      Result:= rez;
    end
    else
    begin
      Result:=nil;
    end;
  end;
end;

implementation

constructor datatype.create(o:object; s:string);
begin
  ob:=o;
  tp:=s;
end;

class function datatype.operator+ (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)+integer(op2.ob), 'integer');
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob)+string(op2.ob), 'string');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a+b, 'real');
  end;
end;

class function datatype.operator- (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)-integer(op2.ob), 'integer');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a-b, 'real');
  end;
end;
class function datatype.operator- (op1: datatype):datatype;
begin
  if (op1.tp = 'integer') then
        Result:= new datatype(-integer(op1.ob), 'integer');
  if (op1.tp = 'real') then
        Result:= new datatype(-real(op1.ob), 'real');
end;
class function datatype.operator* (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)*integer(op2.ob), 'integer');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a*b, 'real');
  end;
end;
class function datatype.operator div (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)/integer(op2.ob), 'real');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a/b, 'real');
  end;
end;
class function datatype.operator and (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'boolean') and (op2.tp = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) and boolean(op2.ob), 'boolean');
end;
class function datatype.operator or (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'boolean') and (op2.tp = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) or boolean(op2.ob), 'boolean');
end;
class function datatype.operator not (op1: datatype):datatype;
begin
  if (op1.tp = 'boolean') then
        Result:= new datatype(not boolean(op1.ob), 'boolean');
end;
class function datatype.operator= (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob) = integer(op2.ob),'boolean')
        else
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob) = string(op2.ob),'boolean')
        else
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
        Result:= new datatype(real(op1.ob) = real(op2.ob),'boolean')
        else
  if ((op1.tp = 'char') and (op2.tp = 'char')) then
        Result:= new datatype(char(op1.ob) = char(op2.ob),'boolean')
        else
  if ((op1.tp = 'boolean') and (op2.tp = 'boolean')) then
        Result:= new datatype(boolean(op1.ob) = boolean(op2.ob),'boolean')
        else
  if ((op2.tp = 'empty_list') and (op1.tp = '') and (op1.type_list='empty')) then
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
          if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
                Result:= new datatype(integer(op1.ob) <> integer(op2.ob),'boolean');
          if ((op1.tp = 'string') and (op2.tp = 'string')) then
                Result:= new datatype(string(op1.ob) <> string(op2.ob),'boolean');
          if ((op1.tp = 'real') or (op2.tp = 'real')) then
                Result:= new datatype(real(op1.ob) <> real(op2.ob),'boolean');
          if ((op1.tp = 'char') and (op2.tp = 'char')) then
                Result:= new datatype(char(op1.ob) <> char(op2.ob),'boolean');
          if ((op1.tp = 'boolean') and (op2.tp = 'boolean')) then
                Result:= new datatype(boolean(op1.ob) <> boolean(op2.ob),'boolean');
        end;
end;
class function datatype.operator< (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)<integer(op2.ob), 'boolean');
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob)<string(op2.ob), 'boolean');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a<b, 'boolean');
  end;
end;
class function datatype.operator> (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)>integer(op2.ob), 'boolean');
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob)>string(op2.ob), 'boolean');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a>b, 'boolean');
  end;
end;
class function datatype.operator<= (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)<=integer(op2.ob), 'boolean');
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob)<=string(op2.ob), 'boolean');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a<=b, 'boolean');
  end;
end;
class function datatype.operator>= (op1,op2: datatype):datatype;
begin
  if ((op1.tp = 'integer') and (op2.tp = 'integer')) then
        Result:= new datatype(integer(op1.ob)>=integer(op2.ob), 'boolean');
  if ((op1.tp = 'string') and (op2.tp = 'string')) then
        Result:= new datatype(string(op1.ob)>=string(op2.ob), 'boolean');
  if ((op1.tp = 'real') or (op2.tp = 'real')) then
  begin
  var a,b:real;
    if (op1.tp = 'integer') then
      a:=integer(op1.ob)
    else
      a:=real(op1.ob);
    if (op2.tp = 'integer') then
      b:=integer(op2.ob)
    else
      b:=real(op2.ob);
    Result:= new datatype(a>=b, 'boolean');
  end;
end;

{type arr=array of integer;
procedure f(params b:arr);
begin
end;}

begin
end.