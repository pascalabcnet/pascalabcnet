UNIT Three_D;

(* when viewing an object, the x-y axis is horizontal and z is vertical *)
INTERFACE
USES
  GraphABC;
VAR

  Longitude,                          (* angle <2Pi in x-y plane at which to
                                         view object                         *)
  Latitude,                           (* x-z plane angle at which to view
                                         object || 0
                                                                            *)
  Size,                               (* scalar > 0 to increase/decrease the
                                         object size                         *)
  Zoom,                                (* viewing angle in radians 0 < Zoom < Pi
                                                                             *)
  Distance                                                                            
                : DOUBLE;

PROCEDURE MapLine(X1, Y1, Z1, X2, Y2, Z2 : DOUBLE);
PROCEDURE SetViewer;

IMPLEMENTATION

VAR
  Ix, Iy, Iz,                         (* X to X' transformation              *)
  Jx, Jy,                             (* Y to Y' transformation              *)
  Kx, Ky, Kz                          (* Z to Z' transformation              *)
                : DOUBLE;

PROCEDURE SetViewer;                  (* this procedure will determine the
                                         coefficients of the rotation and
                                         translation functions               *)
VAR
  Temp1, Temp2  : DOUBLE;

BEGIN

(* start with "longitude" rotation on Z axis and scale by "Size" *)
  Jx := - Size * Sin(Longitude);      (* map x to y'                         *)
  Jy := Size * Cos(Longitude);        (* map y to y'                         *)

(* Jz := 0.0;                      z doesn't map to y', this term not used *)
(* x' y' and z' now rotate on y' axis by "latitude" radians *)
  Temp1 := Sin(Latitude);             (* compute sine once                   *)
  Temp2 := Cos(Latitude);             (* compute cosine once                 *)
  Ix := Jy * Temp2;                   (* map x to x' and incorporate latitude
                                                                             *)
  Iy := - Jx * Temp2;                 (* map y to x' and incorporate latitude
                                                                             *)
  Iz := Size * Temp1;                 (* map z to x' and incorporate latitude
                                                                             *)
  Kx := - Jy * Temp1;                 (* map x to z' and incorporate latitude
                                                                             *)
  Ky := Jx * Temp1;                   (* map y to z' and incorporate latitude
                                                                             *)
  Kz := Size * Temp2;                 (* map z to z' and incorporate latitude
                                                                             *)

(* y & z dot product scalar, Zoom is viewing angle in radians *)
  Temp1 := Zoom / 2.0;
  Temp2 := Cos(Temp1) / Sin(Temp1);
  Jx := Temp2 * Jx;
  Jy := Temp2 * Jy;
  Kx := Temp2 * Kx;
  Ky := Temp2 * Ky;
  Kz := Temp2 * Kz;
END;                                  (* SetViewer                           *)

PROCEDURE MapPoint(X1, Y1, Z1 : DOUBLE;
                   VAR X, Y : DOUBLE);

(* project point onto the focal plane *)
(* point may be out of the viewport... this case handled later *)
VAR
  Temp          : DOUBLE;
BEGIN                                 (* translate rotated object by
                                         "Distance" units                    *)
  Temp := 1.0 / (Distance - (Ix * X1 + Iy * Y1 + Iz * Z1));

(* X' translated and used to determine focal plane intersection.
   Focal plane is always 1 unit away.  Narrow viewing angle is different from
   increased "Size" *)
  X := Temp * (Jx * X1 + Jy * Y1);    (* dot product.. Y'maps to X dimension *)
  Y := Temp * (Kx * X1 + Ky * Y1 + Kz * Z1); (* dot prod. Z' maps to Y
                                         dimension                           *)
END;                                  (* MapPoint                            *)

PROCEDURE MapLine(X1, Y1, Z1, X2, Y2, Z2 : DOUBLE);
VAR
  P1, P2,                             (* have points 1 and 2 been assigned ? *)
  Left, Right, Top, Bot               (* does line segment intersect lines
                                         forming viewport?                   *)
                : BOOLEAN;
  XX1, YY1, XX2, YY2, ILeft, IRight, ITop, IBot : DOUBLE; (* those
                                         intersection points                 *)
  P1_X, P1_Y, P2_X, P2_Y : INTEGER;

BEGIN
  MapPoint(X1, Y1, Z1, XX1, YY1);     (* find point 1 on the focal plane     *)
  MapPoint(X2, Y2, Z2, XX2, YY2);     (* find point 2 on the focal plane     *)

(* these points must lie within the viewport which has limits 1 & -1 in the X
   and Y dimension *)
  P1_X := Round((XX1 + 1.5) * 200);   (* convert to display coordinates      *)
  P2_X := Round((XX2 + 1.5) * 200);
  P1_Y := Round((YY1 - 1.0) * - 200);
  P2_Y := Round((YY2 - 1.0) * - 200);
  Line(P1_X, P1_Y, P2_X, P2_Y);       (* draw the line on the display        *)
END;

BEGIN

 (****************************************************************************)
 (* initialize viewing variables                                             *)
 (****************************************************************************)
  Longitude := 0.0;                   (* x axis pointing at viewer           *)
  Latitude := 0.0;                    (* viewing the equator                 *)
  Distance := 3.5;                    (* 3.5 units away                      *)
  Size := 1.0;                        (* don't scale the object              *)
  Zoom := 1.0;                        (* 1 radian, approx 57 degrees         *)
  SetViewer;                          (* initialize these settings           *)
END.