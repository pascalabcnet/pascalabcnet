module Main where

myPrint a = do {
                  (print a);
               }
main = do {
            (myPrint 1);
            (myPrint "Hello, world!");
            (myPrint [1,2,3,4,5,6,7,8]);
          }