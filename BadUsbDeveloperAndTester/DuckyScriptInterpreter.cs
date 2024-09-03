using WindowsInput.Native;
using WindowsInput;

public class DuckyScriptInterpreter
{
    private readonly InputSimulator inputSimulator;

    public DuckyScriptInterpreter()
    {
        inputSimulator = new InputSimulator();
    }

    public async Task ExecuteLineAsync(string line)
    {
        line = line.Trim();
        if (line.StartsWith("REM") || string.IsNullOrWhiteSpace(line))
        {
            // Пропуск комментариев и пустых строк
            return;
        }

        string[] parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToUpper();
        string argument = parts.Length > 1 ? parts[1].Trim() : string.Empty;

        switch (command)
        {
            case "STRING":
                SendKeys.SendWait(argument);
                break;
            case "DELAY":
                if (int.TryParse(argument, out int delay))
                {
                    await Task.Delay(delay);
                }
                break;
            case "GUI":
                if (string.IsNullOrEmpty(argument))
                {
                    // Если аргумент отсутствует, просто нажимаем Win
                    inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LWIN);
                }
                else
                {
                    // Комбинация Win + аргумент
                    switch (argument.ToUpper())
                    {
                        case "R":
                            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R);
                            break;
                        // Добавьте другие комбинации по необходимости
                        default:
                            throw new InvalidOperationException($"Unknown GUI argument: {argument}");
                    }
                }
                break;
            case "ENTER":
                SendKeys.SendWait("{ENTER}");
                break;
            case "TAB":
                SendKeys.SendWait("{TAB}");
                break;
            case "DOWN":
                SendKeys.SendWait("{DOWN}");
                break;
            case "UP":
                SendKeys.SendWait("{UP}");
                break;
            case "LEFT":
                SendKeys.SendWait("{LEFT}");
                break;
            case "RIGHT":
                SendKeys.SendWait("{RIGHT}");
                break;
            case "ESC":
                SendKeys.SendWait("{ESC}");
                break;
            case "BACKSPACE":
                SendKeys.SendWait("{BACKSPACE}");
                break;
            case "DELETE":
                SendKeys.SendWait("{DELETE}");
                break;
            case "F1":
            case "F2":
            case "F3":
            case "F4":
            case "F5":
            case "F6":
            case "F7":
            case "F8":
            case "F9":
            case "F10":
            case "F11":
            case "F12":
                SendKeys.SendWait("{" + command + "}");
                break;
            case "CTRL":
            case "CONTROL":
                if (!string.IsNullOrEmpty(argument))
                {
                    SendKeys.SendWait("^{" + argument + "}");
                }
                break;
            case "ALT":
                if (!string.IsNullOrEmpty(argument))
                {
                    SendKeys.SendWait("%{" + argument + "}");
                }
                break;
            case "SHIFT":
                if (!string.IsNullOrEmpty(argument))
                {
                    SendKeys.SendWait("+{" + argument + "}");
                }
                break;
            default:
                throw new InvalidOperationException($"Unknown command: {command}");
        }
    }
}
