﻿namespace Screeps3D
{

    internal interface IObjectViewComponent
    {
        void Init();
        void Load(RoomObject roomObject);
        void Delta(JSONObject data);
        void Unload(RoomObject roomObject);
    }
    
    internal interface IEnergyObject
    {
        float Energy { get; set; }
        float EnergyCapacity { get; set; }
    }

    internal interface INamedObject
    {
        string Name { get; set; }
    }

    internal interface IOwnedObject
    {
        string UserId { get; set; }
        ScreepsUser Owner { get; set; }
    }

    internal interface IHitpointsObject
    {
        float Hits { get; set; }
        float HitsMax { get; set; }
    }

    internal interface IDecay
    {
        float NextDecayTime { get; set; }
    }

    internal interface IProgress
    {
        float Progress { get; set; }
        float ProgressMax { get; set; }
    }
}