
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using HelixToolkit.Wpf;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WPFObjectsAppTest
{
    public class BallPoint : UIElement3D
    {
        public Point3D centerPosition;
        public string namereturn;
        public BallPoint()
        {

        }
        public BallPoint(Point3D position, double sphereSize, string name)
        {
            MeshBuilder builder = new MeshBuilder();    

           
            Material material1 = Materials.Black;

        
            centerPosition = position;
            builder.AddSphere(position, sphereSize, 40, 40);
            namereturn = name;
            GeometryModel3D model = new GeometryModel3D(builder.ToMesh(), material1);
            model.SetName(name);
            Visual3DModel = model;
        }





    }
}
