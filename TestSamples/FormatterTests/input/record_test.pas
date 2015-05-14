uses rec_unit;

{type NextNode2 = record
 		 info : real;
		 next : pointer;
		end;

type NextNode = record
 		 info : real;
		 next : ^NextNode2;
		end;

type Node = record
	     info : real;
	     next : ^NextNode;
	    end;}



type TRec = record
	      a, b : integer;
	    end;

TNestedRec = 
	record
	 a : integer;
	 rec : record
		 i, j : integer;
		end;
         tt : string;
	end;

TRecMas = record
	   a : array[1..10] of TRec;
	   b : trec;
	  end;

type TClass = class
       f : TNestedRec;
       f1 : record
	      c,d : integer;
	    end;
     constructor Create;
     end;

constructor TClass.Create;
begin
 f.rec.j := 444;
 f1.c := 777;
 writeln(f.rec.j);
 writeln(f1.c);
end;

var nest_rec : TNestedRec;

procedure TestList;
 var head : ^Node;
begin
 new(head);
 head^.info := 4.4;
 new(head^.next);
 new(head^.next^.next);
 head^.next^.next^.info := 5.5;
 writeln(head^.next^.next^.info);
 Dispose(head^.next^.next);
 Dispose(head^.next);
 Dispose(head);
end;

procedure Test(rec :TRecMas);
var rec2 : record x : real; end;
begin
 rec2.x := 235.245;
 writeln(rec2.x);
 rec.b.a := 1000; 
end;

var  rec, rec2 : TRec;
     rec3, rec4 : Trecmas;
     rec5: record
	    name : string;
	    age : integer;
	   end;
     cls : TClass;
     stud : TStudent;
     cls_rec : TClassWithRecord;
     prec : ^TRec;
     
begin
 TestList;
 new(prec);
 prec^.a := 1111; writeln(prec^.a);
 cls_rec := TClassWithRecord.Create;
 cls_rec.f.k := 1000000;
 cls_rec.f2.name := 'joe';
 cls_rec.f3.name := 'jack';
 stud.name := 'Vasya';
 stud.age := 20;
 writeln('I am '+stud.name);
 cls := TClass.create;
 nest_rec.rec.i := 767;
 writeln(nest_rec.rec.i);
 rec5.name := 'John';
 rec5.age := 22;
 rec := rec2;
 rec.b := 3;
 writeln(rec.b);
 rec3.b := rec;
 writeln(rec3.b.b);
 rec4 := rec3;
 writeln(rec4.b.b);
 writeln('ok');
 Test(rec4);
 writeln(rec4.b.a);
 writeln('Test for mas');
 rec3.a[3].b := 8888;
 writeln(rec3.a[3].b);
 writeln('Copy rec3 to rec4');
 rec4 := rec3;
 writeln(rec4.a[3].b);
 readln;
end.