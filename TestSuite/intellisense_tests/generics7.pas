begin
  ''.Select{@(расширение sequence of T) function Select<char,TResult>(selector: char->TResult): sequence of TResult;@}(x->Ord(x));
  var s: string := '';
  s.Select{@(расширение sequence of T) function Select<char,TResult>(selector: char->TResult): sequence of TResult;@}(x->Ord(x));
end.