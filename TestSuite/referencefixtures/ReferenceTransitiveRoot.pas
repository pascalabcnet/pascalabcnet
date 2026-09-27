library ReferenceTransitiveRoot;

{$reference ReferenceTransitiveLeaf.dll}

type
  RootMarker = class(ReferenceTransitiveLeaf.LeafMarker)
  end;

end.
