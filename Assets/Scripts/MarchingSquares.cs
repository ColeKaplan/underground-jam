using System.Collections.Generic;
using UnityEngine;

public static class MarchingSquares
{
    public static List<Vector2[]> Generate(bool[,] map, float scale = 1f)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);

        List<Vector2[]> paths = new List<Vector2[]>();
        List<Vector2> current = new List<Vector2>();

        for (int y = 0; y < h - 1; y++)
        {
            for (int x = 0; x < w - 1; x++)
            {
                int cellType = 0;
                if (map[x, y]) cellType |= 1;
                if (map[x + 1, y]) cellType |= 2;
                if (map[x + 1, y + 1]) cellType |= 4;
                if (map[x, y + 1]) cellType |= 8;

                if (cellType == 0 || cellType == 15) continue;

                Vector2 pos = new Vector2(x, y) * scale;

                // Super simple: just add center of active cells (for rough demo)
                current.Add(pos);
            }
        }

        if (current.Count > 2)
            paths.Add(current.ToArray());

        return paths;
    }
}
