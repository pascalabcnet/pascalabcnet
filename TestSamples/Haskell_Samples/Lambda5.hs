module Main where

f = ((\x -> x*x)5)+((\y z -> y*z)5 7)

d = f

g = \x y -> do {(print x+y);}

main = do {
            (print f);
            (print d);
            (g 1 2);
          }