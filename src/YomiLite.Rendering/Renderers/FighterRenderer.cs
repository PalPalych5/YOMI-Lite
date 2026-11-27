
using Avalonia;
using Avalonia.Media;
using YomiLite.Rendering.Geometry;

namespace YomiLite.Rendering.Renderers;

public static class FighterRenderer
{
    private const double HeadRadius = 0.2;
    private const double BoneThickness = 0.09;

    private static readonly IBrush Player1Brush =
        new SolidColorBrush(Color.Parse("#69B7FF"));

    private static readonly IBrush Player2Brush =
        new SolidColorBrush(Color.Parse("#FF7070"));

    // Описание базовой позы персонажа.
    // Координаты относительно точки опоры.

    private static readonly Point Head = new(0, 1.8);
    private static readonly Point Neck = new(0, 1.6);
    private static readonly Point Shoulder = new(0, 1.45);
    private static readonly Point Hip = new(0, 0.8);

    private static readonly Point FrontElbow = new(0.4, 1.2);
    private static readonly Point FrontHand = new(0.65, 1.45);

    private static readonly Point RearElbow = new(-0.3, 1.15);
    private static readonly Point RearHand = new(0.15, 1.25);

    private static readonly Point FrontFoot = new(0.35, 0);
    private static readonly Point RearFoot = new(-0.35, 0);

    public static void Draw(
        DrawingContext context,
        in CoordinateTransform transform,
        double x,
        double y,
        int facing,
        bool isPlayerOne)
    {
        if (!transform.IsValid)
            return;

        // Нормализация направления.
        facing = facing < 0 ? -1 : 1;

        IBrush brush = isPlayerOne
            ? Player1Brush
            : Player2Brush;

        var pen = new Pen(
            brush,
            transform.ToScreenLength(BoneThickness));

        Point origin = transform.ToScreen(x, y);
        double scale = transform.Scale;

        // Голова.
        context.DrawEllipse(
            brush,
            null,
            TransformJoint(Head, origin, scale, facing),
            HeadRadius * scale,
            HeadRadius * scale);

        // Туловище.
        DrawBone(context, pen, Neck, Hip,
            origin, scale, facing);

        // Ноги.
        DrawBone(context, pen, Hip, FrontFoot,
            origin, scale, facing);

        DrawBone(context, pen, Hip, RearFoot,
            origin, scale, facing);

        // Передняя рука.
        DrawBone(context, pen, Shoulder, FrontElbow,
            origin, scale, facing);

        DrawBone(context, pen, FrontElbow, FrontHand,
            origin, scale, facing);

        // Задняя рука.
        DrawBone(context, pen, Shoulder, RearElbow,
            origin, scale, facing);

        DrawBone(context, pen, RearElbow, RearHand,
            origin, scale, facing);
    }

    private static Point TransformJoint(
        Point joint,
        Point origin,
        double scale,
        int facing)
    {
        return new Point(
            origin.X + joint.X * scale * facing,
            origin.Y - joint.Y * scale);
    }

    private static void DrawBone(
        DrawingContext context,
        Pen pen,
        Point start,
        Point end,
        Point origin,
        double scale,
        int facing)
    {
        context.DrawLine(
            pen,
            TransformJoint(start, origin, scale, facing),
            TransformJoint(end, origin, scale, facing));
    }
}
