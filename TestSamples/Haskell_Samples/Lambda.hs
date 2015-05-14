module Main where

g 1 3 = \y -> y
g u v = \x -> x*x

lam x y z = x+y+z

h = (g 1)

l x = (lam x 2)

main = do {
            (print(g 2 3 4));
            (print(g 1 2 5));
            (print 2);
            (print(h 3 4));
            (print(l 3 5));
          }