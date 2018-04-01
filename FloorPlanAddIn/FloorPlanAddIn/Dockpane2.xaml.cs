using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FloorPlanAddIn
{
    /// <summary>
    /// Interaction logic for Dockpane2View.xaml
    /// </summary>
    public partial class Dockpane2View : UserControl
    {
        public Dockpane2View()
        {
            InitializeComponent();
        }


        //ptTxtElm.SetRotation(45);
        //MapFrame
        public async void rotateMap(object sender, EventArgs e)
        {
            var currName = MapView.Active.Map.Name;

            LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(currName));

            if (layoutItem != null)
            {
                await QueuedTask.Run(() =>
                {
                    Layout layout = layoutItem.GetLayout();
                    if (layout == null)
                        return;
                    MapFrame mfElm = layout.Elements.FirstOrDefault(item => item.Name.Equals(currName)) as MapFrame;
                    var currRot = mfElm.GetRotation();
                    Camera currCam = mfElm.Camera;
                    double currHeading = currCam.Heading;
                    currCam.Heading = currHeading + 45;
                    MapView.Active.ZoomToAsync(currCam, TimeSpan.Zero);


                });
            }
        }


                    public async void btnExportLayout(object sender, EventArgs e)
        {
            var currName = MapView.Active.Map.Name;
            LayoutProjectItem layoutItem = Project.Current.GetItems<LayoutProjectItem>().FirstOrDefault(item => item.Name.Equals(currName));

            if (layoutItem != null)
            {
                await QueuedTask.Run(() =>
                {
                    Layout layout = layoutItem.GetLayout();
                    if (layout == null)
                        return;

                    var outputFolder = Project.Current.HomeFolderPath;


                    PDFFormat PDF = new PDFFormat()
                    {
                        Resolution = 300,
                        OutputFileName = outputFolder + "\\" + currName +".pdf"
                    };

                    if (PDF.ValidateOutputFilePath())
                    {
                        if (File.Exists(PDF.OutputFileName)){
                            Debug.WriteLine(PDF.OutputFileName + " already exists.");
                            MessageBox.Show(PDF.OutputFileName + " already exists.", PDF.OutputFileName + " already exists.", MessageBoxButton.OK);

                        }
                        else
                        {
                            layout.Export(PDF);
                            MessageBox.Show("The layout was exported to a .pdf.", "The layout was exported to a pdf.", MessageBoxButton.OK);
                        }

                    }
                });
            }
        }


        public void btnChangeLayerFilter(object sender, EventArgs e)
        {
            var labelExpression = ((RadioButton)sender).Tag;
             QueuedTask.Run(() =>
            {
                //Get the active map's definition - CIMMap.
                var cimMap = MapView.Active.Map.GetDefinition();
                //Get the labeling engine from the map definition
                var cimGeneralPlacement = cimMap.GeneralPlacementProperties;
                if (cimGeneralPlacement is CIMMaplexGeneralPlacementProperties)
                {
                    Debug.WriteLine("already using maplex");
                }
                else
                {
                    //Create a new Maplex label engine properties
                    var cimMaplexGeneralPlacementProperties = new CIMMaplexGeneralPlacementProperties();
                    //Set the CIMMap's GeneralPlacementProperties to the new label engine
                    cimMap.GeneralPlacementProperties = cimMaplexGeneralPlacementProperties;
                }
                FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                if (labelExpression.ToString() == "None")
                {
                    a.SetLabelVisibility(false);
                }
                else
                {
                    a.SetLabelVisibility(true);
                    a.LabelClasses[0].SetExpression(labelExpression.ToString()); //$feature.RMNUMB +  TextFormatting.NewLine  + $feature.RMAREA
                    CIMTextSymbol s = a.LabelClasses[0].GetTextSymbol();
                    s.SetSize(14);
                    a.LabelClasses[0].SetTextSymbol(s);
                }
            });
          
        }
        public void btnChangeRefscaleReset(object sender, EventArgs e)
        {
            QueuedTask.Run(() =>
            {
                MapView.Active.Map.SetReferenceScale(0.0); //after 1st click ref scale is 1:100, after 2nd click its 1:200, 5th click its 100 again;
            });
        }
        public void btnChangeRefscaleIncrease(object sender, EventArgs e)
        {
               QueuedTask.Run(() =>
            {
                var currentRefscale = MapView.Active.Map.ReferenceScale;                
                MapView.Active.Map.SetReferenceScale(currentRefscale + 5000); //after 1st click ref scale is 1:100, after 2nd click its 1:200, 5th click its 100 again;
            });
        }
        public void btnChangeRefscaleDecrease(object sender, EventArgs e)
        {
            QueuedTask.Run(() =>
            {
                var currentRefscale = MapView.Active.Map.ReferenceScale;
                    Debug.WriteLine("down");
                    if (currentRefscale < 0)
                    {
                    Debug.WriteLine("refscale is already negative. don't lower further. that's just confusing.");
                    MessageBox.Show("Reference Scale cannot be decreased further. You need to change the appearance of the lines and labels in a different way, for example by modifying the layer symbology/labels settigns.");
                    }
                    else
                    {
                        MapView.Active.Map.SetReferenceScale(currentRefscale - 5000); //now 3rd click and its down to 100 again, 4th click its null again.
                    }
                            });
        }

        public void btnChangeLineSymbology(object sender, EventArgs e)
        {
            var lineSymbology = ((RadioButton)sender).Tag;
            QueuedTask.Run(() =>
            {
                //Get the active map's definition - CIMMap.

                if (lineSymbology.ToString() == "None")
                {
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolyline);
                    SimpleRendererDefinition renderer = new SimpleRendererDefinition() {
                        SymbolTemplate = new CIMSymbolReference()
                        {
                            Symbol = SymbolFactory.Instance.ConstructLineSymbol(
                        ColorFactory.Instance.WhiteRGB, 0.5, SimpleLineStyle.Null)
                }                 };
                    var r = lyr.CreateRenderer(renderer);
                    lyr.SetRenderer(r);
                }
                if (lineSymbology.ToString() == "Gray")
                {
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolyline);
                    SimpleRendererDefinition renderer = new SimpleRendererDefinition()
                    {
                        SymbolTemplate = new CIMSymbolReference()
                        {
                            Symbol = SymbolFactory.Instance.ConstructLineSymbol(
                        ColorFactory.Instance.GreyRGB, 0.5, SimpleLineStyle.Solid)
                        }
                    };
                    var r = lyr.CreateRenderer(renderer);
                    lyr.SetRenderer(r);
                }
                if (lineSymbology.ToString() == "Multicolor")
                {
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolyline);

                    CIMUniqueValueRenderer renderer = lyr.GetRenderer() as CIMUniqueValueRenderer;

                    // color ramp
                    CIMICCColorSpace colorSpace = new CIMICCColorSpace()
                    {
                        URL = "Default RGB"
                    };

                    CIMRandomHSVColorRamp randomHSVColorRamp = new CIMRandomHSVColorRamp()
                    {
                        ColorSpace = colorSpace,
                        MinAlpha = 100,
                        MaxAlpha = 100,
                        MinH = 0,
                        MaxH = 360,
                        MinS = 15,
                        MaxS = 30,
                        MinV = 99,
                        MaxV = 100,
                        Seed = 0,
                        
                    };

                    UniqueValueRendererDefinition uvr = new UniqueValueRendererDefinition()
                    {
                        ValueFields = new string[] { "OBJECTID" }, //multiple fields in the array if needed.
                        ColorRamp = randomHSVColorRamp //Specify color ramp  
                        
                    };

                    
                    CIMRenderer r = lyr.CreateRenderer(uvr);


                    var t = r.ToXml();
                    Debug.WriteLine(t);

                    lyr.SetRenderer(r);
                }
            });
        }

                public void btnChangeRoomSymbology(object sender, EventArgs e)
        {
            var polygonSymbology = ((RadioButton)sender).Tag;
            QueuedTask.Run(() =>
            {
                //Get the active map's definition - CIMMap.

                if (polygonSymbology.ToString() == "None")
                {
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolygon);
                    CIMSimpleRenderer renderer = lyr.GetRenderer() as CIMSimpleRenderer;
                    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                         ColorFactory.Instance.GreyRGB, 0.5, SimpleLineStyle.Solid);
                    CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(
                         ColorFactory.Instance.CreateRGBColor(255, 255, 255), SimpleFillStyle.Solid, outline);
                    //Update the symbol of the current simple renderer
                    renderer.Symbol = fillWithOutline.MakeSymbolReference();
                    //Update the feature layer renderer
                    lyr.SetRenderer(renderer);
                }
                if (polygonSymbology.ToString() == "Multicolor1")
                {
                    Debug.WriteLine("Multicolor1");
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolygon);
                    CIMUniqueValueRenderer  renderer = lyr.GetRenderer() as CIMUniqueValueRenderer;

                    // color ramp
                    CIMICCColorSpace colorSpace = new CIMICCColorSpace()
                    {
                        URL = "Default RGB"
                    };

                    CIMRandomHSVColorRamp randomHSVColorRamp = new CIMRandomHSVColorRamp()
                    {
                        ColorSpace = colorSpace,
                        MinAlpha = 100,
                        MaxAlpha = 100,
                        MinH = 0,
                        MaxH = 360,
                        MinS = 15,
                        MaxS = 30,
                        MinV = 99,
                        MaxV = 100,
                        Seed = 0
                    };

                    UniqueValueRendererDefinition uvr = new UniqueValueRendererDefinition()
                    {
                        ValueFields = new string[] { "RMAREA" }, //multiple fields in the array if needed.
                        ColorRamp = randomHSVColorRamp //Specify color ramp

                    };

                    var r = lyr.CreateRenderer(uvr);
                    lyr.SetRenderer(r);
                }
                if (polygonSymbology.ToString() == "Beige")
                {
                    var cimMap = MapView.Active.Map.GetDefinition();
                    //FeatureLayer a = MapView.Active.Map.Layers[1] as FeatureLayer;
                    var lyr = MapView.Active.Map.GetLayersAsFlattenedList().OfType<FeatureLayer>().FirstOrDefault(s => s.ShapeType == esriGeometryType.esriGeometryPolygon);
                    //CIMSimpleRenderer renderer = lyr.GetRenderer() as CIMSimpleRenderer;
                    CIMSimpleRenderer renderer = new CIMSimpleRenderer(){};
                    CIMStroke outline = SymbolFactory.Instance.ConstructStroke(
                         ColorFactory.Instance.GreyRGB, 0.5, SimpleLineStyle.Solid);
                    CIMPolygonSymbol fillWithOutline = SymbolFactory.Instance.ConstructPolygonSymbol(
                         ColorFactory.Instance.CreateRGBColor(255,222,173), SimpleFillStyle.Solid, outline);

                    //Update the symbol of the current simple renderer
                    renderer.Symbol = fillWithOutline.MakeSymbolReference();
                    //Update the feature layer renderer
                    lyr.SetRenderer(renderer);
                    /*
                     * 
                     * unique value rendere
                     * internal static Task UniqueValueRenderer(FeatureLayer featureLayer)
{
    return QueuedTask.Run(() =>
    {                
        //construct unique value renderer definition                
        UniqueValueRendererDefinition uvr = new
           UniqueValueRendererDefinition()
        {
            ValueFields = new string[] { SDKHelpers.GetDisplayField(featureLayer) }, //multiple fields in the array if needed.
            ColorRamp = SDKHelpers.GetColorRamp(), //Specify color ramp
        };

        //Creates a "Renderer"
        var cimRenderer = featureLayer.CreateRenderer(uvr);

        //Sets the renderer to the feature layer
        featureLayer.SetRenderer(cimRenderer);
    });
                 
}
                     * 
                     * 
                     * diagonal cross hatch example
                     * public static Task<CIMPolygonSymbol> CreateDiagonalCrossPolygonAsync()
{
    return QueuedTask.Run<CIMPolygonSymbol>(() =>
    {
        var trans = 50.0;//semi transparent
        CIMStroke outline = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(0, 0, 0, trans), 2.0, SimpleLineStyle.Solid);

        //Stroke for the fill
        var solid = SymbolFactory.Instance.ConstructStroke(CIMColor.CreateRGBColor(255, 0, 0, trans), 1.0, SimpleLineStyle.Solid);

        //Mimic cross hatch
        CIMFill[] diagonalCross =
            {
            new CIMHatchFill() {
                Enable = true,
                Rotation = 45.0,
                Separation = 5.0,
                LineSymbol = new CIMLineSymbol() { SymbolLayers = new CIMSymbolLayer[1] { solid } }
            },
            new CIMHatchFill() {
                Enable = true,
                Rotation = -45.0,
                Separation = 5.0,
                LineSymbol = new CIMLineSymbol() { SymbolLayers = new CIMSymbolLayer[1] { solid } }
            }
        };
        List<CIMSymbolLayer> symbolLayers = new List<CIMSymbolLayer>();
        symbolLayers.Add(outline);
        foreach (var fill in diagonalCross)
            symbolLayers.Add(fill);
        return new CIMPolygonSymbol() { SymbolLayers = symbolLayers.ToArray() };
    });
}
                     */
                }
            });

            Debug.WriteLine(MapView.Active.Map.Layers[0].Name);// .SetLabelVisibility(true);
            Debug.WriteLine(MapView.Active.Map.Layers[1].Name);// .SetLabelVisibility(true);
            //Debug.WriteLine(MapView.Active.Map.SetLabelEngine
        }

    }
}
