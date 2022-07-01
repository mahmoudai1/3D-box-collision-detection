using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace Graphics_2_Assignments
{
    class _3D_Model
    {
        public List<_3D_Point> L_3D_Pts = new List<_3D_Point>(); 
        public List<Edge> L_Edges = new List<Edge>();
        public Camera cam;


        public void CreateTheObject(String name, float XS, float YS, float ZS, Color clr)
        {
            StreamReader sr = new StreamReader(name + ".txt");

            string strPt;
            while ((strPt = sr.ReadLine()) != null)
            {
                if (strPt[0] == 'L')
                    break;

                string[] s = strPt.Split(',');
                float[] v = new float[3];

                for (int i = 0; i < 3; i++)
                { 
                    v[i] = float.Parse(s[i]); 
                }

                L_3D_Pts.Add(new _3D_Point(v[0] + XS, v[1] + YS, v[2] + ZS));

            }

            int clr_i = 0;
            while ((strPt = sr.ReadLine()) != null)
            {
                string[] s1 = strPt.Split(',');
                int[] indx = new int[2];

                indx[0] = int.Parse(s1[0]);
                indx[1] = int.Parse(s1[1]);

                //Color[] clr = { Color.Red, Color.Orange, Color.Black, Color.Blue };

                Edge pnn = new Edge(indx[0], indx[1]);
                pnn.clr = clr;
                L_Edges.Add(pnn);
                clr_i++;
                if (clr_i == 4)
                {
                    clr_i = 0;
                }

            }
            sr.Close();
        }


        public void DrawYourSelf(Graphics g)
        {
            for (int k = 0; k < L_Edges.Count; k++) //Drawing edges between specific points in the 3D point list
            {
                int i = L_Edges[k].i;
                int j = L_Edges[k].j;
                //float d = 200;

                _3D_Point pi = L_3D_Pts[i];
                _3D_Point pj = L_3D_Pts[j];

                Pen Pn = new Pen(L_Edges[k].clr, 2);
                //g.DrawLine(Pn, pi.X, pi.Y, pj.X, pj.Y);
                //g.DrawLine(Pn, pi.X * (d / pi.Z), pi.Y * (d / pi.Z), pj.X * (d / pj.Z), pj.Y * (d / pj.Z));

                PointF pi_2D = cam.TransformToOrigin_And_Rotate_And_Project(pi);
                PointF pj_2D = cam.TransformToOrigin_And_Rotate_And_Project(pj);

                g.DrawLine(Pn, pi_2D.X, pi_2D.Y, pj_2D.X, pj_2D.Y);

            }
        }
    }
}
