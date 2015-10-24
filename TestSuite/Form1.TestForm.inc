procedure InitializeComponent;
    begin
        self.SuspendLayout();
        // 
        // TestForm
        // 
        self.ClientSize := new System.Drawing.Size(222, 92);
        self.Name := 'TestForm';
        self.Text := 'Form1';
        self.Load += Form1_Load;
        self.ResumeLayout(false);
    end;
