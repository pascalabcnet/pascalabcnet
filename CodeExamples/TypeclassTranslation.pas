type
   ConceptAttribute = class(System.Attribute)
   end;
   IsConceptParameterAttribute = class(System.Attribute)
   end;
   ExplicitModelAttribute = class(System.Attribute)
   end;

(*
  [Concept] SumTC<T> = abstract class
  public
    constructor;
    begin
    end;
    function sum(v1, v2: T): T; abstract;
  end;*)
  SumTC[T] = typeclass
    function sum(v1, v2: T): T;
  end;

(*
  SumTC_Integer = class(SumTC<integer>)
  public
    constructor();
    begin
    end;
    
    function sum(v1, v2: Integer): Integer; override;
    begin
      Result := v1 + v2;
    end;
  end;*)
  SumTC[integer] = instance
    function sum(v1, v2: integer): integer;
    begin
      Result := v1 + v2;
    end;
  end;
(*
  ConceptSingleton<T> = class where T: constructor;
    class _instance: T;
    class inited: boolean := false;
  public
    class function &Instance: T;
    begin
      if inited = false then
      begin
        inited := true;
        _instance := new T;
      end;
      
      Result := _instance;
    end;
  end;*)

(*
function Sum3<T, SumTCT>(v1, v2, v3: T): T; where SumTCT: SumTC<T>, constructor;
begin
  var s := ConceptSingleton&<SumTCT>.&Instance;
  Result := s.sum(v1, s.sum(v2, v3));
end;
*)
function Sum3<T>(v1, v2, v3: T): T; where SumTC[T];
begin
  Result := SumTC&[T].sum(v1, SumTC&[T].sum(v2, v3));
end;

begin
  write(Sum3&<integer, SumTC_integer>(1, 2, 3));
end.