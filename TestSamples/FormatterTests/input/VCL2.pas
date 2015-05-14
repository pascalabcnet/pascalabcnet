unit VCL2;

#reference 'System.Windows.Forms.dll'
#reference 'System.Drawing.dll'

interface

const 
	btNone = 0;
	btLeft = 1;
	btRight = 2;
	
type 
 	ColorType = integer;
	
	CursorType = (crDefault,crNone,crArrow,crCross,crIBeam,crSize,crSizeNESW,crSizeNS,crSizeNWSE,crSizeWE,crUpArrow,crHourGlass,crDrag,
								crNoDrop,crHSplit,crVSplit,crMultiDrag,crSQLWait,crNo,crAppStart,crHelp,crHandPoint,crSizeAll);
	
	AlignType=(alNone,alTop,alBottom,alLeft,alRight,alClient);
	
	BevelType=(bvNone, bvLowered, bvRaised);
	
	FontStyle=(fsBold, fsItalic, fsUnderline, fsStrikeOut);
	
	ShiftState=set of (ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble);
	
	PositionType=(poScreenCenter, poDefault);
	
	WindowStateType=(wsNormal, wsMinimized, wsMaximized);
	
	BorderIconsType=set of (biSystemMenu, biMinimize, biMaximize, biHelp);
	
	FormBorderStyleType=(bsNone,bsSingle,bsSizeable,bsDialog);
	
	ModalResultType=(mrNone, mrOk, mrYes, mrNo, mrCancel);
	
	ScrollStyleType=(ssNone,ssHorizontal,ssVertical,ssBoth);
	
	Component = class
	protected
		function getTag : integer;virtual;
		procedure setTag(i : integer);virtual;
	public
  	constructor Create;
  	property Tag: integer read getTag write setTag;
  end;
  
  FontType = class
  protected
  	function getColor : integer;
  	procedure setColor(i : integer);
  	function getSize : integer;
  	procedure setSize(i : integer);
  	function getName : string;
  	procedure setName(i : string);
  	function getStyle : set of FontStyle;
  	procedure setStyle(i : set of FontStyle);
  	
  public
  	constructor Create;
  	property Color: integer read getColor write setColor;
  	property Size: integer read getSize write setSize;
  	property Name: string read getName write setName;
  	property Style: set of FontStyle read getStyle write setStyle;
  end;
  
  ContainerControl = class;
  PopupMenu = class;
  
  Control = class(Component)
  public
		OnClick: procedure;
		OnDblClick: procedure;
		OnMouseDown: procedure(x,y,button: integer);
		OnMouseUp: procedure(x,y,button: integer);
		OnMouseMove: procedure(x,y,button: integer);
		OnKeyDown: procedure(key: integer);
		OnKeyUp: procedure(key: integer);
		OnKeyPress: procedure(key: char);
		OnEnter: procedure;
		OnExit: procedure;
		OnResize: procedure;
		
		OnClickExt: procedure(Sender: Component);
		OnDblClickExt: procedure(Sender: Component);
		OnMouseDownExt: procedure(Sender: Component; Shift: ShiftState; x,y: integer);
		OnMouseUpExt: procedure(Sender: Component; Shift: ShiftState; x,y: integer);
		OnMouseMoveExt: procedure(Sender: Component; Shift: ShiftState; x,y: integer);
		OnKeyDownExt: procedure(Sender: Component;  key: integer; Shift: ShiftState);
		OnKeyUpExt: procedure(Sender: Component;  key: integer; Shift: ShiftState);
		OnKeyPressExt: procedure(Sender: Component;  key: char);
		OnEnterExt: procedure(Sender: Component);
		OnExitExt: procedure(Sender: Component);
		OnResizeExt: procedure(Sender: Component);
  protected
    ctrl : System.Windows.Forms.Control;
  	function getLeft : integer; virtual;
  	procedure setLeft(i : integer); virtual;
  	function getTop : integer; virtual;
  	procedure setTop(i : integer); virtual;
  	function getWidth : integer; virtual;
  	procedure setWidth(i : integer); virtual;
  	function getHeight : integer; virtual;
  	procedure setHeight(i : integer); virtual;
  	function getClientWidth : integer; virtual;
  	procedure setClientWidth(i : integer); virtual;
  	function getClientHeight : integer; virtual;
  	procedure setClientHeight(i : integer); virtual;
  	function getParent : ContainerControl; virtual;
  	procedure setParent(i : ContainerControl); virtual;
  	function getColor : integer; virtual;
  	procedure setColor(i : ColorType); virtual;
  	function getHint : string; virtual;
  	procedure setHint(i : string); virtual;
  	function getEnabled : boolean; virtual;
  	procedure setEnabled(i : boolean); virtual;
  	function getVisible : boolean; virtual;
  	procedure setVisible(i : boolean); virtual;
  	function getCaption : string; virtual;
  	procedure setCaption(i : string); virtual;
  	function getCursorType : CursorType; virtual;
  	procedure setCursorType(i : CursorType); virtual;
  	function getAlign : AlignType; virtual;
  	procedure setAlign(i : AlignType); virtual;
  	function getPopupMenu : PopupMenu; virtual;
  	procedure setPopupMenu(i : PopupMenu); virtual;
  	function getFont : FontType; virtual;
  	procedure setFont(i : FontType); virtual;
  	function getTabOrder : integer; virtual;
  	procedure setTabOrder(i : integer); virtual;
  	function getTabStop : boolean; virtual;
  	procedure setTabStop(i : boolean); virtual;
  	function getBevelInner : BevelType; virtual;
  	procedure setBevelInner(i : BevelType); virtual;
		function getBevelOuter : BevelType; virtual;
  	procedure setBevelOuter(i : BevelType); virtual;
  	function getTag : integer;override;
		procedure setTag(i : integer);override;
		
  protected
  	procedure InitCtrl; virtual;
  	procedure InternalClicked(sender : object; e : System.EventArgs); virtual;
  	procedure InternalDblClicked(sender : object; e : System.EventArgs); virtual;
  	procedure InternalEnter(sender : object; e : System.EventArgs); virtual;
  	procedure InternalExit(sender : object; e : System.EventArgs); virtual;
  	procedure InternalResize(sender : object; e : System.EventArgs); virtual;
  	procedure InternalMouseDown(sender : object; e : System.Windows.Forms.MouseEventArgs); virtual;
  	procedure InternalMouseUp(sender : object; e : System.Windows.Forms.MouseEventArgs); virtual;
  	procedure InternalMouseMove(sender : object; e : System.Windows.Forms.MouseEventArgs); virtual;
  	procedure InternalKeyDown(sender : object; e : System.Windows.Forms.KeyEventArgs); virtual;
  	procedure InternalKeyUp(sender : object; e : System.Windows.Forms.KeyEventArgs); virtual;
  	procedure InternalKeyPress(sender : object; e : System.Windows.Forms.KeyPressEventArgs); virtual;
  	
  public
  	constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);

		property Left: integer read getLeft write setLeft;
		property Top: integer read getTop write setTop;
		property Width: integer read getWidth write setWidth;
		property Height: integer read getHeight write setHeight;
		property ClientWidth: integer read getClientWidth write setClientWidth;
		property ClientHeight: integer read getClientHeight write setClientHeight;
		property Parent: ContainerControl read getParent write setParent;
		property Color: ColorType read getColor write setColor;
		property Hint: string read getHint write setHint;
		property Enabled: boolean read getEnabled write setEnabled;
		property Visible: boolean read getVisible write setVisible;
		property Caption: string read getCaption write setCaption;
		property Cursor: CursorType read getCursorType write setCursorType;
		property Align: AlignType read getAlign write setAlign;
		property PopMenu: PopupMenu read getPopupMenu write setPopupMenu;
		property Font: FontType read getFont write setFont;
		property TabOrder: integer read getTabOrder write setTabOrder;
		property TabStop: boolean read getTabStop write setTabStop;
		property BevelInner: BevelType read getBevelInner write setBevelInner;
		property BevelOuter: BevelType read getBevelOuter write setBevelOuter;
		
		procedure SetSize(Width,Height: integer);
		procedure SetPos(Left,Top: integer);
		procedure InitControl(Enabled,Visible: boolean; Align: AlignType; Cursor: CursorType; Color: ColorType; Caption,Hint: string);
  end;
  
  ContainerControl = class(Control)
  protected
  	function getControls(ind : integer): Control;
  	procedure setControls(ind: integer; ctrl: Control);
  	function getControlsCount : integer;
  public
  	property Controls[index: integer]: Control read getControls write setControls;
  	property ControlsCount: integer read getControlsCount;
  end;
  
  MenuItem = class(Component)
  public
		OnClick: procedure;
		OnClickExt: procedure(Sender: Component);
  protected
  	function getMenuItem(ind : integer): MenuItem;
  	procedure setMenuItem(ind : integer; it : MenuItem);
  	function getItemsCount: integer;
  	function getCaption: string;
  	procedure setCaption(i : string);
  	function getAutoCheck: boolean;
  	procedure setAutoCheck(i : boolean);
  	function getChecked: boolean;
  	procedure setChecked(i : boolean);
  	function getEnabled: boolean;
  	procedure setEnabled(i : boolean);
  	
  public
  	property Items[i: integer]: MenuItem read getMenuItem write setMenuItem;
  	property ItemsCount: integer read getItemsCount;
  	property Caption: string read getCaption write setCaption;
  	property AutoCheck: boolean read getAutoCheck write setAutoCheck;
  	property Checked: boolean read getChecked write setChecked;
  	property Enabled: boolean read getEnabled write setEnabled;
  	
  	procedure SetImage(name: string);
  	procedure SetImage(name: string; n: integer);
  	procedure ClearImage;
  	procedure Add(caption: string; onClick: procedure);
  	procedure Add(caption: string; onClick: procedure; name: string);
  	procedure Add(caption: string; onClick: procedure; name: string; n: integer);
  	procedure Add(caption: string; onClickExt: procedure (Sender: Component));
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
		
  end;
  
  PopupMenu = class(Component)
  protected
  	function getItems(ind : integer): MenuItem;
  	procedure setItems(ind: integer; it : MenuItem);
  	function getItemsCount : integer;
  public
  	property Items[i: integer]: MenuItem read getItems write setItems;
  	property ItemsCount: integer read getItemsCount;
  	constructor Create;
  	constructor Create(owner: Control);
  	procedure Add(caption: string; onClick: procedure);
  	procedure Add(caption: string; onClick: procedure; name: string);
  	procedure Add(caption: string; onClick: procedure; name: string; n: integer);
		procedure Add(caption: string; onClickExt: procedure (Sender: Component));
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
  end;
  
  MainMenu = class;
  
  Form = class(ContainerControl)
  private
    frm : System.Windows.Forms.Form;
    FBorderIcons : BorderIconsType; 
  protected
  	function getBorderStyle : FormBorderStyleType;
  	procedure setBorderStyle(i : FormBorderStyleType);
  	function getBorderIcons : BorderIconsType;
  	procedure setBorderIcons(i : BorderIconsType);
  	function getMenu : MainMenu;
  	procedure setMenu(i : MainMenu);
  	function getActiveControl : Control;
  	procedure setActiveControl(i : Control);
  	function getWindowState : WindowStateType;
  	procedure setWindowState(i : WindowStateType);
  	function getModalResult : ModalResultType;
  	procedure setModalResult(i : ModalResultType);
  	function getPosition : PositionType;
  	procedure setPosition(i : PositionType);
  
  private
    procedure InitForm;
    procedure InternalFormClicked(sender : object; e : System.EventArgs);
    procedure InternalActivated(sender: object; e : System.EventArgs);
    procedure InternalDeactivate(sender: object; e : System.EventArgs);
    procedure InternalShown(sender: object; e : System.EventArgs);
    procedure InternalClosed(sender: object; e : System.EventArgs);
    procedure InternalClosing(sender: object; e : System.ComponentModel.CancelEventArgs);
    
  public
  	OnShow: procedure;
  	OnHide: procedure;
  	OnClose: procedure;
  	OnActivate: procedure;
  	OnDeactivate: procedure;
  	OnCloseQuery: procedure(var CanClose: boolean);
  	OnShowExt: procedure(Sender: Component);
  	OnHideExt: procedure(Sender: Component);
  	OnCloseExt: procedure(Sender: Component);
  	OnActivateExt: procedure(Sender: Component);
  	OnDeactivateExt: procedure(Sender: Component);
  	OnCloseQueryExt: procedure(Sender: Component; var CanClose: boolean);
  	
  	constructor Create;
    constructor Create(L,T: integer);
    constructor Create(L,T,W,H: integer);
    constructor Create(L,T: integer; c: string);
    constructor Create(L,T,W,H: integer; c: string);
    constructor Create(Parent: ContainerControl);
    constructor Create(Parent: ContainerControl; L,T: integer);
    constructor Create(Parent: ContainerControl; L,T,W,H: integer);
    constructor Create(Parent: ContainerControl; L,T: integer; c: string);
    constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);

  	procedure Show;
		procedure ShowModal;
		procedure Close;
		procedure Hide;
		procedure SetIcon(s: string);
		
		property BorderStyle: FormBorderStyleType read getBorderStyle write setBorderStyle;
		property BorderIcons: BorderIconsType read getBorderIcons write setBorderIcons;
		property Menu: MainMenu read getMenu write setMenu;
		property ActiveControl: Control read getActiveControl write setActiveControl;
		property WindowState: WindowStateType read getWindowState write setWindowState;
		property ModalResult: ModalResultType read getModalResult write setModalResult;
		property Position: PositionType read getPosition write setPosition;
		
  end;
  
  MainMenu = class(Component)
  protected
  	function getItems(ind : integer): MenuItem;
  	procedure setItems(ind: integer; it : MenuItem);
  	function getItemsCount : integer;
  public
  	property Items[i: integer]: MenuItem read getItems write setItems;
  	property ItemsCount: integer read getItemsCount;
  	constructor Create;
  	constructor Create(owner: Form);
  	procedure Add(caption: string; onClick: procedure);
  	procedure Add(caption: string; onClick: procedure; name: string);
  	procedure Add(caption: string; onClick: procedure; name: string; n: integer);
		procedure Add(caption: string; onClickExt: procedure (Sender: Component));
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
		procedure Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
  end;
  
  Button = class(Control)
  private
    procedure InitButton;
  protected
    btn : System.Windows.Forms.Button;
  	function getModalResult : ModalResultType;
  	procedure setModalResult(i : ModalResultType);
  public
  	property ModalResult: ModalResultType read getModalResult write setModalResult;
  	procedure SetImage(name: string);
  	procedure SetImage(name: string; N: integer);
		procedure ClearImage;
		
		constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
  end;
  
  Edit = class(Control)
  private
    procedure InitEdit;
  protected
    edt : System.Windows.Forms.TextBox;
    procedure InternalChange(sender : object; e : System.EventArgs);
    function getText : string;
    procedure setText(i : string);
  public
    OnChange: procedure;
    OnChangeExt: procedure(Sender: Component);
    
    property Text: string read getText write setText;
    constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
  end;
  
  TextLabel = class(Control)
  private
    procedure InitLabel;
  protected
    lbl : System.Windows.Forms.Label;
  public    
    constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
  end;
  
  TextBox = class(Control)
  private
    procedure InitTextBox;
  protected
    txt : System.Windows.Forms.TextBox;
    procedure InternalChange(sender : object; e : System.EventArgs);
    function getScrollBars : ScrollStyleType;
    procedure setScrollBars(i: ScrollStyleType);
    function getReadOnly : boolean;
    procedure setReadOnly(i: boolean);
    function getLines : array of string;
    procedure setLines(i : array of string);
  public
    OnChange: procedure;
    OnChangeExt: procedure(Sender: Component);
    property ScrollBars: ScrollStyleType read getScrollBars write setScrollBars;
    property ReadOnly: boolean read getReadOnly write setReadOnly;
    property Lines : array of string read getLines write setLines;
    constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
  end;
  
  {type Checkbox = class(Control)
  private
    procedure InitCheckbox;
  protected
    chbx : System.Windows.Forms.CheckBox;
  public
    constructor Create;
		constructor Create(L,T: integer);
		constructor Create(L,T,W,H: integer);
		constructor Create(L,T: integer; c: string);
		constructor Create(L,T,W,H: integer; c: string);
		constructor Create(Parent: ContainerControl);
		constructor Create(Parent: ContainerControl; L,T: integer);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer);
		constructor Create(Parent: ContainerControl; L,T: integer; c: string);
		constructor Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
  end;}
  
