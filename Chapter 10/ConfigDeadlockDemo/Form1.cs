namespace ConfigDeadlockDemo;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        this.label1.Text = GetData(); //Will deadlock the application
    }

    private async void button2_Click(object sender, EventArgs e)
    {
        try
        {
            this.label1.Text = await FetchDataAsync(); //Will update the label correctly
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public string GetData()
    {
        return FetchDataAsync().Result; // Blocks the thread and waits
    }

    public async Task<string> FetchDataAsync()
    {
        return await Task.Delay(1000).ContinueWith(_ => "Hello World");
    }
}
