module Main where

ff x = x+1

g = \a b x -> ((a x) x)+(b 2)

f x y = x*y

main = do {
            (print 3);
            (print (g f ff (g f ff 5)  ) );
          }