implementation

var WinFormsVCL : System.Collections.Generic.Dictionary<object,Component> := new System.Collections.Generic.Dictionary<object,Component>();
    MainForm : Form;
    
constructor Component.Create;
begin
  
end;

function Component.getTag: integer;
begin
  
end;

procedure Component.setTag(i : integer);
begin
  
end;

function FontType.getColor: integer;
begin
  
end;

procedure FontType.setColor(i : integer);
begin
  
end;

function FontType.getSize: integer;
begin
  
end;

procedure FontType.setSize(i : integer);
begin
  
end;

function FontType.getName: string;
begin
  
end;

procedure FontType.setName(i : string);
begin
  
end;

function FontType.getStyle: set of FontStyle;
begin
  
end;

procedure FontType.setStyle(i : set of FontStyle);
begin
  
end;

constructor FontType.Create;
begin
  
end;

function ContainerControl.getControls(ind : integer): Control;
begin
  
end;

procedure ContainerControl.setControls(ind: integer; ctrl: Control);
begin
  
end;

function ContainerControl.getControlsCount: integer;
begin
  
end;

function Control.getLeft: integer;
begin
  Result := ctrl.Left;
end;

procedure Control.setLeft(i : integer);
begin
  ctrl.Left := i;
end;

function Control.getTop: integer;
begin
  Result := ctrl.Top;
