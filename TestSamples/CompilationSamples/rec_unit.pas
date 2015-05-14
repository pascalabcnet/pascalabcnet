unit rec_unit;

interface

type TStudent = record
      name : string;
      age : integer;
     end;
	
     type NextNode2 = record
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
	    end;

     TClassWithRecord = final class
      f : record k,l,m : integer; end;
      f2 : TStudent;
      f3 : TStudent;
      constructor create;
     end;

implementation

constructor TClassWithRecord.create;
begin
end;

end.