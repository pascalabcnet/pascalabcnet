module Main where

f = ((\x -> x*x) 5)+((\y z -> y*z)5 7)

h a = a

g a = (h a) 

p = \x -> do {(print x);}

main = do {
            (print f);
            (print(g 5));
            (p 8);
          }