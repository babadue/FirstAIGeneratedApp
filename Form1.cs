namespace winform3;

// public partial class Form1 : Form
// {
//     public Form1()
//     {
//         InitializeComponent();
//     }
// }

// dotnet add package System.Speech --version 8.0.0-rc.1.23419.4

using System;
using System.Speech.Recognition;
using System.Windows.Forms;
using System.Runtime.InteropServices;


public partial class Form1 : Form
{
    private Label label1; // Declare a Label control

    private SpeechRecognitionEngine recognizer;

    // Import the SendInput function from user32.dll
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);


    public Form1()
    {
        InitializeComponent();
        InitializeSpeechRecognition();
        InitializeUI(); // Call a method to initialize the UI
        MoveMouseAtOnce();
    }

    // private void moveMouseButton_Click(object sender, EventArgs e)
    // {
    //     // Define the X and Y coordinates where you want to move the mouse pointer
    //     int targetX = 900; // Replace with your desired X coordinate
    //     int targetY = 900; // Replace with your desired Y coordinate

    //     // Move the mouse pointer to the specified location
    //     Cursor.Position = new System.Drawing.Point(targetX, targetY);
    // }

    private static void MoveMouseAtOnce()
    {
        // Define the X and Y coordinates where you want to move the mouse pointer
        // int targetX = 900; // Replace with your desired X coordinate
        // int targetY = 800; // Replace with your desired Y coordinate

        // Get the left-most screen's bounds 
        // Not left-most but left monitor of a 3 monitors setup
        Screen leftMostScreen = Screen.AllScreens[2];
        Rectangle screenBounds = leftMostScreen.Bounds;

        // Calculate the center of the left-most screen
        // int targetX = screenBounds.Left + (screenBounds.Width / 2);

        // Calculate the X coordinate for the left edge of the left-most screen
        // int targetX = screenBounds.Left;
        // int targetY = screenBounds.Top + (screenBounds.Height / 2);

        // Specify the X and Y coordinates on the left monitor where you want the cursor to move
        int targetX = screenBounds.Left + 2350;  // Adjust the X coordinate as needed
        int targetY = screenBounds.Top + 1210;   // Adjust the Y coordinate as needed

        // Move the mouse pointer to the specified location
        Cursor.Position = new System.Drawing.Point(targetX, targetY);
    }
    private void InitializeUI()
    {
        label1 = new Label(); // Create a new Label control
        label1.Text = "Hello, Windows Forms!"; // Set the label's text
        label1.Location = new System.Drawing.Point(50, 50); // Set the label's position
        label1.AutoSize = true; // Make the label size automatically fit its content

        // Add the label to the form's controls
        this.Controls.Add(label1);
    }


    private void InitializeSpeechRecognition()
    {
        try
        {
            recognizer = new SpeechRecognitionEngine();
            recognizer.LoadGrammar(new DictationGrammar());
            recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(Recognizer_SpeechRecognized);
            recognizer.SetInputToDefaultAudioDevice();
            recognizer.RecognizeAsync(RecognizeMode.Multiple);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error initializing speech recognition: " + ex.Message);
        }
    }

    private void Recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        string command = e.Result.Text.ToLower();


        // Display the recognized command in the label
        label1.Text = "Command Received: " + command;
        SimulateMouseClick();

        // if (command.Contains("click"))
        // {
        //     SimulateMouseClick();
        // }
    }

    private void SimulateMouseClick()
    {
        // Simulate a left mouse click here (you may need to use P/Invoke or an external library for this)
        // Example of using SendInput:
        INPUT mouseInput = new INPUT();
        mouseInput.type = INPUT_MOUSE;
        mouseInput.mi.dwFlags = MOUSEEVENTF_LEFTDOWN;
        SendInput(1, new INPUT[] { mouseInput }, Marshal.SizeOf(typeof(INPUT)));
        mouseInput.mi.dwFlags = MOUSEEVENTF_LEFTUP;
        SendInput(1, new INPUT[] { mouseInput }, Marshal.SizeOf(typeof(INPUT)));
    }

    // Constants and structures needed for SendInput function (for mouse input simulation)
    private const int INPUT_MOUSE = 0;
    private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
    private const int MOUSEEVENTF_LEFTUP = 0x0004;

    [StructLayout(LayoutKind.Sequential)]
    public struct INPUT
    {
        public int type;
        public MOUSEINPUT mi;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MOUSEINPUT
    {
        public int dx;
        public int dy;
        public int mouseData;
        public int dwFlags;
        public int time;
        public IntPtr dwExtraInfo;
    }
}