end;

procedure Control.setTop(i : integer);
begin
  ctrl.Top := i;
end;

function Control.getWidth: integer;
begin
  Result := ctrl.Width;
end;

procedure Control.setWidth(i : integer);
begin
  ctrl.Width := i;
end;

function Control.getHeight: integer;
begin
  Result := ctrl.Height;
end;

procedure Control.setHeight(i : integer);
begin
  ctrl.Height := i;
end;

function Control.getClientWidth: integer;
begin
  Result := ctrl.ClientSize.Width;
end;

procedure Control.setClientWidth(i : integer);
begin
  ctrl.ClientSize.Width := i;
end;

function Control.getClientHeight: integer;
begin
  Result := ctrl.ClientSize.Height;
end;

procedure Control.setClientHeight(i : integer);
begin
  ctrl.ClientSize.Height := i;
end;

function Control.getParent: ContainerControl;
begin
  
end;

procedure Control.setParent(i : ContainerControl);
begin
  
end;

function Control.getColor: integer;
begin
  
end;

procedure Control.setColor(i : ColorType);
begin
  //ctrl.BackColor := i;
end;

function Control.getHint: string;
begin
  
end;

procedure Control.setHint(i : string);
begin
  
end;

function Control.getEnabled: boolean;
begin
  Result := ctrl.Enabled;
