//make a listbox that lists the currently open layouts y name and a button that lets one refresh that list. Implememnt as MVVM. make all of them sleected by default.

//make a button that for each layout zooms to the correct extend

//make a button that for each layout sets the line symbology
// --> ultimately this should have options, but just get it to work first

//make a button that for each layout sets the polygon symbology
// --> ultimately this should have options, but just get it to work first

//make a button that for each layout sets the labeling properties
// --> ultimately this should have options, but just get it to work first


using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;

namespace FloorPlanAddIn
{
    internal class Dockpane2ViewModel : DockPane
    {
        private const string _dockPaneID = "FloorPlanAddIn_Dockpane2";

        protected Dockpane2ViewModel() { }


        private ICommand _cmdZoomAndCenterActiveLayout;
        public ICommand CmdZoomAndCenterActiveLayout => _cmdZoomAndCenterActiveLayout ?? (_cmdZoomAndCenterActiveLayout = new RelayCommand(async () =>
        {
            //need to select the active map view. could possibly select it as a layout element but can't get that to work.

            if (LayoutView.Active.IsReady)
            {
                Debug.WriteLine("ready - zoom!");
                await QueuedTask.Run(() => { return ZoomToVisibleLayersAsync(MapView.Active); });
            }
            else
            {
                Debug.WriteLine("NOT ready - DON'T zoom!");
            };
        }));



        public async Task<bool> ZoomToVisibleLayersAsync(MapView activeMapView)
        {
            //Get the active map view.
            var mapView = activeMapView; // MapView.Active;
            if (mapView == null)
                return await Task.FromResult(false);

            //Zoom to all visible layers in the map.
            var visibleLayers = mapView.Map.Layers.Where(l => l.IsVisible);
            return await mapView.ZoomToAsync(visibleLayers);
        }

        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            DockPane pane = FrameworkApplication.DockPaneManager.Find(_dockPaneID);
            if (pane == null)
                return;

            pane.Activate();
        }

        /// <summary>
        /// Text shown near the top of the DockPane.
        /// </summary>
        private string _heading = "Style Floor Pans";
        public string Heading
        {
            get { return _heading; }
            set
            {
                SetProperty(ref _heading, value, () => Heading);
            }
        }
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Dockpane2_ShowButton : Button
    {
        protected override void OnClick()
        {
            Dockpane2ViewModel.Show();
        }
    }
}
