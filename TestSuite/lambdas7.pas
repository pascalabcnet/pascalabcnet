type
  TBase<T> = abstract class
    procedure p1<T2>(f: T->T2);
    begin
      assert(f(default(T)).ToString() = 'abc');
    end;
  end;
  
  TErr = class(TBase<byte>) // обязательно наследовать от TBase
    
    procedure p2;
    begin
      var o: TBase<byte> := new TErr;
      
      o.p1(function(b: byte)->
      begin
        Result := 'abc';
        var s: string := Result;
        var a: byte := b;
      end);
      
    end;
    
  end;

begin 
  var o := new TErr;
  o.p2;
end.