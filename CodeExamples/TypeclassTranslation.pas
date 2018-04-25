type
   ConceptAttribute = class(System.Attribute)
   end;
   IsConceptParameterAttribute = class(System.Attribute)
   end;
   ExplicitModelAttribute = class(System.Attribute)
   end;

  [Concept] SumTC<T> = abstract class
  public
    constructor;
    begin
    end;
    function sum(v1, v2: T): T; abstract;
  end;

  SumTC_Integer = class(SumTC<integer>)
  public
    constructor();
    begin
    end;
    
    function sum(v1, v2: Integer): Integer; override;
    begin
      Result := v1 + v2;
    end;
  end;


function Sum3<T, SumTCT>(v1, v2, v3: T): T; where SumTCT: SumTC<T>, constructor;
begin
  var s := __ConceptSingleton&<SumTCT>.&Instance;
  Result := s.sum(v1, s.sum(v2, v3));
end;


begin
  write(Sum3&<integer, SumTC_integer>(1, 2, 3));
end.