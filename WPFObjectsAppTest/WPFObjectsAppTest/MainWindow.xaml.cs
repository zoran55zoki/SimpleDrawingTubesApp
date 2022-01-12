using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFObjectsAppTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string ModeActivated = "";
        public Vector3D cameraDirectionVector;

        public Point3DCollection clickedPointsList = new Point3DCollection();
        public Point3DCollection clickedConnectedPointsList = new Point3DCollection();
        public MainWindow()
        {
            InitializeComponent();
            AddTransparentCylinder();
            AddVerticalTube1();
            AddVerticalTube2();
            AddVerticalTube3();
        }

        public void AddTransparentCylinder()
        {
            Point3D p1 = new Point3D();
            p1.X = 0;
            p1.Y = 0;
            p1.Z = 0;

            Point3D p2 = new Point3D();
            p2.X = 0;
            p2.Y = 0;
            p2.Z = 4;


            TransparentCylinder tc = new TransparentCylinder(p1, p2, 4);

            tc.IsHitTestVisible = false;
            tc.MouseLeftButtonUp += new MouseButtonEventHandler(TransparentSurfaceClick);

            tc.SetName("TransparentCylinder");

            ContainerUIElement3D cui = new ContainerUIElement3D();
            cui.Children.Add(tc);
            _transparentSurface.Children.Add(cui);
            InvalidateVisual();

        }

        public void AddVerticalTube1()
        {

            Point3DCollection p3collection = new Point3DCollection();

            Point3D p1 = new Point3D();
            p1.X = -2;
            p1.Y = 0;
            p1.Z = 0;

            Point3D p2 = new Point3D();
            p2.X = -2;
            p2.Y = 2;
            p2.Z = 2;

            Point3D p3 = new Point3D();
            p3.X = -2;
            p3.Y = 0;
            p3.Z = 4;
            p3collection.Add(p1);
            p3collection.Add(p2);
            p3collection.Add(p3);

            Material tube1material = Materials.Orange;
            Tube tube1 = new Tube(p3collection, "First", 0.3, 36, tube1material);

            tube1.IsHitTestVisible = true;
            tube1.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectAddedonClick);

            ContainerUIElement3D cuit1 = new ContainerUIElement3D();
            cuit1.Children.Add(tube1);
            _tubeObjects.Children.Add(cuit1);
            InvalidateVisual();




        }
        public void AddVerticalTube2()
        {
            Point3DCollection p3collection = new Point3DCollection();

            Point3D p1 = new Point3D();
            p1.X = 2;
            p1.Y = 0;
            p1.Z = 0;

            Point3D p2 = new Point3D();
            p2.X = 2;
            p2.Y = 2;
            p2.Z = 2;

            Point3D p3 = new Point3D();
            p3.X = 2;
            p3.Y = 0;
            p3.Z = 4;
            p3collection.Add(p1);
            p3collection.Add(p2);
            p3collection.Add(p3);

            Material tube1material = Materials.Green;
            Tube tube1 = new Tube(p3collection, "Second", 0.3, 36, tube1material);
            tube1.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectAddedonClick);
            tube1.IsHitTestVisible = true;


            ContainerUIElement3D cuit1 = new ContainerUIElement3D();
            cuit1.Children.Add(tube1);
            _tubeObjects.Children.Add(cuit1);
            InvalidateVisual();


        }
        public void AddVerticalTube3()
        {
            Point3DCollection p3collection = new Point3DCollection();

            Point3D p1 = new Point3D();
            p1.X = -3;
            p1.Y = 0;
            p1.Z = 2;

            Point3D p2 = new Point3D();
            p2.X = 0;
            p2.Y = 0;
            p2.Z = 2;

            Point3D p3 = new Point3D();
            p3.X = 3;
            p3.Y = 0;
            p3.Z = 2;
            p3collection.Add(p1);
            p3collection.Add(p2);
            p3collection.Add(p3);

            Material tube1material = Materials.Violet;
            Tube tube1 = new Tube(p3collection, "Third", 0.3, 36, tube1material);
            tube1.MouseLeftButtonUp += new MouseButtonEventHandler(ObjectAddedonClick);

            tube1.IsHitTestVisible = true;


            ContainerUIElement3D cuit1 = new ContainerUIElement3D();
            cuit1.Children.Add(tube1);
            _tubeObjects.Children.Add(cuit1);
            InvalidateVisual();


        }


        ModelVisual3D GetHitResult(Point location)
        {
            HitTestResult result = VisualTreeHelper.HitTest(_viewport, location);
            if (result != null && result.VisualHit is ModelVisual3D)
            {
                ModelVisual3D visual = (ModelVisual3D)result.VisualHit;
                return visual;
            }

            return null;
        }

        private void TransparentSurfaceClick(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (ModeActivated == "DrawTubes"|| ModeActivated == "ConnectedTubes")
            {

                cameraDirectionVector = new Vector3D(_viewport.Camera.NearPlaneDistance, 0, 0);
                Point3D temppos = new Point3D();
                Point mousePos = e.GetPosition(_viewport.Viewport);
                PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
                VisualTreeHelper.HitTest(_viewport, null, ResultCallback, hitParams);


                var hitList = _viewport.Viewport.FindHits(mousePos);

                var ray = Viewport3DHelper.Point2DtoRay3D(this._viewport.Viewport, mousePos);
               


                foreach (var hit in hitList)
                {
                    if (hit.Model != null)
                    {
                        string hittedTube = hit.Model.GetName();
                        if (hittedTube=="TransparentCylinder")
                        {

                            ///  var pi = ray.PlaneIntersection(hit.Position, cameraDirectionVector);

                            //  cameraDirectionVector = new Vector3D(hit.Position.X,hit.Position.Y,hit.Position.Z);
                            var pi = ray.PlaneIntersection(hit.Position, cameraDirectionVector);

                            if (pi.HasValue)
                            {


                                temppos = new Point3D((double)pi.Value.X, (double)pi.Value.Y, (double)pi.Value.Z);

                                break;


                            }

                        }


                    }
                }

                if (ModeActivated=="ConnectedTubes" && clickedConnectedPointsList.Count!=0)
                { clickedConnectedPointsList.Add(temppos);

                    if (clickedConnectedPointsList.Count >= 2) { btnShowConnectedTube.IsEnabled = true; }

                    BallPoint bp = new BallPoint(temppos, 0.1, "Tube Point");

                    ContainerUIElement3D cui = new ContainerUIElement3D();
                    cui.Children.Add(bp);
                    _ballClickPoints.Children.Add(cui);


                }
                else
                { clickedPointsList.Add(temppos);

                    if (clickedPointsList.Count >= 2) { btnShowMainTube.IsEnabled = true; }
                    BallPoint bp = new BallPoint(temppos, 0.1, "Tube Point");

                    ContainerUIElement3D cui = new ContainerUIElement3D();
                    cui.Children.Add(bp);
                    _ballClickPoints.Children.Add(cui);


                }
            

                InvalidateVisual();
            }

                e.Handled = true;

        }



            private void ObjectAddedonClick(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);


          

            cameraDirectionVector = new Vector3D(_viewport.Camera.NearPlaneDistance,0,0);




            if (ModeActivated == "AddCubes")
            {

                Point3D temppos = new Point3D();
                Point mousePos = e.GetPosition(_viewport.Viewport);
                PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
                VisualTreeHelper.HitTest(_viewport, null, ResultCallback, hitParams);


                var hitList = _viewport.Viewport.FindHits(mousePos);

                var ray = Viewport3DHelper.Point2DtoRay3D(this._viewport.Viewport, mousePos);

                List<string> allowedTubes = new List<string>() { "First", "Second", "Third" };


                foreach (var hit in hitList)
                {
                    if (hit.Model != null)
                    {
                        string hittedTube = hit.Model.GetName();
                        if (allowedTubes.Contains(hittedTube))
                        {

                            ///  var pi = ray.PlaneIntersection(hit.Position, cameraDirectionVector);

                            //  cameraDirectionVector = new Vector3D(hit.Position.X,hit.Position.Y,hit.Position.Z);
                            var pi = ray.PlaneIntersection(hit.Position, cameraDirectionVector);

                            if (pi.HasValue)
                            {


                                temppos = new Point3D((double)pi.Value.X, (double)pi.Value.Y, (double)pi.Value.Z);

                                break;


                            }

                        }


                    }
                }            






                Material cubematerial = Materials.Black;

                Cube cube = new Cube(temppos, cubematerial);
                cube.IsHitTestVisible = true;

                ContainerUIElement3D cui = new ContainerUIElement3D();

                cui.Children.Add(cube);
                cui.SetName("Cube");
                _cubeObjects.Children.Add(cui);
                InvalidateVisual();

            }
           
                
            

            e.Handled = true;
        }


        public HitTestResultBehavior ResultCallback(HitTestResult result)
        {
            RayHitTestResult rayResult = result as RayHitTestResult;
            if (rayResult != null)
            {
                RayMeshGeometry3DHitTestResult rayMeshResult = rayResult as RayMeshGeometry3DHitTestResult;

                              
            }

            return HitTestResultBehavior.Continue;
        }

        private void btnAddCube_Checked(object sender, RoutedEventArgs e)
        {
            ModeActivated = "AddCubes";
            btnDrawTubeOnSurface.IsChecked = false;
            pnlDraw.Visibility = Visibility.Hidden;

            ContainerUIElement3D cui3d = _transparentSurface.Children.FirstOrDefault() as ContainerUIElement3D;
            TransparentCylinder tc = cui3d.Children.FirstOrDefault() as TransparentCylinder;

            tc.IsHitTestVisible = false;
            InvalidateVisual();
        }

        private void btnSurfaceTubes_Checked(object sender, RoutedEventArgs e)
        {
            ModeActivated = "DrawTubes";
           btnAddCubes.IsChecked = false;
            pnlDraw.Visibility = Visibility.Visible;

            ContainerUIElement3D cui3d = _transparentSurface.Children.FirstOrDefault() as ContainerUIElement3D;
            TransparentCylinder tc = cui3d.Children.FirstOrDefault() as TransparentCylinder;

            tc.IsHitTestVisible = true;

            btnDrawConnectedTube.IsEnabled = false;
            btnShowConnectedTube.IsEnabled = false;
            btnShowMainTube.IsEnabled = false;

            InvalidateVisual();

        }

        private void btnAddCube_Unchecked(object sender, RoutedEventArgs e)
        {
            ModeActivated = "DrawTubes";

            ContainerUIElement3D cui3d = _transparentSurface.Children.FirstOrDefault() as ContainerUIElement3D;
            TransparentCylinder tc = cui3d.Children.FirstOrDefault() as TransparentCylinder;

            tc.IsHitTestVisible = true;

            pnlDraw.Visibility = Visibility.Hidden;

        }

        private void btnSurfaceTubes_Unchecked(object sender, RoutedEventArgs e)
        {
            ModeActivated = "AddCubes";
            ContainerUIElement3D cui3d = _transparentSurface.Children.FirstOrDefault() as ContainerUIElement3D;
            TransparentCylinder tc = cui3d.Children.FirstOrDefault() as TransparentCylinder;

            tc.IsHitTestVisible = false;

            pnlDraw.Visibility = Visibility.Hidden;

            InvalidateVisual();
        }

        private void btn_DrawNewMainTube(object sender, RoutedEventArgs e)
        {
            //Reset the drawing variables

            clickedPointsList.Clear();
            _ballClickPoints.Children.Clear();
            this.InvalidateVisual();

        }

        private void btn_VisalizeMainTube(object sender, RoutedEventArgs e)
        {
            //Show the Main Tube

            if (clickedPointsList.Count < 2)
            {
                MessageBox.Show("Please select more then 2 points from the surface", "App Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {

                _ballClickPoints.Children.Clear();

                this.InvalidateVisual();

                List<Material> materials = new List<Material>() { Materials.Blue, Materials.Brown, Materials.Gold, Materials.Gray, Materials.Hue, Materials.Orange, Materials.Violet, Materials.Red, Materials.Rainbow, Materials.Indigo };

                Random rand = new Random();
                int number = rand.Next(0, 9);
                Material material = materials[number];

             Tube maintube = new Tube(clickedPointsList, "MainTube", 0.2, 36, material);  ///add clickable function
              maintube.MouseLeftButtonUp += new MouseButtonEventHandler(MainTubeClick);
              maintube.IsHitTestVisible = true;
                maintube.SetName("MainTube");
                ContainerUIElement3D cui = new ContainerUIElement3D();
                cui.SetName("MainTube");
                cui.Children.Add(maintube);

                _tubeObjects.Children.Add(cui);

             //  clickedPointsList.Clear();
           

                this.InvalidateVisual();

            }

            clickedPointsList.Clear();

            btnShowConnectedTube.IsEnabled = false;
            btnDrawConnectedTube.IsEnabled = true;
            btnShowMainTube.IsEnabled = false;

        }


        private void MainTubeClick(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            cameraDirectionVector = new Vector3D(_viewport.Camera.NearPlaneDistance, 0, 0);


            if ( ModeActivated=="ConnectedTubes")
            {

                Point3D temppos = new Point3D();

                Point mousePos = e.GetPosition(_viewport.Viewport);
                PointHitTestParameters hitParams = new PointHitTestParameters(mousePos);
                VisualTreeHelper.HitTest(_viewport, null, ResultCallback, hitParams);

                //Point3D splinepoint3D=new Point3D();

                var hitList = _viewport.Viewport.FindHits(mousePos);


                var ray = Viewport3DHelper.Point2DtoRay3D(this._viewport.Viewport, mousePos);



                foreach (var hit in hitList)
                {
                    if (hit.Model != null)
                    {
                        string hittettube = hit.Model.GetName();
                        if (hittettube == "MainTube"|| hittettube == "ConnectedTube")
                        {


                            //var pi = ray.PlaneIntersection(hit.Position, new Vector3D(0, 0, 1));
                            var pi = ray.PlaneIntersection(hit.Position, cameraDirectionVector);
                            // p1click = hit.Position;
                            // You can use also hit.Mesh
                            // also hit.Model
                            // also hit.Visual
                            // also hit.Normal
                            if (pi.HasValue)
                            {
                                // var pRound = new Point3D(Math.Round(pi.Value.X), Math.Round(pi.Value.Y), 0);
                                //    var pRound = new Point3D(Math.Floor(pi.Value.X), Math.Floor(pi.Value.Y), Math.Floor(pi.Value.Z));

                                //string hittedvisual = hit.Visual.GetHashCode().ToString();
                                //  temppos = new Point3D((int)pi.Value.X, (int)pi.Value.Y, (int)pi.Value.Z);
                                temppos = new Point3D((double)pi.Value.X, (double)pi.Value.Y, (double)pi.Value.Z);
                            }

                        }

                    }
                }


                if (clickedConnectedPointsList.Count == 0)
                {
                    clickedConnectedPointsList.Add(temppos);
                    BallPoint bp = new BallPoint(temppos, 0.1, "Tube Point");
                    bp.SetName("Tube Point");

                    ContainerUIElement3D cui = new ContainerUIElement3D();


                    cui.Children.Add(bp);
                    cui.SetName("Tube Point");
                    _ballClickPoints.Children.Add(cui);




                }

                else
                {
                    MessageBox.Show("Please add points on the leg surface", " Mevec 3.0", MessageBoxButton.OK, MessageBoxImage.Information);

                }







            }






                e.Handled = true;
        }

            private void btn_DrawConnectedTube(object sender, RoutedEventArgs e)
        {
            ModeActivated = "ConnectedTubes";
        }

        private void btn_VisalizeConnectedTube(object sender, RoutedEventArgs e)
        {
            btnDrawConnectedTube.IsChecked = false;

            //Show connected tube


            //Show the Main Tube

            if (clickedConnectedPointsList.Count < 2)
            {
                MessageBox.Show("Please select more then 2 points from the surface", "App Info", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            else
            {

                _ballClickPoints.Children.Clear();

                this.InvalidateVisual();

                List<Material> materials = new List<Material>() { Materials.Blue, Materials.Brown, Materials.Gold, Materials.Gray, Materials.Hue, Materials.Orange, Materials.Violet, Materials.Red, Materials.Rainbow, Materials.Indigo };

                Random rand = new Random();
                int number = rand.Next(0, 9);
                Material material = materials[number];

                Tube connectedtube = new Tube(clickedConnectedPointsList, "ConnectedTube", 0.2, 36, material);  ///add clickable function
                connectedtube.MouseLeftButtonUp += new MouseButtonEventHandler(MainTubeClick);
                connectedtube.IsHitTestVisible = true;
                connectedtube.SetName("ConnectedTube");
                ContainerUIElement3D cui = new ContainerUIElement3D();
                cui.SetName("ConnectedTube");
                cui.Children.Add(connectedtube);

                _tubeObjects.Children.Add(cui);

                //  clickedPointsList.Clear();


                this.InvalidateVisual();

            }

           clickedConnectedPointsList.Clear();

            btnShowConnectedTube.IsEnabled = false;

        }

        private void btn_DrawConnectedTubeUnchecked(object sender, RoutedEventArgs e)
        {
            ModeActivated = "DrawTubes";

        }
    }
}
