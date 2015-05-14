unit OpenGL;

{$reference 'System.Windows.Forms.dll'}
interface

uses System, System.Windows.Forms;

//uses Windows;

type
  THandle = longword;
  {$EXTERNALSYM HGLRC}
  HGLRC = THandle;
  BOOL = longword;
  
type
  PChar = ^byte;
  PWChar = ^char;
  BYTEBOOL = boolean;
  GLenum = Cardinal;
  {$EXTERNALSYM GLenum}
  GLboolean = BYTEBOOL;
  {$EXTERNALSYM GLboolean}
  GLbitfield = Cardinal;
  {$EXTERNALSYM GLbitfield}
  GLbyte = Shortint;   { signed char }
  {$EXTERNALSYM GLbyte}
  GLshort = SmallInt;
  {$EXTERNALSYM GLshort}
  GLint = Integer;
  {$EXTERNALSYM GLint}
  GLsizei = Integer;
  {$EXTERNALSYM GLsizei}
  GLubyte = Byte;
  {$EXTERNALSYM GLubyte}
  GLushort = Word;
  {$EXTERNALSYM GLushort}
  GLuint = Cardinal;
  {$EXTERNALSYM GLuint}
  GLfloat = Single;
  {$EXTERNALSYM GLfloat}
  GLclampf = Single;
  {$EXTERNALSYM GLclampf}
  GLdouble = Double;
  {$EXTERNALSYM GLdouble}
  GLclampd = Double;
  {$EXTERNALSYM GLclampd}
  HDC = longword;
  HWND = longword;
  
  PGLBoolean = ^GLBoolean;
  PGLByte = ^GLByte;
  PGLShort = ^GLShort;
  PGLInt = ^GLInt;
  PGLSizei = ^GLSizei;
  PGLubyte = ^GLubyte;
  PGLushort = ^GLushort;
  PGLuint = ^GLuint;
  PGLclampf = ^GLclampf;
  PGLfloat =  ^GLFloat;
  PGLdouble = ^GLDouble;
  PGLclampd = ^GLclampd;

  TGLArrayf4 = array [0..3] of GLFloat;
  TGLArrayf3 = array [0..2] of GLFloat;
  TGLArrayi4 = array [0..3] of GLint;
  {...}

{*************************************************************}

PIXELFORMATDESCRIPTOR = record
    nSize: Word;
    nVersion: Word;
    dwFlags: longword;
    iPixelType: Byte;
    cColorBits: Byte;
    cRedBits: Byte;
    cRedShift: Byte;
    cGreenBits: Byte;
    cGreenShift: Byte;
    cBlueBits: Byte;
    cBlueShift: Byte;
    cAlphaBits: Byte;
    cAlphaShift: Byte;
    cAccumBits: Byte;
    cAccumRedBits: Byte;
    cAccumGreenBits: Byte;
    cAccumBlueBits: Byte;
    cAccumAlphaBits: Byte;
    cDepthBits: Byte;
    cStencilBits: Byte;
    cAuxBuffers: Byte;
    iLayerType: Byte;
    bReserved: Byte;
    dwLayerMask: longword;
    dwVisibleMask: longword;
    dwDamageMask: longword;
  end;

const
{ AttribMask }
  GL_CURRENT_BIT                      = $00000001;
  {$EXTERNALSYM GL_CURRENT_BIT}
  GL_POINT_BIT                        = $00000002;
  {$EXTERNALSYM GL_POINT_BIT}
  GL_LINE_BIT                         = $00000004;
  {$EXTERNALSYM GL_LINE_BIT}
  GL_POLYGON_BIT                      = $00000008;
  {$EXTERNALSYM GL_POLYGON_BIT}
  GL_POLYGON_STIPPLE_BIT              = $00000010;
  {$EXTERNALSYM GL_POLYGON_STIPPLE_BIT}
  GL_PIXEL_MODE_BIT                   = $00000020;
  {$EXTERNALSYM GL_PIXEL_MODE_BIT}
  GL_LIGHTING_BIT                     = $00000040;
  {$EXTERNALSYM GL_LIGHTING_BIT}
  GL_FOG_BIT                          = $00000080;
  {$EXTERNALSYM GL_FOG_BIT}
  GL_DEPTH_BUFFER_BIT                 = $00000100;
  {$EXTERNALSYM GL_DEPTH_BUFFER_BIT}
  GL_ACCUM_BUFFER_BIT                 = $00000200;
  {$EXTERNALSYM GL_ACCUM_BUFFER_BIT}
  GL_STENCIL_BUFFER_BIT               = $00000400;
  {$EXTERNALSYM GL_STENCIL_BUFFER_BIT}
  GL_VIEWPORT_BIT                     = $00000800;
  {$EXTERNALSYM GL_VIEWPORT_BIT}
  GL_TRANSFORM_BIT                    = $00001000;
  {$EXTERNALSYM GL_TRANSFORM_BIT}
  GL_ENABLE_BIT                       = $00002000;
  {$EXTERNALSYM GL_ENABLE_BIT}
  GL_COLOR_BUFFER_BIT                 = $00004000;
  {$EXTERNALSYM GL_COLOR_BUFFER_BIT}
  GL_HINT_BIT                         = $00008000;
  {$EXTERNALSYM GL_HINT_BIT}
  GL_EVAL_BIT                         = $00010000;
  {$EXTERNALSYM GL_EVAL_BIT}
  GL_LIST_BIT                         = $00020000;
  {$EXTERNALSYM GL_LIST_BIT}
  GL_TEXTURE_BIT                      = $00040000;
  {$EXTERNALSYM GL_TEXTURE_BIT}
  GL_SCISSOR_BIT                      = $00080000;
  {$EXTERNALSYM GL_SCISSOR_BIT}
  GL_ALL_ATTRIB_BITS                  = $000fffff;
  {$EXTERNALSYM GL_ALL_ATTRIB_BITS}

{ ClearBufferMask }
{      GL_COLOR_BUFFER_BIT }
{      GL_ACCUM_BUFFER_BIT }
{      GL_STENCIL_BUFFER_BIT }
{      GL_DEPTH_BUFFER_BIT }

{ Boolean }
  GL_FALSE                            = false;
  {$EXTERNALSYM GL_FALSE}
  GL_TRUE                             = true;
  {$EXTERNALSYM GL_TRUE}

{ BeginMode }
  GL_POINTS                           = $0000    ;
  {$EXTERNALSYM GL_POINTS}
  GL_LINES                            = $0001    ;
  {$EXTERNALSYM GL_LINES}
  GL_LINE_LOOP                        = $0002    ;
  {$EXTERNALSYM GL_LINE_LOOP}
  GL_LINE_STRIP                       = $0003    ;
  {$EXTERNALSYM GL_LINE_STRIP}
  GL_TRIANGLES                        = $0004    ;
  {$EXTERNALSYM GL_TRIANGLES}
  GL_TRIANGLE_STRIP                   = $0005    ;
  {$EXTERNALSYM GL_TRIANGLE_STRIP}
  GL_TRIANGLE_FAN                     = $0006    ;
  {$EXTERNALSYM GL_TRIANGLE_FAN}
  GL_QUADS                            = $0007    ;
  {$EXTERNALSYM GL_QUADS}
  GL_QUAD_STRIP                       = $0008    ;
  {$EXTERNALSYM GL_QUAD_STRIP}
  GL_POLYGON                          = $0009    ;
  {$EXTERNALSYM GL_POLYGON}

{ AccumOp }
  GL_ACCUM                            = $0100;
  {$EXTERNALSYM GL_ACCUM}
  GL_LOAD                             = $0101;
  {$EXTERNALSYM GL_LOAD}
  GL_RETURN                           = $0102;
  {$EXTERNALSYM GL_RETURN}
  GL_MULT                             = $0103;
  {$EXTERNALSYM GL_MULT}
  GL_ADD                              = $0104;
  {$EXTERNALSYM GL_ADD}

{ AlphaFunction }
  GL_NEVER                            = $0200;
  {$EXTERNALSYM GL_NEVER}
  GL_LESS                             = $0201;
  {$EXTERNALSYM GL_LESS}
  GL_EQUAL                            = $0202;
  {$EXTERNALSYM GL_EQUAL}
  GL_LEQUAL                           = $0203;
  {$EXTERNALSYM GL_LEQUAL}
  GL_GREATER                          = $0204;
  {$EXTERNALSYM GL_GREATER}
  GL_NOTEQUAL                         = $0205;
  {$EXTERNALSYM GL_NOTEQUAL}
  GL_GEQUAL                           = $0206;
  {$EXTERNALSYM GL_GEQUAL}
  GL_ALWAYS                           = $0207;
  {$EXTERNALSYM GL_ALWAYS}

{ BlendingFactorDest }
  GL_ZERO                             = 0;
  {$EXTERNALSYM GL_ZERO}
  GL_ONE                              = 1;
  {$EXTERNALSYM GL_ONE}
  GL_SRC_COLOR                        = $0300;
  {$EXTERNALSYM GL_SRC_COLOR}
  GL_ONE_MINUS_SRC_COLOR              = $0301;
  {$EXTERNALSYM GL_ONE_MINUS_SRC_COLOR}
  GL_SRC_ALPHA                        = $0302;
  {$EXTERNALSYM GL_SRC_ALPHA}
  GL_ONE_MINUS_SRC_ALPHA              = $0303;
  {$EXTERNALSYM GL_ONE_MINUS_SRC_ALPHA}
  GL_DST_ALPHA                        = $0304;
  {$EXTERNALSYM GL_DST_ALPHA}
  GL_ONE_MINUS_DST_ALPHA              = $0305;
  {$EXTERNALSYM GL_ONE_MINUS_DST_ALPHA}

{ BlendingFactorSrc }
{      GL_ZERO }
{      GL_ONE }
  GL_DST_COLOR                        = $0306;
  {$EXTERNALSYM GL_DST_COLOR}
  GL_ONE_MINUS_DST_COLOR              = $0307;
  {$EXTERNALSYM GL_ONE_MINUS_DST_COLOR}
  GL_SRC_ALPHA_SATURATE               = $0308;
  {$EXTERNALSYM GL_SRC_ALPHA_SATURATE}
{      GL_SRC_ALPHA }
{      GL_ONE_MINUS_SRC_ALPHA }
{      GL_DST_ALPHA }
{      GL_ONE_MINUS_DST_ALPHA }

{ BlendingMode }
{      GL_LOGIC_OP }

{ ColorMaterialFace }
{      GL_FRONT }
{      GL_BACK }
{      GL_FRONT_AND_BACK }

{ ColorMaterialParameter }
{      GL_AMBIENT }
{      GL_DIFFUSE }
{      GL_SPECULAR }
{      GL_EMISSION }
{      GL_AMBIENT_AND_DIFFUSE }

{ CullFaceMode }
{      GL_FRONT }
{      GL_BACK }
{      GL_FRONT_AND_BACK }

{ DepthFunction }
{      GL_NEVER }
{      GL_LESS }
{      GL_EQUAL }
{      GL_LEQUAL }
{      GL_GREATER }
{      GL_NOTEQUAL }
{      GL_GEQUAL }
{      GL_ALWAYS }

{ DrawBufferMode }
  GL_NONE                             = 0;
  {$EXTERNALSYM GL_NONE}
  GL_FRONT_LEFT                       = $0400;
  {$EXTERNALSYM GL_FRONT_LEFT}
  GL_FRONT_RIGHT                      = $0401;
  {$EXTERNALSYM GL_FRONT_RIGHT}
  GL_BACK_LEFT                        = $0402;
  {$EXTERNALSYM GL_BACK_LEFT}
  GL_BACK_RIGHT                       = $0403;
  {$EXTERNALSYM GL_BACK_RIGHT}
  GL_FRONT                            = $0404;
  {$EXTERNALSYM GL_FRONT}
  GL_BACK                             = $0405;
  {$EXTERNALSYM GL_BACK}
  GL_LEFT                             = $0406;
  {$EXTERNALSYM GL_LEFT}
  GL_RIGHT                            = $0407;
  {$EXTERNALSYM GL_RIGHT}
  GL_FRONT_AND_BACK                   = $0408;
  {$EXTERNALSYM GL_FRONT_AND_BACK}
  GL_AUX0                             = $0409;
  {$EXTERNALSYM GL_AUX0}
  GL_AUX1                             = $040A;
  {$EXTERNALSYM GL_AUX1}
  GL_AUX2                             = $040B;
  {$EXTERNALSYM GL_AUX2}
  GL_AUX3                             = $040C;
  {$EXTERNALSYM GL_AUX3}

{ ErrorCode }
  GL_NO_ERROR                         = 0;
  {$EXTERNALSYM GL_NO_ERROR}
  GL_INVALID_ENUM                     = $0500;
  {$EXTERNALSYM GL_INVALID_ENUM}
  GL_INVALID_VALUE                    = $0501;
  {$EXTERNALSYM GL_INVALID_VALUE}
  GL_INVALID_OPERATION                = $0502;
  {$EXTERNALSYM GL_INVALID_OPERATION}
  GL_STACK_OVERFLOW                   = $0503;
  {$EXTERNALSYM GL_STACK_OVERFLOW}
  GL_STACK_UNDERFLOW                  = $0504;
  {$EXTERNALSYM GL_STACK_UNDERFLOW}
  GL_OUT_OF_MEMORY                    = $0505;
  {$EXTERNALSYM GL_OUT_OF_MEMORY}

{ FeedBackMode }
  GL_2D                               = $0600;
  {$EXTERNALSYM GL_2D}
  GL_3D                               = $0601;
  {$EXTERNALSYM GL_3D}
  GL_3D_COLOR                         = $0602;
  {$EXTERNALSYM GL_3D_COLOR}
  GL_3D_COLOR_TEXTURE                 = $0603;
  {$EXTERNALSYM GL_3D_COLOR_TEXTURE}
  GL_4D_COLOR_TEXTURE                 = $0604;
  {$EXTERNALSYM GL_4D_COLOR_TEXTURE}

{ FeedBackToken }
  GL_PASS_THROUGH_TOKEN               = $0700;
  {$EXTERNALSYM GL_PASS_THROUGH_TOKEN}
  GL_POINT_TOKEN                      = $0701;
  {$EXTERNALSYM GL_POINT_TOKEN}
  GL_LINE_TOKEN                       = $0702;
  {$EXTERNALSYM GL_LINE_TOKEN}
  GL_POLYGON_TOKEN                    = $0703;
  {$EXTERNALSYM GL_POLYGON_TOKEN}
  GL_BITMAP_TOKEN                     = $0704;
  {$EXTERNALSYM GL_BITMAP_TOKEN}
  GL_DRAW_PIXEL_TOKEN                 = $0705;
  {$EXTERNALSYM GL_DRAW_PIXEL_TOKEN}
  GL_COPY_PIXEL_TOKEN                 = $0706;
  {$EXTERNALSYM GL_COPY_PIXEL_TOKEN}
  GL_LINE_RESET_TOKEN                 = $0707;
  {$EXTERNALSYM GL_LINE_RESET_TOKEN}

{ FogMode }
{      GL_LINEAR }
  GL_EXP                              = $0800;
  {$EXTERNALSYM GL_EXP}
  GL_EXP2                             = $0801;
  {$EXTERNALSYM GL_EXP2}

{ FogParameter }
{      GL_FOG_COLOR }
{      GL_FOG_DENSITY }
{      GL_FOG_END }
{      GL_FOG_INDEX }
{      GL_FOG_MODE }
{      GL_FOG_START }

{ FrontFaceDirection }
  GL_CW                               = $0900;
  {$EXTERNALSYM GL_CW}
  GL_CCW                              = $0901;
  {$EXTERNALSYM GL_CCW}

{ GetMapTarget }
  GL_COEFF                            = $0A00;
  {$EXTERNALSYM GL_COEFF}
  GL_ORDER                            = $0A01;
  {$EXTERNALSYM GL_ORDER}
  GL_DOMAIN                           = $0A02;
  {$EXTERNALSYM GL_DOMAIN}

{ GetPixelMap }
  GL_PIXEL_MAP_I_TO_I                 = $0C70;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_I}
  GL_PIXEL_MAP_S_TO_S                 = $0C71;
  {$EXTERNALSYM GL_PIXEL_MAP_S_TO_S}
  GL_PIXEL_MAP_I_TO_R                 = $0C72;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_R}
  GL_PIXEL_MAP_I_TO_G                 = $0C73;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_G}
  GL_PIXEL_MAP_I_TO_B                 = $0C74;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_B}
  GL_PIXEL_MAP_I_TO_A                 = $0C75;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_A}
  GL_PIXEL_MAP_R_TO_R                 = $0C76;
  {$EXTERNALSYM GL_PIXEL_MAP_R_TO_R}
  GL_PIXEL_MAP_G_TO_G                 = $0C77;
  {$EXTERNALSYM GL_PIXEL_MAP_G_TO_G}
  GL_PIXEL_MAP_B_TO_B                 = $0C78;
  {$EXTERNALSYM GL_PIXEL_MAP_B_TO_B}
  GL_PIXEL_MAP_A_TO_A                 = $0C79;
  {$EXTERNALSYM GL_PIXEL_MAP_A_TO_A}

