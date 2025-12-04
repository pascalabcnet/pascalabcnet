type
  list<T> = class
    function a: list<T>;
    begin
      result := self;
      assert(result = self);
    end;
    
    function b: list<T>;
  end;

function list<T>.b: list<T>;
begin
  result := self;
  assert(result = self);
end;

begin
  var lst: list<integer> := new list<integer>;
  lst.a;
  lst.b;
end.