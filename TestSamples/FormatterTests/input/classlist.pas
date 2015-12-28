// Демонстрация класса односвязного списка целых с внутренним итератором
uses System;

type
  IntListNode=class
    i: integer;
    next: IntListNode;
    constructor Create(ii: integer; nxt: IntListNode);
    begin
      i:=ii;
      next:=nxt;
    end;
    procedure Destroy;
    begin
    end;
  end;

  IntList=class
    first,last,current: IntListNode;
    constructor Create;
    {begin
      first:=nil;
      last:=nil;
      current:=nil;
    end;}
    function IsEmpty: boolean;
    begin
      IsEmpty:=first=nil;
    end;
    procedure Reset;
    begin
      current:=first;
    end;
    procedure Next;
    begin
      current:=current.next;
    end;
    function CurrentValue: integer;
    begin
      CurrentValue:=current.i;
    end;
    

    function EndOfList: boolean;
    begin
      EndOfList:=current=nil;
    end;

    procedure Clear;
    begin
      while first<>nil do
      begin
        current:=first;
        first:=first.next;
        current.Destroy;
      end;
      current:=nil;
      last:=nil;
    end;
    procedure Destroy;
    begin
      Clear;
    end;
    procedure Add(i: integer);
    begin
      if last<>nil then
      begin
        last.next:=IntListNode.Create(i,nil);
        last:=last.next
      end
        else
      begin
        last:=IntListNode.Create(i,nil);
        first:=last;
        current:=first;
      end;
    end;
    
    procedure Print;
    var f: IntListNode;
    begin
      f:=first;
      while f<>nil do
      begin
       Console.Write(f.i.toString()+' '.tostring());
        f:=f.next;
      end;
    end;
  end;

 constructor IntList.Create;
 begin
   first:=nil;
      last:=nil;
      current:=nil;
 end;

type TMas = array[2..6] of real;
type tdatemas = array[-10..10] of System.DateTime;

type ttest = class
a : TMas;
constructor Create;overload;
constructor Create(s : string);overload;
constructor Create(var arr : TMas); overload;

procedure haha (var arr : TMas);
begin
end;

end;

constructor ttest.Create(var arr : TMas);
begin
 a := arr;
end;

constructor ttest.Create;
begin
 writeln(2);
end;

constructor ttest.Create(s : string);
//procedure nested;
//begin
 //writeln(s);a[5] := 321;
 //writeln(a[5]);
//end;
begin
 //nested;
end;


var l: IntList;
   cls : TTest;
   mas : TMas;
   datemas : tdatemas;

begin
 writeln(datemas[0].Hour.tostring);
 cls := ttest.Create('ok');
 mas[2] := 2.71;
 cls := ttest.create(mas); 
 mas[3] := 3.14;
 writeln(cls.a[3].tostring);
 l:=IntList.Create;
  l.Add(1);
  l.Add(2);
  l.Add(3);
  l.Add(7);
  l.Add(5);
  l.Add(6);

  l.Print;
  writeln;

  l.Reset;
  while not l.EndOfList() do
  begin
    Console.Write(l.CurrentValue().ToString);
    l.Next
  end;
  writeln;
  l.Destroy;
end.