end;

procedure Control.setEnabled(i : boolean);
begin
  ctrl.Enabled := i;
end;

function Control.getVisible: boolean;
begin
  Result := ctrl.Visible;
end;

procedure Control.setVisible(i : boolean);
begin
  ctrl.Visible := i;
end;

function Control.getCaption: string;
begin
  Result := ctrl.Text;
end;

procedure Control.setCaption(i : string);
begin
  ctrl.Text := i;
end;

function Control.getCursorType: CursorType;
begin
  if ctrl.Cursor = System.Windows.Forms.Cursors.Arrow then Result := crArrow
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.AppStarting then Result := crAppStart
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.Cross then Result := crCross
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.Default then Result := crDefault
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.Hand then Result := crHandPoint
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.No then Result := crNo
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.HSplit then Result := crHSplit
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.VSplit then Result := crVSplit
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.Help then Result := crHelp
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.IBeam then Result := crIBeam
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.SizeAll then Result := crSizeAll
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.SizeNESW then Result := crSizeNESW
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.SizeNS then Result := crSizeNS
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.SizeNWSE then Result := crSizeNWSE
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.SizeWE then Result := crSizeWE
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.UpArrow then Result := crUpArrow
  else
  if ctrl.Cursor = System.Windows.Forms.Cursors.WaitCursor then Result := crHourGlass
end;

procedure Control.setCursorType(i : CursorType);
begin
  if i = crArrow then ctrl.Cursor := System.Windows.Forms.Cursors.Arrow;
end;

function Control.getAlign: AlignType;
begin
  
end;

procedure Control.setAlign(i : AlignType);
begin
  
end;

function Control.getPopupMenu: PopupMenu;
begin
  
end;

procedure Control.setPopupMenu(i : PopupMenu);
begin
  
end;

function Control.getFont: FontType;
begin
  
end;

procedure Control.setFont(i : FontType);
begin
  
end;

function Control.getTabOrder: integer;
begin
  Result := ctrl.TabIndex;
end;

procedure Control.setTabOrder(i : integer);
begin
  ctrl.TabIndex := i;
end;

function Control.getTabStop: boolean;
begin
  Result := ctrl.TabStop;
end;

procedure Control.setTabStop(i : boolean);
begin
  ctrl.TabStop := i; 
end;

function Control.getBevelInner: BevelType;
begin
  
end;

procedure Control.setBevelInner(i : BevelType);
begin
  
end;

function Control.getBevelOuter: BevelType;
begin
  
end;

procedure Control.setBevelOuter(i : BevelType);
begin
  
end;

constructor Control.Create;
begin
  
end;

constructor Control.Create(L,T: integer);
begin
  
end;

constructor Control.Create(L,T,W,H: integer);
begin
  
end;

constructor Control.Create(L,T: integer; c: string);
begin
  
end;

constructor Control.Create(L,T,W,H: integer; c: string);
begin
  
end;

constructor Control.Create(Parent: ContainerControl);
begin
  
end;

constructor Control.Create(Parent: ContainerControl; L,T: integer);
begin
  
end;

constructor Control.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
  
end;

constructor Control.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
  
end;

constructor Control.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
  
end;

procedure Control.SetSize(Width,Height: integer);
begin
 ctrl.Width := Width;
 ctrl.Height := Height;
end;

procedure Control.SetPos(Left,Top: integer);
begin
 ctrl.Left := Left;
 ctrl.Top := Top;
end;

procedure Control.InitControl(Enabled,Visible: boolean; Align: AlignType; Cursor: CursorType; Color: ColorType; Caption,Hint: string);
begin
end;

function PopupMenu.getItems(ind : integer): MenuItem;
begin
  
end;

procedure PopupMenu.setItems(ind: integer; it : MenuItem);
begin
  
end;

function PopupMenu.getItemsCount: integer;
begin
  
end;

constructor PopupMenu.Create;
begin
  
end;

constructor PopupMenu.Create(owner: Control);
begin
  
end;

procedure PopupMenu.Add(caption: string; onClick: procedure);
begin
  
end;

procedure PopupMenu.Add(caption: string; onClick: procedure; name: string);
begin
  
end;

procedure PopupMenu.Add(caption: string; onClick: procedure; name: string; n: integer);
begin
  
end;

procedure PopupMenu.Add(caption: string; onClickExt: procedure (Sender: Component));
begin
  
end;

procedure PopupMenu.Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
begin
  
