module Main where

func (a:b:c) (q:r) = do {
                        (print a);
                        (print ';'); 
                        (print b);
                        (print ';'); 
                        (print c);
                        (print '*'); 
                        (print q);
                        (print ';'); 
                        (print r);
                      }

main = do {
            (func [4,5,8,7] [2,3]);
          }