
using Avalonia;
using Avalonia.Media;
using YomiLite.Rendering.Geometry;

namespace YomiLite.Rendering.Renderers;

public static class ArenaRenderer
{
    private static readonly IBrush Background =
        new SolidColorBrush(Color.Parse("#151A22"));

    private static readonly IBrush ArenaBackground =
        new SolidColorBrush(Color.Parse("#1C2530"));

    private static readonly Pen GridPen =
        new(Brushes.SlateGray, 0.5);

    private static readonly Pen BorderPen =
        new(Brushes.SlateGray, 1);

    private static readonly Pen GroundPen =
        new(Brushes.LightGray, 2);

    public static void Draw(
        DrawingContext context,
        in CoordinateTransform transform,
        Size viewport,
        bool showGrid = true)
    {
        context.FillRectangle(
            Background, new Rect(viewport));

        if (!transform.IsValid)
            return;

        // Фон и границы арены.
        context.DrawRectangle(
            ArenaBackground,
            BorderPen,
            transform.ArenaBounds);

        if (showGrid)
            DrawGrid(context, in transform);

        DrawGround(context, in transform);
    }

    private static void DrawGrid(
        DrawingContext context,
        in CoordinateTransform transform)
    {
        double halfWidth = transform.ArenaWidth / 2;

        int minX = (int)Math.Ceiling(-halfWidth);
        int maxX = (int)Math.Floor(halfWidth);

        // Вертикальные линии.
        for (int x = minX; x <= maxX; x++)
        {
            context.DrawLine(
                GridPen,
                transform.ToScreen(x, 0),
                transform.ToScreen(x, transform.ArenaHeight));
        }

        // Горизонтальные линии.
        for (int y = 0; y <= transform.ArenaHeight; y++)
        {
            context.DrawLine(
                GridPen,
                transform.ToScreen(-halfWidth, y),
                transform.ToScreen(halfWidth, y));
        }
    }

    private static void DrawGround(
        DrawingContext context,
        in CoordinateTransform transform)
    {
        double halfWidth = transform.ArenaWidth / 2;

        context.DrawLine(
            GroundPen,
            transform.ToScreen(-halfWidth, 0),
            transform.ToScreen(halfWidth, 0));
    }
}