end;

procedure PopupMenu.Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
begin
  
end;

function MenuItem.getMenuItem(ind : integer): MenuItem;
begin
  
end;

procedure MenuItem.setMenuItem(ind : integer; it : MenuItem);
begin
  
end;

function MenuItem.getItemsCount: integer;
begin
  
end;

function MenuItem.getCaption: string;
begin
  
end;

procedure MenuItem.setCaption(i : string);
begin
  
end;

function MenuItem.getAutoCheck: boolean;
begin
  
end;

procedure MenuItem.setAutoCheck(i : boolean);
begin
  
end;

function MenuItem.getChecked: boolean;
begin
  
end;

procedure MenuItem.setChecked(i : boolean);
begin
  
end;

function MenuItem.getEnabled: boolean;
begin
  
end;

procedure MenuItem.setEnabled(i : boolean);
begin
  
end;

procedure MenuItem.SetImage(name: string);
begin
  
end;

procedure MenuItem.SetImage(name: string; n: integer);
begin
  
end;

procedure MenuItem.ClearImage;
begin
  
end;

procedure MenuItem.Add(caption: string; onClick: procedure);
begin
  
end;

procedure MenuItem.Add(caption: string; onClick: procedure; name: string);
begin
  
end;

procedure MenuItem.Add(caption: string; onClick: procedure; name: string; n: integer);
begin
  
end;

procedure MenuItem.Add(caption: string; onClickExt: procedure (Sender: Component));
begin
  
end;

procedure MenuItem.Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
begin
  
end;

procedure MenuItem.Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
begin
  
end;

function Form.getBorderStyle: FormBorderStyleType;
begin
  
end;

procedure Form.setBorderStyle(i : FormBorderStyleType);
begin
  
end;

function Form.getBorderIcons: BorderIconsType;
begin
  Result := FBorderIcons;
end;

procedure Form.setBorderIcons(i : BorderIconsType);
begin
  //FBorderIcons := i;
  if biMinimize in i then frm.MinimizeBox := true
  else frm.MinimizeBox := false;
  if biMaximize in i then frm.MaximizeBox := true
  else frm.MaximizeBox := false;
  if biHelp in i then frm.HelpButton := true
  else frm.HelpButton := false;
end;

function Form.getMenu: MainMenu;
begin
  
end;

procedure Form.setMenu(i : MainMenu);
begin
  
end;

function Form.getActiveControl: Control;
begin
  Result := WinFormsVCL[frm.ActiveControl] as Control;
end;

procedure Form.setActiveControl(i : Control);
begin
  frm.ActiveControl := i.ctrl;
end;

function Form.getWindowState: WindowStateType;
begin
  case frm.WindowState of
    System.Windows.Forms.FormWindowState.Normal : Result := wsNormal;
    System.Windows.Forms.FormWindowState.Minimized : Result := wsMinimized;
    System.Windows.Forms.FormWindowState.Maximized : Result := wsMaximized;
  end;
end;

procedure Form.setWindowState(i : WindowStateType);
begin
  case i of
    wsNormal : frm.WindowState := System.Windows.Forms.FormWindowState.Normal;
    wsMinimized : frm.WindowState := System.Windows.Forms.FormWindowState.Minimized;
    wsMaximized : frm.WindowState := System.Windows.Forms.FormWindowState.Maximized;
  end;
end;

function Form.getModalResult: ModalResultType;
begin
  
end;

procedure Form.setModalResult(i : ModalResultType);
begin
  
end;

function Form.getPosition: PositionType;
begin
  case frm.StartPosition of
    System.Windows.Forms.FormStartPosition.CenterScreen : Result := poScreenCenter;
    System.Windows.Forms.FormStartPosition.WindowsDefaultBounds : Result := poDefault;
  end;
end;

procedure Form.setPosition(i : PositionType);
begin
  case i of
    poScreenCenter : frm.StartPosition := System.Windows.Forms.FormStartPosition.CenterScreen;
    poDefault : frm.StartPosition := System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
  end;
end;

procedure Form.Show;
begin
 frm.Show(); 
end;

procedure Form.ShowModal;
begin
 frm.ShowDialog(); 
end;

procedure Form.Close;
begin
 frm.Close(); 
end;

procedure Form.Hide;
begin
 frm.Hide(); 
end;

procedure Form.SetIcon(s: string);
begin
  
end;

function MainMenu.getItems(ind : integer): MenuItem;
begin
  
end;

procedure MainMenu.setItems(ind: integer; it : MenuItem);
begin
  
end;

function MainMenu.getItemsCount: integer;
begin
  
end;

constructor MainMenu.Create;
begin
  
end;

constructor MainMenu.Create(owner: Form);
begin
  
end;

procedure MainMenu.Add(caption: string; onClick: procedure);
begin
  
end;

procedure MainMenu.Add(caption: string; onClick: procedure; name: string);
begin
  
end;

procedure MainMenu.Add(caption: string; onClick: procedure; name: string; n: integer);
begin
  
end;

procedure MainMenu.Add(caption: string; onClickExt: procedure (Sender: Component));
begin
  
end;

procedure MainMenu.Add(caption: string; onClickExt: procedure (Sender: Component); name: string);
begin
  
end;

procedure MainMenu.Add(caption: string; onClickExt: procedure (Sender: Component); name: string; n: integer);
begin
  
end;

constructor Form.Create;
begin
 frm := new System.Windows.Forms.Form(); 
 ctrl := frm;
 InitForm;
end;

constructor Form.Create(L,T: integer);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Left := L;
 frm.Top := T;
end;

constructor Form.Create(L,T,W,H: integer);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Left := L;
 frm.Top := T;
 frm.Width := W;
 frm.Height := H;
end;

constructor Form.Create(L,T: integer; c: string);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Left := L;
 frm.Top := T;
 frm.Text := c;
end;

constructor Form.Create(L,T,W,H: integer; c: string);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Left := L;
 frm.Top := T;
 frm.Width := W;
 frm.Height := H;
 frm.Text := c;
