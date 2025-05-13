namespace ConfigDeadlockDemo;

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.button1 = new Button();
        this.label1 = new Label();
        this.button2 = new Button();
        SuspendLayout();
        // 
        // button1
        // 
        this.button1.Location = new Point(66, 31);
        this.button1.Name = "button1";
        this.button1.Size = new Size(98, 23);
        this.button1.TabIndex = 0;
        this.button1.Text = "Click To Lock";
        this.button1.UseVisualStyleBackColor = true;
        this.button1.Click += button1_Click;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new Point(206, 53);
        this.label1.Name = "label1";
        this.label1.Size = new Size(38, 15);
        this.label1.TabIndex = 1;
        this.label1.Text = "label1";
        // 
        // button2
        // 
        this.button2.Location = new Point(66, 60);
        this.button2.Name = "button2";
        this.button2.Size = new Size(98, 23);
        this.button2.TabIndex = 2;
        this.button2.Text = "Click To Update";
        this.button2.UseVisualStyleBackColor = true;
        this.button2.Click += button2_Click;
        // 
        // Form1
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(350, 149);
        Controls.Add(this.button2);
        Controls.Add(this.label1);
        Controls.Add(this.button1);
        Name = "Form1";
        Text = "Form1";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button button1;
    private Label label1;
    private Button button2;
}
