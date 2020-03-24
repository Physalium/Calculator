using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string Error = "Error";
        private bool isOperatorNext = false;
        private bool isErrorDisplayed = false;

        public MainWindow()
        {
            InitializeComponent();
        }



        private void numberButton_Click(object sender, RoutedEventArgs e)
        {
            if (isErrorDisplayed)
            {
                displayBox.Text = "";
                isErrorDisplayed = false;
            }
            var source = sender as Button;
            string ch = source.Content.ToString();
            if (ch == "÷") ch = "/";
            if (ch == "×") ch = "*";
            if (isNextKeyValid(ch))
            {
                displayBox.AppendText(ch);
                displayBox.CaretIndex += 1;
            }
            displayBox.Focus();
        }

        private bool isNextKeyValid(string key)
        {
            string[] operators = { "+", "-", "*", "/" };
            if (operators.Contains(key))
            {
                if (displayBox.Text == "" && key != "-")
                {
                    isOperatorNext = false;
                    return false;
                }
                if (operators.Any(x => displayBox.Text.EndsWith(x)))
                {
                    isOperatorNext = false;
                    return false;
                }
            }
            return true;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            displayBox.Focus();
        }

        private void displayBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new Regex("[^0-9+*/./-]+");
            e.Handled = (regex.IsMatch(e.Text) && isNextKeyValid(e.Text));

        }

        private void eraseButton_Click(object sender, RoutedEventArgs e)
        {
            if (isErrorDisplayed)
            {
                displayBox.Text = "";
                isErrorDisplayed = false;
            }
            var source = sender as Button;
            displayBox.Text = "";
            displayBox.Focus();
        }

        private void evaluateButton_Click(object sender, RoutedEventArgs e)
        {


            DataTable dt = new DataTable();
            var result = 0.0;

            try
            {
                if (double.TryParse(dt.Compute(displayBox.Text, "").ToString(), out result))
                {
                    if (double.IsInfinity(result)) { 
                        displayBox.Text = Error;
                        result = 0.0; 
                        isErrorDisplayed = true; }
                    displayBox.Text = result.ToString().Replace(",", ".");

                    displayBox.CaretIndex += displayBox.Text.Length;
                }
                else
                {
                    isErrorDisplayed = true;
                    displayBox.Text = Error;
                }
            }
            catch (System.Exception)
            {
                isErrorDisplayed = true;
                displayBox.Text = Error;
                result = 0.0;
            }

            displayBox.Focus();
        }

        private void backspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (isErrorDisplayed)
            {
                displayBox.Text = "";
                isErrorDisplayed = false;
            }
            var source = sender as Button;
            displayBox.Text += "\b";
            displayBox.Focus();
        }

        private void displayBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Paste)
            {

                e.Handled = true;
            }
        }

        private void displayBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (isErrorDisplayed)
            {
                displayBox.Text = "";
                isErrorDisplayed = false;
            }
            if (e.Key == Key.Enter)
            {

                evaluateButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));

            }
            if (e.Key == Key.Delete)
            {
                displayBox.Text = "";
            }
        }


    }
}