end;

constructor Form.Create(Parent: ContainerControl);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 frm.Parent := Parent.ctrl;
end;

constructor Form.Create(Parent: ContainerControl; L,T: integer);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Parent := Parent.ctrl;
 frm.Left := L;
 frm.Top := T;
end;

constructor Form.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Parent := Parent.ctrl;
 frm.Left := L;
 frm.Top := T;
 frm.Width := W;
 frm.Height := H;
end;

constructor Form.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Parent := Parent.ctrl;
 frm.Left := L;
 frm.Top := T;
 frm.Text := c;
end;

constructor Form.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
 frm := new System.Windows.Forms.Form();
 ctrl := frm;
 InitForm;
 frm.Parent := Parent.ctrl;
 frm.Left := L;
 frm.Top := T;
 frm.Width := W;
 frm.Height := H;
 frm.Text := c;
end;

procedure Form.InitForm;
begin
	InitCtrl;
	frm.Activated += InternalActivated;
	frm.Shown += InternalShown;
	frm.Closed += InternalClosed;
	frm.Deactivate += InternalDeactivate;
	frm.Closing += InternalClosing;
	MainForm := self;
end;

procedure Form.InternalFormClicked(sender : object; e : System.EventArgs);
begin
 if self.OnClick <> nil then OnClick;
end;

procedure Control.InternalClicked(sender : object; e : System.EventArgs);
begin
 if self.OnClick <> nil then OnClick; 
 if self.OnClickExt <> nil then OnClickExt(WinFormsVCL[sender]);
end;

procedure Control.InternalDblClicked(sender : object; e : System.EventArgs);
begin
 if self.OnDblClick <> nil then OnDblClick;
 if self.OnDblClickExt <> nil then OnDblClickExt(WinFormsVCL[sender]);
end;

procedure Control.InternalEnter(sender : object; e : System.EventArgs);
begin
 if self.OnEnter <> nil then OnEnter;
 if self.OnEnterExt <> nil then OnEnterExt(WinFormsVCL[sender]);
end;

procedure Control.InternalExit(sender : object; e : System.EventArgs);
begin
 if self.OnExit <> nil then OnExit;
 if self.OnExitExt <> nil then OnExitExt(WinFormsVCL[sender]);
end;

procedure Control.InternalResize(sender : object; e : System.EventArgs);
begin
 if self.OnEnter <> nil then OnResize;
 if self.OnResizeExt <> nil then OnResizeExt(WinFormsVCL[sender]);
end;

procedure Control.InternalMouseDown(sender : object; e : System.Windows.Forms.MouseEventArgs);
begin
 if self.OnMouseDown <> nil then
 begin
 	if e.Button = System.Windows.Forms.MouseButtons.Left then
 		OnMouseDown(e.X,e.Y,btLeft)
 	else if e.Button = System.Windows.Forms.MouseButtons.Right then
 		OnMouseDown(e.X,e.Y,btRight)
 end;
 if self.OnMouseDownExt <> nil then
 begin
 	var s : ShiftState;
 	if e.Button = System.Windows.Forms.MouseButtons.Left then Include(s,ssLeft);
 	if e.Button = System.Windows.Forms.MouseButtons.Right then Include(s,ssRight);
 	if e.Button = System.Windows.Forms.MouseButtons.Middle then Include(s,ssMiddle);
 	OnMouseDownExt(WinFormsVCL[sender],s,e.X,e.Y);
 end;
end;

procedure Control.InternalMouseUp(sender : object; e : System.Windows.Forms.MouseEventArgs);
begin
 if self.OnMouseUp <> nil then
 begin
 	if e.Button = System.Windows.Forms.MouseButtons.Left then
 		OnMouseUp(e.X,e.Y,btLeft)
 	else if e.Button = System.Windows.Forms.MouseButtons.Right then
 		OnMouseUp(e.X,e.Y,btRight)
 end;
 if self.OnMouseUpExt <> nil then
 begin
 	var s : ShiftState;
 	if e.Button = System.Windows.Forms.MouseButtons.Left then Include(s,ssLeft);
 	if e.Button = System.Windows.Forms.MouseButtons.Right then Include(s,ssRight);
 	if e.Button = System.Windows.Forms.MouseButtons.Middle then Include(s,ssMiddle);
 	OnMouseUpExt(WinFormsVCL[sender],s,e.X,e.Y);
 end;
end;

procedure Control.InternalMouseMove(sender : object; e : System.Windows.Forms.MouseEventArgs);
begin
 if self.OnMouseMove <> nil then
 begin
 	if e.Button = System.Windows.Forms.MouseButtons.Left then
 		OnMouseMove(e.X,e.Y,btLeft)
 	else if e.Button = System.Windows.Forms.MouseButtons.Right then
 		OnMouseMove(e.X,e.Y,btRight)
 	else if e.Button = System.Windows.Forms.MouseButtons.None then
 		OnMouseMove(e.X,e.Y,btNone);
 end;
 if self.OnMouseMoveExt <> nil then
 begin
 	var s : ShiftState;
 	if e.Button = System.Windows.Forms.MouseButtons.Left then Include(s,ssLeft);
 	if e.Button = System.Windows.Forms.MouseButtons.Right then Include(s,ssRight);
 	if e.Button = System.Windows.Forms.MouseButtons.Middle then Include(s,ssMiddle);
 	OnMouseMoveExt(WinFormsVCL[sender],s,e.X,e.Y);
 end;
end;

procedure Control.InternalKeyDown(sender : object; e : System.Windows.Forms.KeyEventArgs);
begin
	if self.OnKeyDown <> nil then OnKeyDown(e.KeyValue);
	if self.OnKeyDownExt <> nil then
	begin
 		var s : ShiftState;
 		if e.Control then Include(s,ssCtrl);
 		if e.Alt then Include(s,ssAlt);
 		if e.Shift then Include(s,ssShift);
 		OnKeyDownExt(WinFormsVCL[sender],e.KeyValue,s);
 	end;