{ GetTarget }
  GL_CURRENT_COLOR                    = $0B00;
  {$EXTERNALSYM GL_CURRENT_COLOR}
  GL_CURRENT_INDEX                    = $0B01;
  {$EXTERNALSYM GL_CURRENT_INDEX}
  GL_CURRENT_NORMAL                   = $0B02;
  {$EXTERNALSYM GL_CURRENT_NORMAL}
  GL_CURRENT_TEXTURE_COORDS           = $0B03;
  {$EXTERNALSYM GL_CURRENT_TEXTURE_COORDS}
  GL_CURRENT_RASTER_COLOR             = $0B04;
  {$EXTERNALSYM GL_CURRENT_RASTER_COLOR}
  GL_CURRENT_RASTER_INDEX             = $0B05;
  {$EXTERNALSYM GL_CURRENT_RASTER_INDEX}
  GL_CURRENT_RASTER_TEXTURE_COORDS    = $0B06;
  {$EXTERNALSYM GL_CURRENT_RASTER_TEXTURE_COORDS}
  GL_CURRENT_RASTER_POSITION          = $0B07;
  {$EXTERNALSYM GL_CURRENT_RASTER_POSITION}
  GL_CURRENT_RASTER_POSITION_VALID    = $0B08;
  {$EXTERNALSYM GL_CURRENT_RASTER_POSITION_VALID}
  GL_CURRENT_RASTER_DISTANCE          = $0B09;
  {$EXTERNALSYM GL_CURRENT_RASTER_DISTANCE}
  GL_POINT_SMOOTH                     = $0B10;
  {$EXTERNALSYM GL_POINT_SMOOTH}
  GL_POINT_SIZE                       = $0B11;
  {$EXTERNALSYM GL_POINT_SIZE}
  GL_POINT_SIZE_RANGE                 = $0B12;
  {$EXTERNALSYM GL_POINT_SIZE_RANGE}
  GL_POINT_SIZE_GRANULARITY           = $0B13;
  {$EXTERNALSYM GL_POINT_SIZE_GRANULARITY}
  GL_LINE_SMOOTH                      = $0B20;
  {$EXTERNALSYM GL_LINE_SMOOTH}
  GL_LINE_WIDTH                       = $0B21;
  {$EXTERNALSYM GL_LINE_WIDTH}
  GL_LINE_WIDTH_RANGE                 = $0B22;
  {$EXTERNALSYM GL_LINE_WIDTH_RANGE}
  GL_LINE_WIDTH_GRANULARITY           = $0B23;
  {$EXTERNALSYM GL_LINE_WIDTH_GRANULARITY}
  GL_LINE_STIPPLE                     = $0B24;
  {$EXTERNALSYM GL_LINE_STIPPLE}
  GL_LINE_STIPPLE_PATTERN             = $0B25;
  {$EXTERNALSYM GL_LINE_STIPPLE_PATTERN}
  GL_LINE_STIPPLE_REPEAT              = $0B26;
  {$EXTERNALSYM GL_LINE_STIPPLE_REPEAT}
  GL_LIST_MODE                        = $0B30;
  {$EXTERNALSYM GL_LIST_MODE}
  GL_MAX_LIST_NESTING                 = $0B31;
  {$EXTERNALSYM GL_MAX_LIST_NESTING}
  GL_LIST_BASE                        = $0B32;
  {$EXTERNALSYM GL_LIST_BASE}
  GL_LIST_INDEX                       = $0B33;
  {$EXTERNALSYM GL_LIST_INDEX}
  GL_POLYGON_MODE                     = $0B40;
  {$EXTERNALSYM GL_POLYGON_MODE}
  GL_POLYGON_SMOOTH                   = $0B41;
  {$EXTERNALSYM GL_POLYGON_SMOOTH}
  GL_POLYGON_STIPPLE                  = $0B42;
  {$EXTERNALSYM GL_POLYGON_STIPPLE}
  GL_EDGE_FLAG                        = $0B43;
  {$EXTERNALSYM GL_EDGE_FLAG}
  GL_CULL_FACE                        = $0B44;
  {$EXTERNALSYM GL_CULL_FACE}
  GL_CULL_FACE_MODE                   = $0B45;
  {$EXTERNALSYM GL_CULL_FACE_MODE}
  GL_FRONT_FACE                       = $0B46;
  {$EXTERNALSYM GL_FRONT_FACE}
  GL_LIGHTING                         = $0B50;
  {$EXTERNALSYM GL_LIGHTING}
  GL_LIGHT_MODEL_LOCAL_VIEWER         = $0B51;
  {$EXTERNALSYM GL_LIGHT_MODEL_LOCAL_VIEWER}
  GL_LIGHT_MODEL_TWO_SIDE             = $0B52;
  {$EXTERNALSYM GL_LIGHT_MODEL_TWO_SIDE}
  GL_LIGHT_MODEL_AMBIENT              = $0B53;
  {$EXTERNALSYM GL_LIGHT_MODEL_AMBIENT}
  GL_SHADE_MODEL                      = $0B54;
  {$EXTERNALSYM GL_SHADE_MODEL}
  GL_COLOR_MATERIAL_FACE              = $0B55;
  {$EXTERNALSYM GL_COLOR_MATERIAL_FACE}
  GL_COLOR_MATERIAL_PARAMETER         = $0B56;
  {$EXTERNALSYM GL_COLOR_MATERIAL_PARAMETER}
  GL_COLOR_MATERIAL                   = $0B57;
  {$EXTERNALSYM GL_COLOR_MATERIAL}
  GL_FOG                              = $0B60;
  {$EXTERNALSYM GL_FOG}
  GL_FOG_INDEX                        = $0B61;
  {$EXTERNALSYM GL_FOG_INDEX}
  GL_FOG_DENSITY                      = $0B62;
  {$EXTERNALSYM GL_FOG_DENSITY}
  GL_FOG_START                        = $0B63;
  {$EXTERNALSYM GL_FOG_START}
  GL_FOG_END                          = $0B64;
  {$EXTERNALSYM GL_FOG_END}
  GL_FOG_MODE                         = $0B65;
  {$EXTERNALSYM GL_FOG_MODE}
  GL_FOG_COLOR                        = $0B66;
  {$EXTERNALSYM GL_FOG_COLOR}
  GL_DEPTH_RANGE                      = $0B70;
  {$EXTERNALSYM GL_DEPTH_RANGE}
  GL_DEPTH_TEST                       = $0B71;
  {$EXTERNALSYM GL_DEPTH_TEST}
  GL_DEPTH_WRITEMASK                  = $0B72;
  {$EXTERNALSYM GL_DEPTH_WRITEMASK}
  GL_DEPTH_CLEAR_VALUE                = $0B73;
  {$EXTERNALSYM GL_DEPTH_CLEAR_VALUE}
  GL_DEPTH_FUNC                       = $0B74;
  {$EXTERNALSYM GL_DEPTH_FUNC}
  GL_ACCUM_CLEAR_VALUE                = $0B80;
  {$EXTERNALSYM GL_ACCUM_CLEAR_VALUE}
  GL_STENCIL_TEST                     = $0B90;
  {$EXTERNALSYM GL_STENCIL_TEST}
  GL_STENCIL_CLEAR_VALUE              = $0B91;
  {$EXTERNALSYM GL_STENCIL_CLEAR_VALUE}
  GL_STENCIL_FUNC                     = $0B92;
  {$EXTERNALSYM GL_STENCIL_FUNC}
  GL_STENCIL_VALUE_MASK               = $0B93;
  {$EXTERNALSYM GL_STENCIL_VALUE_MASK}
  GL_STENCIL_FAIL                     = $0B94;
  {$EXTERNALSYM GL_STENCIL_FAIL}
  GL_STENCIL_PASS_DEPTH_FAIL          = $0B95;
  {$EXTERNALSYM GL_STENCIL_PASS_DEPTH_FAIL}
  GL_STENCIL_PASS_DEPTH_PASS          = $0B96;
  {$EXTERNALSYM GL_STENCIL_PASS_DEPTH_PASS}
  GL_STENCIL_REF                      = $0B97;
  {$EXTERNALSYM GL_STENCIL_REF}
  GL_STENCIL_WRITEMASK                = $0B98;
  {$EXTERNALSYM GL_STENCIL_WRITEMASK}
  GL_MATRIX_MODE                      = $0BA0;
  {$EXTERNALSYM GL_MATRIX_MODE}
  GL_NORMALIZE                        = $0BA1;
  {$EXTERNALSYM GL_NORMALIZE}
  GL_VIEWPORT                         = $0BA2;
  {$EXTERNALSYM GL_VIEWPORT}
  GL_MODELVIEW_STACK_DEPTH            = $0BA3;
  {$EXTERNALSYM GL_MODELVIEW_STACK_DEPTH}
  GL_PROJECTION_STACK_DEPTH           = $0BA4;
  {$EXTERNALSYM GL_PROJECTION_STACK_DEPTH}
  GL_TEXTURE_STACK_DEPTH              = $0BA5;
  {$EXTERNALSYM GL_TEXTURE_STACK_DEPTH}
  GL_MODELVIEW_MATRIX                 = $0BA6;
  {$EXTERNALSYM GL_MODELVIEW_MATRIX}
  GL_PROJECTION_MATRIX                = $0BA7;
  {$EXTERNALSYM GL_PROJECTION_MATRIX}
  GL_TEXTURE_MATRIX                   = $0BA8;
  {$EXTERNALSYM GL_TEXTURE_MATRIX}
  GL_ATTRIB_STACK_DEPTH               = $0BB0;
  {$EXTERNALSYM GL_ATTRIB_STACK_DEPTH}
  GL_ALPHA_TEST                       = $0BC0;
  {$EXTERNALSYM GL_ALPHA_TEST}
  GL_ALPHA_TEST_FUNC                  = $0BC1;
  {$EXTERNALSYM GL_ALPHA_TEST_FUNC}
  GL_ALPHA_TEST_REF                   = $0BC2;
  {$EXTERNALSYM GL_ALPHA_TEST_REF}
  GL_DITHER                           = $0BD0;
  {$EXTERNALSYM GL_DITHER}
  GL_BLEND_DST                        = $0BE0;
  {$EXTERNALSYM GL_BLEND_DST}
  GL_BLEND_SRC                        = $0BE1;
  {$EXTERNALSYM GL_BLEND_SRC}
  GL_BLEND                            = $0BE2;
  {$EXTERNALSYM GL_BLEND}
  GL_LOGIC_OP_MODE                    = $0BF0;
  {$EXTERNALSYM GL_LOGIC_OP_MODE}
  GL_LOGIC_OP                         = $0BF1;
  {$EXTERNALSYM GL_LOGIC_OP}
  GL_AUX_BUFFERS                      = $0C00;
  {$EXTERNALSYM GL_AUX_BUFFERS}
  GL_DRAW_BUFFER                      = $0C01;
  {$EXTERNALSYM GL_DRAW_BUFFER}
  GL_READ_BUFFER                      = $0C02;
  {$EXTERNALSYM GL_READ_BUFFER}
  GL_SCISSOR_BOX                      = $0C10;
  {$EXTERNALSYM GL_SCISSOR_BOX}
  GL_SCISSOR_TEST                     = $0C11;
  {$EXTERNALSYM GL_SCISSOR_TEST}
  GL_INDEX_CLEAR_VALUE                = $0C20;
  {$EXTERNALSYM GL_INDEX_CLEAR_VALUE}
  GL_INDEX_WRITEMASK                  = $0C21;
  {$EXTERNALSYM GL_INDEX_WRITEMASK}
  GL_COLOR_CLEAR_VALUE                = $0C22;
  {$EXTERNALSYM GL_COLOR_CLEAR_VALUE}
  GL_COLOR_WRITEMASK                  = $0C23;
  {$EXTERNALSYM GL_COLOR_WRITEMASK}
  GL_INDEX_MODE                       = $0C30;
  {$EXTERNALSYM GL_INDEX_MODE}
  GL_RGBA_MODE                        = $0C31;
  {$EXTERNALSYM GL_RGBA_MODE}
  GL_DOUBLEBUFFER                     = $0C32;
  {$EXTERNALSYM GL_DOUBLEBUFFER}
  GL_STEREO                           = $0C33;
  {$EXTERNALSYM GL_STEREO}
  GL_RENDER_MODE                      = $0C40;
  {$EXTERNALSYM GL_RENDER_MODE}
  GL_PERSPECTIVE_CORRECTION_HINT      = $0C50;
  {$EXTERNALSYM GL_PERSPECTIVE_CORRECTION_HINT}
  GL_POINT_SMOOTH_HINT                = $0C51;
  {$EXTERNALSYM GL_POINT_SMOOTH_HINT}
  GL_LINE_SMOOTH_HINT                 = $0C52;
  {$EXTERNALSYM GL_LINE_SMOOTH_HINT}
  GL_POLYGON_SMOOTH_HINT              = $0C53;
  {$EXTERNALSYM GL_POLYGON_SMOOTH_HINT}
  GL_FOG_HINT                         = $0C54;
  {$EXTERNALSYM GL_FOG_HINT}
  GL_TEXTURE_GEN_S                    = $0C60;
  {$EXTERNALSYM GL_TEXTURE_GEN_S}
  GL_TEXTURE_GEN_T                    = $0C61;
  {$EXTERNALSYM GL_TEXTURE_GEN_T}
  GL_TEXTURE_GEN_R                    = $0C62;
  {$EXTERNALSYM GL_TEXTURE_GEN_R}
  GL_TEXTURE_GEN_Q                    = $0C63;
  {$EXTERNALSYM GL_TEXTURE_GEN_Q}
  GL_PIXEL_MAP_I_TO_I_SIZE            = $0CB0;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_I_SIZE}
  GL_PIXEL_MAP_S_TO_S_SIZE            = $0CB1;
  {$EXTERNALSYM GL_PIXEL_MAP_S_TO_S_SIZE}
  GL_PIXEL_MAP_I_TO_R_SIZE            = $0CB2;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_R_SIZE}
  GL_PIXEL_MAP_I_TO_G_SIZE            = $0CB3;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_G_SIZE}
  GL_PIXEL_MAP_I_TO_B_SIZE            = $0CB4;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_B_SIZE}
  GL_PIXEL_MAP_I_TO_A_SIZE            = $0CB5;
  {$EXTERNALSYM GL_PIXEL_MAP_I_TO_A_SIZE}
  GL_PIXEL_MAP_R_TO_R_SIZE            = $0CB6;
  {$EXTERNALSYM GL_PIXEL_MAP_R_TO_R_SIZE}
  GL_PIXEL_MAP_G_TO_G_SIZE            = $0CB7;
  {$EXTERNALSYM GL_PIXEL_MAP_G_TO_G_SIZE}
  GL_PIXEL_MAP_B_TO_B_SIZE            = $0CB8;
  {$EXTERNALSYM GL_PIXEL_MAP_B_TO_B_SIZE}
  GL_PIXEL_MAP_A_TO_A_SIZE            = $0CB9;
  {$EXTERNALSYM GL_PIXEL_MAP_A_TO_A_SIZE}
  GL_UNPACK_SWAP_BYTES                = $0CF0;
  {$EXTERNALSYM GL_UNPACK_SWAP_BYTES}
  GL_UNPACK_LSB_FIRST                 = $0CF1;
  {$EXTERNALSYM GL_UNPACK_LSB_FIRST}
  GL_UNPACK_ROW_LENGTH                = $0CF2;
  {$EXTERNALSYM GL_UNPACK_ROW_LENGTH}
  GL_UNPACK_SKIP_ROWS                 = $0CF3;
  {$EXTERNALSYM GL_UNPACK_SKIP_ROWS}
  GL_UNPACK_SKIP_PIXELS               = $0CF4;
  {$EXTERNALSYM GL_UNPACK_SKIP_PIXELS}
  GL_UNPACK_ALIGNMENT                 = $0CF5;
  {$EXTERNALSYM GL_UNPACK_ALIGNMENT}
  GL_PACK_SWAP_BYTES                  = $0D00;
  {$EXTERNALSYM GL_PACK_SWAP_BYTES}
  GL_PACK_LSB_FIRST                   = $0D01;
  {$EXTERNALSYM GL_PACK_LSB_FIRST}
  GL_PACK_ROW_LENGTH                  = $0D02;
  {$EXTERNALSYM GL_PACK_ROW_LENGTH}
  GL_PACK_SKIP_ROWS                   = $0D03;
  {$EXTERNALSYM GL_PACK_SKIP_ROWS}
  GL_PACK_SKIP_PIXELS                 = $0D04;
  {$EXTERNALSYM GL_PACK_SKIP_PIXELS}
  GL_PACK_ALIGNMENT                   = $0D05;
  {$EXTERNALSYM GL_PACK_ALIGNMENT}
  GL_MAP_COLOR                        = $0D10;
  {$EXTERNALSYM GL_MAP_COLOR}
  GL_MAP_STENCIL                      = $0D11;
  {$EXTERNALSYM GL_MAP_STENCIL}
  GL_INDEX_SHIFT                      = $0D12;
  {$EXTERNALSYM GL_INDEX_SHIFT}
  GL_INDEX_OFFSET                     = $0D13;
  {$EXTERNALSYM GL_INDEX_OFFSET}
  GL_RED_SCALE                        = $0D14;
  {$EXTERNALSYM GL_RED_SCALE}
  GL_RED_BIAS                         = $0D15;
  {$EXTERNALSYM GL_RED_BIAS}
  GL_ZOOM_X                           = $0D16;
  {$EXTERNALSYM GL_ZOOM_X}
  GL_ZOOM_Y                           = $0D17;
  {$EXTERNALSYM GL_ZOOM_Y}
  GL_GREEN_SCALE                      = $0D18;
  {$EXTERNALSYM GL_GREEN_SCALE}
  GL_GREEN_BIAS                       = $0D19;
  {$EXTERNALSYM GL_GREEN_BIAS}
  GL_BLUE_SCALE                       = $0D1A;
  {$EXTERNALSYM GL_BLUE_SCALE}
  GL_BLUE_BIAS                        = $0D1B;
  {$EXTERNALSYM GL_BLUE_BIAS}
  GL_ALPHA_SCALE                      = $0D1C;
  {$EXTERNALSYM GL_ALPHA_SCALE}
  GL_ALPHA_BIAS                       = $0D1D;
  {$EXTERNALSYM GL_ALPHA_BIAS}
  GL_DEPTH_SCALE                      = $0D1E;
  {$EXTERNALSYM GL_DEPTH_SCALE}
  GL_DEPTH_BIAS                       = $0D1F;
  {$EXTERNALSYM GL_DEPTH_BIAS}
  GL_MAX_EVAL_ORDER                   = $0D30;
  {$EXTERNALSYM GL_MAX_EVAL_ORDER}
  GL_MAX_LIGHTS                       = $0D31;
  {$EXTERNALSYM GL_MAX_EVAL_ORDER}
  GL_MAX_CLIP_PLANES                  = $0D32;
  {$EXTERNALSYM GL_MAX_CLIP_PLANES}
  GL_MAX_TEXTURE_SIZE                 = $0D33;
  {$EXTERNALSYM GL_MAX_TEXTURE_SIZE}
  GL_MAX_PIXEL_MAP_TABLE              = $0D34;
  {$EXTERNALSYM GL_MAX_PIXEL_MAP_TABLE}
  GL_MAX_ATTRIB_STACK_DEPTH           = $0D35;
  {$EXTERNALSYM GL_MAX_ATTRIB_STACK_DEPTH}
  GL_MAX_MODELVIEW_STACK_DEPTH        = $0D36;
  {$EXTERNALSYM GL_MAX_MODELVIEW_STACK_DEPTH}
  GL_MAX_NAME_STACK_DEPTH             = $0D37;
  {$EXTERNALSYM GL_MAX_NAME_STACK_DEPTH}
  GL_MAX_PROJECTION_STACK_DEPTH       = $0D38;
  {$EXTERNALSYM GL_MAX_PROJECTION_STACK_DEPTH}
  GL_MAX_TEXTURE_STACK_DEPTH          = $0D39;
  {$EXTERNALSYM GL_MAX_TEXTURE_STACK_DEPTH}
  GL_MAX_VIEWPORT_DIMS                = $0D3A;
  {$EXTERNALSYM GL_MAX_VIEWPORT_DIMS}
  GL_SUBPIXEL_BITS                    = $0D50;
  {$EXTERNALSYM GL_SUBPIXEL_BITS}
  GL_INDEX_BITS                       = $0D51;
  {$EXTERNALSYM GL_INDEX_BITS}
  GL_RED_BITS                         = $0D52;
  {$EXTERNALSYM GL_RED_BITS}
  GL_GREEN_BITS                       = $0D53;
  {$EXTERNALSYM GL_GREEN_BITS}
  GL_BLUE_BITS                        = $0D54;
  {$EXTERNALSYM GL_BLUE_BITS}
  GL_ALPHA_BITS                       = $0D55;
  {$EXTERNALSYM GL_ALPHA_BITS}
  GL_DEPTH_BITS                       = $0D56;
  {$EXTERNALSYM GL_DEPTH_BITS}
  GL_STENCIL_BITS                     = $0D57;
  {$EXTERNALSYM GL_STENCIL_BITS}
  GL_ACCUM_RED_BITS                   = $0D58;
  {$EXTERNALSYM GL_ACCUM_RED_BITS}
  GL_ACCUM_GREEN_BITS                 = $0D59;
  {$EXTERNALSYM GL_ACCUM_GREEN_BITS}
  GL_ACCUM_BLUE_BITS                  = $0D5A;
  {$EXTERNALSYM GL_ACCUM_BLUE_BITS}
  GL_ACCUM_ALPHA_BITS                 = $0D5B;
  {$EXTERNALSYM GL_ACCUM_ALPHA_BITS}
  GL_NAME_STACK_DEPTH                 = $0D70;
  {$EXTERNALSYM GL_NAME_STACK_DEPTH}
  GL_AUTO_NORMAL                      = $0D80;
  {$EXTERNALSYM GL_AUTO_NORMAL}
  GL_MAP1_COLOR_4                     = $0D90;
  {$EXTERNALSYM GL_MAP1_COLOR_4}
  GL_MAP1_INDEX                       = $0D91;
  {$EXTERNALSYM GL_MAP1_INDEX}
  GL_MAP1_NORMAL                      = $0D92;
  {$EXTERNALSYM GL_MAP1_NORMAL}
  GL_MAP1_TEXTURE_COORD_1             = $0D93;
  {$EXTERNALSYM GL_MAP1_TEXTURE_COORD_1}
  GL_MAP1_TEXTURE_COORD_2             = $0D94;
  {$EXTERNALSYM GL_MAP1_TEXTURE_COORD_2}
  GL_MAP1_TEXTURE_COORD_3             = $0D95;
  {$EXTERNALSYM GL_MAP1_TEXTURE_COORD_3}
  GL_MAP1_TEXTURE_COORD_4             = $0D96;
  {$EXTERNALSYM GL_MAP1_TEXTURE_COORD_4}
  GL_MAP1_VERTEX_3                    = $0D97;
  {$EXTERNALSYM GL_MAP1_VERTEX_3}
  GL_MAP1_VERTEX_4                    = $0D98;
  {$EXTERNALSYM GL_MAP1_VERTEX_4}
  GL_MAP2_COLOR_4                     = $0DB0;
  {$EXTERNALSYM GL_MAP2_COLOR_4}
  GL_MAP2_INDEX                       = $0DB1;
  {$EXTERNALSYM GL_MAP2_INDEX}
  GL_MAP2_NORMAL                      = $0DB2;
  {$EXTERNALSYM GL_MAP2_NORMAL}
  GL_MAP2_TEXTURE_COORD_1             = $0DB3;
  {$EXTERNALSYM GL_MAP2_TEXTURE_COORD_1}
  GL_MAP2_TEXTURE_COORD_2             = $0DB4;
  {$EXTERNALSYM GL_MAP2_TEXTURE_COORD_2}
  GL_MAP2_TEXTURE_COORD_3             = $0DB5;
  {$EXTERNALSYM GL_MAP2_TEXTURE_COORD_3}
  GL_MAP2_TEXTURE_COORD_4             = $0DB6;
  {$EXTERNALSYM GL_MAP2_TEXTURE_COORD_4}
  GL_MAP2_VERTEX_3                    = $0DB7;
  {$EXTERNALSYM GL_MAP2_VERTEX_3}
  GL_MAP2_VERTEX_4                    = $0DB8;
  {$EXTERNALSYM GL_MAP2_VERTEX_4}
  GL_MAP1_GRID_DOMAIN                 = $0DD0;
  {$EXTERNALSYM GL_MAP1_GRID_DOMAIN}
  GL_MAP1_GRID_SEGMENTS               = $0DD1;
  {$EXTERNALSYM GL_MAP1_GRID_SEGMENTS}
  GL_MAP2_GRID_DOMAIN                 = $0DD2;
  {$EXTERNALSYM GL_MAP2_GRID_DOMAIN}
  GL_MAP2_GRID_SEGMENTS               = $0DD3;
  {$EXTERNALSYM GL_MAP2_GRID_SEGMENTS}
  GL_TEXTURE_1D                       = $0DE0;
  {$EXTERNALSYM GL_TEXTURE_1D}
  GL_TEXTURE_2D                       = $0DE1;
  {$EXTERNALSYM GL_TEXTURE_2D}

{ GetTextureParameter }
{      GL_TEXTURE_MAG_FILTER }
{      GL_TEXTURE_MIN_FILTER }
{      GL_TEXTURE_WRAP_S }
{      GL_TEXTURE_WRAP_T }
  GL_TEXTURE_WIDTH                    = $1000;
  {$EXTERNALSYM GL_TEXTURE_WIDTH}
  GL_TEXTURE_HEIGHT                   = $1001;
  {$EXTERNALSYM GL_TEXTURE_HEIGHT}
  GL_TEXTURE_COMPONENTS               = $1003;
  {$EXTERNALSYM GL_TEXTURE_COMPONENTS}
  GL_TEXTURE_BORDER_COLOR             = $1004;
  {$EXTERNALSYM GL_TEXTURE_BORDER_COLOR}
  GL_TEXTURE_BORDER                   = $1005;
  {$EXTERNALSYM GL_TEXTURE_BORDER}

{ HintMode }
  GL_DONT_CARE                        = $1100;
  {$EXTERNALSYM GL_DONT_CARE}
  GL_FASTEST                          = $1101;
  {$EXTERNALSYM GL_FASTEST}
  GL_NICEST                           = $1102;
  {$EXTERNALSYM GL_NICEST}

{ HintTarget }
{      GL_PERSPECTIVE_CORRECTION_HINT }
{      GL_POINT_SMOOTH_HINT }
{      GL_LINE_SMOOTH_HINT }
{      GL_POLYGON_SMOOTH_HINT }
{      GL_FOG_HINT }

{ LightModelParameter }
{      GL_LIGHT_MODEL_AMBIENT }
{      GL_LIGHT_MODEL_LOCAL_VIEWER }
{      GL_LIGHT_MODEL_TWO_SIDE }

{ LightParameter }
  GL_AMBIENT                          = $1200;
  {$EXTERNALSYM GL_AMBIENT}
  GL_DIFFUSE                          = $1201;
  {$EXTERNALSYM GL_DIFFUSE}
  GL_SPECULAR                         = $1202;
  {$EXTERNALSYM GL_SPECULAR}
  GL_POSITION                         = $1203;
  {$EXTERNALSYM GL_POSITION}
  GL_SPOT_DIRECTION                   = $1204;
  {$EXTERNALSYM GL_SPOT_DIRECTION}
  GL_SPOT_EXPONENT                    = $1205;
  {$EXTERNALSYM GL_SPOT_EXPONENT}
  GL_SPOT_CUTOFF                      = $1206;
  {$EXTERNALSYM GL_SPOT_CUTOFF}
  GL_CONSTANT_ATTENUATION             = $1207;
  {$EXTERNALSYM GL_CONSTANT_ATTENUATION}
  GL_LINEAR_ATTENUATION               = $1208;
  {$EXTERNALSYM GL_LINEAR_ATTENUATION}
  GL_QUADRATIC_ATTENUATION            = $1209;
  {$EXTERNALSYM GL_QUADRATIC_ATTENUATION}

{ ListMode }
  GL_COMPILE                          = $1300;
  {$EXTERNALSYM GL_COMPILE}
  GL_COMPILE_AND_EXECUTE              = $1301;
  {$EXTERNALSYM GL_COMPILE_AND_EXECUTE}

{ ListNameType }
  GL_BYTE                             = $1400;
  {$EXTERNALSYM GL_BYTE}
  GL_UNSIGNED_BYTE                    = $1401;
  {$EXTERNALSYM GL_UNSIGNED_BYTE}
  GL_SHORT                            = $1402;
  {$EXTERNALSYM GL_SHORT}
  GL_UNSIGNED_SHORT                   = $1403;
  {$EXTERNALSYM GL_UNSIGNED_SHORT}
  GL_INT                              = $1404;
  {$EXTERNALSYM GL_INT}
  GL_UNSIGNED_INT                     = $1405;
  {$EXTERNALSYM GL_UNSIGNED_INT}
  GL_FLOAT                            = $1406;
  {$EXTERNALSYM GL_FLOAT}
  GL_2_BYTES                          = $1407;
  {$EXTERNALSYM GL_2_BYTES}
  GL_3_BYTES                          = $1408;
  {$EXTERNALSYM GL_3_BYTES}
  GL_4_BYTES                          = $1409;
  {$EXTERNALSYM GL_4_BYTES}

{ LogicOp }
  GL_CLEAR                            = $1500;
  {$EXTERNALSYM GL_CLEAR}
  GL_AND                              = $1501;
  {$EXTERNALSYM GL_AND}
  GL_AND_REVERSE                      = $1502;
  {$EXTERNALSYM GL_AND_REVERSE}
  GL_COPY                             = $1503;
  {$EXTERNALSYM GL_COPY}
  GL_AND_INVERTED                     = $1504;
  {$EXTERNALSYM GL_AND_INVERTED}
  GL_NOOP                             = $1505;
  {$EXTERNALSYM GL_NOOP}
  GL_XOR                              = $1506;
  {$EXTERNALSYM GL_XOR}
  GL_OR                               = $1507;
  {$EXTERNALSYM GL_OR}
  GL_NOR                              = $1508;
  {$EXTERNALSYM GL_NOR}
  GL_EQUIV                            = $1509;
  {$EXTERNALSYM GL_EQUIV}
  GL_INVERT                           = $150A;
  {$EXTERNALSYM GL_INVERT}
  GL_OR_REVERSE                       = $150B;
  {$EXTERNALSYM GL_OR_REVERSE}
  GL_COPY_INVERTED                    = $150C;
  {$EXTERNALSYM GL_COPY_INVERTED}
  GL_OR_INVERTED                      = $150D;
  {$EXTERNALSYM GL_OR_INVERTED}
  GL_NAND                             = $150E;
  {$EXTERNALSYM GL_NAND}
  GL_SET                              = $150F;
  {$EXTERNALSYM GL_SET}

{ MapTarget }
{      GL_MAP1_COLOR_4 }
{      GL_MAP1_INDEX }
{      GL_MAP1_NORMAL }
{      GL_MAP1_TEXTURE_COORD_1 }
{      GL_MAP1_TEXTURE_COORD_2 }
{      GL_MAP1_TEXTURE_COORD_3 }
{      GL_MAP1_TEXTURE_COORD_4 }
{      GL_MAP1_VERTEX_3 }
{      GL_MAP1_VERTEX_4 }
{      GL_MAP2_COLOR_4 }
{      GL_MAP2_INDEX }
{      GL_MAP2_NORMAL }
{      GL_MAP2_TEXTURE_COORD_1 }
{      GL_MAP2_TEXTURE_COORD_2 }
{      GL_MAP2_TEXTURE_COORD_3 }
{      GL_MAP2_TEXTURE_COORD_4 }
{      GL_MAP2_VERTEX_3 }
{      GL_MAP2_VERTEX_4 }

{ MaterialFace }
{      GL_FRONT }
{      GL_BACK }
{      GL_FRONT_AND_BACK }

{ MaterialParameter }
  GL_EMISSION                         = $1600;
  {$EXTERNALSYM GL_EMISSION}
  GL_SHININESS                        = $1601;
  {$EXTERNALSYM GL_SHININESS}
  GL_AMBIENT_AND_DIFFUSE              = $1602;
  {$EXTERNALSYM GL_AMBIENT_AND_DIFFUSE}
  GL_COLOR_INDEXES                    = $1603;
  {$EXTERNALSYM GL_COLOR_INDEXES}
{      GL_AMBIENT }
{      GL_DIFFUSE }
{      GL_SPECULAR }

{ MatrixMode }
  GL_MODELVIEW                        = $1700;
  {$EXTERNALSYM GL_MODELVIEW}
  GL_PROJECTION                       = $1701;
  {$EXTERNALSYM GL_PROJECTION}
  GL_TEXTURE                          = $1702;
  {$EXTERNALSYM GL_TEXTURE}

{ MeshMode1 }
{      GL_POINT }
{      GL_LINE }

{ MeshMode2 }
{      GL_POINT }
{      GL_LINE }
{      GL_FILL }

