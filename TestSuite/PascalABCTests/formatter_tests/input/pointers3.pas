type 
     TRec = record
             a : set of byte;
             b : array[1..4] of integer;
            end;
     
var prec : ^TRec;
    rec : TRec;

begin
 New(prec);
 Include(prec^.a,23);
 assert(prec^.a=[23]);
 prec^.b[2] := 4;
 assert(prec^.b[2] = 4);
 rec := prec^;
 assert(rec.a=[23]);
 assert(rec.b[2] = 4);
 Include(rec.a,12);
 assert(prec^.a = [23]);
 rec.b[2] := 5; assert(prec^.b[2] = 4);
 Dispose(prec);
 
end.