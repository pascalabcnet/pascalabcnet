unit PT4Linq;

uses PT4;

function  ReadSeqInteger(): IEnumerable<integer>;
begin
  result := Range(1, GetInteger()).Select(e -> GetInteger()).ToArray();
end;  

function ReadSeqString(): IEnumerable<string>;
begin
  result := Range(1, GetInteger()).Select(e -> GetString()).ToArray();
end;           

procedure System.Collections.Generic.IEnumerable<T>.Write();
begin
  var b := self.ToArray();
  PT4.Put(b.Length);
  foreach e : T in b do
    PT4.Put(e);
end;

function System.Collections.Generic.IEnumerable<TSource>.Show
  (cmt: string; selector: System.Func<TSource, string>): 
  System.Collections.Generic.IEnumerable<TSource>;
begin
  var b := self.Select(selector).ToArray();
  PT4.Show(cmt);
  PT4.Show((b.Length + ':').PadLeft(3));
  foreach var e in b do
    PT4.Show(e);
  PT4.ShowLine();
  result := self; 
end;

function System.Collections.Generic.IEnumerable<TSource>.Show
  (selector: System.Func<TSource, string>): 
  System.Collections.Generic.IEnumerable<TSource>;
begin
  result := self.Show('', selector); 
end;

function System.Collections.Generic.IEnumerable<TSource>.Show(cmt: string): 
  System.Collections.Generic.IEnumerable<TSource>;
begin
  result := self.Show(cmt, e -> e.ToString()); 
end;

function System.Collections.Generic.IEnumerable<TSource>.Show(): 
  System.Collections.Generic.IEnumerable<TSource>;
begin
  result := self.Show(''); 
end;

function ReadAllLines(name: string): 
  System.Collections.Generic.IEnumerable<string>;
begin
  result := System.IO.File.ReadAllLines(name, Encoding.Default);
end;

procedure WriteAllLines(name: string; seq: 
  System.Collections.Generic.IEnumerable<string>); 
begin
  System.IO.File.WriteAllLines(name, seq.ToArray, Encoding.Default);
end;

end.