{ PixelCopyType }
  GL_COLOR                            = $1800;
  {$EXTERNALSYM GL_COLOR}
  GL_DEPTH                            = $1801;
  {$EXTERNALSYM GL_DEPTH}
  GL_STENCIL                          = $1802;
  {$EXTERNALSYM GL_STENCIL}

{ PixelFormat }
  GL_COLOR_INDEX                      = $1900;
  {$EXTERNALSYM GL_COLOR_INDEX}
  GL_STENCIL_INDEX                    = $1901;
  {$EXTERNALSYM GL_STENCIL_INDEX}
  GL_DEPTH_COMPONENT                  = $1902;
  {$EXTERNALSYM GL_DEPTH_COMPONENT}
  GL_RED                              = $1903;
  {$EXTERNALSYM GL_RED}
  GL_GREEN                            = $1904;
  {$EXTERNALSYM GL_GREEN}
  GL_BLUE                             = $1905;
  {$EXTERNALSYM GL_BLUE}
  GL_ALPHA                            = $1906;
  {$EXTERNALSYM GL_ALPHA}
  GL_RGB                              = $1907;
  {$EXTERNALSYM GL_RGB}
  GL_RGBA                             = $1908;
  {$EXTERNALSYM GL_RGBA}
  GL_LUMINANCE                        = $1909;
  {$EXTERNALSYM GL_LUMINANCE}
  GL_LUMINANCE_ALPHA                  = $190A;
  {$EXTERNALSYM GL_LUMINANCE_ALPHA}

{ PixelMap }
{      GL_PIXEL_MAP_I_TO_I }
{      GL_PIXEL_MAP_S_TO_S }
{      GL_PIXEL_MAP_I_TO_R }
{      GL_PIXEL_MAP_I_TO_G }
{      GL_PIXEL_MAP_I_TO_B }
{      GL_PIXEL_MAP_I_TO_A }
{      GL_PIXEL_MAP_R_TO_R }
{      GL_PIXEL_MAP_G_TO_G }
{      GL_PIXEL_MAP_B_TO_B }
{      GL_PIXEL_MAP_A_TO_A }

{ PixelStore }
{      GL_UNPACK_SWAP_BYTES }
{      GL_UNPACK_LSB_FIRST }
{      GL_UNPACK_ROW_LENGTH }
{      GL_UNPACK_SKIP_ROWS }
{      GL_UNPACK_SKIP_PIXELS }
{      GL_UNPACK_ALIGNMENT }
{      GL_PACK_SWAP_BYTES }
{      GL_PACK_LSB_FIRST }
{      GL_PACK_ROW_LENGTH }
{      GL_PACK_SKIP_ROWS }
{      GL_PACK_SKIP_PIXELS }
{      GL_PACK_ALIGNMENT }

{ PixelTransfer }
{      GL_MAP_COLOR }
{      GL_MAP_STENCIL }
{      GL_INDEX_SHIFT }
{      GL_INDEX_OFFSET }
{      GL_RED_SCALE }
{      GL_RED_BIAS }
{      GL_GREEN_SCALE }
{      GL_GREEN_BIAS }
{      GL_BLUE_SCALE }
{      GL_BLUE_BIAS }
{      GL_ALPHA_SCALE }
{      GL_ALPHA_BIAS }
{      GL_DEPTH_SCALE }
{      GL_DEPTH_BIAS }

{ PixelType }
  GL_BITMAP                           = $1A00;
  {$EXTERNALSYM GL_BITMAP}
{      GL_BYTE }
{      GL_UNSIGNED_BYTE }
{      GL_SHORT }
{      GL_UNSIGNED_SHORT }
{      GL_INT }
{      GL_UNSIGNED_INT }
{      GL_FLOAT }

{ PolygonMode }
  GL_POINT                            = $1B00;
  {$EXTERNALSYM GL_POINT}
  GL_LINE                             = $1B01;
  {$EXTERNALSYM GL_LINE}
  GL_FILL                             = $1B02;
  {$EXTERNALSYM GL_FILL}

{ ReadBufferMode }
{      GL_FRONT_LEFT }
{      GL_FRONT_RIGHT }
{      GL_BACK_LEFT }
{      GL_BACK_RIGHT }
{      GL_FRONT }
{      GL_BACK }
{      GL_LEFT }
{      GL_RIGHT }
{      GL_AUX0 }
{      GL_AUX1 }
{      GL_AUX2 }
{      GL_AUX3 }

{ RenderingMode }
  GL_RENDER                           = $1C00;
  {$EXTERNALSYM GL_RENDER}
  GL_FEEDBACK                         = $1C01;
  {$EXTERNALSYM GL_FEEDBACK}
  GL_SELECT                           = $1C02;
  {$EXTERNALSYM GL_SELECT}

{ ShadingModel }
  GL_FLAT                             = $1D00;
  {$EXTERNALSYM GL_FLAT}
  GL_SMOOTH                           = $1D01;
  {$EXTERNALSYM GL_SMOOTH}

{ StencilFunction }
{      GL_NEVER }
{      GL_LESS }
{      GL_EQUAL }
{      GL_LEQUAL }
{      GL_GREATER }
{      GL_NOTEQUAL }
{      GL_GEQUAL }
{      GL_ALWAYS }

{ StencilOp }
{      GL_ZERO }
  GL_KEEP                             = $1E00;
  {$EXTERNALSYM GL_KEEP}
  GL_REPLACE                          = $1E01;
  {$EXTERNALSYM GL_REPLACE}
  GL_INCR                             = $1E02;
  {$EXTERNALSYM GL_INCR}
  GL_DECR                             = $1E03;
  {$EXTERNALSYM GL_DECR}
{      GL_INVERT }

{ StringName }
  GL_VENDOR                           = $1F00;
  {$EXTERNALSYM GL_VENDOR}
  GL_RENDERER                         = $1F01;
  {$EXTERNALSYM GL_RENDERER}
  GL_VERSION                          = $1F02;
  {$EXTERNALSYM GL_VERSION}
  GL_EXTENSIONS                       = $1F03;
  {$EXTERNALSYM GL_EXTENSIONS}

{ TextureCoordName }
  GL_S                                = $2000;
  {$EXTERNALSYM GL_S}
  GL_T                                = $2001;
  {$EXTERNALSYM GL_T}
  GL_R                                = $2002;
  {$EXTERNALSYM GL_R}
  GL_Q                                = $2003;
  {$EXTERNALSYM GL_Q}

{ TextureEnvMode }
  GL_MODULATE                         = $2100;
  {$EXTERNALSYM GL_MODULATE}
  GL_DECAL                            = $2101;
  {$EXTERNALSYM GL_DECAL}
{      GL_BLEND }

{ TextureEnvParameter }
  GL_TEXTURE_ENV_MODE                 = $2200;
  {$EXTERNALSYM GL_TEXTURE_ENV_MODE}
  GL_TEXTURE_ENV_COLOR                = $2201;
  {$EXTERNALSYM GL_TEXTURE_ENV_COLOR}

{ TextureEnvTarget }
  GL_TEXTURE_ENV                      = $2300;
  {$EXTERNALSYM GL_TEXTURE_ENV}

{ TextureGenMode }
  GL_EYE_LINEAR                       = $2400;
  {$EXTERNALSYM GL_EYE_LINEAR}
  GL_OBJECT_LINEAR                    = $2401;
  {$EXTERNALSYM GL_OBJECT_LINEAR}
  GL_SPHERE_MAP                       = $2402;
  {$EXTERNALSYM GL_SPHERE_MAP}

{ TextureGenParameter }
  GL_TEXTURE_GEN_MODE                 = $2500;
  {$EXTERNALSYM GL_TEXTURE_GEN_MODE}
  GL_OBJECT_PLANE                     = $2501;
  {$EXTERNALSYM GL_OBJECT_PLANE}
  GL_EYE_PLANE                        = $2502;
  {$EXTERNALSYM GL_EYE_PLANE}

{ TextureMagFilter }
  GL_NEAREST                          = $2600;
  {$EXTERNALSYM GL_NEAREST}
  GL_LINEAR                           = $2601;
  {$EXTERNALSYM GL_LINEAR}

{ TextureMinFilter }
{      GL_NEAREST }
{      GL_LINEAR }
  GL_NEAREST_MIPMAP_NEAREST           = $2700;
  {$EXTERNALSYM GL_NEAREST_MIPMAP_NEAREST}
  GL_LINEAR_MIPMAP_NEAREST            = $2701;
  {$EXTERNALSYM GL_LINEAR_MIPMAP_NEAREST}
  GL_NEAREST_MIPMAP_LINEAR            = $2702;
  {$EXTERNALSYM GL_NEAREST_MIPMAP_LINEAR}
  GL_LINEAR_MIPMAP_LINEAR             = $2703;
  {$EXTERNALSYM GL_LINEAR_MIPMAP_LINEAR}

{ TextureParameterName }
  GL_TEXTURE_MAG_FILTER               = $2800;
  {$EXTERNALSYM GL_TEXTURE_MAG_FILTER}
  GL_TEXTURE_MIN_FILTER               = $2801;
  {$EXTERNALSYM GL_TEXTURE_MIN_FILTER}
  GL_TEXTURE_WRAP_S                   = $2802;
  {$EXTERNALSYM GL_TEXTURE_WRAP_S}
  GL_TEXTURE_WRAP_T                   = $2803;
  {$EXTERNALSYM GL_TEXTURE_WRAP_T}
{      GL_TEXTURE_BORDER_COLOR }

{ TextureTarget }
{      GL_TEXTURE_1D }
{      GL_TEXTURE_2D }

{ TextureWrapMode }
  GL_CLAMP                            = $2900;
  {$EXTERNALSYM GL_CLAMP}
  GL_REPEAT                           = $2901;
  {$EXTERNALSYM GL_REPEAT}

{ ClipPlaneName }
  GL_CLIP_PLANE0                      = $3000;
  {$EXTERNALSYM GL_CLIP_PLANE0}
  GL_CLIP_PLANE1                      = $3001;
  {$EXTERNALSYM GL_CLIP_PLANE1}
  GL_CLIP_PLANE2                      = $3002;
  {$EXTERNALSYM GL_CLIP_PLANE2}
  GL_CLIP_PLANE3                      = $3003;
  {$EXTERNALSYM GL_CLIP_PLANE3}
  GL_CLIP_PLANE4                      = $3004;
  {$EXTERNALSYM GL_CLIP_PLANE4}
  GL_CLIP_PLANE5                      = $3005;
  {$EXTERNALSYM GL_CLIP_PLANE5}

{ LightName }
  GL_LIGHT0                           = $4000;
  {$EXTERNALSYM GL_LIGHT0}
  GL_LIGHT1                           = $4001;
  {$EXTERNALSYM GL_LIGHT1}
  GL_LIGHT2                           = $4002;
  {$EXTERNALSYM GL_LIGHT2}
  GL_LIGHT3                           = $4003;
  {$EXTERNALSYM GL_LIGHT3}
  GL_LIGHT4                           = $4004;
  {$EXTERNALSYM GL_LIGHT4}
  GL_LIGHT5                           = $4005;
  {$EXTERNALSYM GL_LIGHT5}
  GL_LIGHT6                           = $4006;
  {$EXTERNALSYM GL_LIGHT6}
  GL_LIGHT7                           = $4007;
  {$EXTERNALSYM GL_LIGHT7}

// Extensions
  GL_EXT_vertex_array                 = 1;
  {$EXTERNALSYM GL_EXT_vertex_array}
  GL_WIN_swap_hint                    = 1;
  {$EXTERNALSYM GL_WIN_swap_hint}

// EXT_vertex_array
  GL_VERTEX_ARRAY_EXT               = $8074;
  {$EXTERNALSYM GL_VERTEX_ARRAY_EXT}
  GL_NORMAL_ARRAY_EXT               = $8075;
  {$EXTERNALSYM GL_NORMAL_ARRAY_EXT}
  GL_COLOR_ARRAY_EXT                = $8076;
  {$EXTERNALSYM GL_COLOR_ARRAY_EXT}
  GL_INDEX_ARRAY_EXT                = $8077;
  {$EXTERNALSYM GL_INDEX_ARRAY_EXT}
  GL_TEXTURE_COORD_ARRAY_EXT        = $8078;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_EXT}
  GL_EDGE_FLAG_ARRAY_EXT            = $8079;
  {$EXTERNALSYM GL_EDGE_FLAG_ARRAY_EXT}
  GL_VERTEX_ARRAY_SIZE_EXT          = $807A;
  {$EXTERNALSYM GL_VERTEX_ARRAY_SIZE_EXT}
  GL_VERTEX_ARRAY_TYPE_EXT          = $807B;
  {$EXTERNALSYM GL_VERTEX_ARRAY_TYPE_EXT}
  GL_VERTEX_ARRAY_STRIDE_EXT        = $807C;
  {$EXTERNALSYM GL_VERTEX_ARRAY_STRIDE_EXT}
  GL_VERTEX_ARRAY_COUNT_EXT         = $807D;
  {$EXTERNALSYM GL_VERTEX_ARRAY_COUNT_EXT}
  GL_NORMAL_ARRAY_TYPE_EXT          = $807E;
  {$EXTERNALSYM GL_NORMAL_ARRAY_TYPE_EXT}
  GL_NORMAL_ARRAY_STRIDE_EXT        = $807F;
  {$EXTERNALSYM GL_NORMAL_ARRAY_STRIDE_EXT}
  GL_NORMAL_ARRAY_COUNT_EXT         = $8080;
  {$EXTERNALSYM GL_NORMAL_ARRAY_COUNT_EXT}
  GL_COLOR_ARRAY_SIZE_EXT           = $8081;
  {$EXTERNALSYM GL_COLOR_ARRAY_SIZE_EXT}
  GL_COLOR_ARRAY_TYPE_EXT           = $8082;
  {$EXTERNALSYM GL_COLOR_ARRAY_TYPE_EXT}
  GL_COLOR_ARRAY_STRIDE_EXT         = $8083;
  {$EXTERNALSYM GL_COLOR_ARRAY_STRIDE_EXT}
  GL_COLOR_ARRAY_COUNT_EXT          = $8084;
  {$EXTERNALSYM GL_COLOR_ARRAY_COUNT_EXT}
  GL_INDEX_ARRAY_TYPE_EXT           = $8085;
  {$EXTERNALSYM GL_INDEX_ARRAY_TYPE_EXT}
  GL_INDEX_ARRAY_STRIDE_EXT         = $8086;
  {$EXTERNALSYM GL_INDEX_ARRAY_STRIDE_EXT}
  GL_INDEX_ARRAY_COUNT_EXT          = $8087;
  {$EXTERNALSYM GL_INDEX_ARRAY_COUNT_EXT}
  GL_TEXTURE_COORD_ARRAY_SIZE_EXT   = $8088;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_SIZE_EXT}
  GL_TEXTURE_COORD_ARRAY_TYPE_EXT   = $8089;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_TYPE_EXT}
  GL_TEXTURE_COORD_ARRAY_STRIDE_EXT = $808A;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_STRIDE_EXT}
  GL_TEXTURE_COORD_ARRAY_COUNT_EXT  = $808B;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_COUNT_EXT}
  GL_EDGE_FLAG_ARRAY_STRIDE_EXT     = $808C;
  {$EXTERNALSYM GL_EDGE_FLAG_ARRAY_STRIDE_EXT}
  GL_EDGE_FLAG_ARRAY_COUNT_EXT      = $808D;
  {$EXTERNALSYM GL_EDGE_FLAG_ARRAY_COUNT_EXT}
  GL_VERTEX_ARRAY_POINTER_EXT       = $808E;
  {$EXTERNALSYM GL_VERTEX_ARRAY_POINTER_EXT}
  GL_NORMAL_ARRAY_POINTER_EXT       = $808F;
  {$EXTERNALSYM GL_NORMAL_ARRAY_POINTER_EXT}
  GL_COLOR_ARRAY_POINTER_EXT        = $8090;
  {$EXTERNALSYM GL_COLOR_ARRAY_POINTER_EXT}
  GL_INDEX_ARRAY_POINTER_EXT        = $8091;
  {$EXTERNALSYM GL_INDEX_ARRAY_POINTER_EXT}
  GL_TEXTURE_COORD_ARRAY_POINTER_EXT = $8092;
  {$EXTERNALSYM GL_TEXTURE_COORD_ARRAY_POINTER_EXT}
  GL_EDGE_FLAG_ARRAY_POINTER_EXT    = $8093;
  {$EXTERNALSYM GL_EDGE_FLAG_ARRAY_POINTER_EXT}
  
  PFD_MAIN_PLANE = 0;
  PFD_TYPE_RGBA = 0;

  PFD_DOUBLEBUFFER                = $00000001;
  PFD_STEREO                      = $00000002;
  PFD_DRAW_TO_WINDOW              = $00000004;
  PFD_DRAW_TO_BITMAP              = $00000008;
  PFD_SUPPORT_GDI                 = $00000010;
  PFD_SUPPORT_OPENGL              = $00000020;
  PFD_GENERIC_FORMAT              = $00000040;
  PFD_NEED_PALETTE                = $00000080;
  PFD_NEED_SYSTEM_PALETTE         = $00000100;
  PFD_SWAP_EXCHANGE               = $00000200;
  PFD_SWAP_COPY                   = $00000400;
  PFD_SWAP_LAYER_BUFFERS          = $00000800;
  PFD_GENERIC_ACCELERATED         = $00001000;
  PFD_DEPTH_DONTCARE              = $20000000;
  PFD_DOUBLEBUFFER_DONTCARE       = $40000000;
  
type
  PPointFloat = ^TPointFloat;
  {$EXTERNALSYM _POINTFLOAT}
  _POINTFLOAT = record
    X,Y: Single;
  end;
  TPointFloat = _POINTFLOAT;
  {$EXTERNALSYM POINTFLOAT}
  POINTFLOAT = _POINTFLOAT;

  PGlyphMetricsFloat = ^TGlyphMetricsFloat;
  {$EXTERNALSYM _GLYPHMETRICSFLOAT}
  _GLYPHMETRICSFLOAT = record
    gmfBlackBoxX: Single;
    gmfBlackBoxY: Single;
    gmfptGlyphOrigin: TPointFloat;
    gmfCellIncX: Single;
    gmfCellIncY: Single;
  end;
  TGlyphMetricsFloat = _GLYPHMETRICSFLOAT;
  {$EXTERNALSYM GLYPHMETRICSFLOAT}
  GLYPHMETRICSFLOAT = _GLYPHMETRICSFLOAT;

const
  {$EXTERNALSYM WGL_FONT_LINES}
  WGL_FONT_LINES      = 0;
  {$EXTERNALSYM WGL_FONT_POLYGONS}
  WGL_FONT_POLYGONS   = 1;

{***********************************************************}

procedure glAccum (op: GLenum; value: GLfloat);
  {$EXTERNALSYM glAccum}
procedure glAlphaFunc (func: GLenum; ref: GLclampf); 
  {$EXTERNALSYM glAlphaFunc}
procedure glBegin (mode: GLenum); 
  {$EXTERNALSYM glBegin}
procedure glBitmap (width, height: GLsizei; xorig, yorig: GLfloat;
                    xmove, ymove: GLfloat; bitmap: Pointer); 
  {$EXTERNALSYM glBitmap}
procedure glBlendFunc (sfactor, dfactor: GLenum); 
  {$EXTERNALSYM glBlendFunc}
procedure glCallList (list: GLuint); 
  {$EXTERNALSYM glCallList}
procedure glCallLists (n: GLsizei; cltype: GLenum; lists: Pointer); 
  {$EXTERNALSYM glCallLists}
procedure glClear (mask: GLbitfield); 
  {$EXTERNALSYM glClear}
procedure glClearAccum (red, green, blue, alpha: GLfloat); 
  {$EXTERNALSYM glClearAccum}
procedure glClearColor (red, green, blue, alpha: GLclampf); 
  {$EXTERNALSYM glClearColor}
procedure glClearDepth (depth: GLclampd); 
  {$EXTERNALSYM glClearDepth}
procedure glClearIndex (c: GLfloat); 
  {$EXTERNALSYM glClearIndex}
procedure glClearStencil (s: GLint); 
  {$EXTERNALSYM glClearStencil}
procedure glClipPlane (plane: GLenum; equation: PGLDouble); 
  {$EXTERNALSYM glClipPlane}

procedure glColor3b (red, green, blue: GLbyte); 
  {$EXTERNALSYM glColor3b}
procedure glColor3bv (v: PGLByte); 
  {$EXTERNALSYM glColor3bv}
procedure glColor3d (red, green, blue: GLdouble); 
  {$EXTERNALSYM glColor3d}
procedure glColor3dv (v: PGLdouble); 
  {$EXTERNALSYM glColor3dv}
procedure glColor3f (red, green, blue: GLfloat); 
  {$EXTERNALSYM glColor3f}
procedure glColor3fv (v: PGLfloat); 
  {$EXTERNALSYM glColor3fv}
procedure glColor3i (red, green, blue: GLint); 
  {$EXTERNALSYM glColor3i}
procedure glColor3iv (v: PGLint); 
  {$EXTERNALSYM glColor3iv}
procedure glColor3s (red, green, blue: GLshort); 
  {$EXTERNALSYM glColor3s}
procedure glColor3sv (v: PGLshort); 
  {$EXTERNALSYM glColor3sv}
procedure glColor3ub (red, green, blue: GLubyte); 
  {$EXTERNALSYM glColor3ub}
procedure glColor3ubv (v: PGLubyte); 
  {$EXTERNALSYM glColor3ubv}
procedure glColor3ui (red, green, blue: GLuint); 
  {$EXTERNALSYM glColor3ui}
procedure glColor3uiv (v: PGLuint); 
  {$EXTERNALSYM glColor3uiv}
procedure glColor3us (red, green, blue: GLushort); 
  {$EXTERNALSYM glColor3us}
procedure glColor3usv (v: PGLushort); 
  {$EXTERNALSYM glColor3usv}
procedure glColor4b (red, green, blue, alpha: GLbyte); 
  {$EXTERNALSYM glColor4b}
procedure glColor4bv (v: PGLbyte); 
  {$EXTERNALSYM glColor4bv}
procedure glColor4d (red, green, blue, alpha: GLdouble); 
  {$EXTERNALSYM glColor4d}
procedure glColor4dv (v: PGLdouble); 
  {$EXTERNALSYM glColor4dv}
procedure glColor4f (red, green, blue, alpha: GLfloat); 
  {$EXTERNALSYM glColor4f}
procedure glColor4fv (v: PGLfloat); 
  {$EXTERNALSYM glColor4fv}
procedure glColor4i (red, green, blue, alpha: GLint); 
  {$EXTERNALSYM glColor4i}
procedure glColor4iv (v: PGLint); 
  {$EXTERNALSYM glColor4iv}
procedure glColor4s (red, green, blue, alpha: GLshort); 
  {$EXTERNALSYM glColor4s}
procedure glColor4sv (v: PGLshort); 
  {$EXTERNALSYM glColor4sv}
procedure glColor4ub (red, green, blue, alpha: GLubyte); 
  {$EXTERNALSYM glColor4ub}
procedure glColor4ubv (v: PGLubyte); 
  {$EXTERNALSYM glColor4ubv}
procedure glColor4ui (red, green, blue, alpha: GLuint); 
  {$EXTERNALSYM glColor4ui}
procedure glColor4uiv (v: PGLuint); 
  {$EXTERNALSYM glColor4uiv}
procedure glColor4us (red, green, blue, alpha: GLushort); 
  {$EXTERNALSYM glColor4us}
procedure glColor4usv (v: PGLushort); 
  {$EXTERNALSYM glColor4usv}
procedure glColor(red, green, blue: GLbyte);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLdouble);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLfloat);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLint);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLshort);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLubyte);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLuint);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue: GLushort);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLbyte);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLdouble);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLfloat);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLint);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLshort);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLubyte);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLuint);  overload;
  {$EXTERNALSYM glColor}
procedure glColor(red, green, blue, alpha: GLushort);  overload;
  {$EXTERNALSYM glColor}
procedure glColor3(v: PGLbyte);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLdouble);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLfloat);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLint);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLshort);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLubyte);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLuint);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor3(v: PGLushort);  overload;
  {$EXTERNALSYM glColor3}
procedure glColor4(v: PGLbyte);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLdouble);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLfloat);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLint);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLshort);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLubyte);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLuint);  overload;
  {$EXTERNALSYM glColor4}
procedure glColor4(v: PGLushort);  overload;
  {$EXTERNALSYM glColor4}

procedure glColorMask (red, green, blue, alpha: GLboolean); 
  {$EXTERNALSYM glColorMask}
procedure glColorMaterial (face, mode: GLenum); 
  {$EXTERNALSYM glColorMaterial}
procedure glCopyPixels (x,y: GLint; width, height: GLsizei; pixeltype: GLenum); 
  {$EXTERNALSYM glCopyPixels}
procedure glCullFace (mode: GLenum); 
  {$EXTERNALSYM glCullFace}
procedure glDeleteLists (list: GLuint; range: GLsizei); 
  {$EXTERNALSYM glDeleteLists}
procedure glDepthFunc (func: GLenum); 
  {$EXTERNALSYM glDepthFunc}
procedure glDepthMask (flag: GLboolean); 
  {$EXTERNALSYM glDepthMask}
