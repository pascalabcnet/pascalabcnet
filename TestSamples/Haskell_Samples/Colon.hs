module Main where

myprint a:b:c:d = do {
                              (print a);
                              (print b);
                              (print c);
                              (print d);
                           }

main = do {
            (myprint [4,5,8,7]);
          }