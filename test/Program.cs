using System;
using WhatThe.Mods.CitiesSkylines.Asseteer;

namespace test
{
    internal class Program
    {
        internal const string categories = "Default BeautificationExpansion1 BeautificationExpansion2 BeautificationOthers BeautificationParks BeautificationPlazas EducationDefault ElectricityDefault FireDepartmentDisaster FireDepartmentFire HealthcareDefault LandscapingRocks LandscapingTrees LandscapingWaterStructures MonumentCategory1 MonumentCategory2 MonumentCategory3 MonumentCategory4 MonumentCategory5 MonumentCategory6 MonumentExpansion1 MonumentExpansion2 MonumentFootball MonumentLandmarks MonumentModderpack PoliceDefault PropsBillboardsLargeBillboard PropsBillboardsLogo PropsCommonAccessories PropsCommonLights PropsCommonStreets PropsIndustrialConstructionMaterials PropsIndustrialStructures PropsParksFlowersAndPlants PropsParksParkEquipment PropsParksPlaygrounds PropsResidentialGroundTiles PropsResidentialHomeYard PublicTransportBus PublicTransportCableCar PublicTransportMetro PublicTransportMonorail PublicTransportPlane PublicTransportShip PublicTransportTaxi PublicTransportTrain PublicTransportTram RoadsLarge RoadsMaintenance WaterHeating WaterServices";
        internal const string serviceCategories = "PublicTransport Natural Tourism Electricity PoliceDepartment FireDepartment Garbage Water Disaster HealthCare Education Office Road Monument Beautification PublicTransportCableCar PublicTransportTaxi CommercialLeisure Industrial IndustrialOil IndustrialOre PublicTransportBus PublicTransportMonorail CommercialTourist IndustrialFarming PublicTransportShip PublicTransportTram CommercialLow PublicTransportPlane IndustrialGeneric PublicTransportMetro CommercialHigh ResidentialLow PublicTransportTrain ResidentialHigh PropsBillboardsLogo PropsParksPlaygrounds PropsBillboardsLargeBillboard PropsIndustrialConstructionMaterials PropsParksFlowersAndPlants PropsResidentialHomeYard PropsResidentialGroundTiles PropsCommonLights PropsCommonStreets PropsCommonAccessories PropsIndustrialStructures LandscapingTrees Default";

        private static void Main(string[] args)
        {
            foreach (string category in serviceCategories.Split(' '))
            {
                Console.WriteLine("{0}:\t{1}", category, InfoHelper.CleanCategory(category));
            }
            Console.ReadKey();
        }
    }
}