procedure glDepthRange (zNear, zFar: GLclampd); 
  {$EXTERNALSYM glDepthRange}
procedure glDisable (cap: GLenum); 
  {$EXTERNALSYM glDisable}
procedure glDrawBuffer (mode: GLenum); 
  {$EXTERNALSYM glDrawBuffer}
procedure glDrawPixels (width, height: GLsizei; format, pixeltype: GLenum;
             pixels: Pointer); 
  {$EXTERNALSYM glDrawPixels}
procedure glEdgeFlag (flag: GLboolean); 
  {$EXTERNALSYM glEdgeFlag}
procedure glEdgeFlagv (flag: PGLboolean); 
  {$EXTERNALSYM glEdgeFlagv}
procedure glEnable (cap: GLenum); 
  {$EXTERNALSYM glEnable}
procedure glEnd; 
  {$EXTERNALSYM glEnd}
procedure glEndList; 
  {$EXTERNALSYM glEndList}

procedure glEvalCoord1d (u: GLdouble); 
  {$EXTERNALSYM glEvalCoord1d}
procedure glEvalCoord1dv (u: PGLdouble); 
  {$EXTERNALSYM glEvalCoord1dv}
procedure glEvalCoord1f (u: GLfloat); 
  {$EXTERNALSYM glEvalCoord1f}
procedure glEvalCoord1fv (u: PGLfloat); 
  {$EXTERNALSYM glEvalCoord1fv}
procedure glEvalCoord2d (u,v: GLdouble); 
  {$EXTERNALSYM glEvalCoord2d}
procedure glEvalCoord2dv (u: PGLdouble); 
  {$EXTERNALSYM glEvalCoord2dv}
procedure glEvalCoord2f (u,v: GLfloat); 
  {$EXTERNALSYM glEvalCoord2f}
procedure glEvalCoord2fv (u: PGLfloat); 
  {$EXTERNALSYM glEvalCoord2fv}
procedure glEvalCoord(u: GLdouble);  overload;
  {$EXTERNALSYM glEvalCoord}
procedure glEvalCoord(u: GLfloat);  overload;
  {$EXTERNALSYM glEvalCoord}
procedure glEvalCoord(u,v: GLdouble);  overload;
  {$EXTERNALSYM glEvalCoord}
procedure glEvalCoord(u,v: GLfloat);  overload;
  {$EXTERNALSYM glEvalCoord}
procedure glEvalCoord1(v: PGLdouble);  overload;
  {$EXTERNALSYM glEvalCoord1}
procedure glEvalCoord1(v: PGLfloat);  overload;
  {$EXTERNALSYM glEvalCoord1}
procedure glEvalCoord2(v: PGLdouble);  overload;
  {$EXTERNALSYM glEvalCoord2}
procedure glEvalCoord2(v: PGLfloat);  overload;
  {$EXTERNALSYM glEvalCoord2}

procedure glEvalMesh1 (mode: GLenum; i1, i2: GLint); 
  {$EXTERNALSYM glEvalMesh1}
procedure glEvalMesh2 (mode: GLenum; i1, i2, j1, j2: GLint); 
  {$EXTERNALSYM glEvalMesh2}
procedure glEvalMesh(mode: GLenum; i1, i2: GLint);  overload;
  {$EXTERNALSYM glEvalMesh}
procedure glEvalMesh(mode: GLenum; i1, i2, j1, j2: GLint);  overload;
  {$EXTERNALSYM glEvalMesh}

procedure glEvalPoint1 (i: GLint); 
  {$EXTERNALSYM glEvalPoint1}
procedure glEvalPoint2 (i,j: GLint); 
  {$EXTERNALSYM glEvalPoint2}
procedure glEvalPoint(i: GLint);  overload;
  {$EXTERNALSYM glEvalPoint}
procedure glEvalPoint(i,j: GLint);  overload;
  {$EXTERNALSYM glEvalPoint}

procedure glFeedbackBuffer (size: GLsizei; buftype: GLenum; buffer: PGLFloat); 
  {$EXTERNALSYM glFeedbackBuffer}
procedure glFinish; 
  {$EXTERNALSYM glFinish}
procedure glFlush; 
  {$EXTERNALSYM glFlush}

procedure glFogf (pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glFogf}
procedure glFogfv (pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glFogfv}
procedure glFogi (pname: GLenum; param: GLint); 
  {$EXTERNALSYM glFogi}
procedure glFogiv (pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glFogiv}
procedure glFog(pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glFog}
procedure glFog(pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glFog}
procedure glFog(pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glFog}
procedure glFog(pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glFog}

procedure glFrontFace (mode: GLenum); 
  {$EXTERNALSYM glFrontFace}
procedure glFrustum (left, right, bottom, top, zNear, zFar: GLdouble); 
  {$EXTERNALSYM glFrustum}
function  glGenLists (range: GLsizei): GLuint; 
  {$EXTERNALSYM glGenLists}
procedure glGetBooleanv (pname: GLenum; prms: PGLboolean); 
  {$EXTERNALSYM glGetBooleanv}
procedure glGetClipPlane (plane: GLenum; equation: PGLdouble); 
  {$EXTERNALSYM glGetClipPlane}
procedure glGetDoublev (pname: GLenum; prms: PGLdouble); 
  {$EXTERNALSYM glGetDoublev}
function  glGetError: GLenum; 
  {$EXTERNALSYM glGetError}
procedure glGetFloatv (pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetFloatv}
procedure glGetIntegerv (pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetIntegerv}

procedure glGetLightfv (light: GLenum; pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetLightfv}
procedure glGetLightiv (light: GLenum; pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetLightiv}
procedure glGetLight(light: GLenum; pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetLight}
procedure glGetLight(light: GLenum; pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetLight}

procedure glGetMapdv (target: GLenum; query: GLenum; v: PGLdouble); 
  {$EXTERNALSYM glGetMapdv}
procedure glGetMapfv (target: GLenum; query: GLenum; v: PGLfloat); 
  {$EXTERNALSYM glGetMapfv}
procedure glGetMapiv (target: GLenum; query: GLenum; v: PGLint); 
  {$EXTERNALSYM glGetMapiv}
procedure glGetMap(target: GLenum; query: GLenum; v: PGLdouble);  overload;
  {$EXTERNALSYM glGetMap}
procedure glGetMap(target: GLenum; query: GLenum; v: PGLfloat);  overload;
  {$EXTERNALSYM glGetMap}
procedure glGetMap(target: GLenum; query: GLenum; v: PGLint);  overload;
  {$EXTERNALSYM glGetMap}

procedure glGetMaterialfv (face: GLenum; pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetMaterialfv}
procedure glGetMaterialiv (face: GLenum; pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetMaterialiv}
procedure glGetMaterial(face: GLenum; pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetMaterial}
procedure glGetMaterial(face: GLenum; pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetMaterial}

procedure glGetPixelMapfv (map: GLenum; values: PGLfloat); 
  {$EXTERNALSYM glGetPixelMapfv}
procedure glGetPixelMapuiv (map: GLenum; values: PGLuint); 
  {$EXTERNALSYM glGetPixelMapuiv}
procedure glGetPixelMapusv (map: GLenum; values: PGLushort); 
  {$EXTERNALSYM glGetPixelMapusv}
procedure glGetPixelMap(map: GLenum; values: PGLfloat);  overload;
  {$EXTERNALSYM glGetPixelMap}
procedure glGetPixelMap(map: GLenum; values: PGLuint);  overload;
  {$EXTERNALSYM glGetPixelMap}
procedure glGetPixelMap(map: GLenum; values: PGLushort);  overload;
  {$EXTERNALSYM glGetPixelMap}

procedure glGetPolygonStipple (var mask: GLubyte); 
  {$EXTERNALSYM glGetPolygonStipple}
function  glGetString (name: GLenum): PChar; 
  {$EXTERNALSYM glGetString}

procedure glGetTexEnvfv (target: GLenum; pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetTexEnvfv}
procedure glGetTexEnviv (target: GLenum; pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetTexEnviv}
procedure glGetTexEnv(target: GLenum; pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetTexEnv}
procedure glGetTexEnv(target: GLenum; pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetTexEnv}

procedure glGetTexGendv (coord: GLenum; pname: GLenum; prms: PGLdouble); 
  {$EXTERNALSYM glGetTexGendv}
procedure glGetTexGenfv (coord: GLenum; pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetTexGenfv}
procedure glGetTexGeniv (coord: GLenum; pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetTexGeniv}
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLdouble);  overload;
  {$EXTERNALSYM glGetTexGen}
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetTexGen}
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetTexGen}

procedure glGetTexImage (target: GLenum; level: GLint; format: GLenum; _type: GLenum; pixels: pointer); 
  {$EXTERNALSYM glGetTexImage}

procedure glGetTexLevelParameterfv (target: GLenum; level: GLint; pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetTexLevelParameterfv}
procedure glGetTexLevelParameteriv (target: GLenum; level: GLint; pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetTexLevelParameteriv}
procedure glGetTexLevelParameter(target: GLenum; level: GLint; pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetTexLevelParameter}
procedure glGetTexLevelParameter(target: GLenum; level: GLint; pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetTexLevelParameter}

procedure glGetTexParameterfv (target, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glGetTexParameterfv}
procedure glGetTexParameteriv (target, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glGetTexParameteriv}
procedure glGetTexParameter(target, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glGetTexParameter}
procedure glGetTexParameter(target, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glGetTexParameter}

procedure glHint (target, mode: GLenum); 
  {$EXTERNALSYM glHint}
procedure glIndexMask (mask: GLuint); 
  {$EXTERNALSYM glIndexMask}

procedure glIndexd (c: GLdouble); 
  {$EXTERNALSYM glIndexd}
procedure glIndexdv (c: PGLdouble); 
  {$EXTERNALSYM glIndexdv}
procedure glIndexf (c: GLfloat); 
  {$EXTERNALSYM glIndexf}
procedure glIndexfv (c: PGLfloat); 
  {$EXTERNALSYM glIndexfv}
procedure glIndexi (c: GLint); 
  {$EXTERNALSYM glIndexi}
procedure glIndexiv (c: PGLint); 
  {$EXTERNALSYM glIndexiv}
procedure glIndexs (c: GLshort); 
  {$EXTERNALSYM glIndexs}
procedure glIndexsv (c: PGLshort); 
  {$EXTERNALSYM glIndexsv}
procedure glIndex(c: GLdouble);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: PGLdouble);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: GLfloat);   overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: PGLfloat);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: GLint);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: PGLint);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: GLshort);  overload;
  {$EXTERNALSYM glIndex}
procedure glIndex(c: PGLshort);  overload;
  {$EXTERNALSYM glIndex}

procedure glInitNames; 
  {$EXTERNALSYM glInitNames}
function  glIsEnabled (cap: GLenum): GLBoolean; 
  {$EXTERNALSYM glIsEnabled}
function  glIsList (list: GLuint): GLBoolean;   
  {$EXTERNALSYM glIsList}

procedure glLightModelf (pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glLightModelf}
procedure glLightModelfv (pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glLightModelfv}
procedure glLightModeli (pname: GLenum; param: GLint); 
  {$EXTERNALSYM glLightModeli}
procedure glLightModeliv (pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glLightModeliv}
procedure glLightModel(pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glLightModel}
procedure glLightModel(pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glLightModel}
procedure glLightModel(pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glLightModel}
procedure glLightModel(pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glLightModel}

procedure glLightf (light, pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glLightf}
procedure glLightfv (light, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glLightfv}
procedure glLighti (light, pname: GLenum; param: GLint); 
  {$EXTERNALSYM glLighti}
procedure glLightiv (light, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glLightiv}
procedure glLight(light, pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glLight}
procedure glLight(light, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glLight}
procedure glLight(light, pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glLight}
procedure glLight(light, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glLight}

procedure glLineStipple (factor: GLint; pattern: GLushort); 
  {$EXTERNALSYM glLineStipple}
procedure glLineWidth (width: GLfloat); 
  {$EXTERNALSYM glLineWidth}
procedure glListBase (base: GLuint); 
  {$EXTERNALSYM glListBase}
procedure glLoadIdentity; 
  {$EXTERNALSYM glLoadIdentity}

procedure glLoadMatrixd (m: PGLdouble); 
  {$EXTERNALSYM glLoadMatrixd}
procedure glLoadMatrixf (m: PGLfloat); 
  {$EXTERNALSYM glLoadMatrixf}
procedure glLoadMatrix(m: PGLdouble);  overload;
  {$EXTERNALSYM glLoadMatrix}
procedure glLoadMatrix(m: PGLfloat);  overload;
  {$EXTERNALSYM glLoadMatrix}

procedure glLoadName (name: GLuint); 
  {$EXTERNALSYM glLoadName}
procedure glLogicOp (opcode: GLenum); 
  {$EXTERNALSYM glLogicOp}

procedure glMap1d (target: GLenum; u1,u2: GLdouble; stride, order: GLint;
  Points: PGLdouble); 
  {$EXTERNALSYM glMap1d}
procedure glMap1f (target: GLenum; u1,u2: GLfloat; stride, order: GLint;
  Points: PGLfloat); 
  {$EXTERNALSYM glMap1f}
procedure glMap2d (target: GLenum;
  u1,u2: GLdouble; ustride, uorder: GLint;
  v1,v2: GLdouble; vstride, vorder: GLint; Points: PGLdouble); 
  {$EXTERNALSYM glMap2d}
procedure glMap2f (target: GLenum;
  u1,u2: GLfloat; ustride, uorder: GLint;
  v1,v2: GLfloat; vstride, vorder: GLint; Points: PGLfloat); 
  {$EXTERNALSYM glMap2f}
procedure glMap(target: GLenum; u1,u2: GLdouble; stride, order: GLint;
  Points: PGLdouble);  overload;
  {$EXTERNALSYM glMap}
procedure glMap(target: GLenum; u1,u2: GLfloat; stride, order: GLint;
  Points: PGLfloat);  overload;
  {$EXTERNALSYM glMap}
procedure glMap(target: GLenum;
  u1,u2: GLdouble; ustride, uorder: GLint;
  v1,v2: GLdouble; vstride, vorder: GLint; Points: PGLdouble);  overload;
  {$EXTERNALSYM glMap}
procedure glMap(target: GLenum;
  u1,u2: GLfloat; ustride, uorder: GLint;
  v1,v2: GLfloat; vstride, vorder: GLint; Points: PGLfloat);  overload;
  {$EXTERNALSYM glMap}

procedure glMapGrid1d (un: GLint; u1, u2: GLdouble); 
  {$EXTERNALSYM glMapGrid1d}
procedure glMapGrid1f (un: GLint; u1, u2: GLfloat); 
  {$EXTERNALSYM glMapGrid1f}
procedure glMapGrid2d (un: GLint; u1, u2: GLdouble;
                       vn: GLint; v1, v2: GLdouble); 
  {$EXTERNALSYM glMapGrid2d}
procedure glMapGrid2f (un: GLint; u1, u2: GLfloat;
                       vn: GLint; v1, v2: GLfloat); 
  {$EXTERNALSYM glMapGrid2f}
procedure glMapGrid(un: GLint; u1, u2: GLdouble);  overload;
  {$EXTERNALSYM glMapGrid}
procedure glMapGrid(un: GLint; u1, u2: GLfloat);   overload;
  {$EXTERNALSYM glMapGrid}
procedure glMapGrid(un: GLint; u1, u2: GLdouble;
                    vn: GLint; v1, v2: GLdouble);  overload;
  {$EXTERNALSYM glMapGrid}
procedure glMapGrid(un: GLint; u1, u2: GLfloat;
                    vn: GLint; v1, v2: GLfloat);  overload;
  {$EXTERNALSYM glMapGrid}

procedure glMaterialf (face, pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glMaterialf}
procedure glMaterialfv (face, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glMaterialfv}
procedure glMateriali (face, pname: GLenum; param: GLint); 
  {$EXTERNALSYM glMateriali}
procedure glMaterialiv (face, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glMaterialiv}
procedure glMaterial(face, pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glMaterial}
procedure glMaterial(face, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glMaterial}
procedure glMaterial(face, pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glMaterial}
procedure glMaterial(face, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glMaterial}

procedure glMatrixMode (mode: GLenum); 
  {$EXTERNALSYM glMatrixMode}

procedure glMultMatrixd (m: PGLdouble); 
  {$EXTERNALSYM glMultMatrixd}
procedure glMultMatrixf (m: PGLfloat); 
  {$EXTERNALSYM glMultMatrixf}
procedure glMultMatrix(m: PGLdouble);  overload;
  {$EXTERNALSYM glMultMatrix}
procedure glMultMatrix(m: PGLfloat);  overload;
  {$EXTERNALSYM glMultMatrix}

procedure glNewList (ListIndex: GLuint; mode: GLenum); 
  {$EXTERNALSYM glNewList}

procedure glNormal3b (nx, ny, nz: GLbyte); 
  {$EXTERNALSYM glNormal3b}
procedure glNormal3bv (v: PGLbyte); 
  {$EXTERNALSYM glNormal3bv}
procedure glNormal3d (nx, ny, nz: GLdouble); 
  {$EXTERNALSYM glNormal3d}
procedure glNormal3dv (v: PGLdouble); 
  {$EXTERNALSYM glNormal3dv}
procedure glNormal3f (nx, ny, nz: GLFloat); 
  {$EXTERNALSYM glNormal3f}
procedure glNormal3fv (v: PGLfloat); 
  {$EXTERNALSYM glNormal3fv}
procedure glNormal3i (nx, ny, nz: GLint); 
  {$EXTERNALSYM glNormal3i}
procedure glNormal3iv (v: PGLint); 
  {$EXTERNALSYM glNormal3iv}
procedure glNormal3s (nx, ny, nz: GLshort); 
  {$EXTERNALSYM glNormal3s}
procedure glNormal3sv (v: PGLshort); 
  {$EXTERNALSYM glNormal3sv}
procedure glNormal(nx, ny, nz: GLbyte);  overload;
  {$EXTERNALSYM glNormal}
procedure glNormal3(v: PGLbyte);  overload;
  {$EXTERNALSYM glNormal3}
procedure glNormal(nx, ny, nz: GLdouble);  overload;
  {$EXTERNALSYM glNormal}
procedure glNormal3(v: PGLdouble);  overload;
  {$EXTERNALSYM glNormal3}
procedure glNormal(nx, ny, nz: GLFloat);  overload;
  {$EXTERNALSYM glNormal}
procedure glNormal3(v: PGLfloat);  overload;
  {$EXTERNALSYM glNormal3}
procedure glNormal(nx, ny, nz: GLint);  overload;
  {$EXTERNALSYM glNormal}
procedure glNormal3(v: PGLint);  overload;
  {$EXTERNALSYM glNormal3}
procedure glNormal(nx, ny, nz: GLshort);  overload;
  {$EXTERNALSYM glNormal}
procedure glNormal3(v: PGLshort);  overload;
  {$EXTERNALSYM glNormal3}

procedure glOrtho (left, right, bottom, top, zNear, zFar: GLdouble); 
  {$EXTERNALSYM glOrtho}
procedure glPassThrough (token: GLfloat); 
  {$EXTERNALSYM glPassThrough}

procedure glPixelMapfv (map: GLenum; mapsize: GLint; values: PGLfloat); 
  {$EXTERNALSYM glPixelMapfv}
procedure glPixelMapuiv (map: GLenum; mapsize: GLint; values: PGLuint); 
  {$EXTERNALSYM glPixelMapuiv}
procedure glPixelMapusv (map: GLenum; mapsize: GLint; values: PGLushort); 
  {$EXTERNALSYM glPixelMapusv}
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLfloat);  overload;
  {$EXTERNALSYM glPixelMap}
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLuint);   overload;
  {$EXTERNALSYM glPixelMap}
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLushort);  overload;
  {$EXTERNALSYM glPixelMap}

procedure glPixelStoref (pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glPixelStoref}
procedure glPixelStorei (pname: GLenum; param: GLint); 
  {$EXTERNALSYM glPixelStorei}
procedure glPixelStore(pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glPixelStore}
procedure glPixelStore(pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glPixelStore}

procedure glPixelTransferf (pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glPixelTransferf}
procedure glPixelTransferi (pname: GLenum; param: GLint); 
  {$EXTERNALSYM glPixelTransferi}
procedure glPixelTransfer(pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glPixelTransfer}
procedure glPixelTransfer(pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glPixelTransfer}

procedure glPixelZoom (xfactor, yfactor: GLfloat); 
  {$EXTERNALSYM glPixelZoom}
procedure glPointSize (size: GLfloat); 
  {$EXTERNALSYM glPointSize}
procedure glPolygonMode (face, mode: GLenum); 
  {$EXTERNALSYM glPolygonMode}
procedure glPolygonStipple (mask: PGLubyte); 
  {$EXTERNALSYM glPolygonStipple}
procedure glPopAttrib; 
  {$EXTERNALSYM glPopAttrib}
procedure glPopMatrix; 
  {$EXTERNALSYM glPopMatrix}
procedure glPopName; 
  {$EXTERNALSYM glPopName}
procedure glPushAttrib(mask: GLbitfield); 
  {$EXTERNALSYM glPushAttrib}
procedure glPushMatrix; 
  {$EXTERNALSYM glPushMatrix}
procedure glPushName(name: GLuint); 
  {$EXTERNALSYM glPushName}

procedure glRasterPos2d (x,y: GLdouble); 
  {$EXTERNALSYM glRasterPos2d}
procedure glRasterPos2dv (v: PGLdouble); 
  {$EXTERNALSYM glRasterPos2dv}
procedure glRasterPos2f (x,y: GLfloat); 
  {$EXTERNALSYM glRasterPos2f}
procedure glRasterPos2fv (v: PGLfloat); 
  {$EXTERNALSYM glRasterPos2fv}
procedure glRasterPos2i (x,y: GLint); 
  {$EXTERNALSYM glRasterPos2i}
procedure glRasterPos2iv (v: PGLint); 
  {$EXTERNALSYM glRasterPos2iv}
procedure glRasterPos2s (x,y: GLshort); 
  {$EXTERNALSYM glRasterPos2s}
procedure glRasterPos2sv (v: PGLshort); 
  {$EXTERNALSYM glRasterPos2sv}
procedure glRasterPos3d (x,y,z: GLdouble); 
  {$EXTERNALSYM glRasterPos3d}
procedure glRasterPos3dv (v: PGLdouble); 
  {$EXTERNALSYM glRasterPos3dv}
procedure glRasterPos3f (x,y,z: GLfloat); 
  {$EXTERNALSYM glRasterPos3f}
procedure glRasterPos3fv (v: PGLfloat); 
  {$EXTERNALSYM glRasterPos3fv}
procedure glRasterPos3i (x,y,z: GLint); 
  {$EXTERNALSYM glRasterPos3i}
procedure glRasterPos3iv (v: PGLint); 
  {$EXTERNALSYM glRasterPos3iv}
procedure glRasterPos3s (x,y,z: GLshort); 
  {$EXTERNALSYM glRasterPos3s}
procedure glRasterPos3sv (v: PGLshort); 
  {$EXTERNALSYM glRasterPos3sv}
procedure glRasterPos4d (x,y,z,w: GLdouble); 
  {$EXTERNALSYM glRasterPos4d}
procedure glRasterPos4dv (v: PGLdouble); 
  {$EXTERNALSYM glRasterPos4dv}
procedure glRasterPos4f (x,y,z,w: GLfloat); 
  {$EXTERNALSYM glRasterPos4f}
procedure glRasterPos4fv (v: PGLfloat); 
  {$EXTERNALSYM glRasterPos4fv}
procedure glRasterPos4i (x,y,z,w: GLint); 
  {$EXTERNALSYM glRasterPos4i}
procedure glRasterPos4iv (v: PGLint); 
  {$EXTERNALSYM glRasterPos4iv}
procedure glRasterPos4s (x,y,z,w: GLshort); 
  {$EXTERNALSYM glRasterPos4s}
procedure glRasterPos4sv (v: PGLshort); 
  {$EXTERNALSYM glRasterPos4sv}
procedure glRasterPos(x,y: GLdouble);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos2(v: PGLdouble);  overload;
  {$EXTERNALSYM glRasterPos2}
procedure glRasterPos(x,y: GLfloat);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos2(v: PGLfloat);  overload;
  {$EXTERNALSYM glRasterPos2}
procedure glRasterPos(x,y: GLint);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos2(v: PGLint);  overload;
  {$EXTERNALSYM glRasterPos2}
procedure glRasterPos(x,y: GLshort);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos2(v: PGLshort);  overload;
  {$EXTERNALSYM glRasterPos2}
procedure glRasterPos(x,y,z: GLdouble);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos3(v: PGLdouble);  overload;
  {$EXTERNALSYM glRasterPos3}
procedure glRasterPos(x,y,z: GLfloat);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos3(v: PGLfloat);  overload;
  {$EXTERNALSYM glRasterPos3}
procedure glRasterPos(x,y,z: GLint);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos3(v: PGLint);  overload;
  {$EXTERNALSYM glRasterPos3}
procedure glRasterPos(x,y,z: GLshort);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos3(v: PGLshort);  overload;
  {$EXTERNALSYM glRasterPos3}
procedure glRasterPos(x,y,z,w: GLdouble);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos4(v: PGLdouble);  overload;
  {$EXTERNALSYM glRasterPos4}
