module Main where

func (g:h:k) a = do {(print g+a);(print ';'); (print h);(print ';'); (print k);}

main = do {
            (func [4,5,8,7] 3);
          }