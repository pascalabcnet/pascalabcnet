###
var a{@var a: sequence of class;@} := (1..18).Sel(i->new class(f := i, g := i.ToS));
var x{@var x: string;@} := a.First().g;