procedure glRasterPos(x,y,z,w: GLfloat);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos4(v: PGLfloat);  overload;
  {$EXTERNALSYM glRasterPos4}
procedure glRasterPos(x,y,z,w: GLint);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos4(v: PGLint);  overload;
  {$EXTERNALSYM glRasterPos4}
procedure glRasterPos(x,y,z,w: GLshort);  overload;
  {$EXTERNALSYM glRasterPos}
procedure glRasterPos4(v: PGLshort);  overload;
  {$EXTERNALSYM glRasterPos4}

procedure glReadBuffer (mode: GLenum); 
  {$EXTERNALSYM glReadBuffer}
procedure glReadPixels (x,y: GLint; width, height: GLsizei;
  format, _type: GLenum; pixels: Pointer); 
  {$EXTERNALSYM glReadPixels}

procedure glRectd (x1, y1, x2, y2: GLdouble); 
  {$EXTERNALSYM glRectd}
procedure glRectdv (v1, v2: PGLdouble); 
  {$EXTERNALSYM glRectdv}
procedure glRectf (x1, y1, x2, y2: GLfloat); 
  {$EXTERNALSYM glRectf}
procedure glRectfv (v1, v2: PGLfloat); 
  {$EXTERNALSYM glRectfv}
procedure glRecti (x1, y1, x2, y2: GLint); 
  {$EXTERNALSYM glRecti}
procedure glRectiv (v1, v2: PGLint); 
  {$EXTERNALSYM glRectiv}
procedure glRects (x1, y1, x2, y2: GLshort); 
  {$EXTERNALSYM glRects}
procedure glRectsv (v1, v2: PGLshort); 
  {$EXTERNALSYM glRectsv}
procedure glRect(x1, y1, x2, y2: GLdouble);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(v1, v2: PGLdouble);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(x1, y1, x2, y2: GLfloat);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(v1, v2: PGLfloat);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(x1, y1, x2, y2: GLint);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(v1, v2: PGLint);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(x1, y1, x2, y2: GLshort);  overload;
  {$EXTERNALSYM glRect}
procedure glRect(v1, v2: PGLshort);  overload;
  {$EXTERNALSYM glRect}

function  glRenderMode (mode: GLenum): GLint; 
  {$EXTERNALSYM glRenderMode}

procedure glRotated (angle, x,y,z: GLdouble); 
  {$EXTERNALSYM glRotated}
procedure glRotatef (angle, x,y,z: GLfloat); 
  {$EXTERNALSYM glRotatef}
procedure glRotate(angle, x,y,z: GLdouble);  overload;
  {$EXTERNALSYM glRotate}
procedure glRotate(angle, x,y,z: GLfloat);  overload;
  {$EXTERNALSYM glRotate}

procedure glScaled (x,y,z: GLdouble); 
  {$EXTERNALSYM glScaled}
procedure glScalef (x,y,z: GLfloat); 
  {$EXTERNALSYM glScalef}
procedure glScale(x,y,z: GLdouble);  overload;
  {$EXTERNALSYM glScale}
procedure glScale(x,y,z: GLfloat);  overload;
  {$EXTERNALSYM glScale}

procedure glScissor (x,y: GLint; width, height: GLsizei); 
  {$EXTERNALSYM glScissor}
procedure glSelectBuffer (size: GLsizei; buffer: PGLuint); 
  {$EXTERNALSYM glSelectBuffer}
procedure glShadeModel (mode: GLenum); 
  {$EXTERNALSYM glShadeModel}
procedure glStencilFunc (func: GLenum; ref: GLint; mask: GLuint); 
  {$EXTERNALSYM glStencilFunc}
procedure glStencilMask (mask: GLuint); 
  {$EXTERNALSYM glStencilMask}
procedure glStencilOp (fail, zfail, zpass: GLenum); 
  {$EXTERNALSYM glStencilOp}

procedure glTexCoord1d (s: GLdouble); 
  {$EXTERNALSYM glTexCoord1d}
procedure glTexCoord1dv (v: PGLdouble); 
  {$EXTERNALSYM glTexCoord1dv}
procedure glTexCoord1f (s: GLfloat); 
  {$EXTERNALSYM glTexCoord1f}
procedure glTexCoord1fv (v: PGLfloat); 
  {$EXTERNALSYM glTexCoord1fv}
procedure glTexCoord1i (s: GLint); 
  {$EXTERNALSYM glTexCoord1i}
procedure glTexCoord1iv (v: PGLint); 
  {$EXTERNALSYM glTexCoord1iv}
procedure glTexCoord1s (s: GLshort); 
  {$EXTERNALSYM glTexCoord1s}
procedure glTexCoord1sv (v: PGLshort); 
  {$EXTERNALSYM glTexCoord1sv}
procedure glTexCoord2d (s,t: GLdouble); 
  {$EXTERNALSYM glTexCoord2d}
procedure glTexCoord2dv (v: PGLdouble); 
  {$EXTERNALSYM glTexCoord2dv}
procedure glTexCoord2f (s,t: GLfloat); 
  {$EXTERNALSYM glTexCoord2f}
procedure glTexCoord2fv (v: PGLfloat); 
  {$EXTERNALSYM glTexCoord2fv}
procedure glTexCoord2i (s,t: GLint); 
  {$EXTERNALSYM glTexCoord2i}
procedure glTexCoord2iv (v: PGLint); 
  {$EXTERNALSYM glTexCoord2iv}
procedure glTexCoord2s (s,t: GLshort); 
  {$EXTERNALSYM glTexCoord2s}
procedure glTexCoord2sv (v: PGLshort); 
  {$EXTERNALSYM glTexCoord2sv}
procedure glTexCoord3d (s,t,r: GLdouble); 
  {$EXTERNALSYM glTexCoord3d}
procedure glTexCoord3dv (v: PGLdouble); 
  {$EXTERNALSYM glTexCoord3dv}
procedure glTexCoord3f (s,t,r: GLfloat); 
  {$EXTERNALSYM glTexCoord3f}
procedure glTexCoord3fv (v: PGLfloat); 
  {$EXTERNALSYM glTexCoord3fv}
procedure glTexCoord3i (s,t,r: GLint); 
  {$EXTERNALSYM glTexCoord3i}
procedure glTexCoord3iv (v: PGLint); 
  {$EXTERNALSYM glTexCoord3iv}
procedure glTexCoord3s (s,t,r: GLshort); 
  {$EXTERNALSYM glTexCoord3s}
procedure glTexCoord3sv (v: PGLshort); 
  {$EXTERNALSYM glTexCoord3sv}
procedure glTexCoord4d (s,t,r,q: GLdouble); 
  {$EXTERNALSYM glTexCoord4d}
procedure glTexCoord4dv (v: PGLdouble); 
  {$EXTERNALSYM glTexCoord4dv}
procedure glTexCoord4f (s,t,r,q: GLfloat); 
  {$EXTERNALSYM glTexCoord4f}
procedure glTexCoord4fv (v: PGLfloat); 
  {$EXTERNALSYM glTexCoord4fv}
procedure glTexCoord4i (s,t,r,q: GLint); 
  {$EXTERNALSYM glTexCoord4i}
procedure glTexCoord4iv (v: PGLint); 
  {$EXTERNALSYM glTexCoord4iv}
procedure glTexCoord4s (s,t,r,q: GLshort); 
  {$EXTERNALSYM glTexCoord4s}
procedure glTexCoord4sv (v: PGLshort); 
  {$EXTERNALSYM glTexCoord4sv}
procedure glTexCoord(s: GLdouble);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord1(v: PGLdouble);  overload;
  {$EXTERNALSYM glTexCoord1}
procedure glTexCoord(s: GLfloat);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord1(v: PGLfloat);  overload;
  {$EXTERNALSYM glTexCoord1}
procedure glTexCoord(s: GLint);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord1(v: PGLint);  overload;
  {$EXTERNALSYM glTexCoord1}
procedure glTexCoord(s: GLshort);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord1(v: PGLshort);  overload;
  {$EXTERNALSYM glTexCoord1}
procedure glTexCoord(s,t: GLdouble);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord2(v: PGLdouble);  overload;
  {$EXTERNALSYM glTexCoord2}
procedure glTexCoord(s,t: GLfloat);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord2(v: PGLfloat);  overload;
  {$EXTERNALSYM glTexCoord2}
procedure glTexCoord(s,t: GLint);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord2(v: PGLint);  overload;
  {$EXTERNALSYM glTexCoord2}
procedure glTexCoord(s,t: GLshort);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord2(v: PGLshort);  overload;
  {$EXTERNALSYM glTexCoord2}
procedure glTexCoord(s,t,r: GLdouble);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord3(v: PGLdouble);  overload;
  {$EXTERNALSYM glTexCoord3}
procedure glTexCoord(s,t,r: GLfloat);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord3(v: PGLfloat);  overload;
  {$EXTERNALSYM glTexCoord3}
procedure glTexCoord(s,t,r: GLint);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord3(v: PGLint);  overload;
  {$EXTERNALSYM glTexCoord3}
procedure glTexCoord(s,t,r: GLshort);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord3(v: PGLshort);  overload;
  {$EXTERNALSYM glTexCoord3}
procedure glTexCoord(s,t,r,q: GLdouble);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord4(v: PGLdouble);  overload;
  {$EXTERNALSYM glTexCoord4}
procedure glTexCoord(s,t,r,q: GLfloat);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord4(v: PGLfloat);  overload;
  {$EXTERNALSYM glTexCoord4}
procedure glTexCoord(s,t,r,q: GLint);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord4(v: PGLint);  overload;
  {$EXTERNALSYM glTexCoord4}
procedure glTexCoord(s,t,r,q: GLshort);  overload;
  {$EXTERNALSYM glTexCoord}
procedure glTexCoord4(v: PGLshort);  overload;
  {$EXTERNALSYM glTexCoord4}

procedure glTexEnvf (target, pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glTexEnvf}
procedure glTexEnvfv (target, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glTexEnvfv}
procedure glTexEnvi (target, pname: GLenum; param: GLint); 
  {$EXTERNALSYM glTexEnvi}
procedure glTexEnviv (target, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glTexEnviv}
procedure glTexEnv(target, pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glTexEnv}
procedure glTexEnv(target, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glTexEnv}
procedure glTexEnv(target, pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glTexEnv}
procedure glTexEnv(target, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glTexEnv}

procedure glTexGend (coord, pname: GLenum; param: GLdouble); 
  {$EXTERNALSYM glTexGend}
procedure glTexGendv (coord, pname: GLenum; prms: PGLdouble); 
  {$EXTERNALSYM glTexGendv}
procedure glTexGenf (coord, pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glTexGenf}
procedure glTexGenfv (coord, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glTexGenfv}
procedure glTexGeni (coord, pname: GLenum; param: GLint); 
  {$EXTERNALSYM glTexGeni}
procedure glTexGeniv (coord, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glTexGeniv}
procedure glTexGen(coord, pname: GLenum; param: GLdouble);  overload;
  {$EXTERNALSYM glTexGen}
procedure glTexGen(coord, pname: GLenum; prms: PGLdouble);  overload;
  {$EXTERNALSYM glTexGen}
procedure glTexGen(coord, pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glTexGen}
procedure glTexGen(coord, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glTexGen}
procedure glTexGen(coord, pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glTexGen}
procedure glTexGen(coord, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glTexGen}

procedure glTexImage1D (target: GLenum; level, components: GLint;
  width: GLsizei; border: GLint; format, _type: GLenum; pixels: Pointer); 
  {$EXTERNALSYM glTexImage1D}
procedure glTexImage2D (target: GLenum; level, components: GLint;
  width, height: GLsizei; border: GLint; format, _type: GLenum; pixels: Pointer); 
  {$EXTERNALSYM glTexImage2D}

procedure glTexParameterf (target, pname: GLenum; param: GLfloat); 
  {$EXTERNALSYM glTexParameterf}
procedure glTexParameterfv (target, pname: GLenum; prms: PGLfloat); 
  {$EXTERNALSYM glTexParameterfv}
procedure glTexParameteri (target, pname: GLenum; param: GLint); 
  {$EXTERNALSYM glTexParameteri}
procedure glTexParameteriv (target, pname: GLenum; prms: PGLint); 
  {$EXTERNALSYM glTexParameteriv}
procedure glTexParameter(target, pname: GLenum; param: GLfloat);  overload;
  {$EXTERNALSYM glTexParameter}
procedure glTexParameter(target, pname: GLenum; prms: PGLfloat);  overload;
  {$EXTERNALSYM glTexParameter}
procedure glTexParameter(target, pname: GLenum; param: GLint);  overload;
  {$EXTERNALSYM glTexParameter}
procedure glTexParameter(target, pname: GLenum; prms: PGLint);  overload;
  {$EXTERNALSYM glTexParameter}

procedure glTranslated (x,y,z: GLdouble); 
  {$EXTERNALSYM glTranslated}
procedure glTranslatef (x,y,z: GLfloat); 
  {$EXTERNALSYM glTranslatef}
procedure glTranslate(x,y,z: GLdouble);  overload;
  {$EXTERNALSYM glTranslate}
procedure glTranslate(x,y,z: GLfloat);  overload;
  {$EXTERNALSYM glTranslate}

procedure glVertex2d (x,y: GLdouble); 
  {$EXTERNALSYM glVertex2d}
procedure glVertex2dv (v: PGLdouble); 
  {$EXTERNALSYM glVertex2dv}
procedure glVertex2f (x,y: GLfloat); 
  {$EXTERNALSYM glVertex2f}
procedure glVertex2fv (v: PGLfloat); 
  {$EXTERNALSYM glVertex2fv}
procedure glVertex2i (x,y: GLint); 
  {$EXTERNALSYM glVertex2i}
procedure glVertex2iv (v: PGLint); 
  {$EXTERNALSYM glVertex2iv}
procedure glVertex2s (x,y: GLshort); 
  {$EXTERNALSYM glVertex2s}
procedure glVertex2sv (v: PGLshort); 
  {$EXTERNALSYM glVertex2sv}
procedure glVertex3d (x,y,z: GLdouble); 
  {$EXTERNALSYM glVertex3d}
procedure glVertex3dv (v: PGLdouble); 
  {$EXTERNALSYM glVertex3dv}
procedure glVertex3f (x,y,z: GLfloat); 
  {$EXTERNALSYM glVertex3f}
procedure glVertex3fv (v: PGLfloat); 
  {$EXTERNALSYM glVertex3fv}
procedure glVertex3i (x,y,z: GLint); 
  {$EXTERNALSYM glVertex3i}
procedure glVertex3iv (v: PGLint); 
  {$EXTERNALSYM glVertex3iv}
procedure glVertex3s (x,y,z: GLshort); 
  {$EXTERNALSYM glVertex3s}
procedure glVertex3sv (v: PGLshort); 
  {$EXTERNALSYM glVertex3sv}
procedure glVertex4d (x,y,z,w: GLdouble); 
  {$EXTERNALSYM glVertex4d}
procedure glVertex4dv (v: PGLdouble); 
  {$EXTERNALSYM glVertex4dv}
procedure glVertex4f (x,y,z,w: GLfloat); 
  {$EXTERNALSYM glVertex4f}
procedure glVertex4fv (v: PGLfloat); 
  {$EXTERNALSYM glVertex4fv}
procedure glVertex4i (x,y,z,w: GLint); 
  {$EXTERNALSYM glVertex4i}
procedure glVertex4iv (v: PGLint); 
  {$EXTERNALSYM glVertex4iv}
procedure glVertex4s (x,y,z,w: GLshort); 
  {$EXTERNALSYM glVertex4s}
procedure glVertex4sv (v: PGLshort); 
  {$EXTERNALSYM glVertex4sv}
procedure glVertex(x,y: GLdouble);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex2(v: PGLdouble);  overload;
  {$EXTERNALSYM glVertex2}
procedure glVertex(x,y: GLfloat);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex2(v: PGLfloat);  overload;
  {$EXTERNALSYM glVertex2}
procedure glVertex(x,y: GLint);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex2(v: PGLint);  overload;
  {$EXTERNALSYM glVertex2}
procedure glVertex(x,y: GLshort);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex2(v: PGLshort);  overload;
  {$EXTERNALSYM glVertex2}
procedure glVertex(x,y,z: GLdouble);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex3(v: PGLdouble);  overload;
  {$EXTERNALSYM glVertex3}
procedure glVertex(x,y,z: GLfloat);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex3(v: PGLfloat);  overload;
  {$EXTERNALSYM glVertex3}
procedure glVertex(x,y,z: GLint);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex3(v: PGLint);  overload;
  {$EXTERNALSYM glVertex3}
procedure glVertex(x,y,z: GLshort);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex3(v: PGLshort);  overload;
  {$EXTERNALSYM glVertex3}
procedure glVertex(x,y,z,w: GLdouble);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex4(v: PGLdouble);  overload;
  {$EXTERNALSYM glVertex4}
procedure glVertex(x,y,z,w: GLfloat);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex4(v: PGLfloat);  overload;
  {$EXTERNALSYM glVertex4}
procedure glVertex(x,y,z,w: GLint);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex4(v: PGLint);  overload;
  {$EXTERNALSYM glVertex4}
procedure glVertex(x,y,z,w: GLshort);  overload;
  {$EXTERNALSYM glVertex}
procedure glVertex4(v: PGLshort);  overload;
  {$EXTERNALSYM glVertex4}

procedure glViewport (x,y: GLint; width, height: GLsizei); 
  {$EXTERNALSYM glViewport}

type

// EXT_vertex_array
  PFNGLARRAYELEMENTEXTPROC = procedure (i: GLint); 
  {$EXTERNALSYM PFNGLARRAYELEMENTEXTPROC}
  TGLARRAYELEMENTEXTPROC = PFNGLARRAYELEMENTEXTPROC;
  PFNGLDRAWARRAYSEXTPROC = procedure (mode: GLenum; first: GLint; count: GLsizei); 
  {$EXTERNALSYM PFNGLDRAWARRAYSEXTPROC}
  TGLDRAWARRAYSEXTPROC = PFNGLDRAWARRAYSEXTPROC;
  PFNGLVERTEXPOINTEREXTPROC = procedure (size: GLint; type_: GLenum;
    stride, count: GLsizei; P: Pointer); 
  {$EXTERNALSYM PFNGLVERTEXPOINTEREXTPROC}
  TGLVERTEXPOINTEREXTPROC = PFNGLVERTEXPOINTEREXTPROC;
  PFNGLNORMALPOINTEREXTPROC = procedure (type_: GLenum; stride, count: GLsizei;
    P: Pointer); 
  {$EXTERNALSYM PFNGLNORMALPOINTEREXTPROC}
  TGLNORMALPOINTEREXTPROC = PFNGLNORMALPOINTEREXTPROC;
  PFNGLCOLORPOINTEREXTPROC = procedure (size: GLint; type_: GLenum;
    stride, count: GLsizei; P: Pointer); 
  {$EXTERNALSYM PFNGLCOLORPOINTEREXTPROC}
  TGLCOLORPOINTEREXTPROC = PFNGLCOLORPOINTEREXTPROC;
  PFNGLINDEXPOINTEREXTPROC = procedure (type_: GLenum; stride, count: GLsizei;
    P: Pointer); 
  {$EXTERNALSYM PFNGLINDEXPOINTEREXTPROC}
  TGLINDEXPOINTEREXTPROC = PFNGLINDEXPOINTEREXTPROC;
  PFNGLTEXCOORDPOINTEREXTPROC = procedure (size: GLint; type_: GLenum;
    stride, count: GLsizei; P: Pointer); 
  {$EXTERNALSYM PFNGLTEXCOORDPOINTEREXTPROC}
  TGLTEXCOORDPOINTEREXTPROC = PFNGLTEXCOORDPOINTEREXTPROC;
  PFNGLEDGEFLAGPOINTEREXTPROC = procedure (stride, count: GLsizei;
    P: PGLboolean); 
  {$EXTERNALSYM PFNGLEDGEFLAGPOINTEREXTPROC}
  TGLEDGEFLAGPOINTEREXTPROC = PFNGLEDGEFLAGPOINTEREXTPROC;
  PFNGLGETPOINTERVEXTPROC = procedure (pname: GLenum; var prms : pointer); 
  {$EXTERNALSYM PFNGLGETPOINTERVEXTPROC}
  TGLGETPOINTERVEXTPROC = PFNGLGETPOINTERVEXTPROC;

// WIN_swap_hint

  PFNGLADDSWAPHINTRECTWINPROC = procedure (x, y: GLint; width, height: GLsizei); 
  {$EXTERNALSYM PFNGLADDSWAPHINTRECTWINPROC}
  TGLADDSWAPHINTRECTWINPROC = PFNGLADDSWAPHINTRECTWINPROC;

{ OpenGL Utility routines (glu.h) =======================================}

function gluErrorString (errCode: GLenum): PChar; 
  {$EXTERNALSYM gluErrorString}
function gluErrorUnicodeStringEXT (errCode: GLenum): PWChar; 
  {$EXTERNALSYM gluErrorUnicodeStringEXT}
function gluGetString (name: GLenum): PChar; 
  {$EXTERNALSYM gluGetString}

procedure gluLookAt(eyex, eyey, eyez,
                    centerx, centery, centerz,
                    upx, upy, upz: GLdouble); 
  {$EXTERNALSYM gluLookAt}
procedure gluOrtho2D(left, right, bottom, top: GLdouble); 
  {$EXTERNALSYM gluOrtho2D}
procedure gluPerspective(fovy, aspect, zNear, zFar: GLdouble); 
  {$EXTERNALSYM gluPerspective}
procedure gluPickMatrix (x, y, width, height: GLdouble; viewport: PGLint); 
  {$EXTERNALSYM gluPickMatrix}
function  gluProject (objx, objy, obyz: GLdouble;
                      modelMatrix: PGLdouble;
                      projMatrix: PGLdouble;
                      viewport: PGLint;
                      var winx, winy, winz: GLDouble): Integer; 
  {$EXTERNALSYM gluProject}
function  gluUnProject(winx, winy, winz: GLdouble;
                      modelMatrix: PGLdouble;
                      projMatrix: PGLdouble;
                      viewport: PGLint;
                      var objx, objy, objz: GLdouble): Integer; 
  {$EXTERNALSYM gluUnProject}
function  gluScaleImage(format: GLenum;
   widthin, heightin: GLint; typein: GLenum; datain: Pointer;
   widthout, heightout: GLint; typeout: GLenum; dataout: Pointer): Integer; 
  {$EXTERNALSYM gluScaleImage}

function  gluBuild1DMipmaps (target: GLenum; components, width: GLint;
                             format, atype: GLenum; data: Pointer): Integer; 
  {$EXTERNALSYM gluBuild1DMipmaps}
function  gluBuild2DMipmaps (target: GLenum; components, width, height: GLint;
                             format, atype: GLenum; data: Pointer): Integer; 
  {$EXTERNALSYM gluBuild2DMipmaps}

type
  _GLUquadricObj = record end;
  GLUquadricObj = ^_GLUquadricObj;
  {$EXTERNALSYM GLUquadricObj}

  GLUquadricErrorProc = procedure (error: GLenum); 
  TGLUquadricErrorProc = GLUquadricErrorProc;
  {$EXTERNALSYM GLUquadricErrorProc}

function  gluNewQuadric: GLUquadricObj; 
  {$EXTERNALSYM gluNewQuadric}
procedure gluDeleteQuadric (state: GLUquadricObj); 
  {$EXTERNALSYM gluDeleteQuadric}
procedure gluQuadricNormals (quadObject: GLUquadricObj; normals: GLenum);  
  {$EXTERNALSYM gluQuadricNormals}
procedure gluQuadricTexture (quadObject: GLUquadricObj; textureCoords: GLboolean );
  {$EXTERNALSYM gluQuadricTexture}
procedure gluQuadricOrientation (quadObject: GLUquadricObj; orientation: GLenum); 
  {$EXTERNALSYM gluQuadricOrientation}
procedure gluQuadricDrawStyle (quadObject: GLUquadricObj; drawStyle: GLenum); 
  {$EXTERNALSYM gluQuadricDrawStyle}
procedure gluCylinder (quadObject: GLUquadricObj;
  baseRadius, topRadius, height: GLdouble; slices, stacks: GLint); 
  {$EXTERNALSYM gluCylinder}
procedure gluDisk (quadObject: GLUquadricObj;
  innerRadius, outerRadius: GLdouble; slices, loops: GLint); 
  {$EXTERNALSYM gluDisk}
procedure gluPartialDisk (quadObject: GLUquadricObj;
  innerRadius, outerRadius: GLdouble; slices, loops: GLint;
  startAngle, sweepAngle: GLdouble); 
  {$EXTERNALSYM gluPartialDisk}
procedure gluSphere (quadObject: GLUquadricObj; radius: GLdouble; slices, loops: GLint); 
procedure gluQuadricCallback (quadObject: GLUquadricObj; which: GLenum;
  callback: Pointer); 
  {$EXTERNALSYM gluSphere}

