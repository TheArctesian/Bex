﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Path 
{
    [SerializeField, HideInInspector]
    
    List<Vector2> points;


    public Path(Vector2 centre)
    {
        points = new List<Vector2> //place points in []
        {
            centre+Vector2.left,
            centre+(Vector2.left+Vector2.up)*.5f,
            centre + (Vector2.right+Vector2.down)*.5f,
            centre + Vector2.right
        };
    }

    public Vector2 this[int i]
    {
        get
        {
            return points[i];
        }
    }

    public int NumPoints
    {
        get
        {
            return points.Count;
        }
    }

    public int NumSegments
    {
        get
        {
            return (points.Count - 4) / 3 + 1;
            // i dont rly know why this works 
        }
    }

    public void AddSegment(Vector2 anchorPos)
    {
        points.Add(points[points.Count -1] * 2 - points[points.Count - 2]); 
        /* 1st control point 
            postion of control point is vector plus displacement of point 
            eg P3 + (P3-P2) or 2P3 - P2
            so point [-1] would be control 
            and point [-2] would be point 
        */

        points.Add((points[points.Count-1] + anchorPos) *.5f);
        /* 2nd control p
            postion of control point is half the distance between next 2 points
            eg (P4-P6)/2
        */
    }

    public Vector2[] GetPointsInSegment(int i)
    {
        //self explanatory i think
        return new Vector2[] { points[i * 3], points[i * 3 + 1], points[i * 3 + 2], points[i * 3 +3]};
    }


    public void MovePoint(int i, Vector2 pos)
    {
        Vector2 deltaMove = pos - points[i];
        points[i] = pos;

        if (i % 3 == 0)
        {
            if (i + 1 < points.Count)
            {
                points[i + 1] += deltaMove;
            }
            if (i - 1 >= 0)
            {
                points[i - 1] += deltaMove;
            }
        }
        else
        {
            bool nextPointIsAnchor = (i + 1) % 3 == 0;
            int correspondingControlIndex = (nextPointIsAnchor) ? i + 2 : i - 2;
            int anchorIndex = (nextPointIsAnchor) ? i + 1 : i - 1;

            if (correspondingControlIndex >= 0 && correspondingControlIndex < points.Count)
            {
                float dst = (points[anchorIndex] - points[correspondingControlIndex]).magnitude;
                Vector2 dir = (points[anchorIndex] - pos).normalized;
                points[correspondingControlIndex] = points[anchorIndex] + dir * dst;
            }
        }
    }
}