
using Avalonia.Controls;
using Avalonia.Media;
using YomiLite.Rendering.Geometry;
using YomiLite.Rendering.Renderers;

namespace YomiLite.Rendering.Controls;

public class BattleSceneControl : Control
{
    private const double ArenaWidth = 20;
    private const double ArenaHeight = 10;
    private const double Padding = 24;

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        // Подготовка системы координат.
        var transform = new CoordinateTransform(
            Bounds.Size,
            ArenaWidth,
            ArenaHeight,
            Padding);

        // Отрисовка окружения.
        ArenaRenderer.Draw(
            context,
            in transform,
            Bounds.Size);

        if (!transform.IsValid)
            return;

        // Отрисовка первого бойца.
        FighterRenderer.Draw(
            context,
            in transform,
            x: -3,
            y: 0,
            facing: 1,
            isPlayerOne: true);

        // Отрисовка второго бойца.
        FighterRenderer.Draw(
            context,
            in transform,
            x: 3,
            y: 0,
            facing: -1,
            isPlayerOne: false);
    }
}
