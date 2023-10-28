namespace RadialTabs;

using UnityEngine;
using uGUI_CraftNode = uGUI_CraftingMenu.Node;

internal sealed class RadialCell
{
    private static readonly RadialCell InvalidCell = new(0, 0, 0, 0, 0, null);

    public Vector2 Position => new(radius * Mathf.Cos(angle), radius * Mathf.Sin(angle));
    public readonly float radius;
    public readonly float size;
    public readonly float angle;
    public readonly float groupAngle;
    public readonly int siblings;
    public readonly RadialCell parent;

    private RadialCell(float radius, float angle, float size, float groupAngle, int siblings, RadialCell parent)
    {
        this.radius = radius;
        this.size = size;
        this.angle = angle;
        this.parent = parent;
        this.groupAngle = groupAngle;
        this.siblings = siblings;
    }

    public static RadialCell Create(uGUI_CraftNode node)
    {
        if (node.parent == null) { return InvalidCell; }

        var parentCell = Create(node.parent as uGUI_CraftNode);
        return parentCell == InvalidCell
            ? CreateRootCell(node)
            : parentCell.radius == 0 ? CreateChildCellWithOneElementAtRoot(node, parentCell) : CreateChildCell(node, parentCell);
    }

    private static RadialCell CreateRootCell(uGUI_CraftNode node)
    {
        int childCount = node.parent.childCount;
        float lineSize = (float)Config.RootIconSize;
        if(childCount <= 1)
        {
            return new RadialCell(0f, 0f, lineSize, float.NaN, childCount, InvalidCell);
        }
        int num = node.parent.IndexOf(node);
        float polygonRadius = GetPolygonRadius(lineSize, (float)childCount);
        if(childCount > Config.MaxRootIconCount)
        {
            polygonRadius = GetPolygonRadius(lineSize, (float)Config.MaxRootIconCount);
            lineSize = GetPolygonLineSize(polygonRadius, (float)childCount);
        }
        float num2 = (6.2831855f / (float)childCount * (float)num) + GetExtraAngleOffset(childCount);
        return new RadialCell(polygonRadius, num2, lineSize, float.NaN, childCount, InvalidCell);
    }

    private static RadialCell CreateChildCellWithOneElementAtRoot(uGUI_CraftNode node, RadialCell parent)
    {
        int childCount = node.parent.childCount;
        float lineSize = ComputeNewSize(parent);
        float num = ComputeNewRadius(parent, lineSize);
        int num2 = node.parent.IndexOf(node);
        float polygonLineCount = GetPolygonLineCount(num, lineSize);
        if((float)childCount > polygonLineCount)
        {
            lineSize = GetPolygonLineSize(num, (float)childCount);
        }
        float num3 = (6.2831855f / (float)childCount * (float)num2) + GetExtraAngleOffset(childCount);
        return new RadialCell(num, num3, lineSize, float.NaN, childCount, parent);
    }

    private static RadialCell CreateChildCell(uGUI_CraftNode node, RadialCell parent)
    {
        int childCount = node.parent.childCount;
        float lineSize = ComputeNewSize(parent);
        float num = ComputeNewRadius(parent, lineSize);
        float polygonLineCount = GetPolygonLineCount(num, lineSize);
        int num2 = node.parent.IndexOf(node);
        if((float)childCount > polygonLineCount)
        {
            lineSize = GetPolygonLineSize(num, (float)childCount);
            float num3 = (6.2831855f / (float)childCount * (float)num2) + GetExtraAngleOffset(childCount);
            return new RadialCell(num, num3, lineSize, float.NaN, childCount, parent);
        }
        float num4 = 6.2831855f / polygonLineCount;
        float num5 = (childCount > parent.siblings) ? parent.groupAngle : parent.angle;
        if(float.IsNaN(num5))
        {
            num5 = parent.angle;
        }
        float num6 = num5 + (num4 * ((float)num2 - ((float)(childCount - 1) / 2f)));
        return new RadialCell(num, num6, lineSize, num5, childCount, parent);
    }

    private static float ComputeNewSize(RadialCell parent)
    {
        var size = (parent.size - Config.IconSizeReductionDelta) * (float)Config.IconSizeReductionMult;
        return Mathf.Max(size, Config.MinIconSize);
    }

    private static float ComputeNewRadius(RadialCell parent, float size)
    {
        return parent.radius + (parent.size / 2) + (size / 2);
    }

    private static float GetExtraAngleOffset(int elementCount)
    {
        return elementCount < 2 ? 0 : -Mathf.PI * (elementCount - 2) / (2 * elementCount);
    }

    private static float GetPolygonRadius(float lineSize, float lineCount)
    {
        return lineSize / (2 * Mathf.Sin(Mathf.PI / lineCount));
    }

    public static float GetPolygonLineCount(float radius, float lineSize)
    {
        return (float)(Mathf.PI / System.Math.Asin(lineSize / (2 * radius)));
    }

    public static float GetPolygonLineSize(float radius, float lineCount)
    {
        return 2 * radius * Mathf.Sin(Mathf.PI / lineCount);
    }
}