type
  _GLUtesselator = record end;
  GLUtesselator = ^_GLUtesselator;
  {$EXTERNALSYM GLUtesselator}

  // tesselator callback procedure types
  GLUtessBeginProc = procedure (a: GLenum); 
  {$EXTERNALSYM GLUtessBeginProc}
  TGLUtessBeginProc = GLUtessBeginProc;
  GLUtessEdgeFlagProc = procedure (flag: GLboolean); 
  {$EXTERNALSYM GLUtessEdgeFlagProc}
  TGLUtessEdgeFlagProc = GLUtessEdgeFlagProc;
  GLUtessVertexProc = procedure (p: Pointer); 
  {$EXTERNALSYM GLUtessVertexProc}
  TGLUtessVertexProc = GLUtessVertexProc;
  GLUtessEndProc = procedure; 
  {$EXTERNALSYM GLUtessEndProc}
  TGLUtessEndProc = GLUtessEndProc;
  GLUtessErrorProc = TGLUquadricErrorProc;
  {$EXTERNALSYM GLUtessErrorProc}
  GLUtessCombineProc = procedure (a: PGLdouble; b: Pointer;
                                   c: PGLfloat; var d: Pointer); 
  {$EXTERNALSYM GLUtessCombineProc}
  TGLUtessCombineProc = GLUtessCombineProc;

function gluNewTess: GLUtesselator; 
  {$EXTERNALSYM gluNewTess}
procedure gluDeleteTess( tess: GLUtesselator ); 
  {$EXTERNALSYM gluDeleteTess}
procedure gluTessBeginPolygon( tess: GLUtesselator; gon_data: Pointer ); 
  {$EXTERNALSYM gluTessBeginPolygon}
procedure gluTessBeginContour( tess: GLUtesselator ); 
  {$EXTERNALSYM gluTessBeginContour}
procedure gluTessVertex( tess: GLUtesselator; coords: PGLdouble; data: Pointer ); 
  {$EXTERNALSYM gluTessVertex}
procedure gluTessEndContour( tess: GLUtesselator ); 
  {$EXTERNALSYM gluTessEndContour}
procedure gluTessEndPolygon( tess: GLUtesselator ); 
  {$EXTERNALSYM gluTessEndPolygon}
procedure gluTessProperty( tess: GLUtesselator; which: GLenum; value: GLdouble); 
  {$EXTERNALSYM gluTessProperty}
procedure gluTessNormal( tess: GLUtesselator; x,y,z: GLdouble); 
  {$EXTERNALSYM gluTessNormal}
procedure gluTessCallback( tess: GLUtesselator; which: GLenum; callback: pointer); 
  {$EXTERNALSYM gluTessCallback}

type
  TGLUnurbsObj = record end;
  GLUnurbsObj = ^TGLUnurbsObj;
  {$EXTERNALSYM GLUnurbsObj}

  GLUnurbsErrorProc = GLUquadricErrorProc;
  {$EXTERNALSYM GLUnurbsErrorProc}
  TGLUnurbsErrorProc = GLUnurbsErrorProc;

function gluNewNurbsRenderer: GLUnurbsObj; 
  {$EXTERNALSYM gluNewNurbsRenderer}
procedure gluDeleteNurbsRenderer (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluDeleteNurbsRenderer}
procedure gluBeginSurface (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluBeginSurface}
procedure gluBeginCurve (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluBeginCurve}
procedure gluEndCurve (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluEndCurve}
procedure gluEndSurface (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluEndSurface}
procedure gluBeginTrim (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluBeginTrim}
procedure gluEndTrim (nobj: GLUnurbsObj); 
  {$EXTERNALSYM gluEndTrim}
procedure gluPwlCurve (nobj: GLUnurbsObj; count: GLint; points: PGLfloat;
  stride: GLint; _type: GLenum); 
  {$EXTERNALSYM gluPwlCurve}
procedure gluNurbsCurve (nobj: GLUnurbsObj; nknots: GLint; knot: PGLfloat;
  stride: GLint; ctlpts: PGLfloat; order: GLint; _type: GLenum); 
  {$EXTERNALSYM gluNurbsCurve}
procedure gluNurbsSurface (nobj: GLUnurbsObj;
  sknot_count: GLint; sknot: PGLfloat;
  tknot_count: GLint; tknot: PGLfloat;
  s_stride, t_stride: GLint;
  ctlpts: PGLfloat; sorder, torder: GLint; _type: GLenum); 
  {$EXTERNALSYM gluNurbsSurface}
procedure gluLoadSamplingMatrices (nobj: GLUnurbsObj;
  modelMatrix: PGLdouble; projMatrix: PGLdouble; viewport: PGLint); 
  {$EXTERNALSYM gluLoadSamplingMatrices}
procedure gluNurbsProperty (nobj: GLUnurbsObj; prop: GLenum; value: GLfloat); 
  {$EXTERNALSYM gluNurbsProperty}
procedure gluGetNurbsProperty (nobj: GLUnurbsObj; prop: GLenum; var value: GLfloat); 
  {$EXTERNALSYM gluGetNurbsProperty}
procedure gluNurbsCallback (nobj: GLUnurbsObj; which: GLenum; callback: pointer); 
  {$EXTERNALSYM gluNurbsCallback}

{****           Generic constants               ****}
const
  GLU_VERSION_1_1  =               1;
  {$EXTERNALSYM GLU_VERSION_1_1}

{ Errors: (return value 0 = no error) }
  GLU_INVALID_ENUM       = 100900;
  {$EXTERNALSYM GLU_INVALID_ENUM}
  GLU_INVALID_VALUE      = 100901;
  {$EXTERNALSYM GLU_INVALID_VALUE}
  GLU_OUT_OF_MEMORY      = 100902;
  {$EXTERNALSYM GLU_OUT_OF_MEMORY}
  GLU_INCOMPATIBLE_GL_VERSION  =   100903;
  {$EXTERNALSYM GLU_INCOMPATIBLE_GL_VERSION}

{ gets }
  GLU_VERSION            = 100800;
  {$EXTERNALSYM GLU_VERSION}
  GLU_EXTENSIONS         = 100801;
  {$EXTERNALSYM GLU_EXTENSIONS}

{ For laughs: }
  GLU_TRUE               = GL_TRUE;
  {$EXTERNALSYM GLU_TRUE}
  GLU_FALSE              = GL_FALSE;
  {$EXTERNALSYM GLU_FALSE}

{***           Quadric constants               ***}

{ Types of normals: }
  GLU_SMOOTH             = 100000;
  {$EXTERNALSYM GLU_SMOOTH}
  GLU_FLAT               = 100001;
  {$EXTERNALSYM GLU_FLAT}
  GLU_NONE               = 100002;
  {$EXTERNALSYM GLU_NONE}

{ DrawStyle types: }
  GLU_POINT              = 100010;
  {$EXTERNALSYM GLU_POINT}
  GLU_LINE               = 100011;
  {$EXTERNALSYM GLU_LINE}
  GLU_FILL               = 100012;
  {$EXTERNALSYM GLU_FILL}
  GLU_SILHOUETTE         = 100013;
  {$EXTERNALSYM GLU_SILHOUETTE}

{ Orientation types: }
  GLU_OUTSIDE            = 100020;
  {$EXTERNALSYM GLU_OUTSIDE}
  GLU_INSIDE             = 100021;
  {$EXTERNALSYM GLU_INSIDE}

{ Callback types: }
{      GLU_ERROR               100103 }


{***           Tesselation constants           ***}

  GLU_TESS_MAX_COORD     =         1.0e150;
  {$EXTERNALSYM GLU_TESS_MAX_COORD}

{ Property types: }
  GLU_TESS_WINDING_RULE  =         100110;
  {$EXTERNALSYM GLU_TESS_WINDING_RULE}
  GLU_TESS_BOUNDARY_ONLY =         100111;
  {$EXTERNALSYM GLU_TESS_BOUNDARY_ONLY}
  GLU_TESS_TOLERANCE     =         100112;
  {$EXTERNALSYM GLU_TESS_TOLERANCE}

{ Possible winding rules: }
  GLU_TESS_WINDING_ODD          =  100130;
  {$EXTERNALSYM GLU_TESS_WINDING_ODD}
  GLU_TESS_WINDING_NONZERO      =  100131;
  {$EXTERNALSYM GLU_TESS_WINDING_NONZERO}
  GLU_TESS_WINDING_POSITIVE     =  100132;
  {$EXTERNALSYM GLU_TESS_WINDING_POSITIVE}
  GLU_TESS_WINDING_NEGATIVE     =  100133;
  {$EXTERNALSYM GLU_TESS_WINDING_NEGATIVE}
  GLU_TESS_WINDING_ABS_GEQ_TWO  =  100134;
  {$EXTERNALSYM GLU_TESS_WINDING_ABS_GEQ_TWO}

{ Callback types: }
  GLU_TESS_BEGIN     = 100100 ;     { void (*)(GLenum    type)         }
  {$EXTERNALSYM GLU_TESS_BEGIN}
  GLU_TESS_VERTEX    = 100101 ;     { void (*)(void      *data)        }
  {$EXTERNALSYM GLU_TESS_VERTEX}
  GLU_TESS_END       = 100102 ;     { void (*)(void)                   }
  {$EXTERNALSYM GLU_TESS_END}
  GLU_TESS_ERROR     = 100103 ;     { void (*)(GLenum    errno)        }
  {$EXTERNALSYM GLU_TESS_ERROR}
  GLU_TESS_EDGE_FLAG = 100104 ;     { void (*)(GLboolean boundaryEdge) }
  {$EXTERNALSYM GLU_TESS_EDGE_FLAG}
  GLU_TESS_COMBINE   = 100105 ;     { void (*)(GLdouble  coords[3],;
                                                    void      *data[4],;
                                                    GLfloat   weight[4],;
                                                    void      **dataOut)    }
  {$EXTERNALSYM GLU_TESS_COMBINE}

{ Errors: }
  GLU_TESS_ERROR1    = 100151;
  {$EXTERNALSYM GLU_TESS_ERROR1}
  GLU_TESS_ERROR2    = 100152;
  {$EXTERNALSYM GLU_TESS_ERROR2}
  GLU_TESS_ERROR3    = 100153;
  {$EXTERNALSYM GLU_TESS_ERROR3}
  GLU_TESS_ERROR4    = 100154;
  {$EXTERNALSYM GLU_TESS_ERROR4}
  GLU_TESS_ERROR5    = 100155;
  {$EXTERNALSYM GLU_TESS_ERROR5}
  GLU_TESS_ERROR6    = 100156;
  {$EXTERNALSYM GLU_TESS_ERROR6}
  GLU_TESS_ERROR7    = 100157;
  {$EXTERNALSYM GLU_TESS_ERROR7}
  GLU_TESS_ERROR8    = 100158;
  {$EXTERNALSYM GLU_TESS_ERROR8}

  GLU_TESS_MISSING_BEGIN_POLYGON  = GLU_TESS_ERROR1;
  {$EXTERNALSYM GLU_TESS_MISSING_BEGIN_POLYGON}
  GLU_TESS_MISSING_BEGIN_CONTOUR  = GLU_TESS_ERROR2;
  {$EXTERNALSYM GLU_TESS_MISSING_BEGIN_CONTOUR}
  GLU_TESS_MISSING_END_POLYGON    = GLU_TESS_ERROR3;
  {$EXTERNALSYM GLU_TESS_MISSING_END_POLYGON}
  GLU_TESS_MISSING_END_CONTOUR    = GLU_TESS_ERROR4;
  {$EXTERNALSYM GLU_TESS_MISSING_END_CONTOUR}
  GLU_TESS_COORD_TOO_LARGE        = GLU_TESS_ERROR5;
  {$EXTERNALSYM GLU_TESS_COORD_TOO_LARGE}
  GLU_TESS_NEED_COMBINE_CALLBACK  = GLU_TESS_ERROR6;
  {$EXTERNALSYM GLU_TESS_NEED_COMBINE_CALLBACK}

{***           NURBS constants                 ***}

{ Properties: }
  GLU_AUTO_LOAD_MATRIX          =  100200;
  {$EXTERNALSYM GLU_AUTO_LOAD_MATRIX}
  GLU_CULLING                   =  100201;
  {$EXTERNALSYM GLU_CULLING}
  GLU_SAMPLING_TOLERANCE        =  100203;
  {$EXTERNALSYM GLU_SAMPLING_TOLERANCE}
  GLU_DISPLAY_MODE              =  100204;
  {$EXTERNALSYM GLU_DISPLAY_MODE}
  GLU_PARAMETRIC_TOLERANCE      =  100202;
  {$EXTERNALSYM GLU_PARAMETRIC_TOLERANCE}
  GLU_SAMPLING_METHOD           =  100205;
  {$EXTERNALSYM GLU_SAMPLING_METHOD}
  GLU_U_STEP                    =  100206;
  {$EXTERNALSYM GLU_U_STEP}
  GLU_V_STEP                    =  100207;
  {$EXTERNALSYM GLU_V_STEP}

{ Sampling methods: }
  GLU_PATH_LENGTH               =  100215;
  {$EXTERNALSYM GLU_PATH_LENGTH}
  GLU_PARAMETRIC_ERROR          =  100216;
  {$EXTERNALSYM GLU_PARAMETRIC_ERROR}
  GLU_DOMAIN_DISTANCE           =  100217;
  {$EXTERNALSYM GLU_DOMAIN_DISTANCE}

{ Trimming curve types }
  GLU_MAP1_TRIM_2       =  100210;
  {$EXTERNALSYM GLU_MAP1_TRIM_2}
  GLU_MAP1_TRIM_3       =  100211;
  {$EXTERNALSYM GLU_MAP1_TRIM_3}

{ Display modes: }
{      GLU_FILL                100012 }
  GLU_OUTLINE_POLYGON    = 100240;
  {$EXTERNALSYM GLU_OUTLINE_POLYGON}
  GLU_OUTLINE_PATCH      = 100241;
  {$EXTERNALSYM GLU_OUTLINE_PATCH}

{ Callbacks: }
{      GLU_ERROR               100103 }

{ Errors: }
  GLU_NURBS_ERROR1       = 100251;
  {$EXTERNALSYM GLU_NURBS_ERROR1}
  GLU_NURBS_ERROR2       = 100252;
  {$EXTERNALSYM GLU_NURBS_ERROR2}
  GLU_NURBS_ERROR3       = 100253;
  {$EXTERNALSYM GLU_NURBS_ERROR3}
  GLU_NURBS_ERROR4       = 100254;
  {$EXTERNALSYM GLU_NURBS_ERROR4}
  GLU_NURBS_ERROR5       = 100255;
  {$EXTERNALSYM GLU_NURBS_ERROR5}
  GLU_NURBS_ERROR6       = 100256;
  {$EXTERNALSYM GLU_NURBS_ERROR6}
  GLU_NURBS_ERROR7       = 100257;
  {$EXTERNALSYM GLU_NURBS_ERROR7}
  GLU_NURBS_ERROR8       = 100258;
  {$EXTERNALSYM GLU_NURBS_ERROR8}
  GLU_NURBS_ERROR9       = 100259;
  {$EXTERNALSYM GLU_NURBS_ERROR9}
  GLU_NURBS_ERROR10      = 100260;
  {$EXTERNALSYM GLU_NURBS_ERROR10}
  GLU_NURBS_ERROR11      = 100261;
  {$EXTERNALSYM GLU_NURBS_ERROR11}
  GLU_NURBS_ERROR12      = 100262;
  {$EXTERNALSYM GLU_NURBS_ERROR12}
  GLU_NURBS_ERROR13      = 100263;
  {$EXTERNALSYM GLU_NURBS_ERROR13}
  GLU_NURBS_ERROR14      = 100264;
  {$EXTERNALSYM GLU_NURBS_ERROR14}
  GLU_NURBS_ERROR15      = 100265;
  {$EXTERNALSYM GLU_NURBS_ERROR15}
  GLU_NURBS_ERROR16      = 100266;
  {$EXTERNALSYM GLU_NURBS_ERROR16}
  GLU_NURBS_ERROR17      = 100267;
  {$EXTERNALSYM GLU_NURBS_ERROR17}
  GLU_NURBS_ERROR18      = 100268;
  {$EXTERNALSYM GLU_NURBS_ERROR18}
  GLU_NURBS_ERROR19      = 100269;
  {$EXTERNALSYM GLU_NURBS_ERROR19}
  GLU_NURBS_ERROR20      = 100270;
  {$EXTERNALSYM GLU_NURBS_ERROR20}
  GLU_NURBS_ERROR21      = 100271;
  {$EXTERNALSYM GLU_NURBS_ERROR21}
  GLU_NURBS_ERROR22      = 100272;
  {$EXTERNALSYM GLU_NURBS_ERROR22}
  GLU_NURBS_ERROR23      = 100273;
  {$EXTERNALSYM GLU_NURBS_ERROR23}
  GLU_NURBS_ERROR24      = 100274;
  {$EXTERNALSYM GLU_NURBS_ERROR24}
  GLU_NURBS_ERROR25      = 100275;
  {$EXTERNALSYM GLU_NURBS_ERROR25}
  GLU_NURBS_ERROR26      = 100276;
  {$EXTERNALSYM GLU_NURBS_ERROR26}
  GLU_NURBS_ERROR27      = 100277;
  {$EXTERNALSYM GLU_NURBS_ERROR27}
  GLU_NURBS_ERROR28      = 100278;
  {$EXTERNALSYM GLU_NURBS_ERROR28}
  GLU_NURBS_ERROR29      = 100279;
  {$EXTERNALSYM GLU_NURBS_ERROR29}
  GLU_NURBS_ERROR30      = 100280;
  {$EXTERNALSYM GLU_NURBS_ERROR30}
  GLU_NURBS_ERROR31      = 100281;
  {$EXTERNALSYM GLU_NURBS_ERROR31}
  GLU_NURBS_ERROR32      = 100282;
  {$EXTERNALSYM GLU_NURBS_ERROR32}
  GLU_NURBS_ERROR33      = 100283;
  {$EXTERNALSYM GLU_NURBS_ERROR33}
  GLU_NURBS_ERROR34      = 100284;
  {$EXTERNALSYM GLU_NURBS_ERROR34}
  GLU_NURBS_ERROR35      = 100285;
  {$EXTERNALSYM GLU_NURBS_ERROR35}
  GLU_NURBS_ERROR36      = 100286;
  {$EXTERNALSYM GLU_NURBS_ERROR36}
  GLU_NURBS_ERROR37      = 100287;
  {$EXTERNALSYM GLU_NURBS_ERROR37}

{
/****           Backwards compatibility for old tesselator           ****/

typedef GLUtesselator GLUtriangulatorObj;

procedure   gluBeginPolygon( tess: GLUtesselator );

procedure   gluNextContour(  tess: GLUtesselator,
                                 GLenum        type );

procedure   gluEndPolygon(   tess: GLUtesselator );

/* Contours types -- obsolete! */
#define GLU_CW          100120
#define GLU_CCW         100121
#define GLU_INTERIOR    100122
#define GLU_EXTERIOR    100123
#define GLU_UNKNOWN     100124

/* Names without "TESS_" prefix */
#define GLU_BEGIN       GLU_TESS_BEGIN
#define GLU_VERTEX      GLU_TESS_VERTEX
#define GLU_END         GLU_TESS_END
#define GLU_ERROR       GLU_TESS_ERROR
#define GLU_EDGE_FLAG   GLU_TESS_EDGE_FLAG
}

{ GDI support routines for OpenGL ==========================================}

function wglGetProcAddress(ProcName: PChar): Pointer;  
  {$EXTERNALSYM wglGetProcAddress}

function wglMakeCurrent(_hdc : HDC; _hglrc : HGLRC) : BOOL;
function wglCreateContext(_hdc : HDC) : HGLRC;
function GetDC(_hwnd : HWND) : HDC;
function SetPixelFormat(_hdc: HDC; iPixelFormat : integer; ppfd : ^PIXELFORMATDESCRIPTOR): BOOL;
function ChoosePixelFormat(_hdc: HDC; ppfd : ^PIXELFORMATDESCRIPTOR): integer;
function SwapBuffers(_hdc : HDC) : BOOL;
function wglDeleteContext(_hgrlc : HGLRC) :BOOL;

procedure OpenGLInit(Handle : IntPtr);
procedure OpenGLUninit(Handle : IntPtr);

const
  glu32 = 'glu32.dll';
  opengl32 = 'opengl32.dll';
  
implementation

procedure glAccum (op: GLenum; value: GLfloat); external opengl32 name 'glAccum';
procedure glAlphaFunc (func: GLenum; ref: GLclampf); external opengl32 name 'glAlphaFunc';
procedure glBegin (mode: GLenum); external opengl32 name 'glBegin';
procedure glBitmap (width, height: GLsizei; xorig, yorig: GLfloat;
                    xmove, ymove: GLfloat; bitmap: Pointer); external opengl32 name 'glBitmap';
procedure glBlendFunc (sfactor, dfactor: GLenum); external opengl32 name 'glBlendFunc';
procedure glCallList (list: GLuint); external opengl32 name 'glCallList';
procedure glCallLists (n: GLsizei; cltype: GLenum; lists: Pointer); external opengl32 name 'glCallLists';
procedure glClear (mask: GLbitfield); external opengl32 name 'glClear';
procedure glClearAccum (red, green, blue, alpha: GLfloat); external opengl32 name 'glClearAccum';
procedure glClearColor (red, green, blue, alpha: GLclampf); external opengl32 name 'glClearColor';
procedure glClearDepth (depth: GLclampd); external opengl32 name 'glClearDepth';
procedure glClearIndex (c: GLfloat); external opengl32 name 'glClearIndex';
procedure glClearStencil (s: GLint); external opengl32 name 'glClearStencil';
procedure glClipPlane (plane: GLenum; equation: PGLDouble); external opengl32 name 'glClipPlane';
procedure glColor3b (red, green, blue: GLbyte); external opengl32 name 'glColor3b';
procedure glColor3bv (v: PGLByte); external opengl32 name 'glColor3bv';
procedure glColor3d (red, green, blue: GLdouble); external opengl32 name 'glColor3d';
procedure glColor3dv (v: PGLdouble); external opengl32 name 'glColor3dv';
procedure glColor3f (red, green, blue: GLfloat); external opengl32 name 'glColor3f';
procedure glColor3fv (v: PGLfloat); external opengl32 name 'glColor3fv';
procedure glColor3i (red, green, blue: GLint); external opengl32 name 'glColor3i';
procedure glColor3iv (v: PGLint); external opengl32 name 'glColor3iv';
procedure glColor3s (red, green, blue: GLshort); external opengl32 name 'glColor3s';
procedure glColor3sv (v: PGLshort); external opengl32 name 'glColor3sv';
procedure glColor3ub (red, green, blue: GLubyte); external opengl32 name 'glColor3ub';
procedure glColor3ubv (v: PGLubyte); external opengl32 name 'glColor3ubv';
procedure glColor3ui (red, green, blue: GLuint); external opengl32 name 'glColor3ui';
procedure glColor3uiv (v: PGLuint); external opengl32 name 'glColor3uiv';
procedure glColor3us (red, green, blue: GLushort); external opengl32 name 'glColor3us';
procedure glColor3usv (v: PGLushort); external opengl32 name 'glColor3usv';
procedure glColor4b (red, green, blue, alpha: GLbyte); external opengl32 name 'glColor4b';
procedure glColor4bv (v: PGLbyte); external opengl32 name 'glColor4bv';
procedure glColor4d (red, green, blue, alpha: GLdouble); external opengl32 name 'glColor4d';
procedure glColor4dv (v: PGLdouble); external opengl32 name 'glColor4dv';
procedure glColor4f (red, green, blue, alpha: GLfloat); external opengl32 name 'glColor4f';
procedure glColor4fv (v: PGLfloat); external opengl32 name 'glColor4fv';
procedure glColor4i (red, green, blue, alpha: GLint); external opengl32 name 'glColor4i';
procedure glColor4iv (v: PGLint); external opengl32 name 'glColor4iv';
procedure glColor4s (red, green, blue, alpha: GLshort); external opengl32 name 'glColor4s';
procedure glColor4sv (v: PGLshort); external opengl32 name 'glColor4sv';
procedure glColor4ub (red, green, blue, alpha: GLubyte); external opengl32 name 'glColor4ub';
procedure glColor4ubv (v: PGLubyte); external opengl32 name 'glColor4ubv';
procedure glColor4ui (red, green, blue, alpha: GLuint); external opengl32 name 'glColor4ui';
procedure glColor4uiv (v: PGLuint); external opengl32 name 'glColor4uiv';
procedure glColor4us (red, green, blue, alpha: GLushort); external opengl32 name 'glColor4us';
procedure glColor4usv (v: PGLushort); external opengl32 name 'glColor4usv';
procedure glColor(red, green, blue: GLbyte); external opengl32 name 'glColor3b';
procedure glColor(red, green, blue: GLdouble); external opengl32 name 'glColor3d';
procedure glColor(red, green, blue: GLfloat); external opengl32 name 'glColor3f';
procedure glColor(red, green, blue: GLint); external opengl32 name 'glColor3i';
procedure glColor(red, green, blue: GLshort); external opengl32 name 'glColor3s';
procedure glColor(red, green, blue: GLubyte); external opengl32 name 'glColor3ub';
procedure glColor(red, green, blue: GLuint); external opengl32 name 'glColor3ui';
procedure glColor(red, green, blue: GLushort); external opengl32 name 'glColor3us';
procedure glColor(red, green, blue, alpha: GLbyte); external opengl32 name 'glColor4b';
procedure glColor(red, green, blue, alpha: GLdouble); external opengl32 name 'glColor4d';
procedure glColor(red, green, blue, alpha: GLfloat); external opengl32 name 'glColor4f';
procedure glColor(red, green, blue, alpha: GLint); external opengl32 name 'glColor4i';
procedure glColor(red, green, blue, alpha: GLshort); external opengl32 name 'glColor4s';
procedure glColor(red, green, blue, alpha: GLubyte); external opengl32 name 'glColor4ub';
procedure glColor(red, green, blue, alpha: GLuint); external opengl32 name 'glColor4ui';
procedure glColor(red, green, blue, alpha: GLushort); external opengl32 name 'glColor4us';
procedure glColor3(v: PGLbyte); external opengl32 name 'glColor3bv';
procedure glColor3(v: PGLdouble); external opengl32 name 'glColor3dv';
procedure glColor3(v: PGLfloat); external opengl32 name 'glColor3fv';
procedure glColor3(v: PGLint); external opengl32 name 'glColor3iv';
procedure glColor3(v: PGLshort); external opengl32 name 'glColor3sv';
procedure glColor3(v: PGLubyte); external opengl32 name 'glColor3ubv';
procedure glColor3(v: PGLuint); external opengl32 name 'glColor3uiv';
procedure glColor3(v: PGLushort); external opengl32 name 'glColor3usv';
procedure glColor4(v: PGLbyte); external opengl32 name 'glColor4bv';
procedure glColor4(v: PGLdouble); external opengl32 name 'glColor4dv';
procedure glColor4(v: PGLfloat); external opengl32 name 'glColor4fv';
procedure glColor4(v: PGLint); external opengl32 name 'glColor4iv';
procedure glColor4(v: PGLshort); external opengl32 name 'glColor4sv';
procedure glColor4(v: PGLubyte); external opengl32 name 'glColor4ubv';
procedure glColor4(v: PGLuint); external opengl32 name 'glColor4uiv';
procedure glColor4(v: PGLushort); external opengl32 name 'glColor4usv';
procedure glColorMask (red, green, blue, alpha: GLboolean); external opengl32 name 'glColorMask';
procedure glColorMaterial (face, mode: GLenum); external opengl32 name 'glColorMaterial';
procedure glCopyPixels (x,y: GLint; width, height: GLsizei; pixeltype: GLenum); external opengl32 name 'glCopyPixels';
procedure glCullFace (mode: GLenum); external opengl32 name 'glCullFace';
procedure glDeleteLists (list: GLuint; range: GLsizei); external opengl32 name 'glDeleteLists';
procedure glDepthFunc (func: GLenum); external opengl32 name 'glDepthFunc';
procedure glDepthMask (flag: GLboolean); external opengl32 name 'glDepthMask';
procedure glDepthRange (zNear, zFar: GLclampd); external opengl32 name 'glDepthRange';
procedure glDisable (cap: GLenum); external opengl32 name 'glDisable';
procedure glDrawBuffer (mode: GLenum); external opengl32 name 'glDrawBuffer';
procedure glDrawPixels (width, height: GLsizei; format, pixeltype: GLenum;
             pixels: Pointer); external opengl32 name 'glDrawPixels';
