namespace DrawingProgram;

public abstract class ToolFolder
{
    public List<DrawTool> drawTools = new();  
}

public class Drawing : ToolFolder
{
    public Drawing()
    {
        drawTools.Add(new Pencil());
        drawTools.Add(new PaintBrush());
        drawTools.Add(new Checker());
        drawTools.Add(new Eraser());
    }
}