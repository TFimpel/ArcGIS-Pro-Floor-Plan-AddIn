using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using ArcGIS.Core.CIM;
using ArcGIS.Core.Data;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Layouts;
using ArcGIS.Desktop.Mapping;

namespace FloorPlanAddIn
{
    internal class Dockpane1ViewModel : DockPane
    {
        private const string _dockPaneID = "FloorPlanAddIn_Dockpane1";

        protected Dockpane1ViewModel() { }


        private bool _buildingChb;
        public bool BuildingChb
        {
            get
            {
                if (_buildingChb)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                _buildingChb = value;
                Debug.WriteLine("set (_buildingChb)");
                NotifyPropertyChanged("BuildingChb");
            }
        }

        private bool _floorChb;
        public bool FloorChb
        {
            get
            {
                if (_floorChb)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                _floorChb = value;
                Debug.WriteLine("set (_floorChb)");
                NotifyPropertyChanged("FloorChb");
            }
        }

        private bool _groupIDChb;
        public bool GroupIDChb
        {
            get
            {
                if (_groupIDChb)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            set
            {
                _groupIDChb = value;
                Debug.WriteLine("set (_groupIDChb)");
                NotifyPropertyChanged("GrupIDChb");
            }
        }



        private ObservableCollection<SiteData> _sites = new ObservableCollection<SiteData>();
        public ObservableCollection<SiteData> Sites
        {
            get {
                return _sites; }
            set
            {
                _sites = value;
            }
        }

        public String SelectedSitesWhereClause
        {
            get {
                if (BuildingChb)
                {
                    if ((Sites.Where(o => o.IsSelected).Select(x => x.Site)).Count() > 1)
                    {
                        return "SITE in ('" + string.Join("','", (Sites.Where(o => o.IsSelected).Select(x => x.Site))) + "')";
                    }
                    if ((Sites.Where(o => o.IsSelected).Select(x => x.Site)).Count() == 1)
                    {
                        return "SITE in ('" + string.Join("", (Sites.Where(o => o.IsSelected).Select(x => x.Site))) + "')";
                    }
                    else
                    {
                        return "SITE IS NOT NULL";
                    }
                }
                else
                {
                    return "SITE IS NOT NULL";
                }
            }
        }

        public String SelectedBuildingsWhereClause
        {
            get
            {
                if (FloorChb)
                {
                    if ((Buildings.Where(o => o.IsSelected).Select(x => x.Building)).Count() > 1)
                    {
                        return "BUILDING in ('" + string.Join("','", (Buildings.Where(o => o.IsSelected).Select(x => x.Building))) + "')";
                    }
                    if ((Buildings.Where(o => o.IsSelected).Select(x => x.Building)).Count() == 1)
                    {
                        return "BUILDING in ('" + string.Join("", (Buildings.Where(o => o.IsSelected).Select(x => x.Building))) + "')";
                    }
                    else
                    {
                        return "BUILDING IS NOT NULL";
                    }
                }
                else
                {
                    return "BUILDING IS NOT NULL";
                }
            }
        }

        public String SelectedFloorsWhereClause
        {
            get
            {
                if (GroupIDChb)
                {
                    if ((Floors.Where(o => o.IsSelected).Select(x => x.Floor)).Count() > 1)
                    {
                        return "FLOOR in ('" + string.Join("','", (Floors.Where(o => o.IsSelected).Select(x => x.Floor))) + "')";
                    }
                    if ((Floors.Where(o => o.IsSelected).Select(x => x.Floor)).Count() == 1)
                    {
                        return "FLOOR in ('" + string.Join("", (Floors.Where(o => o.IsSelected).Select(x => x.Floor))) + "')";
                    }
                    else
                    {
                        return "FLOOR IS NOT NULL";
                    }
                }
                else
                {
                    return "FLOOR IS NOT NULL";
                }
            }
        }

        public String SelectedGroupIDsWhereClause
        {
            get
            {
                if (1==1) //we always need this to create he maps
                {
                    if ((GroupIDs.Where(o => o.IsSelected).Select(x => x.GroupID)).Count() > 1)
                    {
                        return "GROUP_ID in ('" + string.Join("','", (GroupIDs.Where(o => o.IsSelected).Select(x => x.GroupID))) + "')";
                    }
                    if ((GroupIDs.Where(o => o.IsSelected).Select(x => x.GroupID)).Count() == 1)
                    {
                        return "GROUP_ID in ('" + string.Join("", (GroupIDs.Where(o => o.IsSelected).Select(x => x.GroupID))) + "')";
                    }
                    else
                    {
                        return "GROUP_ID IS NOT NULL";
                    }
                }
                //unreachable code in this case
                //else
                //{
                //    return "GROUP_ID IS NOT NULL";
                //}
            }
        }

        private ObservableCollection<BuildingData> _buildings = new ObservableCollection<BuildingData>();
        public ObservableCollection<BuildingData> Buildings
        {
            get { return _buildings; }
            set
            {
                _buildings = value;
            }
        }
        private ObservableCollection<FloorData> _floors = new ObservableCollection<FloorData>();
        public ObservableCollection<FloorData> Floors
        {
            get { return _floors; }
            set
            {
                _floors = value;
            }
        }
        private ObservableCollection<GroupIDData> _groupIDs = new ObservableCollection<GroupIDData>();
        public ObservableCollection<GroupIDData> GroupIDs
        {
            get { return _groupIDs; }
            set
            {
                _groupIDs = value;
            }
        }

        private readonly object _lockListOfSites = new object();
        private ICommand _cmdRefreshSiteData;
        public ICommand CmdRefreshSiteData => _cmdRefreshSiteData ?? (_cmdRefreshSiteData = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Sites, _lockListOfSites); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))

                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "SITE",
                        WhereClause = "",
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY SITE"
                    };

                    Sites.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Sites.Add(new SiteData()
                                {
                                    Site = Convert.ToString(row["SITE"]),
                                    IsSelected = false
                                });
                                Debug.WriteLine("added row");
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return

        private ICommand _cmdSelectAllSites;
        public ICommand CmdSelectAllSites => _cmdSelectAllSites ?? (_cmdSelectAllSites = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Sites, _lockListOfSites); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))

                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "SITE",
                        WhereClause = "",
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY SITE"
                    };

                    Sites.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Sites.Add(new SiteData()
                                {
                                    Site = Convert.ToString(row["SITE"]),
                                    IsSelected = true
                                });
                                Debug.WriteLine("added row");
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return


        private readonly object _lockListOfBuildings = new object();
        private ICommand _cmdRefreshBuildingData;
        public ICommand CmdRefreshBuildingData => _cmdRefreshBuildingData ?? (_cmdRefreshBuildingData = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Buildings, _lockListOfBuildings); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    Debug.WriteLine(SelectedSitesWhereClause);

                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "BUILDING",
                        WhereClause = SelectedSitesWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY BUILDING"
                    };

                    Buildings.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Buildings.Add(new BuildingData()
                                {
                                    Building = Convert.ToString(row["BUILDING"]),
                                    IsSelected = false
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return

        private ICommand _cmdSelectAllBuildings;
        public ICommand CmdSelectAllBuildings => _cmdSelectAllBuildings ?? (_cmdSelectAllBuildings = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Buildings, _lockListOfBuildings); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    Debug.WriteLine(SelectedSitesWhereClause);

                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "BUILDING",
                        WhereClause = SelectedSitesWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY BUILDING"
                    };

                    Buildings.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Buildings.Add(new BuildingData()
                                {
                                    Building = Convert.ToString(row["BUILDING"]),
                                    IsSelected = true
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return


        private readonly object _lockListOfFloors = new object();
        private ICommand _cmdRefreshFloorData;
        public ICommand CmdRefreshFloorData => _cmdRefreshFloorData ?? (_cmdRefreshFloorData = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Floors, _lockListOfFloors); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "FLOOR",
                        WhereClause = SelectedBuildingsWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY FLOOR"
                    };

                    Floors.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Floors.Add(new FloorData()
                                {
                                    Floor = Convert.ToString(row["FLOOR"]),
                                    IsSelected = false
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return

        private ICommand _cmdSelectAllFloors;
        public ICommand CmdSelectAllFloors => _cmdSelectAllFloors ?? (_cmdSelectAllFloors = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(Floors, _lockListOfFloors); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "FLOOR",
                        WhereClause = SelectedBuildingsWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY FLOOR"
                    };

                    Floors.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                Floors.Add(new FloorData()
                                {
                                    Floor = Convert.ToString(row["FLOOR"]),
                                    IsSelected = true
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return

        private readonly object _lockListOfGroupIDs = new object();
        private ICommand _cmdRefreshGroupIDData;
        public ICommand CmdRefreshGroupIDData => _cmdRefreshGroupIDData ?? (_cmdRefreshGroupIDData = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(GroupIDs, _lockListOfGroupIDs); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "GROUP_ID",
                        WhereClause = SelectedFloorsWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY GROUP_ID"
                    };

                    GroupIDs.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                GroupIDs.Add(new GroupIDData()
                                {
                                    GroupID = Convert.ToString(row["GROUP_ID"]),
                                    IsSelected = false
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return

        private ICommand _cmdSelectAllGroupIDs;
        public ICommand CmdSelectAllGroupIDs => _cmdSelectAllGroupIDs ?? (_cmdSelectAllGroupIDs = new RelayCommand(async () =>
        {
            BindingOperations.EnableCollectionSynchronization(GroupIDs, _lockListOfGroupIDs); //needed to avoid multithreading related error because a different thread is used to modify the collection than was used to create the collection.
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {
                    QueryDef queryDef = new QueryDef
                    {
                        Tables = "UMN_BLDG_ROOM",
                        SubFields = "GROUP_ID",
                        WhereClause = SelectedFloorsWhereClause,
                        PrefixClause = "DISTINCT",
                        PostfixClause = "ORDER BY GROUP_ID"
                    };

                    GroupIDs.Clear();

                    using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                    {
                        while (rowCursor.MoveNext())
                        {
                            using (Row row = rowCursor.Current)
                            {
                                Feature feature = row as Feature;
                                GroupIDs.Add(new GroupIDData()
                                {
                                    GroupID = Convert.ToString(row["GROUP_ID"]),
                                    IsSelected = true
                                });
                            }
                        }
                    }
                }
            });
        }, true //end queuedtask
                 )); // end relaycommand//end return


        List<string> uniqueSiteBuildingFloors = new List<string>(); 
        private ICommand _cmdValidate;
        public ICommand CmdValidate => _cmdValidate ?? (_cmdValidate = new RelayCommand(async () =>
        {
            await QueuedTask.Run(() =>
            {
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {

                    string combinedWhereClause = SelectedSitesWhereClause + " AND " + SelectedBuildingsWhereClause + " AND " + SelectedFloorsWhereClause + " AND " + SelectedGroupIDsWhereClause;
                    Debug.WriteLine("combinedWhereClause:");

                    Debug.WriteLine(combinedWhereClause);

                QueryDef queryDef = new QueryDef
                {
                    Tables = "UMN_BLDG_ROOM",
                    SubFields = "SITE, BUILDING, FLOOR",
                    WhereClause = combinedWhereClause,
                    PrefixClause = "DISTINCT",
                    PostfixClause = "ORDER BY SITE, BUILDING, FLOOR"
                };
                using (RowCursor rowCursor = geodatabase.Evaluate(queryDef, false))
                {
                        uniqueSiteBuildingFloors.Clear();
                    while (rowCursor.MoveNext())
                    {
                        using (Row row = rowCursor.Current)
                        {
                            Feature feature = row as Feature;
                                var site = Convert.ToString(row["SITE"]);
                                var building = Convert.ToString(row["BUILDING"]);
                                var floor = Convert.ToString(row["FLOOR"]);
                                uniqueSiteBuildingFloors.Add(site + building + floor);
                        }
                    }
                }
                Debug.WriteLine(String.Join(", ", uniqueSiteBuildingFloors.ToArray()) );
                }

            });
        },true));

        private ICommand _cmdCreateMapsLiveData;
        public ICommand CmdCreateMapsLiveData => _cmdCreateMapsLiveData ?? (_cmdCreateMapsLiveData = new RelayCommand(async () =>
        {
            //await QueuedTask.Run(async () =>
            //{
                using (Geodatabase geodatabase = new Geodatabase(new FileGeodatabaseConnectionPath(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\UMN_BLDG_ROOM.gdb"))))
                {

                    //for each site-building-floor combo floor open a new map and add the layers
                    foreach (var item in uniqueSiteBuildingFloors)
                    {
                        //create new layout
                        Layout layout = await QueuedTask.Run<Layout>(async () =>
                        {
                            //*** CREATE A NEW LAYOUT ***

                            //Set up a page
                            CIMPage newPage = new CIMPage();
                            //required properties
                            newPage.Width = 17;
                            newPage.Height = 11;
                            newPage.Units = LinearUnit.Inches;
                          

                            //optional rulers
                            newPage.ShowRulers = true;
                            newPage.SmallestRulerDivision = 0.5;

                            layout = LayoutFactory.Instance.CreateLayout(newPage);
                            layout.SetName("FL" + item);

                            //*** INSERT MAP FRAME ***

                            // create a new map
                            Map map = MapFactory.Instance.CreateMap("FL" + item, MapType.Map, MapViewingMode.Map, Basemap.None);

                            //Build map frame geometry
                            Coordinate2D ll = new Coordinate2D(4, 0.5);
                            Coordinate2D ur = new Coordinate2D(13, 6.5);
                            Envelope env = EnvelopeBuilder.CreateEnvelope(ll, ur);

                            //Create map frame and add to layout
                            MapFrame mfElm = LayoutElementFactory.Instance.CreateMapFrame(layout, env, map);
                            mfElm.SetName("FL" + item);

                            return layout;
                        });

                    //*** OPEN LAYOUT VIEW (must be in the GUI thread) ***
                    MapProjectItem mapPrjItem = Project.Current.GetItems<MapProjectItem>().FirstOrDefault(i => i.Name.Equals("FL" + item));
                    Map map2 = await QueuedTask.Run<Map>(() => { return mapPrjItem.GetMap(); });
                    FeatureLayer rooms = await QueuedTask.Run<FeatureLayer>(() => { return (FeatureLayer)LayerFactory.Instance.CreateLayer(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\floorplandata.gdb\\" + "FL" + item + "_ROOMS"), map2); });
                    FeatureLayer details = await QueuedTask.Run<FeatureLayer>(() => { return (FeatureLayer)LayerFactory.Instance.CreateLayer(new Uri("C:\\Users\\fimpe\\Documents\\ArcGIS\\Projects\\MyProject58\\floorplandata.gdb\\" + "FL" + item + "_DETAILS"), map2); });
                    Legend legend =  await QueuedTask.Run(() =>{Coordinate2D leg_ll = new Coordinate2D(1, 1); Coordinate2D leg_ur = new Coordinate2D(7.5, 3); Envelope leg_env = EnvelopeBuilder.CreateEnvelope(leg_ll, leg_ur); MapFrame mf = layout.FindElement("FL" + item) as MapFrame; Legend legendElm = LayoutElementFactory.Instance.CreateLegend(layout, leg_env, mf); legendElm.SetName("New Legend"); return legendElm;});

                    var layoutPane = await ProApp.Panes.CreateLayoutPaneAsync(layout);
                    var sel = layoutPane.LayoutView.GetSelectedElements();
                    if (sel.Count > 0)
                    {
                        layoutPane.LayoutView.ClearElementSelection();
                    }
                }
            }

            }));




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
    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class Dockpane1_ShowButton : Button
    {
        protected override void OnClick()
        {
            Dockpane1ViewModel.Show();
        }
    }

    internal class SiteData
    {
        public string Site { get; set; }
        public bool IsSelected { get; set; }

    }
    internal class BuildingData
    {
        public string Building { get; set; }
        public bool IsSelected { get; set; }
    }
    internal class FloorData
    {
        public string Floor { get; set; }
        public bool IsSelected { get; set; }
    }
    internal class GroupIDData
    {
        public string GroupID { get; set; }
        public bool IsSelected { get; set; }
    }


}
