module Main where

f x y = x*y

ff x y z = x*y*z

h x y z t= x+y+z+t

g a b 1 = 1
g a b x = (a x x)+(b x x x)+((h 1 2 3) 3)

p a = (a 1 2) 

m = (p ((h 1) 5) )

my x y = x              

main = do {
            (print m);
            (print((((h 4) 5) 6) 7));
            (print (g f ff 3));
          }