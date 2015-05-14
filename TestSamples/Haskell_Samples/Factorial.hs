module Main where

factor 1 = 1
factor i = (factor i-1)*i

main = do { 
              (print(factor 100));
              (print 999999*99999*99999*9999999*99999999);
          }