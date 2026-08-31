using System.Windows;
using System.Windows.Input;

namespace LuaSTGEditorSharp.Windows.Input
{
    /// <summary>
    /// CodeInput.xaml 的交互逻辑
    /// </summary>
    public partial class CodeInput : InputWindow
    {
        private bool isCtrlDown = false;

        public CodeInput(string s)
        {
            InitializeComponent();
            codeText.Text = s;

            IAppSettings settings = (IAppSettings)Application.Current;
            codeText.Options.ConvertTabsToSpaces = settings.SpaceIndentation;
            codeText.Options.IndentationSize = settings.IndentationSpaceLength;
        }

        private void ButtonOK_Click(object sender, RoutedEventArgs e)
        {
            Confirm();
        }

        private void Confirm()
        {
            DialogResult = true;
            Result = codeText.Text;
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Result = codeText.Text;
            this.Close();
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(codeText);
        }

        private void CodeText_KeyDown(object sender, KeyEventArgs e)
        {
            if (isCtrlDown)
            {
                if (e.Key == Key.Enter)
                {
                    e.Handled = true;
                    Confirm();
                }
            }
            else
            {
                if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) isCtrlDown = true;
            }
        }

        private void CodeText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl) isCtrlDown = false;
        }
    }
}
