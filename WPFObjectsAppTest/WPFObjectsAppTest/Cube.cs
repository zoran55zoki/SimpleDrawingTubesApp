


using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;

using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Timers;
using System.Windows.Input;
using System.Collections.Specialized;
using HelixToolkit.Wpf;
using System;

namespace WPFObjectsAppTest
{
    class Cube : UIElement3D, INotifyCollectionChanged
    {
        private readonly Timer _timer;
        private readonly ToolTip _toolTip;

        public Cube DataContext { get; }
        public Material materialtype;
        public double diameter;
      
        
       
        
        public Material Texture { get { return materialtype; } set { materialtype = value; } }
        public Cube(Point3D position, Material material)
        {
            MeshBuilder builder = new MeshBuilder();         


            builder.AddBox(position, 0.2, 0.3, 0.2);
            materialtype = material;

            GeometryModel3D model = new GeometryModel3D(builder.ToMesh(), material);

          
         
            model.SetName("Cube");           

            Visual3DModel = model;

            _toolTip = new ToolTip();
          
            _timer = new Timer { AutoReset = false };
            _timer.Elapsed += ShowToolTip;

            DataContext = this;

        }



        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.LeftButton == MouseButtonState.Pressed)
            {

                var gm = Visual3DModel as GeometryModel3D;
                gm.Material = gm.Material == Materials.Black ? Materials.Yellow : Materials.Black;
                e.Handled = true;
            }
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add
            {
                ((INotifyCollectionChanged)DataContext).CollectionChanged += value;
            }

            remove
            {
                ((INotifyCollectionChanged)DataContext).CollectionChanged -= value;
            }
        }

        public object ToolTipContent { get { return _toolTip.Content; } set { _toolTip.Content = value; } }


        private void ShowToolTip(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            if (_toolTip != null)
                _toolTip.Dispatcher.Invoke(new Action(() => { _toolTip.IsOpen = true; }));
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            var gm = Visual3DModel as GeometryModel3D;



          


            if (_toolTip != null)
            {
                _toolTip.IsOpen = true;
                _toolTip.Content = gm.GetName().ToString().Trim() + " object";
            }

            _timer.Interval = 50;
            _timer.Start();

            e.Handled = true;
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);

            var gm = Visual3DModel as GeometryModel3D;
        

            _timer.Stop();
            if (_toolTip != null)
            {
                _toolTip.IsOpen = false;
                _toolTip.Content = "";
            }


            e.Handled = true;

        }







    }
}
