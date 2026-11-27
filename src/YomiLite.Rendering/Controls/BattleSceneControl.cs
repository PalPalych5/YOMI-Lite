
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace YomiLite.Rendering.Controls;

public class BattleSceneControl : Control
{
    // Временные размеры игрового пространства.
    private const double ArenaWidth = 20;
    private const double ArenaHeight = 10;
    private const double Padding = 24;

    private static readonly IBrush Background =
        new SolidColorBrush(Color.Parse("#151A22"));

    private static readonly IBrush ArenaBackground =
        new SolidColorBrush(Color.Parse("#1C2530"));

    private static readonly IBrush Player1Brush =
        new SolidColorBrush(Color.Parse("#69B7FF"));

    private static readonly IBrush Player2Brush =
        new SolidColorBrush(Color.Parse("#FF7070"));

    private static readonly Pen GridPen =
        new(Brushes.SlateGray, 0.5);

    private static readonly Pen BorderPen =
        new(Brushes.SlateGray, 1);

    private static readonly Pen GroundPen =
        new(Brushes.LightGray, 2);

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        context.FillRectangle(
            Background, new Rect(Bounds.Size));

        double availableWidth = Bounds.Width - Padding * 2;
        double availableHeight = Bounds.Height - Padding * 2;

        if (availableWidth <= 0 || availableHeight <= 0)
            return;

        double scale = Math.Min(
            availableWidth / ArenaWidth,
            availableHeight / ArenaHeight);

        double left = (Bounds.Width - ArenaWidth * scale) / 2;
        double top = (Bounds.Height - ArenaHeight * scale) / 2;

        double groundY = top + ArenaHeight * scale;

        // Преобразование игровых координат в экранные.
        Point ToScreen(double x, double y)
        {
            return new Point(
                left + (x + ArenaWidth / 2) * scale,
                groundY - y * scale);
        }

        // Фон и границы арены.
        context.DrawRectangle(
            ArenaBackground,
            BorderPen,
            new Rect(
                left,
                top,
                ArenaWidth * scale,
                ArenaHeight * scale));

        // Вертикальные линии сетки.
        for (int x = -10; x <= 10; x++)
        {
            context.DrawLine(
                GridPen,
                ToScreen(x, 0),
                ToScreen(x, ArenaHeight));
        }

        // Горизонтальные линии сетки.
        for (int y = 0; y <= 10; y++)
        {
            context.DrawLine(
                GridPen,
                ToScreen(-ArenaWidth / 2, y),
                ToScreen(ArenaWidth / 2, y));
        }

        // Поверхность земли.
        context.DrawLine(
            GroundPen,
            ToScreen(-ArenaWidth / 2, 0),
            ToScreen(ArenaWidth / 2, 0));

        // Демонстрационные позиции бойцов.
        DrawFighter(
            context, ToScreen(-3, 0),
            scale, Player1Brush, 1);

        DrawFighter(
            context, ToScreen(3, 0),
            scale, Player2Brush, -1);
    }

    private static void DrawFighter(
        DrawingContext context,
        Point position,
        double scale,
        IBrush brush,
        int facing)
    {
        double x = position.X;
        double y = position.Y;

        Point P(double dx, double dy)
        {
            return new Point(
                x + dx * scale * facing,
                y - dy * scale);
        }

        var pen = new Pen(brush, 0.09 * scale);

        // Голова.
        context.DrawEllipse(
            brush, null,
            P(0, 1.8),
            0.2 * scale,
            0.2 * scale);

        // Туловище.
        context.DrawLine(pen, P(0, 1.6), P(0, 0.8));

        // Левая нога.
        context.DrawLine(pen, P(0, 0.8), P(-0.35, 0));

        // Правая нога.
        context.DrawLine(pen, P(0, 0.8), P(0.35, 0));

        // Передняя рука.
        context.DrawLine(pen, P(0, 1.45), P(0.4, 1.2));
        context.DrawLine(pen, P(0.4, 1.2), P(0.65, 1.45));

        // Задняя рука.
        context.DrawLine(pen, P(0, 1.45), P(-0.3, 1.15));
        context.DrawLine(pen, P(-0.3, 1.15), P(0.15, 1.25));
    }
}
