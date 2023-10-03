using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;

namespace Tekla.Extension;
/// <summary>
/// Class for working with <see cref="Beam"/> in Tekla Structures
/// </summary>
public static class BeamExtension
{
    public static LineSegment GetLineSegment(this Beam beam)
    {
        return new LineSegment(beam.StartPoint, beam.EndPoint);
    }
    public static LineSegment GetCenterLineSegment(this Beam beam, bool withCutsFittings = true)
    {
        ArrayList centerLine = beam.GetCenterLine(withCutsFittings);
        Point point1 = centerLine[0] as Point;
        Point point2 = centerLine[1] as Point;
        return new LineSegment(point1, point2);
    }
    public static Point GetCenterPoint(this Beam beam, bool withCutsFittings = false)
    {
        return GetCenterLineSegment(beam).GetCenterPoint();
    }
 
    public static Vector GetVector(this Beam beam)
    {
        return new Vector(beam.EndPoint - beam.StartPoint);
    }
    public static double GetBeamLength(this Beam beam)
    {
        return Distance.PointToPoint(beam.StartPoint, beam.EndPoint);
    }
}
