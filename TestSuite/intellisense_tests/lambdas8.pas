begin
  System.Collections.Generic.Comparer&<byte>.Create(
    (b1{@parameter b1: byte;@},b2)->
    begin
      Result := b2{@parameter b2: byte;@}-b1;
    end
  )
end.