type
  trec=record
         x: integer;
         y: integer;
         a: array[1..3] of integer;
         r: record
           z:integer;
           r: record
             z:string;
           end;
         end;
       end;
const
  rec: trec
       = (
         x: 10; 
         y: 100; 
         a: (1,2,3); 
         r: (
           z:1000; 
           r:(
             z:'ZZZ'
           )
         )
       );
  
  rec2: trec = rec;
  
  arrofrec: array [1..2] of record
         x,y:integer;
       end
       = ((x:1;y:2),(x:3;y:4));
   
var i:integer;

procedure writerec(rec:trec);
var i:integer;
begin
  writeln('=========');
  writeln(rec.x);
  writeln(rec.y);
  for i:=1 to 3 do
    writeln(rec.a[i]);
  writeln(rec.r.z);
  writeln(rec.r.r.z);
  writeln('=========');
end;


procedure p;
const rc: record i:integer end = (i:101);
var r: record i:integer end := (i:98);
begin
  r.i:=r.i+1;
  writeln(r.i);
  writeln(rc.i);
end;

var r:trec:=rec;

begin
  writerec(r);
  writerec(rec2);
  for i:=1 to 2 do
    writeln(arrofrec[i].x,' ',arrofrec[i].y);
  p;
end.