using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using NodeGenerator;

namespace NodesGenerator
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button close;
		private System.Windows.Forms.Button save;
		private System.Windows.Forms.Button save_as;
		private System.Windows.Forms.Button open;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.ListBox nodes_list;
		private System.Windows.Forms.Button add;
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.Button generate_code;
		private System.Windows.Forms.Button generate_visitor_template;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.TextBox file_name;
		private System.Windows.Forms.TextBox namespace_name;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TextBox factory_name;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox visitor_interface_file_name;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Button generate_pcu_write_visitor;
        private Label label5;
        private TextBox pcu_writer_name;
        private TextBox tb_pcu_writer;
        private Label label6;
        private TextBox tb_pcu_reader;
        private Button generate_pcu_read_visitor;
        private Button button1;
        private Button generate_cpp_code;
        private Button generate_cross_platform_writer;
        private Button generate_cross_platform_reader_cpp;
        private Button generate_visualizator_cpp;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.save = new System.Windows.Forms.Button();
            this.close = new System.Windows.Forms.Button();
            this.nodes_list = new System.Windows.Forms.ListBox();
            this.save_as = new System.Windows.Forms.Button();
            this.open = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.add = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.generate_code = new System.Windows.Forms.Button();
            this.generate_visitor_template = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.generate_visualizator_cpp = new System.Windows.Forms.Button();
            this.generate_cross_platform_reader_cpp = new System.Windows.Forms.Button();
            this.generate_cross_platform_writer = new System.Windows.Forms.Button();
            this.generate_cpp_code = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.generate_pcu_read_visitor = new System.Windows.Forms.Button();
            this.generate_pcu_write_visitor = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.file_name = new System.Windows.Forms.TextBox();
            this.namespace_name = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tb_pcu_reader = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_pcu_writer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.visitor_interface_file_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.factory_name = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(10, 46);
            this.save.Name = "save";
            this.save.Size = new System.Drawing.Size(182, 27);
            this.save.TabIndex = 1;
            this.save.Text = "Сохранить";
            this.save.Click += new System.EventHandler(this.save_Click);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(620, 378);
            this.close.Name = "close";
            this.close.Size = new System.Drawing.Size(135, 37);
            this.close.TabIndex = 2;
            this.close.Text = "Выход";
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // nodes_list
            // 
            this.nodes_list.ItemHeight = 16;
            this.nodes_list.Location = new System.Drawing.Point(10, 9);
            this.nodes_list.Name = "nodes_list";
            this.nodes_list.Size = new System.Drawing.Size(230, 372);
            this.nodes_list.TabIndex = 3;
            this.nodes_list.DoubleClick += new System.EventHandler(this.nodes_list_DoubleClick);
            // 
            // save_as
            // 
            this.save_as.Location = new System.Drawing.Point(10, 83);
            this.save_as.Name = "save_as";
            this.save_as.Size = new System.Drawing.Size(182, 27);
            this.save_as.TabIndex = 4;
            this.save_as.Text = "Сохранить как";
            this.save_as.Click += new System.EventHandler(this.save_as_Click);
            // 
            // open
            // 
            this.open.Location = new System.Drawing.Point(10, 9);
            this.open.Name = "open";
            this.open.Size = new System.Drawing.Size(182, 27);
            this.open.TabIndex = 5;
            this.open.Text = "Открыть";
            this.open.Click += new System.EventHandler(this.open_Click);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(250, 18);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(76, 27);
            this.add.TabIndex = 6;
            this.add.Text = "Add";
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(250, 55);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(76, 27);
            this.delete.TabIndex = 7;
            this.delete.Text = "Delete";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // generate_code
            // 
            this.generate_code.Location = new System.Drawing.Point(10, 55);
            this.generate_code.Name = "generate_code";
            this.generate_code.Size = new System.Drawing.Size(182, 28);
            this.generate_code.TabIndex = 8;
            this.generate_code.Text = "Сгенерировать код";
            this.generate_code.Click += new System.EventHandler(this.generate_code_Click);
            // 
            // generate_visitor_template
            // 
            this.generate_visitor_template.Location = new System.Drawing.Point(10, 90);
            this.generate_visitor_template.Name = "generate_visitor_template";
            this.generate_visitor_template.Size = new System.Drawing.Size(182, 40);
            this.generate_visitor_template.TabIndex = 9;
            this.generate_visitor_template.Text = "Генерировать шаблон визитора";
            this.generate_visitor_template.Click += new System.EventHandler(this.generate_visitor_template_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.open);
            this.panel1.Controls.Add(this.save_as);
            this.panel1.Controls.Add(this.save);
            this.panel1.Location = new System.Drawing.Point(586, 246);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(201, 120);
            this.panel1.TabIndex = 10;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.generate_visualizator_cpp);
            this.panel2.Controls.Add(this.generate_cross_platform_reader_cpp);
            this.panel2.Controls.Add(this.generate_cross_platform_writer);
            this.panel2.Controls.Add(this.generate_cpp_code);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.generate_pcu_read_visitor);
            this.panel2.Controls.Add(this.generate_pcu_write_visitor);
            this.panel2.Controls.Add(this.generate_code);
            this.panel2.Controls.Add(this.generate_visitor_template);
            this.panel2.Location = new System.Drawing.Point(586, 9);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(384, 231);
            this.panel2.TabIndex = 11;
            // 
            // generate_visualizator_cpp
            // 
            this.generate_visualizator_cpp.Location = new System.Drawing.Point(199, 90);
            this.generate_visualizator_cpp.Name = "generate_visualizator_cpp";
            this.generate_visualizator_cpp.Size = new System.Drawing.Size(167, 40);
            this.generate_visualizator_cpp.TabIndex = 16;
            this.generate_visualizator_cpp.Text = "Генерировать cpp-визуализатор";
            this.generate_visualizator_cpp.UseVisualStyleBackColor = true;
            this.generate_visualizator_cpp.Click += new System.EventHandler(this.generate_visualizator_cpp_Click);
            // 
            // generate_cross_platform_reader_cpp
            // 
            this.generate_cross_platform_reader_cpp.Location = new System.Drawing.Point(199, 185);
            this.generate_cross_platform_reader_cpp.Name = "generate_cross_platform_reader_cpp";
            this.generate_cross_platform_reader_cpp.Size = new System.Drawing.Size(167, 40);
            this.generate_cross_platform_reader_cpp.TabIndex = 15;
            this.generate_cross_platform_reader_cpp.Text = "Генерировать C++ десериализатор";
            this.generate_cross_platform_reader_cpp.UseVisualStyleBackColor = true;
            this.generate_cross_platform_reader_cpp.Click += new System.EventHandler(this.generate_cross_platform_reader_cpp_Click);
            // 
            // generate_cross_platform_writer
            // 
            this.generate_cross_platform_writer.Location = new System.Drawing.Point(199, 138);
            this.generate_cross_platform_writer.Name = "generate_cross_platform_writer";
            this.generate_cross_platform_writer.Size = new System.Drawing.Size(167, 40);
            this.generate_cross_platform_writer.TabIndex = 14;
            this.generate_cross_platform_writer.Text = "Генерировать cross - platform сериализатор";
            this.generate_cross_platform_writer.UseVisualStyleBackColor = true;
            this.generate_cross_platform_writer.Click += new System.EventHandler(this.button2_Click);
            // 
            // generate_cpp_code
            // 
            this.generate_cpp_code.Location = new System.Drawing.Point(199, 57);
            this.generate_cpp_code.Name = "generate_cpp_code";
            this.generate_cpp_code.Size = new System.Drawing.Size(167, 26);
            this.generate_cpp_code.TabIndex = 13;
            this.generate_cpp_code.Text = "Генерировать C++ код";
            this.generate_cpp_code.UseVisualStyleBackColor = true;
            this.generate_cpp_code.Click += new System.EventHandler(this.generate_cpp_code_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(10, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 36);
            this.button1.TabIndex = 12;
            this.button1.Text = "Сгенерировать все";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // generate_pcu_read_visitor
            // 
            this.generate_pcu_read_visitor.Location = new System.Drawing.Point(10, 185);
            this.generate_pcu_read_visitor.Name = "generate_pcu_read_visitor";
            this.generate_pcu_read_visitor.Size = new System.Drawing.Size(182, 40);
            this.generate_pcu_read_visitor.TabIndex = 11;
            this.generate_pcu_read_visitor.Text = "Генерировать pcu-десериализатор";
            this.generate_pcu_read_visitor.UseVisualStyleBackColor = true;
            this.generate_pcu_read_visitor.Click += new System.EventHandler(this.generate_pcu_read_visitor_Click);
            // 
            // generate_pcu_write_visitor
            // 
            this.generate_pcu_write_visitor.Location = new System.Drawing.Point(10, 137);
            this.generate_pcu_write_visitor.Name = "generate_pcu_write_visitor";
            this.generate_pcu_write_visitor.Size = new System.Drawing.Size(182, 41);
            this.generate_pcu_write_visitor.TabIndex = 10;
            this.generate_pcu_write_visitor.Text = "Генерировать pcu-сериализатор";
            this.generate_pcu_write_visitor.UseVisualStyleBackColor = true;
            this.generate_pcu_write_visitor.Click += new System.EventHandler(this.generate_pcu_write_visitor_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.nodes_list);
            this.panel3.Controls.Add(this.add);
            this.panel3.Controls.Add(this.delete);
            this.panel3.Location = new System.Drawing.Point(10, 9);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(336, 406);
            this.panel3.TabIndex = 12;
            // 
            // file_name
            // 
            this.file_name.Location = new System.Drawing.Point(10, 28);
            this.file_name.Name = "file_name";
            this.file_name.Size = new System.Drawing.Size(201, 22);
            this.file_name.TabIndex = 13;
            // 
            // namespace_name
            // 
            this.namespace_name.Location = new System.Drawing.Point(10, 83);
            this.namespace_name.Name = "namespace_name";
            this.namespace_name.Size = new System.Drawing.Size(201, 22);
            this.namespace_name.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(120, 19);
            this.label1.TabIndex = 15;
            this.label1.Text = "Имя файла";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 18);
            this.label2.TabIndex = 16;
            this.label2.Text = "Пространство имен";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.tb_pcu_reader);
            this.panel4.Controls.Add(this.label6);
            this.panel4.Controls.Add(this.tb_pcu_writer);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.label4);
            this.panel4.Controls.Add(this.visitor_interface_file_name);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Controls.Add(this.factory_name);
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.namespace_name);
            this.panel4.Controls.Add(this.file_name);
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(355, 9);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(221, 357);
            this.panel4.TabIndex = 17;
            // 
            // tb_pcu_reader
            // 
            this.tb_pcu_reader.Location = new System.Drawing.Point(10, 313);
            this.tb_pcu_reader.Name = "tb_pcu_reader";
            this.tb_pcu_reader.Size = new System.Drawing.Size(201, 22);
            this.tb_pcu_reader.TabIndex = 25;
            this.tb_pcu_reader.Text = "SyntaxTreeStreamReader";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 293);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(192, 17);
            this.label6.TabIndex = 24;
            this.label6.Text = "Имя pcu - десериализатора";
            // 
            // tb_pcu_writer
            // 
            this.tb_pcu_writer.Location = new System.Drawing.Point(10, 253);
            this.tb_pcu_writer.Name = "tb_pcu_writer";
            this.tb_pcu_writer.Size = new System.Drawing.Size(201, 22);
            this.tb_pcu_writer.TabIndex = 23;
            this.tb_pcu_writer.Text = "SyntaxTreeStreamWriter";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 231);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(192, 18);
            this.label5.TabIndex = 22;
            this.label5.Text = "Имя pcu - сериализатора";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(192, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "Имя файла интеф. визитора";
            // 
            // visitor_interface_file_name
            // 
            this.visitor_interface_file_name.Location = new System.Drawing.Point(10, 138);
            this.visitor_interface_file_name.Name = "visitor_interface_file_name";
            this.visitor_interface_file_name.Size = new System.Drawing.Size(201, 22);
            this.visitor_interface_file_name.TabIndex = 19;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 175);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(124, 19);
            this.label3.TabIndex = 18;
            this.label3.Text = "Factory name";
            // 
            // factory_name
            // 
            this.factory_name.Location = new System.Drawing.Point(10, 194);
            this.factory_name.Name = "factory_name";
            this.factory_name.Size = new System.Drawing.Size(201, 22);
            this.factory_name.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.ClientSize = new System.Drawing.Size(978, 421);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.close);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PascalABC.NET: syntax tree generator  v1.5(с) Водолазов Н., Ткачук А.В., Иванов С" +
                ".О., 2007 ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private NodeGenerator.NodeGenerator node_gen=new NodeGenerator.NodeGenerator();

		private void button1_Click(object sender, System.EventArgs e)
		{
			NodeGenerator.NodeGenerator ng=new NodeGenerator.NodeGenerator();

			ng.factory_name="nodes_factory";
			ng.file_name="f1.cs";
			ng.namespace_name="fnm";
			ng.visitor_interface_file_name="vis.cs";

			node_info nb=new node_info();
			nb.node_name="base_node";
			node_info n1=new node_info();
			n1.node_name="if_node";
			n1.base_class=nb;
			node_info n2=new node_info();
			n2.node_name="condition";
			n2.base_class=nb;
			node_info n3=new node_info();
			n3.node_name="body";
			n3.base_class=n2;
			
			node_field_info nf11=new node_field_info();
			nf11.field_name="first";
			nf11.field_type=n2;
			n1.add_subnode(nf11);

			node_field_info nf12=new node_field_info();
			nf12.field_name="second";
			nf12.field_type=n3;
			n1.add_subnode(nf12);

			node_field_info nf21=new node_field_info();
			nf21.field_name="third";
			nf21.field_type=n1;
			n2.add_subnode(nf21);

			ng.add_node(nb);
			ng.add_node(n1);
			ng.add_node(n2);
			ng.add_node(n3);

			ng.generate_code();
			ng.generate_visitor_interface_code();
			ng.generate_visitor_template("vis1","vis_templ.cs");

		}

		private void close_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void set_nodes_information()
		{
			if (node_gen==null)
			{
				node_gen=new NodeGenerator.NodeGenerator();
			}

			nodes_list.Items.Clear();
			nodes_list.Items.AddRange(node_gen.all_nodes.ToArray());

			file_name.Text=node_gen.file_name;
			namespace_name.Text=node_gen.namespace_name;
			factory_name.Text=node_gen.factory_name;
			visitor_interface_file_name.Text=node_gen.visitor_interface_file_name;

		}

		private void get_nodes_information()
		{
			if (node_gen==null)
			{
				node_gen=new NodeGenerator.NodeGenerator();
			}

			node_gen.set_nodes(nodes_list.Items);

			node_gen.file_name=file_name.Text;
			node_gen.namespace_name=namespace_name.Text;
			node_gen.factory_name=factory_name.Text;
			node_gen.visitor_interface_file_name=visitor_interface_file_name.Text;
            node_gen.pcu_writer_file_name = tb_pcu_writer.Text + ".cs";
            node_gen.pcu_writer_name = tb_pcu_writer.Text;
            node_gen.pcu_reader_file_name = tb_pcu_reader.Text + ".cs";
            node_gen.pcu_reader_file_name_h = tb_pcu_reader.Text + ".h";
            node_gen.pcu_reader_file_name_cpp = tb_pcu_reader.Text + ".cpp";
            node_gen.pcu_reader_name = tb_pcu_reader.Text;
		}

		private void open_Click(object sender, System.EventArgs e)
		{
			if (openFileDialog1.ShowDialog()==DialogResult.OK)
			{
				saveFileDialog1.FileName=openFileDialog1.FileName;
				node_gen=NodeGenerator.NodeGenerator.deserialize(openFileDialog1.FileName);
				if (node_gen==null)
				{
					MessageBox.Show("Can not read nodes information from: "+openFileDialog1.FileName);
				}
				else
				{
					set_nodes_information();
				}
			}
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
		
		}

		private bool updated=false;
		private void update()
		{
			updated=true;
			get_nodes_information();
		}

		private void save_Click(object sender, System.EventArgs e)
		{
			update();
			if ((saveFileDialog1.FileName!=null)&&(saveFileDialog1.FileName!=""))
			{
				node_gen.serialize(saveFileDialog1.FileName);
			}
			else
			{
				save_as_Click(this,e);
			}
		}

		private void save_as_Click(object sender, System.EventArgs e)
		{
			update();
			if (saveFileDialog1.ShowDialog()==DialogResult.OK)
			{
				node_gen.serialize(saveFileDialog1.FileName);
			}
		}

		private void delete_item()
		{
			int t=nodes_list.SelectedIndex;
			if (t<0)
			{
				return;
			}
			nodes_list.Items.RemoveAt(t);
		}

		private void generate_code_Click(object sender, System.EventArgs e)
		{
			get_nodes_information();
			node_gen.generate_code();
			//node_gen.generate_nodes_fabric("tree_fabric","tree_fabric.cs");
		}

		private void generate_visitor_template_Click(object sender, System.EventArgs e)
		{
			get_nodes_information();
			template_form.generate(node_gen);
		}

		private void add_Click(object sender, System.EventArgs e)
		{
			node_info ni=node_def.show_modal(null,node_gen);
			if (ni!=null)
			{
				nodes_list.Items.Add(ni);
			}
			update();
		}

		private void nodes_list_DoubleClick(object sender, System.EventArgs e)
		{
			int t=nodes_list.SelectedIndex;
			if (t<0)
			{
				return;
			}
			node_def.show_modal((nodes_list.Items[t] as node_info),node_gen);
			update();
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			delete_item();
			update();
		}

        private void generate_pcu_write_visitor_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_pcu_writer_code(false);
        }

        private void generate_pcu_read_visitor_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_pcu_reader_code();
        }

        private void generate_cpp_code_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_cpp_code();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_pcu_writer_code(true);
        }

        private void generate_cross_platform_reader_cpp_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_stream_reader_cpp();
        }

        private void generate_visualizator_cpp_Click(object sender, EventArgs e)
        {
            get_nodes_information();
            node_gen.generate_vizualizator_cpp();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
	}
}