end;

procedure Control.InternalKeyUp(sender : object; e : System.Windows.Forms.KeyEventArgs);
begin
  if self.OnKeyUp <> nil then OnKeyUp(e.KeyValue);
  if self.OnKeyUpExt <> nil then
	begin
 		var s : ShiftState;
 		if e.Control then Include(s,ssCtrl);
 		if e.Alt then Include(s,ssAlt);
 		if e.Shift then Include(s,ssShift);
 		OnKeyUpExt(WinFormsVCL[sender],e.KeyValue,s);
 	end;
end;

procedure Control.InternalKeyPress(sender : object; e : System.Windows.Forms.KeyPressEventArgs);
begin
  if self.OnKeyPress <> nil then OnKeyPress(e.KeyChar);
  if self.OnKeyPressExt <> nil then
  begin
  	OnKeyPressExt(WinFormsVCL[sender],e.KeyChar);
  end;
end;

procedure Control.InitCtrl;
begin
	WinFormsVCL[ctrl] := self;
	ctrl.Click += InternalClicked;
	ctrl.DoubleClick += InternalDblClicked;
	ctrl.Enter += InternalEnter;
	ctrl.Leave += InternalExit;
	ctrl.MouseDown += InternalMouseDown;
	ctrl.MouseUp += InternalMouseUp;
	ctrl.Resize += InternalResize;
	ctrl.KeyDown += InternalKeyDown;
	ctrl.KeyUp += InternalKeyUp;
	ctrl.KeyPress += InternalKeyPress;
end;

procedure Form.InternalActivated(sender: object; e : System.EventArgs);
begin
  if self.OnActivate <> nil then OnActivate;
  if self.OnActivateExt <> nil then OnActivateExt(WinFormsVCL[sender]);
end;

procedure Form.InternalDeactivate(sender: object; e : System.EventArgs);
begin
  if self.OnDeactivate <> nil then OnDeactivate;
  if self.OnDeactivateExt <> nil then OnDeactivateExt(WinFormsVCL[sender]);
end;

procedure Form.InternalShown(sender: object; e : System.EventArgs);
begin
  if self.OnShow <> nil then OnShow;
  if self.OnShowExt <> nil then OnShowExt(WinFormsVCL[sender]);
end;

procedure Form.InternalClosed(sender: object; e : System.EventArgs);
begin
  if self.OnClose <> nil then OnClose;
  if self.OnCloseExt <> nil then OnCloseExt(WinFormsVCL[sender]);
end;

procedure Form.InternalClosing(sender: object; e : System.ComponentModel.CancelEventArgs);
begin
  var f := false;
  if self.OnCloseQuery <> nil then OnCloseQuery(f);
  if self.OnCloseQueryExt <> nil then OnCloseQueryExt(WinFormsVCL[sender],f);
  e.Cancel := f;
end;

function Control.getTag: integer;
begin
  Result := integer(ctrl.Tag); 
end;

procedure Control.setTag(i : integer);
begin
	ctrl.Tag := i;  
end;


function Button.getModalResult: ModalResultType;
begin
  
end;

procedure Button.setModalResult(i : ModalResultType);
begin
  
end;

procedure Button.SetImage(name: string);
begin
  
end;

procedure Button.SetImage(name: string; N: integer);
begin
  
end;

procedure Button.ClearImage;
begin
  
end;

constructor Button.Create;
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 InitButton;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(btn);
end;

constructor Button.Create(L,T: integer);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 InitButton;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(btn); 
end;

constructor Button.Create(L,T,W,H: integer);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Width := W;
 btn.Height := H;
 InitButton;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(btn); 
end;

constructor Button.Create(L,T: integer; c: string);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Text := c;
 InitButton;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(btn); 
end;

constructor Button.Create(L,T,W,H: integer; c: string);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Width := W;
 btn.Height := H;
 btn.Text := c;
 InitButton;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(btn); 
end;

constructor Button.Create(Parent: ContainerControl);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 InitButton;
 Parent.ctrl.Controls.Add(btn); 
end;

constructor Button.Create(Parent: ContainerControl; L,T: integer);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 InitButton;
 Parent.ctrl.Controls.Add(btn);   
end;

constructor Button.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Width := W;
 btn.Height := H;
 InitButton;
 Parent.ctrl.Controls.Add(btn); 
end;

constructor Button.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Text := c;
 InitButton;
 Parent.ctrl.Controls.Add(btn); 
end;

constructor Button.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
 btn := new System.Windows.Forms.Button(); 
 ctrl := btn;
 btn.Left := L;
 btn.Top := T;
 btn.Width := W;
 btn.Height := H;
 btn.Text := c;
 InitButton;
 Parent.ctrl.Controls.Add(btn);  
end;

procedure Button.InitButton;
begin
  InitCtrl;
end;

procedure Edit.InitEdit;
begin
  InitCtrl;
  edt.TextChanged += InternalChange;
end;

constructor Edit.Create;
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 InitEdit;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(edt);
end;

constructor Edit.Create(L,T: integer);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 InitEdit;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(edt); 
end;

constructor Edit.Create(L,T,W,H: integer);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Width := W;
 edt.Height := H;
 InitEdit;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(edt); 
end;

constructor Edit.Create(L,T: integer; c: string);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Text := c;
 InitEdit;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(edt); 
end;

constructor Edit.Create(L,T,W,H: integer; c: string);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Width := W;
 edt.Height := H;
 edt.Text := c;
 InitEdit;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(edt); 
end;

constructor Edit.Create(Parent: ContainerControl);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 InitEdit;
 Parent.ctrl.Controls.Add(edt); 
end;

constructor Edit.Create(Parent: ContainerControl; L,T: integer);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 InitEdit;
 Parent.ctrl.Controls.Add(edt); 
end;

constructor Edit.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Width := W;
 edt.Height := H;
 InitEdit;
 Parent.ctrl.Controls.Add(edt); 
end;

