using Microsoft.VisualBasic.FileIO;

namespace DrawingProgram;

public abstract class PopupWindow : IDrawable
{
    protected Rectangle windowRect;
    public string[] messages;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, Color.DarkGray);
        TextHandling.DrawCenteredText(messages, (int)windowRect.Y + 20, 30, 60, Color.Black);
    }

    public virtual void Logic(Canvas canvas, Vector2 mousePos)
    {

    }

    public PopupWindow(int width, int height, string[] messagesExtern, Canvas canvas)
    {
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
    }
}

public class SavePopup : PopupWindow
{
    public Dictionary<KeyboardKey, string> alphabet;
    public string fileName = "";

    public SavePopup(int width, int height, string[] messagesExtern, Canvas canvas) : base(width, height, messagesExtern, canvas)
    {
        alphabet = new();
        for (int i = (int)KeyboardKey.A; i <= (int)KeyboardKey.Z; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)(i + 32)).ToString());
        }

        // Add numbers 0-9
        for (int i = 48; i <= 57; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)i).ToString());
        }
    }

    public void SaveCanvas(Canvas canvas)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) && fileName != "")
        {
            canvas.SaveProject(fileName + ".png");
        }
    }

    public string UpdateFileName()
    {
        KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();
        if (keyPressed != KeyboardKey.Null && alphabet.ContainsKey(keyPressed) && fileName.Length <= 26)
        {
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift))
                fileName += alphabet[keyPressed].ToUpper();

            else
                fileName += alphabet[keyPressed];
        }

        if (keyPressed == KeyboardKey.Backspace && fileName.Length != 0)
            fileName = fileName[..^1]; //Tar bort sista bokstaven/siffran i stringen

        return fileName;
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)windowRect.X + 10, (int)(windowRect.Y + windowRect.Height) - 80, (int)windowRect.Width - 20, 50, Color.LightGray);
        Raylib.DrawText(fileName, (int)windowRect.X + 20, (int)(windowRect.Y + windowRect.Height) - 70, 30, Color.Black);
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        fileName = UpdateFileName();
        SaveCanvas(canvas);
    }
}

public class ColorSelector : PopupWindow
{
    private Texture2D colors;
    private Image colorsImg;
    private Rectangle colorsRect;
    public ColorSelector(int width, int height, string[] messagesExtern, Canvas canvas) : base(width, height, messagesExtern, canvas)
    {
        colors = Raylib.LoadTexture("Icons/colors.png");
        colorsImg = Raylib.LoadImageFromTexture(colors);
        colorsRect = new(ProgramManager.ScreenWidth/2 - colors.Width/2, 300, colors.Width, colors.Height);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawTexture(colors, (int)colorsRect.X, (int)colorsRect.Y, Color.White);
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        if (Raylib.CheckCollisionPointRec(mousePos, colorsRect) && Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawTool.drawingColor = Raylib.GetImageColor(colorsImg, (int)mousePos.X - (int)colorsRect.X, (int)mousePos.Y - (int)colorsRect.Y);
        }
    }
}