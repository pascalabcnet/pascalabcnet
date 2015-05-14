module Main where

g = \x y z -> x+y+z

h = (g 2)

f = let x=8;y=9 in x*y

main = do {
            (print(g 2 3 4));
            (print(h 3 4));
            (print f);
          }