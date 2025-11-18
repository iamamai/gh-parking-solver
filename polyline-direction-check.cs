// Grasshopper Script Instance
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

using Rhino;
using Rhino.Geometry;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

public class Script_Instance : GH_ScriptInstance
{
  /* 
    Members:
      RhinoDoc RhinoDocument
      GH_Document GrasshopperDocument
      IGH_Component Component
      int Iteration

    Methods (Virtual & overridable):
      Print(string text)
      Print(string format, params object[] args)
      Reflect(object obj)
      Reflect(object obj, string method_name)
  */
  
  private void RunScript(object C, out object a)
  {
    a = null; // default output

    // Cast input to Curve
    Curve curve = C as Curve;
    if (curve == null)
    {
      Print("Input is not a curve.");
      return;
    }

    // Try to get polyline from curve
    Polyline poly;
    if (!curve.TryGetPolyline(out poly))
    {
      Print("Input is not a polyline.");
      return;
    }

    // Copy points to a List<Point3d>
    List<Point3d> pts = new List<Point3d>(poly);

    // Compute signed area  - SHOELACE FORMULA 
    double area = 0;
    for (int i = 0; i < pts.Count - 1; i++)
    {
      area += (pts[i + 1].X - pts[i].X) * (pts[i + 1].Y + pts[i].Y);
    }

    // If area < 0 → clockwise, so flip
    if (area > 0)
      pts.Reverse();

    // Convert back to polyline
    Polyline result = new Polyline(pts);

    // Output the clockwise polyline
    a = new GH_Curve(result.ToNurbsCurve());
  }
}
