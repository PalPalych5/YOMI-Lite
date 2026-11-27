
using Avalonia;

namespace YomiLite.Rendering.Geometry;

public readonly struct CoordinateTransform
{
    public double ArenaWidth { get; }
    public double ArenaHeight { get; }

    public double Scale { get; }
    public double Left { get; }
    public double Top { get; }

    public double Right => Left + ArenaWidth * Scale;
    public double GroundY => Top + ArenaHeight * Scale;

    public bool IsValid => Scale > 0;

    public Rect ArenaBounds => new(
        Left,
        Top,
        ArenaWidth * Scale,
        ArenaHeight * Scale);

    public CoordinateTransform(
        Size viewport,
        double arenaWidth,
        double arenaHeight,
        double padding = 24)
    {
        ArenaWidth = arenaWidth;
        ArenaHeight = arenaHeight;

        Scale = 0;
        Left = 0;
        Top = 0;

        double availableWidth = viewport.Width - 2 * padding;
        double availableHeight = viewport.Height - 2 * padding;

        if (availableWidth <= 0 ||
            availableHeight <= 0 ||
            arenaWidth <= 0 ||
            arenaHeight <= 0)
        {
            return;
        }

        Scale = Math.Min(
            availableWidth / arenaWidth,
            availableHeight / arenaHeight);

        Left = (viewport.Width - arenaWidth * Scale) / 2;
        Top = (viewport.Height - arenaHeight * Scale) / 2;
    }

    // Игровые координаты -> экранные координаты.
    public Point ToScreen(double x, double y)
    {
        return new Point(
            Left + (x + ArenaWidth / 2) * Scale,
            GroundY - y * Scale);
    }

    // Экранные координаты -> игровые координаты.
    public Point ToWorld(Point point)
    {
        if (!IsValid)
            throw new InvalidOperationException(
                "Coordinate transform is not valid.");

        return new Point(
            (point.X - Left) / Scale - ArenaWidth / 2,
            (GroundY - point.Y) / Scale);
    }

    // Преобразование длины из игровых единиц.
    public double ToScreenLength(double length)
    {
        return length * Scale;
    }
}
