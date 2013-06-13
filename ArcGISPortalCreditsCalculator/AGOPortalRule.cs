using ArcGISPortalCreditsCalculator.Interface;
using ArcGISPortalCreditsCalculator.Interface.Types;
using System;
using System.Collections.Generic;

namespace ArcGISPortalCreditsCalculator
{    
    public class AGOPortalRule : IRuleCollection
    {
        public IEnumerable<IRule> Rules { get; set; }

        // TODO : complete rule map for all types and rules
        internal static Dictionary<string, Func<IRuleCollection>> RuleMap =
            new Dictionary<string, Func<IRuleCollection>>
            {                
                { ItemTypes.MapService, () => new AGOPortalRule 
                                                  {
                                                      Rules = new List<IRule>
                                                      {
                                                          Resolve<MapTileGenerationRule>()
                                                      } 
                                                  }
                },
                { ItemTypes.FeatureService, () => new AGOPortalRule 
                                                  {
                                                      Rules = new List<IRule>
                                                      {
                                                          Resolve<MapTileGenerationRule>()
                                                      } 
                                                  }
                },
                { ItemTypes.GeocodingService, () => new AGOPortalRule 
                                                    {
                                                        Rules = new List<IRule>
                                                        {
                                                            Resolve<GeocodingRule>() 
                                                        }
                                                    } 
                },
                { ItemTypes.NetworkAnalysis, () => new AGOPortalRule 
                                                   {
                                                       Rules = new List<IRule>
                                                       { 
                                                           Resolve<SimpleRouteRule>(), 
                                                           Resolve<OptimizedRouteRule>(), 
                                                           Resolve<DriveTimeRule>(), 
                                                           Resolve<ClosestFacilityRule>(), 
                                                           Resolve<MultiVehicleRoutingRule>()
                                                       }
                                                   }
                }
            };

        static T Resolve<T>() where T : IRule, new()
        {            
            return new T();
        }
    }
        
    /// <summary>
    /// Geocoding 80 credits per 1,000 geocodes 
    /// </summary>
    public class GeocodingRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 80.0 / 1000.0; } }

        public String Type { get { return ItemTypes.GeocodingService; } }
    }

    /// <summary>
    /// Map Tile Generation 1 credit per 1,000 tiles generated     
    /// </summary>
    public class MapTileGenerationRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 1.0 / 1000.0; } }

        public String Type { get { return ItemTypes.FeatureService; } }
    }

    /// <summary>
    /// Simple Route Service 200 credits per 1,000 simple routes
    /// </summary>
    public class SimpleRouteRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 200.0 / 1000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Optimized Route Service 500 credits per 1,000 optimized routes
    /// </summary>
    public class OptimizedRouteRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 500.0 / 1000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Drive-Time (Service Areas) Service 500 credits per 1,000 drive-times (service area) 
    /// </summary>
    public class DriveTimeRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 500.0 / 1000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Closest Facility Service 500 credits per 1,000 closest facilities 
    /// </summary>
    public class ClosestFacilityRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 500.0 / 1000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Multi-Vehicle Routing (VRP) Service 2,000 credits per 1,000 VRP routes
    /// </summary>
    public class MultiVehicleRoutingRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 2000.0 / 1000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Tile Loading 1 credit per 12,000,000 tile
    /// </summary>
    public class TileLoadingRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 1.0 / 12000000.0; } }

        public String Type { get { return ItemTypes.NetworkAnalysis; } }
    }

    /// <summary>
    /// Feature Services 2.4 credits per 10 MB stored per month 
    /// </summary>
    public class FeatureServiceHostingRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 2.4; } }

        public String Type { get { return ItemTypes.FeatureService; } }

        public double SizeModifier { get { return 10485760; } } // 10 MB in bytes
    }

    /// <summary>
    /// Data Transfer (outbound) 6 credits per 1 GB
    /// </summary>
    public class DataTransferRule : IRule
    {
        public int NumberOfOperations { get; set; }

        public double CreditsPerOperation { get { return 6; } }

        public String Type { get { return ItemTypes.WebMap; } }

        public double SizeModifier { get { return 1073741824; } } // 1 GB in bytes
    }
    
    // TODO : finish mapping item types and creating rules, add capability of storage related rules
    
    //Geographic Data Enrichment 60 credits per 1,000 data variables 
    //Infographics 60 credits per 1,000 data variables     
    // Tile and Data Storage 1.2 credits per 1 GB stored per month 
     
















}