procedure glEdgeFlag (flag: GLboolean); external opengl32 name 'glEdgeFlag';
procedure glEdgeFlagv (flag: PGLboolean); external opengl32 name 'glEdgeFlagv';
procedure glEnable (cap: GLenum); external opengl32 name 'glEnable';
procedure glEnd; external opengl32 name 'glEnd';
procedure glEndList; external opengl32 name 'glEndList';
procedure glEvalCoord1d (u: GLdouble); external opengl32 name 'glEvalCoord1d';
procedure glEvalCoord1dv (u: PGLdouble); external opengl32 name 'glEvalCoord1dv';
procedure glEvalCoord1f (u: GLfloat); external opengl32 name 'glEvalCoord1f';
procedure glEvalCoord1fv (u: PGLfloat); external opengl32 name 'glEvalCoord1fv';
procedure glEvalCoord2d (u,v: GLdouble); external opengl32 name 'glEvalCoord2d';
procedure glEvalCoord2dv (u: PGLdouble); external opengl32 name 'glEvalCoord2dv';
procedure glEvalCoord2f (u,v: GLfloat); external opengl32 name 'glEvalCoord2f';
procedure glEvalCoord2fv (u: PGLfloat); external opengl32 name 'glEvalCoord2fv';
procedure glEvalCoord(u: GLdouble); external opengl32 name 'glEvalCoord1d';
procedure glEvalCoord(u: GLfloat); external opengl32 name 'glEvalCoord1f';
procedure glEvalCoord(u,v: GLdouble); external opengl32 name 'glEvalCoord2d';
procedure glEvalCoord(u,v: GLfloat); external opengl32 name 'glEvalCoord2f';
procedure glEvalCoord1(v: PGLdouble); external opengl32 name 'glEvalCoord1dv';
procedure glEvalCoord1(v: PGLfloat); external opengl32 name 'glEvalCoord1fv';
procedure glEvalCoord2(v: PGLdouble); external opengl32 name 'glEvalCoord2dv';
procedure glEvalCoord2(v: PGLfloat); external opengl32 name 'glEvalCoord2fv';
procedure glEvalMesh1 (mode: GLenum; i1, i2: GLint); external opengl32 name 'glEvalMesh1';
procedure glEvalMesh2 (mode: GLenum; i1, i2, j1, j2: GLint); external opengl32 name 'glEvalMesh2';
procedure glEvalMesh(mode: GLenum; i1, i2: GLint); external opengl32 name 'glEvalMesh1';
procedure glEvalMesh(mode: GLenum; i1, i2, j1, j2: GLint); external opengl32 name 'glEvalMesh2';
procedure glEvalPoint1 (i: GLint); external opengl32 name 'glEvalPoint1';
procedure glEvalPoint2 (i,j: GLint); external opengl32 name 'glEvalPoint2';
procedure glEvalPoint(i: GLint); external opengl32 name 'glEvalPoint1';
procedure glEvalPoint(i,j: GLint); external opengl32 name 'glEvalPoint2';
procedure glFeedbackBuffer (size: GLsizei; buftype: GLenum; buffer: PGLFloat); external opengl32 name 'glFeedbackBuffer';
procedure glFinish; external opengl32 name 'glFinish';
procedure glFlush; external opengl32 name 'glFlush';
procedure glFogf (pname: GLenum; param: GLfloat); external opengl32 name 'glFogf';
procedure glFogfv (pname: GLenum; prms: PGLfloat); external opengl32 name 'glFogfv';
procedure glFogi (pname: GLenum; param: GLint); external opengl32 name 'glFogi';
procedure glFogiv (pname: GLenum; prms: PGLint); external opengl32 name 'glFogiv';
procedure glFog(pname: GLenum; param: GLfloat); external opengl32 name 'glFogf';
procedure glFog(pname: GLenum; prms: PGLfloat); external opengl32 name 'glFogfv';
procedure glFog(pname: GLenum; param: GLint); external opengl32 name 'glFogi';
procedure glFog(pname: GLenum; prms: PGLint); external opengl32 name 'glFogiv';
procedure glFrontFace (mode: GLenum); external opengl32 name 'glFrontFace';
procedure glFrustum (left, right, bottom, top, zNear, zFar: GLdouble); external opengl32 name 'glFrustum';
function  glGenLists (range: GLsizei): GLuint; external opengl32 name 'glGenLists';
procedure glGetBooleanv (pname: GLenum; prms: PGLboolean); external opengl32 name 'glGetBooleanv';
procedure glGetClipPlane (plane: GLenum; equation: PGLdouble); external opengl32 name 'glGetClipPlane';
procedure glGetDoublev (pname: GLenum; prms: PGLdouble); external opengl32 name 'glGetDoublev';
function  glGetError: GLenum; external opengl32 name 'glGetError';
procedure glGetFloatv (pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetFloatv';
procedure glGetIntegerv (pname: GLenum; prms: PGLint); external opengl32 name 'glGetIntegerv';
procedure glGetLightfv (light: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetLightfv';
procedure glGetLightiv (light: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetLightiv';
procedure glGetLight(light: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetLightfv';
procedure glGetLight(light: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetLightiv';
procedure glGetMapdv (target: GLenum; query: GLenum; v: PGLdouble); external opengl32 name 'glGetMapdv';
procedure glGetMapfv (target: GLenum; query: GLenum; v: PGLfloat); external opengl32 name 'glGetMapfv';
procedure glGetMapiv (target: GLenum; query: GLenum; v: PGLint); external opengl32 name 'glGetMapiv';
procedure glGetMap(target: GLenum; query: GLenum; v: PGLdouble); external opengl32 name 'glGetMapdv';
procedure glGetMap(target: GLenum; query: GLenum; v: PGLfloat); external opengl32 name 'glGetMapfv';
procedure glGetMap(target: GLenum; query: GLenum; v: PGLint); external opengl32 name 'glGetMapiv';
procedure glGetMaterialfv (face: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetMaterialfv';
procedure glGetMaterialiv (face: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetMaterialiv';
procedure glGetMaterial(face: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetMaterialfv';
procedure glGetMaterial(face: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetMaterialiv';
procedure glGetPixelMapfv (map: GLenum; values: PGLfloat); external opengl32 name 'glGetPixelMapfv';
procedure glGetPixelMapuiv (map: GLenum; values: PGLuint); external opengl32 name 'glGetPixelMapuiv';
procedure glGetPixelMapusv (map: GLenum; values: PGLushort); external opengl32 name 'glGetPixelMapusv';
procedure glGetPixelMap(map: GLenum; values: PGLfloat); external opengl32 name 'glGetPixelMapfv';
procedure glGetPixelMap(map: GLenum; values: PGLuint); external opengl32 name 'glGetPixelMapuiv';
procedure glGetPixelMap(map: GLenum; values: PGLushort); external opengl32 name 'glGetPixelMapusv';
procedure glGetPolygonStipple (var mask: GLubyte); external opengl32 name 'glGetPolygonStipple';
function  glGetString (name: GLenum): PChar; external opengl32 name 'glGetString';
procedure glGetTexEnvfv (target: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexEnvfv';
procedure glGetTexEnviv (target: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexEnviv';
procedure glGetTexEnv(target: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexEnvfv';
procedure glGetTexEnv(target: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexEnviv';
procedure glGetTexGendv (coord: GLenum; pname: GLenum; prms: PGLdouble); external opengl32 name 'glGetTexGendv';
procedure glGetTexGenfv (coord: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexGenfv';
procedure glGetTexGeniv (coord: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexGeniv';
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLdouble); external opengl32 name 'glGetTexGendv';
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexGenfv';
procedure glGetTexGen(coord: GLenum; pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexGeniv';
procedure glGetTexImage (target: GLenum; level: GLint; format: GLenum; _type: GLenum; pixels: pointer); external opengl32 name 'glGetTexImage';
procedure glGetTexLevelParameterfv (target: GLenum; level: GLint; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexLevelParameterfv';
procedure glGetTexLevelParameteriv (target: GLenum; level: GLint; pname: GLenum; prms: PGLint); external opengl32;
procedure glGetTexLevelParameter(target: GLenum; level: GLint; pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexLevelParameterfv';
procedure glGetTexLevelParameter(target: GLenum; level: GLint; pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexLevelParameteriv';
procedure glGetTexParameterfv (target, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glGetTexParameteriv (target, pname: GLenum; prms: PGLint); external opengl32;
procedure glGetTexParameter(target, pname: GLenum; prms: PGLfloat); external opengl32 name 'glGetTexParameterfv';
procedure glGetTexParameter(target, pname: GLenum; prms: PGLint); external opengl32 name 'glGetTexParameteriv';
procedure glHint (target, mode: GLenum); external opengl32;
procedure glIndexMask (mask: GLuint); external opengl32;
procedure glIndexd (c: GLdouble); external opengl32;
procedure glIndexdv (c: PGLdouble); external opengl32;
procedure glIndexf (c: GLfloat); external opengl32;
procedure glIndexfv (c: PGLfloat); external opengl32;
procedure glIndexi (c: GLint); external opengl32;
procedure glIndexiv (c: PGLint); external opengl32;
procedure glIndexs (c: GLshort); external opengl32;
procedure glIndexsv (c: PGLshort); external opengl32;
procedure glIndex(c: GLdouble); external opengl32 name 'glIndexd';
procedure glIndex(c: PGLdouble); external opengl32 name 'glIndexdv';
procedure glIndex(c: GLfloat); external opengl32 name 'glIndexf';
procedure glIndex(c: PGLfloat); external opengl32 name 'glIndexfv';
procedure glIndex(c: GLint); external opengl32 name 'glIndexi';
procedure glIndex(c: PGLint); external opengl32 name 'glIndexiv';
procedure glIndex(c: GLshort); external opengl32 name 'glIndexs';
procedure glIndex(c: PGLshort); external opengl32 name 'glIndexsv';
procedure glInitNames; external opengl32;
function  glIsEnabled (cap: GLenum): GLBoolean; external opengl32;
function  glIsList (list: GLuint): GLBoolean; external opengl32;
procedure glLightModelf (pname: GLenum; param: GLfloat); external opengl32;
procedure glLightModelfv (pname: GLenum; prms: PGLfloat); external opengl32;
procedure glLightModeli (pname: GLenum; param: GLint); external opengl32;
procedure glLightModeliv (pname: GLenum; prms: PGLint); external opengl32;
procedure glLightModel(pname: GLenum; param: GLfloat); external opengl32 name 'glLightModelf';
procedure glLightModel(pname: GLenum; prms: PGLfloat); external opengl32 name 'glLightModelfv';
procedure glLightModel(pname: GLenum; param: GLint); external opengl32 name 'glLightModeli';
procedure glLightModel(pname: GLenum; prms: PGLint); external opengl32 name 'glLightModeliv';
procedure glLightf (light, pname: GLenum; param: GLfloat); external opengl32;
procedure glLightfv (light, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glLighti (light, pname: GLenum; param: GLint); external opengl32;
procedure glLightiv (light, pname: GLenum; prms: PGLint); external opengl32;
procedure glLight(light, pname: GLenum; param: GLfloat); external opengl32 name 'glLightf';
procedure glLight(light, pname: GLenum; prms: PGLfloat); external opengl32 name 'glLightfv';
procedure glLight(light, pname: GLenum; param: GLint); external opengl32 name 'glLighti';
procedure glLight(light, pname: GLenum; prms: PGLint); external opengl32 name 'glLightiv';
procedure glLineStipple (factor: GLint; pattern: GLushort); external opengl32;
procedure glLineWidth (width: GLfloat); external opengl32;
procedure glListBase (base: GLuint); external opengl32;
procedure glLoadIdentity; external opengl32;
procedure glLoadMatrixd (m: PGLdouble); external opengl32;
procedure glLoadMatrixf (m: PGLfloat); external opengl32;
procedure glLoadMatrix(m: PGLdouble); external opengl32 name 'glLoadMatrixd';
procedure glLoadMatrix(m: PGLfloat); external opengl32 name 'glLoadMatrixf';
procedure glLoadName (name: GLuint); external opengl32;
procedure glLogicOp (opcode: GLenum); external opengl32;
procedure glMap1d (target: GLenum; u1,u2: GLdouble; stride, order: GLint;
  Points: PGLdouble); external opengl32;
procedure glMap1f (target: GLenum; u1,u2: GLfloat; stride, order: GLint;
  Points: PGLfloat); external opengl32;
procedure glMap2d (target: GLenum;
  u1,u2: GLdouble; ustride, uorder: GLint;
  v1,v2: GLdouble; vstride, vorder: GLint; Points: PGLdouble); external opengl32;
procedure glMap2f (target: GLenum;
  u1,u2: GLfloat; ustride, uorder: GLint;
  v1,v2: GLfloat; vstride, vorder: GLint; Points: PGLfloat); external opengl32;
procedure glMap(target: GLenum; u1,u2: GLdouble; stride, order: GLint;
  Points: PGLdouble); external opengl32 name 'glMap1d';
procedure glMap(target: GLenum; u1,u2: GLfloat; stride, order: GLint;
  Points: PGLfloat); external opengl32 name 'glMap1f';
procedure glMap(target: GLenum;
  u1,u2: GLdouble; ustride, uorder: GLint;
  v1,v2: GLdouble; vstride, vorder: GLint; Points: PGLdouble); external opengl32 name 'glMap2d';
procedure glMap(target: GLenum;
  u1,u2: GLfloat; ustride, uorder: GLint;
  v1,v2: GLfloat; vstride, vorder: GLint; Points: PGLfloat); external opengl32 name 'glMap2f';
procedure glMapGrid1d (un: GLint; u1, u2: GLdouble); external opengl32;
procedure glMapGrid1f (un: GLint; u1, u2: GLfloat); external opengl32;
procedure glMapGrid2d (un: GLint; u1, u2: GLdouble;
                       vn: GLint; v1, v2: GLdouble); external opengl32;
procedure glMapGrid2f (un: GLint; u1, u2: GLfloat;
                       vn: GLint; v1, v2: GLfloat); external opengl32;
procedure glMapGrid(un: GLint; u1, u2: GLdouble); external opengl32 name 'glMapGrid1d';
procedure glMapGrid(un: GLint; u1, u2: GLfloat); external opengl32 name 'glMapGrid1f';
procedure glMapGrid(un: GLint; u1, u2: GLdouble;
                    vn: GLint; v1, v2: GLdouble); external opengl32 name 'glMapGrid2d';
procedure glMapGrid(un: GLint; u1, u2: GLfloat;
                    vn: GLint; v1, v2: GLfloat); external opengl32 name 'glMapGrid2f';
procedure glMaterialf (face, pname: GLenum; param: GLfloat); external opengl32;
procedure glMaterialfv (face, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glMateriali (face, pname: GLenum; param: GLint); external opengl32;
procedure glMaterialiv (face, pname: GLenum; prms: PGLint); external opengl32;
procedure glMaterial(face, pname: GLenum; param: GLfloat); external opengl32 name 'glMaterialf';
procedure glMaterial(face, pname: GLenum; prms: PGLfloat); external opengl32 name 'glMaterialfv';
procedure glMaterial(face, pname: GLenum; param: GLint); external opengl32 name 'glMateriali';
procedure glMaterial(face, pname: GLenum; prms: PGLint); external opengl32 name 'glMaterialiv';
procedure glMatrixMode (mode: GLenum); external opengl32;
procedure glMultMatrixd (m: PGLdouble); external opengl32;
procedure glMultMatrixf (m: PGLfloat); external opengl32;
procedure glMultMatrix(m: PGLdouble); external opengl32 name 'glMultMatrixd';
procedure glMultMatrix(m: PGLfloat); external opengl32 name 'glMultMatrixf';
procedure glNewList (ListIndex: GLuint; mode: GLenum); external opengl32;
procedure glNormal3b (nx, ny, nz: GLbyte); external opengl32;
procedure glNormal3bv (v: PGLbyte); external opengl32;
procedure glNormal3d (nx, ny, nz: GLdouble); external opengl32;
procedure glNormal3dv (v: PGLdouble); external opengl32;
procedure glNormal3f (nx, ny, nz: GLFloat); external opengl32;
procedure glNormal3fv (v: PGLfloat); external opengl32;
procedure glNormal3i (nx, ny, nz: GLint); external opengl32;
procedure glNormal3iv (v: PGLint); external opengl32;
procedure glNormal3s (nx, ny, nz: GLshort); external opengl32;
procedure glNormal3sv (v: PGLshort); external opengl32;
procedure glNormal(nx, ny, nz: GLbyte); external opengl32 name 'glNormal3b';
procedure glNormal3(v: PGLbyte); external opengl32 name 'glNormal3bv';
procedure glNormal(nx, ny, nz: GLdouble); external opengl32 name 'glNormal3d';
procedure glNormal3(v: PGLdouble); external opengl32 name 'glNormal3dv';
procedure glNormal(nx, ny, nz: GLFloat); external opengl32 name 'glNormal3f';
procedure glNormal3(v: PGLfloat); external opengl32 name 'glNormal3fv';
procedure glNormal(nx, ny, nz: GLint); external opengl32 name 'glNormal3i';
procedure glNormal3(v: PGLint); external opengl32 name 'glNormal3iv';
procedure glNormal(nx, ny, nz: GLshort); external opengl32 name 'glNormal3s';
procedure glNormal3(v: PGLshort); external opengl32 name 'glNormal3sv';
procedure glOrtho (left, right, bottom, top, zNear, zFar: GLdouble); external opengl32;
procedure glPassThrough (token: GLfloat); external opengl32;
procedure glPixelMapfv (map: GLenum; mapsize: GLint; values: PGLfloat); external opengl32;
procedure glPixelMapuiv (map: GLenum; mapsize: GLint; values: PGLuint); external opengl32;
procedure glPixelMapusv (map: GLenum; mapsize: GLint; values: PGLushort); external opengl32;
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLfloat); external opengl32 name 'glPixelMapfv';
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLuint); external opengl32 name 'glPixelMapuiv';
procedure glPixelMap(map: GLenum; mapsize: GLint; values: PGLushort); external opengl32 name 'glPixelMapusv';
procedure glPixelStoref (pname: GLenum; param: GLfloat); external opengl32;
procedure glPixelStorei (pname: GLenum; param: GLint); external opengl32;
procedure glPixelStore(pname: GLenum; param: GLfloat); external opengl32 name 'glPixelStoref';
procedure glPixelStore(pname: GLenum; param: GLint); external opengl32 name 'glPixelStorei';
procedure glPixelTransferf (pname: GLenum; param: GLfloat); external opengl32;
procedure glPixelTransferi (pname: GLenum; param: GLint); external opengl32;
procedure glPixelTransfer(pname: GLenum; param: GLfloat); external opengl32 name 'glPixelTransferf';
procedure glPixelTransfer(pname: GLenum; param: GLint); external opengl32 name 'glPixelTransferi';
procedure glPixelZoom (xfactor, yfactor: GLfloat); external opengl32;
procedure glPointSize (size: GLfloat); external opengl32;
procedure glPolygonMode (face, mode: GLenum); external opengl32;
procedure glPolygonStipple (mask: PGLubyte); external opengl32;
procedure glPopAttrib; external opengl32;
procedure glPopMatrix; external opengl32;
procedure glPopName; external opengl32;
procedure glPushAttrib(mask: GLbitfield); external opengl32;
procedure glPushMatrix; external opengl32;
procedure glPushName(name: GLuint); external opengl32;
procedure glRasterPos2d (x,y: GLdouble); external opengl32;
procedure glRasterPos2dv (v: PGLdouble); external opengl32;
procedure glRasterPos2f (x,y: GLfloat); external opengl32;
procedure glRasterPos2fv (v: PGLfloat); external opengl32;
procedure glRasterPos2i (x,y: GLint); external opengl32;
procedure glRasterPos2iv (v: PGLint); external opengl32;
procedure glRasterPos2s (x,y: GLshort); external opengl32;
procedure glRasterPos2sv (v: PGLshort); external opengl32;
procedure glRasterPos3d (x,y,z: GLdouble); external opengl32;
procedure glRasterPos3dv (v: PGLdouble); external opengl32;
procedure glRasterPos3f (x,y,z: GLfloat); external opengl32;
procedure glRasterPos3fv (v: PGLfloat); external opengl32;
procedure glRasterPos3i (x,y,z: GLint); external opengl32;
procedure glRasterPos3iv (v: PGLint); external opengl32;
procedure glRasterPos3s (x,y,z: GLshort); external opengl32;
procedure glRasterPos3sv (v: PGLshort); external opengl32;
procedure glRasterPos4d (x,y,z,w: GLdouble); external opengl32;
procedure glRasterPos4dv (v: PGLdouble); external opengl32;
procedure glRasterPos4f (x,y,z,w: GLfloat); external opengl32;
procedure glRasterPos4fv (v: PGLfloat); external opengl32;
procedure glRasterPos4i (x,y,z,w: GLint); external opengl32;
procedure glRasterPos4iv (v: PGLint); external opengl32;
procedure glRasterPos4s (x,y,z,w: GLshort); external opengl32;
procedure glRasterPos4sv (v: PGLshort); external opengl32;
procedure glRasterPos(x,y: GLdouble); external opengl32 name 'glRasterPos2d';
procedure glRasterPos2(v: PGLdouble); external opengl32 name 'glRasterPos2dv';
procedure glRasterPos(x,y: GLfloat); external opengl32 name 'glRasterPos2f';
procedure glRasterPos2(v: PGLfloat); external opengl32 name 'glRasterPos2fv';
procedure glRasterPos(x,y: GLint); external opengl32 name 'glRasterPos2i';
procedure glRasterPos2(v: PGLint); external opengl32 name 'glRasterPos2iv';
procedure glRasterPos(x,y: GLshort); external opengl32 name 'glRasterPos2s';
procedure glRasterPos2(v: PGLshort); external opengl32 name 'glRasterPos2sv';
procedure glRasterPos(x,y,z: GLdouble); external opengl32 name 'glRasterPos3d';
procedure glRasterPos3(v: PGLdouble); external opengl32 name 'glRasterPos3dv';
procedure glRasterPos(x,y,z: GLfloat); external opengl32 name 'glRasterPos3f';
procedure glRasterPos3(v: PGLfloat); external opengl32 name 'glRasterPos3fv';
procedure glRasterPos(x,y,z: GLint); external opengl32 name 'glRasterPos3i';
procedure glRasterPos3(v: PGLint); external opengl32 name 'glRasterPos3iv';
procedure glRasterPos(x,y,z: GLshort); external opengl32 name 'glRasterPos3s';
procedure glRasterPos3(v: PGLshort); external opengl32 name 'glRasterPos3sv';
procedure glRasterPos(x,y,z,w: GLdouble); external opengl32 name 'glRasterPos4d';
procedure glRasterPos4(v: PGLdouble); external opengl32 name 'glRasterPos4dv';
procedure glRasterPos(x,y,z,w: GLfloat); external opengl32 name 'glRasterPos4f';
procedure glRasterPos4(v: PGLfloat); external opengl32 name 'glRasterPos4fv';
procedure glRasterPos(x,y,z,w: GLint); external opengl32 name 'glRasterPos4i';
procedure glRasterPos4(v: PGLint); external opengl32 name 'glRasterPos4iv';
procedure glRasterPos(x,y,z,w: GLshort); external opengl32 name 'glRasterPos4s';
procedure glRasterPos4(v: PGLshort); external opengl32 name 'glRasterPos4sv';
procedure glReadBuffer (mode: GLenum); external opengl32;
procedure glReadPixels (x,y: GLint; width, height: GLsizei;
  format, _type: GLenum; pixels: Pointer); external opengl32;
procedure glRectd (x1, y1, x2, y2: GLdouble); external opengl32;
procedure glRectdv (v1, v2: PGLdouble); external opengl32;
procedure glRectf (x1, y1, x2, y2: GLfloat); external opengl32;
procedure glRectfv (v1, v2: PGLfloat); external opengl32;
procedure glRecti (x1, y1, x2, y2: GLint); external opengl32;
procedure glRectiv (v1, v2: PGLint); external opengl32;
procedure glRects (x1, y1, x2, y2: GLshort); external opengl32;
procedure glRectsv (v1, v2: PGLshort); external opengl32;
procedure glRect(x1, y1, x2, y2: GLdouble); external opengl32 name 'glRectd';
procedure glRect(v1, v2: PGLdouble); external opengl32 name 'glRectdv';
procedure glRect(x1, y1, x2, y2: GLfloat); external opengl32 name 'glRectf';
procedure glRect(v1, v2: PGLfloat); external opengl32 name 'glRectfv';
procedure glRect(x1, y1, x2, y2: GLint); external opengl32 name 'glRecti';
procedure glRect(v1, v2: PGLint); external opengl32 name 'glRectiv';
procedure glRect(x1, y1, x2, y2: GLshort); external opengl32 name 'glRects';
procedure glRect(v1, v2: PGLshort); external opengl32 name 'glRectsv';
function  glRenderMode (mode: GLenum): GLint; external opengl32;
procedure glRotated (angle, x,y,z: GLdouble); external opengl32;
procedure glRotatef (angle, x,y,z: GLfloat); external opengl32;
procedure glRotate(angle, x,y,z: GLdouble); external opengl32 name 'glRotated';
procedure glRotate(angle, x,y,z: GLfloat); external opengl32 name 'glRotatef';
procedure glScaled (x,y,z: GLdouble); external opengl32;
procedure glScalef (x,y,z: GLfloat); external opengl32;
procedure glScale(x,y,z: GLdouble); external opengl32 name 'glScaled';
procedure glScale(x,y,z: GLfloat); external opengl32 name 'glScalef';
procedure glScissor (x,y: GLint; width, height: GLsizei); external opengl32;
procedure glSelectBuffer (size: GLsizei; buffer: PGLuint); external opengl32;
procedure glShadeModel (mode: GLenum); external opengl32;
procedure glStencilFunc (func: GLenum; ref: GLint; mask: GLuint); external opengl32;
procedure glStencilMask (mask: GLuint); external opengl32;
procedure glStencilOp (fail, zfail, zpass: GLenum); external opengl32;
procedure glTexCoord1d (s: GLdouble); external opengl32;
procedure glTexCoord1dv (v: PGLdouble); external opengl32;
procedure glTexCoord1f (s: GLfloat); external opengl32;
procedure glTexCoord1fv (v: PGLfloat); external opengl32;
procedure glTexCoord1i (s: GLint); external opengl32;
procedure glTexCoord1iv (v: PGLint); external opengl32;
procedure glTexCoord1s (s: GLshort); external opengl32;
procedure glTexCoord1sv (v: PGLshort); external opengl32;
procedure glTexCoord2d (s,t: GLdouble); external opengl32;
procedure glTexCoord2dv (v: PGLdouble); external opengl32;
procedure glTexCoord2f (s,t: GLfloat); external opengl32;
procedure glTexCoord2fv (v: PGLfloat); external opengl32;
procedure glTexCoord2i (s,t: GLint); external opengl32;
procedure glTexCoord2iv (v: PGLint); external opengl32;
procedure glTexCoord2s (s,t: GLshort); external opengl32;
procedure glTexCoord2sv (v: PGLshort); external opengl32;
procedure glTexCoord3d (s,t,r: GLdouble); external opengl32;
procedure glTexCoord3dv (v: PGLdouble); external opengl32;
procedure glTexCoord3f (s,t,r: GLfloat); external opengl32;
procedure glTexCoord3fv (v: PGLfloat); external opengl32;
procedure glTexCoord3i (s,t,r: GLint); external opengl32;
procedure glTexCoord3iv (v: PGLint); external opengl32;
procedure glTexCoord3s (s,t,r: GLshort); external opengl32;
procedure glTexCoord3sv (v: PGLshort); external opengl32;
procedure glTexCoord4d (s,t,r,q: GLdouble); external opengl32;
procedure glTexCoord4dv (v: PGLdouble); external opengl32;
procedure glTexCoord4f (s,t,r,q: GLfloat); external opengl32;
procedure glTexCoord4fv (v: PGLfloat); external opengl32;
procedure glTexCoord4i (s,t,r,q: GLint); external opengl32;
procedure glTexCoord4iv (v: PGLint); external opengl32;
procedure glTexCoord4s (s,t,r,q: GLshort); external opengl32;
procedure glTexCoord4sv (v: PGLshort); external opengl32;
procedure glTexCoord(s: GLdouble); external opengl32 name 'glTexCoord1d';
procedure glTexCoord1(v: PGLdouble); external opengl32 name 'glTexCoord1dv';
procedure glTexCoord(s: GLfloat); external opengl32 name 'glTexCoord1f';
procedure glTexCoord1(v: PGLfloat); external opengl32 name 'glTexCoord1fv';
procedure glTexCoord(s: GLint); external opengl32 name 'glTexCoord1i';
procedure glTexCoord1(v: PGLint); external opengl32 name 'glTexCoord1iv';
procedure glTexCoord(s: GLshort); external opengl32 name 'glTexCoord1s';
procedure glTexCoord1(v: PGLshort); external opengl32 name 'glTexCoord1sv';
procedure glTexCoord(s,t: GLdouble); external opengl32 name 'glTexCoord2d';
procedure glTexCoord2(v: PGLdouble); external opengl32 name 'glTexCoord2dv';
procedure glTexCoord(s,t: GLfloat); external opengl32 name 'glTexCoord2f';
procedure glTexCoord2(v: PGLfloat); external opengl32 name 'glTexCoord2fv';
procedure glTexCoord(s,t: GLint); external opengl32 name 'glTexCoord2i';
procedure glTexCoord2(v: PGLint); external opengl32 name 'glTexCoord2iv';
procedure glTexCoord(s,t: GLshort); external opengl32 name 'glTexCoord2s';
procedure glTexCoord2(v: PGLshort); external opengl32 name 'glTexCoord2sv';
procedure glTexCoord(s,t,r: GLdouble); external opengl32 name 'glTexCoord3d';
procedure glTexCoord3(v: PGLdouble); external opengl32 name 'glTexCoord3dv';
procedure glTexCoord(s,t,r: GLfloat); external opengl32 name 'glTexCoord3f';
procedure glTexCoord3(v: PGLfloat); external opengl32 name 'glTexCoord3fv';
procedure glTexCoord(s,t,r: GLint); external opengl32 name 'glTexCoord3i';
procedure glTexCoord3(v: PGLint); external opengl32 name 'glTexCoord3iv';
procedure glTexCoord(s,t,r: GLshort); external opengl32 name 'glTexCoord3s';
procedure glTexCoord3(v: PGLshort); external opengl32 name 'glTexCoord3sv';
procedure glTexCoord(s,t,r,q: GLdouble); external opengl32 name 'glTexCoord4d';
procedure glTexCoord4(v: PGLdouble); external opengl32 name 'glTexCoord4dv';
procedure glTexCoord(s,t,r,q: GLfloat); external opengl32 name 'glTexCoord4f';
procedure glTexCoord4(v: PGLfloat); external opengl32 name 'glTexCoord4fv';
procedure glTexCoord(s,t,r,q: GLint); external opengl32 name 'glTexCoord4i';
procedure glTexCoord4(v: PGLint); external opengl32 name 'glTexCoord4iv';
procedure glTexCoord(s,t,r,q: GLshort); external opengl32 name 'glTexCoord4s';
procedure glTexCoord4(v: PGLshort); external opengl32 name 'glTexCoord4sv';
procedure glTexEnvf (target, pname: GLenum; param: GLfloat); external opengl32;
procedure glTexEnvfv (target, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glTexEnvi (target, pname: GLenum; param: GLint); external opengl32;
procedure glTexEnviv (target, pname: GLenum; prms: PGLint); external opengl32;
procedure glTexEnv(target, pname: GLenum; param: GLfloat); external opengl32 name 'glTexEnvf';
procedure glTexEnv(target, pname: GLenum; prms: PGLfloat); external opengl32 name 'glTexEnvfv';
procedure glTexEnv(target, pname: GLenum; param: GLint); external opengl32 name 'glTexEnvi';
procedure glTexEnv(target, pname: GLenum; prms: PGLint); external opengl32 name 'glTexEnviv';
procedure glTexGend (coord, pname: GLenum; param: GLdouble); external opengl32;
procedure glTexGendv (coord, pname: GLenum; prms: PGLdouble); external opengl32;
procedure glTexGenf (coord, pname: GLenum; param: GLfloat); external opengl32;
procedure glTexGenfv (coord, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glTexGeni (coord, pname: GLenum; param: GLint); external opengl32;
procedure glTexGeniv (coord, pname: GLenum; prms: PGLint); external opengl32;
procedure glTexGen(coord, pname: GLenum; param: GLdouble); external opengl32 name 'glTexGend';
procedure glTexGen(coord, pname: GLenum; prms: PGLdouble); external opengl32 name 'glTexGendv';
procedure glTexGen(coord, pname: GLenum; param: GLfloat); external opengl32 name 'glTexGenf';
procedure glTexGen(coord, pname: GLenum; prms: PGLfloat); external opengl32 name 'glTexGenfv';
procedure glTexGen(coord, pname: GLenum; param: GLint); external opengl32 name 'glTexGeni';
procedure glTexGen(coord, pname: GLenum; prms: PGLint); external opengl32 name 'glTexGeniv';
procedure glTexImage1D (target: GLenum; level, components: GLint;
  width: GLsizei; border: GLint; format, _type: GLenum; pixels: Pointer); external opengl32;
procedure glTexImage2D (target: GLenum; level, components: GLint;
  width, height: GLsizei; border: GLint; format, _type: GLenum; pixels: Pointer); external opengl32;
procedure glTexParameterf (target, pname: GLenum; param: GLfloat); external opengl32;
procedure glTexParameterfv (target, pname: GLenum; prms: PGLfloat); external opengl32;
procedure glTexParameteri (target, pname: GLenum; param: GLint); external opengl32;
procedure glTexParameteriv (target, pname: GLenum; prms: PGLint); external opengl32;
procedure glTexParameter(target, pname: GLenum; param: GLfloat); external opengl32 name 'glTexParameterf';
procedure glTexParameter(target, pname: GLenum; prms: PGLfloat); external opengl32 name 'glTexParameterfv';
procedure glTexParameter(target, pname: GLenum; param: GLint); external opengl32 name 'glTexParameteri';
procedure glTexParameter(target, pname: GLenum; prms: PGLint); external opengl32 name 'glTexParameteriv';
procedure glTranslated (x,y,z: GLdouble); external opengl32;
procedure glTranslatef (x,y,z: GLfloat); external opengl32;
procedure glTranslate(x,y,z: GLdouble); external opengl32 name 'glTranslated';
procedure glTranslate(x,y,z: GLfloat); external opengl32 name 'glTranslatef';
procedure glVertex2d (x,y: GLdouble); external opengl32;
procedure glVertex2dv (v: PGLdouble); external opengl32;
procedure glVertex2f (x,y: GLfloat); external opengl32;
procedure glVertex2fv (v: PGLfloat); external opengl32;
procedure glVertex2i (x,y: GLint); external opengl32;
procedure glVertex2iv (v: PGLint); external opengl32;
procedure glVertex2s (x,y: GLshort); external opengl32;
procedure glVertex2sv (v: PGLshort); external opengl32;
procedure glVertex3d (x,y,z: GLdouble); external opengl32;
procedure glVertex3dv (v: PGLdouble); external opengl32;
procedure glVertex3f (x,y,z: GLfloat); external opengl32;
procedure glVertex3fv (v: PGLfloat); external opengl32;
procedure glVertex3i (x,y,z: GLint); external opengl32;
procedure glVertex3iv (v: PGLint); external opengl32;
procedure glVertex3s (x,y,z: GLshort); external opengl32;
procedure glVertex3sv (v: PGLshort); external opengl32;
procedure glVertex4d (x,y,z,w: GLdouble); external opengl32;
procedure glVertex4dv (v: PGLdouble); external opengl32;
procedure glVertex4f (x,y,z,w: GLfloat); external opengl32;
procedure glVertex4fv (v: PGLfloat); external opengl32;
procedure glVertex4i (x,y,z,w: GLint); external opengl32;
procedure glVertex4iv (v: PGLint); external opengl32;
procedure glVertex4s (x,y,z,w: GLshort); external opengl32;
procedure glVertex4sv (v: PGLshort); external opengl32;
procedure glVertex(x,y: GLdouble); external opengl32 name 'glVertex2d';
procedure glVertex2(v: PGLdouble); external opengl32 name 'glVertex2dv';
procedure glVertex(x,y: GLfloat); external opengl32 name 'glVertex2f';
procedure glVertex2(v: PGLfloat); external opengl32 name 'glVertex2fv';
procedure glVertex(x,y: GLint); external opengl32 name 'glVertex2i';
procedure glVertex2(v: PGLint); external opengl32 name 'glVertex2iv';
procedure glVertex(x,y: GLshort); external opengl32 name 'glVertex2s';
procedure glVertex2(v: PGLshort); external opengl32 name 'glVertex2sv';
procedure glVertex(x,y,z: GLdouble); external opengl32 name 'glVertex3d';
procedure glVertex3(v: PGLdouble); external opengl32 name 'glVertex3dv';
procedure glVertex(x,y,z: GLfloat); external opengl32 name 'glVertex3f';
procedure glVertex3(v: PGLfloat); external opengl32 name 'glVertex3fv';
procedure glVertex(x,y,z: GLint); external opengl32 name 'glVertex3i';
procedure glVertex3(v: PGLint); external opengl32 name 'glVertex3iv';
procedure glVertex(x,y,z: GLshort); external opengl32 name 'glVertex3s';
procedure glVertex3(v: PGLshort); external opengl32 name 'glVertex3sv';
procedure glVertex(x,y,z,w: GLdouble); external opengl32 name 'glVertex4d';
procedure glVertex4(v: PGLdouble); external opengl32 name 'glVertex4dv';
procedure glVertex(x,y,z,w: GLfloat); external opengl32 name 'glVertex4f';
procedure glVertex4(v: PGLfloat); external opengl32 name 'glVertex4fv';
procedure glVertex(x,y,z,w: GLint); external opengl32 name 'glVertex4i';
procedure glVertex4(v: PGLint); external opengl32 name 'glVertex4iv';
procedure glVertex(x,y,z,w: GLshort); external opengl32 name 'glVertex4s';
procedure glVertex4(v: PGLshort); external opengl32 name 'glVertex4sv';
procedure glViewport (x,y: GLint; width, height: GLsizei); external opengl32;

function wglGetProcAddress(ProcName: PChar): Pointer;    external opengl32;


function gluErrorString (errCode: GLenum): PChar;                     external glu32;
function gluErrorUnicodeStringEXT (errCode: GLenum): PWChar;           external glu32;
function gluGetString (name: GLenum): PChar;                       external glu32;
procedure gluLookAt(eyex, eyey, eyez,
                    centerx, centery, centerz,
                    upx, upy, upz: GLdouble);                         external glu32;
procedure gluOrtho2D(left, right, bottom, top: GLdouble);                        external glu32;
procedure gluPerspective(fovy, aspect, zNear, zFar: GLdouble);                    external glu32;
procedure gluPickMatrix (x, y, width, height: GLdouble; viewport: PGLint);                     external glu32;
function  gluProject (objx, objy, obyz: GLdouble;
                      modelMatrix: PGLdouble;
                      projMatrix: PGLdouble;
                      viewport: PGLint;
                      var winx, winy, winz: GLDouble): Integer;                        external glu32;
function  gluUnProject(winx, winy, winz: GLdouble;
                      modelMatrix: PGLdouble;
                      projMatrix: PGLdouble;
                      viewport: PGLint;
                      var objx, objy, objz: GLdouble): Integer;                      external glu32;
function  gluScaleImage(format: GLenum;
   widthin, heightin: GLint; typein: GLenum; datain: Pointer;
   widthout, heightout: GLint; typeout: GLenum; dataout: Pointer): Integer;                     external glu32;
function  gluBuild1DMipmaps (target: GLenum; components, width: GLint;
                             format, atype: GLenum; data: Pointer): Integer;                 external glu32;
function  gluBuild2DMipmaps (target: GLenum; components, width, height: GLint;
                             format, atype: GLenum; data: Pointer): Integer;                 external glu32;
function  gluNewQuadric: GLUquadricObj;                     external glu32;
procedure gluDeleteQuadric (state: GLUquadricObj);                  external glu32;
procedure gluQuadricNormals (quadObject: GLUquadricObj; normals: GLenum);                 external glu32;
procedure gluQuadricTexture (quadObject: GLUquadricObj; textureCoords: GLboolean );                 external glu32;
procedure gluQuadricOrientation (quadObject: GLUquadricObj; orientation: GLenum);             external glu32;
procedure gluQuadricDrawStyle (quadObject: GLUquadricObj; drawStyle: GLenum);               external glu32;
procedure gluCylinder (quadObject: GLUquadricObj;
  baseRadius, topRadius, height: GLdouble; slices, stacks: GLint);                       external glu32;
procedure gluDisk (quadObject: GLUquadricObj;
  innerRadius, outerRadius: GLdouble; slices, loops: GLint);                           external glu32;
procedure gluPartialDisk (quadObject: GLUquadricObj;
  innerRadius, outerRadius: GLdouble; slices, loops: GLint;
  startAngle, sweepAngle: GLdouble);                    external glu32;
procedure gluSphere (quadObject: GLUquadricObj; radius: GLdouble; slices, loops: GLint);                         external glu32;
procedure gluQuadricCallback (quadObject: GLUquadricObj; which: GLenum;
  callback: Pointer);                external glu32;

function gluNewTess: GLUtesselator                         ;external glu32;
procedure gluDeleteTess( tess: GLUtesselator )                     ;external glu32;
procedure gluTessBeginPolygon( tess: GLUtesselator; gon_data: Pointer )               ;external glu32;
procedure gluTessBeginContour( tess: GLUtesselator )               ;external glu32;
procedure gluTessVertex( tess: GLUtesselator; coords: PGLdouble; data: Pointer )                     ;external glu32;
procedure gluTessEndContour( tess: GLUtesselator )                 ;external glu32;
procedure gluTessEndPolygon( tess: GLUtesselator )                 ;external glu32;
procedure gluTessProperty( tess: GLUtesselator; which: GLenum; value: GLdouble)                   ;external glu32;
procedure gluTessNormal( tess: GLUtesselator; x,y,z: GLdouble)                     ;external glu32;
procedure gluTessCallback( tess: GLUtesselator; which: GLenum; callback: pointer)                   ;external glu32;

function gluNewNurbsRenderer: GLUnurbsObj                ;external glu32;
procedure gluDeleteNurbsRenderer (nobj: GLUnurbsObj)            ;external glu32;
procedure gluBeginSurface (nobj: GLUnurbsObj)                   ;external glu32;
procedure gluBeginCurve (nobj: GLUnurbsObj)                     ;external glu32;
procedure gluEndCurve (nobj: GLUnurbsObj)                       ;external glu32;
procedure gluEndSurface (nobj: GLUnurbsObj)                     ;external glu32;
procedure gluBeginTrim (nobj: GLUnurbsObj)                      ;external glu32;
procedure gluEndTrim (nobj: GLUnurbsObj)                        ;external glu32;
procedure gluPwlCurve (nobj: GLUnurbsObj; count: GLint; points: PGLfloat;
  stride: GLint; _type: GLenum)                       ;external glu32;
procedure gluNurbsCurve (nobj: GLUnurbsObj; nknots: GLint; knot: PGLfloat;
  stride: GLint; ctlpts: PGLfloat; order: GLint; _type: GLenum)                     ;external glu32;
procedure gluNurbsSurface (nobj: GLUnurbsObj;
  sknot_count: GLint; sknot: PGLfloat;
  tknot_count: GLint; tknot: PGLfloat;
  s_stride, t_stride: GLint;
  ctlpts: PGLfloat; sorder, torder: GLint; _type: GLenum)                   ;external glu32;
procedure gluLoadSamplingMatrices (nobj: GLUnurbsObj;
  modelMatrix: PGLdouble; projMatrix: PGLdouble; viewport: PGLint)           ;external glu32;
procedure gluNurbsProperty (nobj: GLUnurbsObj; prop: GLenum; value: GLfloat)                  ;external glu32;
procedure gluGetNurbsProperty (nobj: GLUnurbsObj; prop: GLenum; var value: GLfloat)               ;external glu32;
procedure gluNurbsCallback (nobj: GLUnurbsObj; which: GLenum; callback: pointer)                  ;external glu32;

function wglMakeCurrent(_hdc : HDC; _hglrc : HGLRC) : BOOL; external opengl32;
function wglCreateContext(_hdc : HDC) : HGLRC; external opengl32;
function GetDC(_hwnd : HWND) : HDC; external 'user32.dll';
function _SetPixelFormat(_hdc: HDC; iPixelFormat : integer; ppfd : ^PIXELFORMATDESCRIPTOR): BOOL; external 'gdi32.dll' name 'SetPixelFormat';
function LoadLibrary(s : string) : BOOL; external 'kernel32.dll';
function SetPixelFormat(_hdc: HDC; iPixelFormat : integer; ppfd : ^PIXELFORMATDESCRIPTOR): BOOL;
begin
  LoadLibrary('opengl32.dll');
  Result := _SetPixelFormat(_hdc,iPixelFormat,ppfd);
end;
function ChoosePixelFormat(_hdc: HDC; ppfd : ^PIXELFORMATDESCRIPTOR): integer; external 'gdi32.dll';

function wglDeleteContext(_hgrlc : HGLRC) :BOOL; external opengl32;

function SwapBuffers(_hdc : HDC) : BOOL; external 'gdi32.dll';

type HandleInfo = record
_hdc : HDC;
_hgrlc : HGLRC;
end;

var contexts : System.Collections.Generic.Dictionary<IntPtr,HandleInfo> := new System.Collections.Generic.Dictionary<IntPtr,HandleInfo>;

procedure OpenGLInit(Handle : IntPtr);
begin
  var pfd : PIXELFORMATDESCRIPTOR;
  var iformat : integer;
  var _hdc := GetDC(Handle.ToInt32());
  pfd.nSize := sizeof( PIXELFORMATDESCRIPTOR );
  pfd.nVersion := 1;
  pfd.dwFlags := PFD_DRAW_TO_WINDOW or PFD_SUPPORT_OPENGL or
                  PFD_DOUBLEBUFFER;
  pfd.iPixelType := PFD_TYPE_RGBA;
  pfd.cColorBits := 24;
  pfd.cDepthBits := 16;
  pfd.iLayerType := PFD_MAIN_PLANE;
  iFormat := ChoosePixelFormat( _hDC, @pfd );
  SetPixelFormat( _hdc, iFormat, @pfd );
  var _hgrlc := wglCreateContext(_hdc);
  wglMakeCurrent(_hdc,_hgrlc);
  var hi : HandleInfo;
  hi._hdc := _hdc;
  hi._hgrlc := _hgrlc;
  contexts.Add(Handle,hi);
end;

procedure OpenGLUninit(Handle : IntPtr);
begin
  wglDeleteContext(contexts[Handle]._hgrlc);
end;

begin
  //Set8087CW($133F);
end.