constructor Edit.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Text := c;
 InitEdit;
 Parent.ctrl.Controls.Add(edt); 
end;

constructor Edit.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
 edt := new System.Windows.Forms.TextBox();
 edt.Multiline := false;
 ctrl := edt;
 edt.Left := L;
 edt.Top := T;
 edt.Width := W;
 edt.Height := H;
 edt.Text := c;
 InitEdit;
 Parent.ctrl.Controls.Add(edt); 
end;

procedure Edit.InternalChange(sender : object; e : System.EventArgs);
begin
 if self.OnChange <> nil then OnChange;
 if self.OnChangeExt <> nil then OnChangeExt(WinFormsVCL[sender]);
end;


function Edit.getText: string;
begin
  Result := edt.Text;
end;

procedure Edit.setText(i : string);
begin
 edt.Text := i; 
end;

procedure TextLabel.InitLabel;
begin
 InitCtrl; 
end;

constructor TextLabel.Create;
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 InitLabel;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(lbl);   
end;

constructor TextLabel.Create(L,T: integer);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 InitLabel;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(lbl);  
end;

constructor TextLabel.Create(L,T,W,H: integer);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Width := W;
 lbl.Height := H;
 InitLabel;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(lbl);   
end;

constructor TextLabel.Create(L,T: integer; c: string);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Text := c;
 InitLabel;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(lbl);   
end;

constructor TextLabel.Create(L,T,W,H: integer; c: string);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Width := W;
 lbl.Height := H;
 lbl.Text := c;
 InitLabel;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(lbl);   
end;

constructor TextLabel.Create(Parent: ContainerControl);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 InitLabel;
 Parent.ctrl.Controls.Add(lbl);  
end;

constructor TextLabel.Create(Parent: ContainerControl; L,T: integer);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 InitLabel;
 Parent.ctrl.Controls.Add(lbl);   
end;

constructor TextLabel.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Width := W;
 lbl.Height := H;
 InitLabel;
 Parent.ctrl.Controls.Add(lbl);   
end;

constructor TextLabel.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Text := c;
 InitLabel;
 Parent.ctrl.Controls.Add(lbl);   
end;

constructor TextLabel.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
 lbl := new System.Windows.Forms.Label();
 ctrl := lbl;
 lbl.Left := L;
 lbl.Top := T;
 lbl.Width := W;
 lbl.Height := H;
 lbl.Text := c;
 InitLabel;
 Parent.ctrl.Controls.Add(lbl);   
end;


procedure TextBox.InitTextBox;
begin
 InitCtrl;
 txt.Multiline := true;
end;

procedure TextBox.InternalChange(sender : object; e : System.EventArgs);
begin
 if self.OnChange <> nil then OnChange;
 if self.OnChangeExt <> nil then OnChangeExt(WinFormsVCL[sender]);
end;

function TextBox.getScrollBars: ScrollStyleType;
begin
 if txt.ScrollBars = System.Windows.Forms.ScrollBars.None then Result := ssNone
 else if txt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical then Result := ssVertical
 else if txt.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal then Result := ssHorizontal
 else if txt.ScrollBars = System.Windows.Forms.ScrollBars.Both then Result := ssBoth;
end;

procedure TextBox.setScrollBars(i: ScrollStyleType);
begin
 case i of
    ssNone : txt.ScrollBars := System.Windows.Forms.ScrollBars.None;
    ssVertical : txt.ScrollBars := System.Windows.Forms.ScrollBars.Vertical;
    ssHorizontal : txt.ScrollBars := System.Windows.Forms.ScrollBars.Horizontal;
    ssBoth : txt.ScrollBars := System.Windows.Forms.ScrollBars.Both;
 end;
end;

function TextBox.getReadOnly: boolean;
begin
 Result := txt.ReadOnly; 
end;

procedure TextBox.setReadOnly(i: boolean);
begin
 txt.ReadOnly := i; 
end;

function TextBox.getLines: array of string;
begin
 Result := txt.Lines; 
end;

procedure TextBox.setLines(i : array of string);
begin
 txt.Lines := i; 
end;

constructor TextBox.Create;
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 InitTextBox;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(txt); 
end;

constructor TextBox.Create(L,T: integer);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 InitTextBox;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(txt);  
end;

constructor TextBox.Create(L,T,W,H: integer);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Width := W;
 txt.Height := H;
 InitTextBox;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(txt);   
end;

constructor TextBox.Create(L,T: integer; c: string);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Text := c;
 InitTextBox;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(txt);   
end;

constructor TextBox.Create(L,T,W,H: integer; c: string);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Width := W;
 txt.Height := H;
 txt.Text := c;
 InitTextBox;
 if MainForm <> nil then
 MainForm.frm.Controls.Add(txt); 
end;

constructor TextBox.Create(Parent: ContainerControl);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 InitTextBox;
 Parent.ctrl.Controls.Add(txt); 
end;

constructor TextBox.Create(Parent: ContainerControl; L,T: integer);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 InitTextBox;
 Parent.ctrl.Controls.Add(txt);   
end;

constructor TextBox.Create(Parent: ContainerControl; L,T,W,H: integer);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Width := W;
 txt.Height := H;
 InitTextBox;
 Parent.ctrl.Controls.Add(txt);   
end;

constructor TextBox.Create(Parent: ContainerControl; L,T: integer; c: string);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Text := c;
 InitTextBox;
 Parent.ctrl.Controls.Add(txt);   
end;

constructor TextBox.Create(Parent: ContainerControl; L,T,W,H: integer; c: string);
begin
 txt := new System.Windows.Forms.TextBox();
 ctrl := txt;
 txt.Left := L;
 txt.Top := T;
 txt.Width := W;
 txt.Height := H;
 txt.Text := c;
 InitTextBox;
 Parent.ctrl.Controls.Add(txt);   
end;

initialization

finalization

if MainForm <> nil then 
begin
 MainForm.Show;
 System.Windows.Forms.Application.Run(MainForm.frm);
end;
end.