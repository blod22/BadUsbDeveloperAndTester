using WindowsInput.Native;
using WindowsInput;

public class BadUsbInterpreter
{
    private readonly InputSimulator inputSimulator;
    private int defaultDelay = 0; // Default value for delay
    private string lastCommand; // The last executed line

    public BadUsbInterpreter()
    {
        inputSimulator = new InputSimulator();
    }

    public async Task ExecuteLineAsync(string line)
    {
        line = line.Trim();
        if (line.StartsWith("REM") || string.IsNullOrWhiteSpace(line))
        {
            // Skip comments and empty lines
            return;
        }

        string[] parts = line.Split(new[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);
        string command = parts[0].ToUpper();
        string argument = parts.Length > 1 ? parts[1].Trim() : string.Empty;

        if (command != "REPEAT")
        {
            lastCommand = line; // Save the current command as the last executed, but only if it's not REPEAT
        }

        switch (command)
        {
            case "DEFAULT_DELAY":
                if (int.TryParse(argument, out int delay))
                {
                    defaultDelay = delay;
                }
                else
                {
                    throw new InvalidOperationException("Invalid delay value.");
                }
                break;
            case "STRING":
                await ApplyDelay();
                SendKeys.SendWait(argument);
                break;
            case "DELAY":
                if (int.TryParse(argument, out int specificDelay))
                {
                    await Task.Delay(specificDelay);
                }
                else
                {
                    throw new InvalidOperationException("Invalid delay value.");
                }
                break;
            case "GUI":
                await ApplyDelay();
                ExecuteGuiCommand(argument);
                break;
            case "CTRL":
                await ApplyDelay();
                ExecuteModifiedKeyCommand(VirtualKeyCode.CONTROL, argument);
                break;
            case "SHIFT":
                await ApplyDelay();
                ExecuteModifiedKeyCommand(VirtualKeyCode.SHIFT, argument);
                break;
            case "ALT":
                await ApplyDelay();
                ExecuteModifiedKeyCommand(VirtualKeyCode.MENU, argument);
                break;
            case "ALTCHAR":
                await ApplyDelay();
                // ExecuteAltCharCommand(argument);
                break;
            case "ALTSTRING":
            case "ALTCODE":
                await ApplyDelay();
                // ExecuteAltStringCommand(argument);
                break;
            case "REPEAT":
                if (int.TryParse(argument, out int repeatCount))
                {
                    if (!string.IsNullOrEmpty(lastCommand) && !lastCommand.StartsWith("REPEAT"))
                    {
                        for (int i = 0; i < repeatCount; i++)
                        {
                            await ExecuteLineAsync(lastCommand);
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException("No previous command to repeat or last command is REPEAT.");
                    }
                }
                else
                {
                    throw new InvalidOperationException("Invalid repeat count.");
                }
                break;
            default:
                await ApplyDelay();
                ExecuteKeyCommand(command, argument);
                break;
        }
    }


    private async Task ApplyDelay()
    {
        if (defaultDelay > 0)
        {
            await Task.Delay(defaultDelay);
        }
    }

    private void ExecuteGuiCommand(string argument)
    {
        if (string.IsNullOrEmpty(argument))
        {
            inputSimulator.Keyboard.KeyPress(VirtualKeyCode.LWIN);
        }
        else
        {
            VirtualKeyCode keyCode;
            try
            {
                keyCode = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "VK_" + argument.ToUpper());
            }
            catch
            {
                throw new InvalidOperationException($"Unknown GUI argument: {argument}");
            }

            inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, keyCode);
        }
    }


    private void ExecuteModifiedKeyCommand(VirtualKeyCode modifier, string argument)
    {
        if (!string.IsNullOrEmpty(argument))
        {
            VirtualKeyCode key;

            // Check if the argument is a special key (DOWN, UP, etc.)
            switch (argument.ToUpper())
            {
                case "DOWN":
                    key = VirtualKeyCode.DOWN;
                    break;
                case "UP":
                    key = VirtualKeyCode.UP;
                    break;
                case "LEFT":
                    key = VirtualKeyCode.LEFT;
                    break;
                case "RIGHT":
                    key = VirtualKeyCode.RIGHT;
                    break;
                case "HOME":
                    key = VirtualKeyCode.HOME;
                    break;
                case "END":
                    key = VirtualKeyCode.END;
                    break;
                case "PGUP":
                case "PAGEUP":
                    key = VirtualKeyCode.PRIOR; // Page Up
                    break;
                case "PGDN":
                case "PAGEDOWN":
                    key = VirtualKeyCode.NEXT; // Page Down
                    break;
                case "INSERT":
                    key = VirtualKeyCode.INSERT;
                    break;
                case "DELETE":
                    key = VirtualKeyCode.DELETE;
                    break;
                case "ESC":
                    key = VirtualKeyCode.ESCAPE;
                    break;
                case "BACKSPACE":
                    key = VirtualKeyCode.BACK;
                    break;
                case "TAB":
                    key = VirtualKeyCode.TAB;
                    break;
                case "ENTER":
                    key = VirtualKeyCode.RETURN;
                    break;
                default:
                    // If not a special key, try to interpret it as a character
                    key = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "VK_" + argument.ToUpper());
                    break;
            }

            inputSimulator.Keyboard.ModifiedKeyStroke(modifier, key);
        }
    }


    private void ExecuteShiftCommand(string argument)
    {
        if (!string.IsNullOrEmpty(argument))
        {
            foreach (char c in argument)
            {
                VirtualKeyCode key = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "VK_" + c.ToString().ToUpper());
                inputSimulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, key);
            }
        }
    }

    private async Task ExecuteAltCharCommand(string argument)
    {
        if (int.TryParse(argument, out int code) && code >= 0 && code <= 9999)
        {
            foreach (char digit in code.ToString())
            {
                // Press and hold the Alt key
                inputSimulator.Keyboard.KeyDown(VirtualKeyCode.MENU);
                await Task.Delay(10); // Minimum delay for stability

                // Enter the digit on the numeric keypad
                VirtualKeyCode numpadKey = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), "NUMPAD" + digit);
                inputSimulator.Keyboard.KeyPress(numpadKey);
                await Task.Delay(10); // Minimum delay for stability

                // Release the Alt key
                inputSimulator.Keyboard.KeyUp(VirtualKeyCode.MENU);
                await Task.Delay(10); // Minimum delay before the next iteration
            }
        }
        else
        {
            throw new InvalidOperationException("Invalid ALTCHAR code.");
        }
    }

    private async Task ExecuteAltStringCommand(string argument)
    {
        foreach (char c in argument)
        {
            int code = (int)c;
            await ExecuteAltCharCommand(code.ToString());
            await Task.Delay(10); // Minimum delay before moving to the next character
        }
    }

    private void ExecuteKeyCommand(string command, string argument)
    {
        switch (command)
        {
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
            case "HOME":
                SendKeys.SendWait("{HOME}");
                break;
            case "END":
                SendKeys.SendWait("{END}");
                break;
            case "INSERT":
                SendKeys.SendWait("{INSERT}");
                break;
            case "PGUP":
            case "PAGEUP":
                SendKeys.SendWait("{PGUP}");
                break;
            case "PGDN":
            case "PAGEDOWN":
                SendKeys.SendWait("{PGDN}");
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
            default:
                throw new InvalidOperationException($"Unknown command: {command}");
        }
    }
}
