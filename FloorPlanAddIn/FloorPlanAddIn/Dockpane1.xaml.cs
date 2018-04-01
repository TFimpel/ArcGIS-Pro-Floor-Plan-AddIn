using ArcGIS.Desktop.Framework;
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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace FloorPlanAddIn
{
    /// <summary>
    /// Interaction logic for Dockpane1View.xaml
    /// </summary>
    public partial class Dockpane1View : UserControl
    {
        public Dockpane1View()
        {
            InitializeComponent();



        }

        
                    public void btnOpenStyler(object sender, EventArgs e)
        {
            // in order to find a dockpane you need to know it's DAML id
            var pane = FrameworkApplication.DockPaneManager.Find("FloorPlanAddIn_Dockpane2");

            // determine visibility
            bool visible = pane.IsVisible;

            // activate it
            pane.Activate();

        }
    }
}
